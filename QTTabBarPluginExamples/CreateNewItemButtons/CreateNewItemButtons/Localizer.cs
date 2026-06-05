using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QTPlugin;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Drawing;
using QTPlugin.Interop;

namespace QuizoPlugin
{
	sealed class Localizer : LocalizedStringProvider2
	{
		public static string[] resources, tooltip, messages;

		public Localizer()
		{			
		}

		static Localizer()
		{
			Initialize();
		}

		private static void Initialize()
		{
			resources = new string[7];

			StringBuilder sb = new StringBuilder( 260 );
			if( W32.S_OK != ShellAPI.SHLoadIndirectString( "@" + Environment.ExpandEnvironmentVariables( @"%SystemRoot%\system32\shell32.dll" ) + ",-30396", sb, sb.Capacity, IntPtr.Zero ) )		// "New folder"
			{
				sb.Append( "New folder" );
			}
			resources[0] = sb.ToString();

			sb.Clear();
			if( W32.S_OK != ShellAPI.SHLoadIndirectString( "@" + Environment.ExpandEnvironmentVariables( @"%SystemRoot%\system32\notepad.exe" ) + ",-470", sb, sb.Capacity, IntPtr.Zero ) )		// "New text file"
			{
				sb.Append( "New File" );
			}
			resources[1] = sb.ToString();

			bool fJa = CultureInfo.CurrentCulture.Parent.Name == "ja";

			resources[2] = fJa ? "連番ファイル" : "Create Sequentially Named";
			resources[3] = fJa ? "新しいフォルダーを作成するボタンです。ver 128以降専用。" : "Button to create a new folder. (for ver 128-)";
			resources[4] = fJa ? "新しいテキストファイルを作成するボタンです。ver 128以降専用。" : "Button to create a new text file. (for ver 128-)";
			resources[5] = fJa ? "連番ファイルを作成するボタンです。ver 128以降専用。" : "Button to create files or folders numbered consecutively. (for ver 128-)";
			resources[6] = fJa ? "連番ファイル作成" : "Create files or folders numbered consecutively";

			tooltip = ( fJa ? Resource.Tooltips_ja : Resource.Tooltips ).Split( new char[] { ';' } );
			messages = ( fJa ? Resource.Messages_ja : Resource.Messages ).Split( new char[] { ';' } );
		}

		public override void SetKey( int iKey )
		{
			this.Key = iKey;
		}

		private int Key
		{
			get;
			set;
		}

		public override string Name
		{
			get
			{
				return resources[this.Key];
			}
		}

		public override string Author
		{
			get
			{
				return "Quizo";
			}
		}

		public override string Description
		{
			get
			{
				return resources[this.Key + 3];
			}
		}

		public override DateTime LastUpdate
		{
			get
			{
				return new DateTime( 2013, 3, 10 );
			}
		}

		public override string SupportURL
		{
			get
			{
				return "https://qttabbar.backlog.jp/projects/Q";
			}
		}

		public static string[] Tooltips
		{
			get
			{
				return tooltip;
			}
		}
	}

	static class ShellAPI
	{
		[DllImport( "shlwapi.dll", CharSet = CharSet.Unicode )]
		public static extern int SHLoadIndirectString( string pszSource, StringBuilder pszOutBuf, int cchOutBuf, IntPtr ppvReserved );

		[DllImport( "shell32.dll", CharSet = CharSet.Unicode )]
		public static extern IntPtr SHGetFileInfo( string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, int cbSizeFileInfo, SHGFI uFlags );

		[DllImport( "comctl32.dll" )]
		public static extern IntPtr ImageList_GetIcon( IntPtr himl, int i, int flags );

		[DllImport( "shell32.dll" )]
		public static extern int SHGetImageList( SHIL iImageList, [In, MarshalAs( UnmanagedType.LPStruct )] Guid riid, out IntPtr ppv );

		public static readonly Guid IID_IImageList = new Guid( "46EB5926-582E-4017-9FDF-E8998DAA0950" );

		public static bool SUCCEEDED( int hresult )
		{
			return hresult >= 0;
		}

		[DllImport( "user32.dll" )]
		public static extern int DestroyIcon( IntPtr hIcon );											// returns BOOL

		public static IntPtr getHIcon( string path, SHIL imageSize, bool fExt )
		{
			// according to my test, 10% faster than SHGetFileInfo( ,,,, SHGFI.ICON | SHGFI.SMALLICON | SHGFI.PIDL );

			const uint FILE_ATTRIBUTE_NORMAL = 0x00000080;

			// get system icon index.
			SHFILEINFO sfi = new SHFILEINFO();
			int cbSize = Marshal.SizeOf( typeof( SHFILEINFO ) );
			IntPtr himl;
			uint att = 0;
			SHGFI flag = 0;
			if( imageSize == SHIL.SMALL )
			{
				flag = SHGFI.SMALLICON;
			}
			if( fExt )
			{
				att = FILE_ATTRIBUTE_NORMAL;
				flag |= SHGFI.USEFILEATTRIBUTES;
			}


			himl = SHGetFileInfo( path, att, ref sfi, cbSize, SHGFI.SYSICONINDEX | flag );


			if( himl != IntPtr.Zero )
			{
				if( imageSize == SHIL.LARGE || imageSize == SHIL.SMALL )
				{
					return ImageList_GetIcon( himl, sfi.iIcon, 0 );
				}

				// get system imagelist for EXTRALARGE(48) / JUMBO(256) / SYSSMALL icon
				IntPtr himlLarge;
				if( SUCCEEDED( SHGetImageList( imageSize, IID_IImageList, out himlLarge ) ) )
				{
					if( himlLarge != IntPtr.Zero )
					{
						try
						{
							return ImageList_GetIcon( himlLarge, sfi.iIcon, 0 );
						}
						finally
						{
							Marshal.Release( himlLarge );
						}
					}
				}
			}

			return IntPtr.Zero;
		}

