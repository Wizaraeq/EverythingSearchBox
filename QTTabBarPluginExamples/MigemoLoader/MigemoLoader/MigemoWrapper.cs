using System;
using System.Text;
using System.Runtime.InteropServices;

namespace QuizoPlugins
{
	sealed class MigemoWrapper : IDisposable
	{
		[DllImport( "kernel32.dll", CharSet = CharSet.Unicode )]
		private static extern IntPtr LoadLibrary( string lpFileName );

		[DllImport( "kernel32.dll" )]
		private static extern bool FreeLibrary( IntPtr hModule );

		[DllImport( "kernel32.dll", CharSet = CharSet.Ansi )]
		private static extern IntPtr GetProcAddress( IntPtr hModule, string lpProcName );

		private delegate IntPtr migemo_open( string dict );
		private delegate IntPtr migemo_query( IntPtr pMigemo, IntPtr pQuery );
		private delegate void migemo_release( IntPtr pMigemo, IntPtr stringToRelease );
		private delegate void migemo_close( IntPtr pMigemo );
		private delegate int migemo_is_enable( IntPtr pMigemo );

		private migemo_query mQuery;
		private migemo_release mRlease;
		private migemo_close mClose;
		private migemo_is_enable mIsEnable;

		private IntPtr pMigemo;
		private IntPtr hModuleMigemo;
		private bool fUTF8;
		private bool disposed;


		public MigemoWrapper( string pathMigemoDll, string pathDict, bool fUTF8 )
		{
			this.fUTF8 = fUTF8;

			if( !String.IsNullOrEmpty( pathMigemoDll ) && !String.IsNullOrEmpty( pathDict ) )
			{
				this.hModuleMigemo = LoadLibrary( pathMigemoDll );

				if( this.hModuleMigemo != IntPtr.Zero )
				{
					IntPtr pOpen	 = GetProcAddress( this.hModuleMigemo, "migemo_open" );
					IntPtr pQuery	 = GetProcAddress( this.hModuleMigemo, "migemo_query" );
					IntPtr pRelease  = GetProcAddress( this.hModuleMigemo, "migemo_release" );
					IntPtr pClose	 = GetProcAddress( this.hModuleMigemo, "migemo_close" );
					IntPtr pIsEnable = GetProcAddress( this.hModuleMigemo, "migemo_is_enable" );

					bool fSuccess = pOpen != IntPtr.Zero &&
									pQuery != IntPtr.Zero &&
									pRelease != IntPtr.Zero &&
									pClose != IntPtr.Zero &&
									pIsEnable != IntPtr.Zero;

					if( fSuccess )
					{
						migemo_open mOpen = Marshal.GetDelegateForFunctionPointer( pOpen, typeof( migemo_open ) ) as migemo_open;
						this.mQuery		  = Marshal.GetDelegateForFunctionPointer( pQuery, typeof( migemo_query ) ) as migemo_query;
						this.mRlease	  = Marshal.GetDelegateForFunctionPointer( pRelease, typeof( migemo_release ) ) as migemo_release;
						this.mClose		  = Marshal.GetDelegateForFunctionPointer( pClose, typeof( migemo_close ) ) as migemo_close;
						this.mIsEnable	  = Marshal.GetDelegateForFunctionPointer( pIsEnable, typeof( migemo_is_enable ) ) as migemo_is_enable;

						if( mOpen != null && this.mQuery != null && this.mRlease != null && this.mClose != null && this.mIsEnable != null )
						{
							this.pMigemo = mOpen( pathDict );

							if( this.IsEnable )
							{
								return;
							}
							else if( this.pMigemo != IntPtr.Zero  )
							{
								this.mClose( this.pMigemo );
								this.pMigemo = IntPtr.Zero;
							}
						}
					}

					// free if failed.
					FreeLibrary( this.hModuleMigemo );
					this.hModuleMigemo = IntPtr.Zero;
				}
			}

			throw new ArgumentException();
		}

		~MigemoWrapper()
		{
			this.Dispose( false );
		}

		public void Dispose()
		{
			this.Dispose( true );
			GC.SuppressFinalize( this );
		}

		private void Dispose( bool disposing )
		{
			if( !this.disposed )
			{
				if( this.pMigemo != IntPtr.Zero )
				{
					this.mClose( this.pMigemo );
					this.pMigemo = IntPtr.Zero;
				}

				if( this.hModuleMigemo != IntPtr.Zero )
				{
					FreeLibrary( this.hModuleMigemo );
					this.hModuleMigemo = IntPtr.Zero;
				}

				this.disposed = true;
			}
		}

		public string QueryRegexStr( string strQuery )
		{
			if( this.IsEnable && strQuery != null )
			{
				if( strQuery.StartsWith( "/" ) && strQuery.EndsWith( "/" ) && strQuery.Length > 2 )
				{
					// user input is regex
					return strQuery;
				}

				IntPtr pRegexStr = IntPtr.Zero;
				byte[] bQuery = this.fUTF8 ? Encoding.UTF8.GetBytes( strQuery ) : Encoding.GetEncoding( 932 ).GetBytes( strQuery );
				IntPtr pQuery = Marshal.AllocCoTaskMem( bQuery.Length + 1 );
				Marshal.Copy( bQuery, 0, pQuery, bQuery.Length );
				Marshal.Copy( new byte[] { 0 }, 0, pQuery + bQuery.Length, 1 );
				try
				{
					pRegexStr = this.mQuery( this.pMigemo, pQuery );
					if( pRegexStr != IntPtr.Zero )
					{
						int len = 0;
						unsafe
						{
							byte* p = (byte*)pRegexStr;
							while( *p != 0 )	// 1 byte null char terminator of utf-8 / ascii
							{
								len++;
								p++;
							}
						}

						var buffer = new byte[len];
						Marshal.Copy( pRegexStr, buffer, 0, len );

						return this.fUTF8 ? Encoding.UTF8.GetString( buffer ) : Encoding.GetEncoding( 932 ).GetString( buffer );
					}
				}
				catch
				{
				}
				finally
				{
					if( pRegexStr != IntPtr.Zero )
					{
						this.mRlease( this.pMigemo, pRegexStr );
					}

					if( pQuery != IntPtr.Zero )
					{
						Marshal.FreeCoTaskMem( pQuery );
					}
				}
			}
			return strQuery;
		}

		public bool IsEnable
		{
			get
			{
				return this.pMigemo != IntPtr.Zero && 0 != this.mIsEnable( this.pMigemo );
			}
		}

	}
}