		public static Bitmap shrinkBitmap( Bitmap bmp48 )
		{
			var bmp = new Bitmap( 24, 24 );
			using( var g = Graphics.FromImage( bmp ) )
			{
				g.DrawImage( bmp48, new Rectangle( 0, 0, 24, 24 ), new Rectangle( 0, 0, 48, 48 ), GraphicsUnit.Pixel );
			}
			return bmp;
		}


		//public static string GetPath( IShellBrowser shellBrowser )
		//{
		//    Guid IID_IPersistFolder2 = new Guid( "1AC3D9F0-175C-11d1-95BE-00609797EA4F" );

		//    IShellView shellView = null;
		//    IntPtr pidl = IntPtr.Zero;
		//    try
		//    {
		//        if( W32.S_OK == shellBrowser.QueryActiveShellView( out shellView ) )
		//        {
		//            IPersistFolder2 persistFolder;
		//            var folderView = shellView as IFolderView;
		//            if( folderView != null && W32.S_OK == folderView.GetFolder( IID_IPersistFolder2, out persistFolder ) )
		//            {
		//                if( W32.S_OK == persistFolder.GetCurFolder( out pidl ) )
		//                {
		//                    return PInvoke.GetPath( pidl );
		//                }
		//            }
		//        }
		//    }
		//    finally
		//    {
		//        if( shellView != null )
		//        {
		//            Marshal.ReleaseComObject( shellView );
		//        }

		//        if( pidl != IntPtr.Zero )
		//        {
		//            Marshal.FreeCoTaskMem( pidl );
		//        }
		//    }
		//    return null;
		//}
	}

	static class W32
	{
		public const int S_OK = 0;
		public const int FCW_STATUS = 0x0001;
	}

	enum SHIL
	{
		/// <summary>
		/// normally 32x32
		/// </summary>
		LARGE = 0,
		/// <summary>
		/// normally 16x16
		/// </summary> 
		SMALL = 1,
		/// <summary>
		/// These images are the Shell standard extra-large icon size. This is typically 48x48, but the size can be customized by the user.
		/// </summary>
		EXTRALARGE = 2,
		/// <summary>
		/// like SHIL_SMALL, but tracks system small icon metric correctly
		/// </summary>
		SYSSMALL = 3,
		/// <summary>
		/// normally 256x256
		/// </summary>
		JUMBO = 4,

		/* As of Windows Vista, SHIL_SMALL, SHIL_LARGE, and SHIL_EXTRALARGE scale with dots per inch (dpi) if the process is marked as dpi-aware. */
	}

	[StructLayout( LayoutKind.Sequential, CharSet = CharSet.Unicode )]
	struct SHFILEINFO
	{
		// Pack = 1 on Win32, 8 on Win64 ( #if !defined(_WIN64)#include <pshpack1.h>... )
		public IntPtr hIcon;
		public int iIcon;
		public uint dwAttributes;
		[MarshalAs( UnmanagedType.ByValTStr, SizeConst = 260 )]
		public string szDisplayName;
		[MarshalAs( UnmanagedType.ByValTStr, SizeConst = 80 )]
		public string szTypeName;
	}

	[Flags]
	enum SHGFI
	{
		ICON			= 0x000000100,     // get icon
		DISPLAYNAME		= 0x000000200,     // get display name
		TYPENAME		= 0x000000400,     // get type name
		ATTRIBUTES		= 0x000000800,     // get attributes
		ICONLOCATION	= 0x000001000,     // get icon location
		EXETYPE			= 0x000002000,     // return exe type
		SYSICONINDEX	= 0x000004000,     // get system icon index
		LINKOVERLAY		= 0x000008000,     // put a link overlay on icon
		SELECTED		= 0x000010000,     // show icon in selected state
		ATTR_SPECIFIED	= 0x000020000,     // get only specified attributes
		LARGEICON		= 0x000000000,     // get large icon
		SMALLICON		= 0x000000001,     // get small icon
		OPENICON		= 0x000000002,     // get open icon
		SHELLICONSIZE	= 0x000000004,     // get shell size icon
		PIDL			= 0x000000008,     // pszPath is a pidl
		USEFILEATTRIBUTES = 0x000000010,     // use passed dwFileAttribute
		ADDOVERLAYS		= 0x000000020,     // apply the appropriate overlays
		OVERLAYINDEX	= 0x000000040,     // Get the index of the overlay
	}

}
