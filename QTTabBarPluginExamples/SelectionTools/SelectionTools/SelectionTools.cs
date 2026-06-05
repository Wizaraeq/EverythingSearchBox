using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using QTPlugin;
using QTPlugin.Interop;

namespace QuizoPlugins
{
	[Plugin( PluginType.Background, typeof( Localizer ), 0, Version = "1.3.0.0" )]
	public class SelectEmptyButton : IBarButton
	{
		// 1.3.0.0		plugin lib 1.3.0.0., supports extra views.

		private IPluginServer pluginServer;
		private IShellBrowser shellBrowser;
		private Localizer localizer;

		#region IPluginClient members

		public void Open( IPluginServer pluginServer, IShellBrowser shellBrowser )
		{
			this.pluginServer = pluginServer;
			this.shellBrowser = shellBrowser;

			this.localizer = new Localizer();
			this.localizer.SetKey( 0 );
		}

		public bool QueryShortcutKeys( out string[] actions )
		{
			actions = new string[] { this.localizer.KeyShortcutStr[0] };
			return true;
		}

		public void Close( EndCode endCode )
		{
		}

		public bool HasOption
		{
			get
			{
				return false;
			}
		}

		public void OnMenuItemClick( MenuType menuType, string menuText, ITab tab )
		{
		}

		public void OnOption()
		{
		}

		public void OnShortcutKeyPressed( int index )
		{
			this.SelectEmpties();
		}

		#endregion

		#region IBarButton members

		public Image GetImage( bool fLarge )
		{
			return fLarge ? Resource.SelectEmptyButton_large : Resource.SelectEmptyButton_small;
		}

		public void InitializeItem()
		{
		}

		public void OnButtonClick()
		{
			this.SelectEmpties();
		}

		public bool ShowTextLabel
		{
			get
			{
				return false;
			}
		}

		public string Text
		{
			get
			{
				return this.localizer.Name;
			}
		}

		#endregion

		private void SelectEmpties()
		{
			var view = this.pluginServer.FocusedView;
			if( view == QTPlugin.View.None )
			{
				view = QTPlugin.View.Default;
			}

			var tab = this.pluginServer.GetSelectedTab( view );
			if( tab == null )
			{
				return;
			}

			var path = tab.Address.Path;
			if( String.IsNullOrEmpty( path ) )
			{
				return;
			}

			DirectoryInfo di;
			try
			{
				di = new DirectoryInfo( path );
				if( !di.Exists )
				{
					return;
				}
			}
			catch
			{
				System.Media.SystemSounds.Asterisk.Play();
				return;
			}

			try
			{
				var lst = new List<string>();
				foreach( DirectoryInfo diSub in di.GetDirectories() )
				{
					if( FolderIsEmpty( diSub ) )
					{
						lst.Add( diSub.FullName );
					}
				}
				foreach( FileInfo fiSub in di.GetFiles() )
				{
					if( fiSub.Length == 0 )
					{
						lst.Add( fiSub.FullName );
					}
				}

				tab.SelectedPaths = lst;
			}
			catch( Exception ex )
			{
				MessageBox.Show( ex.ToString() );
			}
		}

		private static bool FolderIsEmpty( DirectoryInfo di )
		{
			try
			{
				foreach( FileInfo fiSub in di.EnumerateFiles() )
				{
					try
					{
						if( fiSub.Length > 0 )
						{
							return false;
						}
					}
					catch
					{
					}
				}

				foreach( DirectoryInfo diSub in di.EnumerateDirectories() )
				{
					try
					{
						if( !FolderIsEmpty( diSub ) )
						{
							return false;
						}
					}
					catch
					{
					}
				}
			}
			catch
			{
				// Access violation, etc.
				return false;
			}

			return true;
		}
	}

	[Plugin( PluginType.Background, typeof( Localizer ), 1, Version = "1.3.0.0" )]
	public class SelectFileByExtension : IBarButton
	{
		// 1.2.0.4		remove the code that clears pluginServer in Close() method 
		// 1.3.0.0		plugin lib 1.3.0.0, supports extra views.

		private IPluginServer pluginServer;
		private Localizer localizer;

		#region IPluginClient Members

		public void Open( IPluginServer pluginServer, IShellBrowser shellBrowser )
		{
			this.pluginServer = pluginServer;
			this.pluginServer.ViewEventSource.NavigationComplete += new PluginEventHandler( pluginServer_NavigationComplete );
			this.pluginServer.ViewEventSource.SelectionChanged += new PluginEventHandler( pluginServer_SelectionChanged );
			this.pluginServer.ViewEventSource.ActiveViewChanged += new PluginEventHandler( ViewEventSource_ActiveViewChanged );
			this.pluginServer.SettingsChanged += new PluginEventHandler( pluginServer_SettingsChanged );

			this.localizer = new Localizer();
			this.localizer.SetKey( 1 );
		}

		public bool QueryShortcutKeys( out string[] actions )
		{
			actions = new string[] { this.localizer.KeyShortcutStr[0], this.localizer.KeyShortcutStr[1] };
			return true;
		}

		public void Close( EndCode endCode )
		{
		}

		public bool HasOption
		{
			get
			{
				return false;
			}
		}

		public void OnMenuItemClick( MenuType menuType, string menuText, ITab tab )
		{
		}

		public void OnOption()
		{
		}

		public void OnShortcutKeyPressed( int index )
		{
			this.SelectFiles( index );
		}

		#endregion
		
		#region IBarButton Members

		public Image GetImage( bool fLarge )
		{
			return fLarge ? Resource.SelectFileByExtension_large : Resource.SelectFileByExtension_small;
		}

		public void InitializeItem()
		{
		}

		public void OnButtonClick()
		{
			this.SelectFiles( 0 );
		}

		public bool ShowTextLabel
		{
			get
			{
				return false;
			}
		}

		public string Text
		{
			get
			{
				return this.localizer.Name;
			}
		}

		#endregion
		
		private static bool IsSevenOrLater
		{
			get
			{
				Version version = Environment.OSVersion.Version;
				return ( version.Major == 6 && version.Minor > 0 ) || version.Major > 6;
			}
		}

		private void SelectFiles( int index )
		{
			var view = this.pluginServer.FocusedView;
			if( view == QTPlugin.View.None )
			{
				view = QTPlugin.View.Default;
			}

			var tab = this.pluginServer.GetSelectedTab( view );
			if( tab == null )
			{
				return;
			}

			var sf = ShellFolderFromTab( tab );
			if( sf == null )
			{
				return;
			}

			IEnumIDList enumIDList = null;
			try
			{
				var lstExts = new List<string>();
				bool fFolder = false;
				var idls = tab.SelectedIDLs;
				if( idls != null )
				{
					foreach( byte[] idl in idls )
					{
						if( idl != null && idl.Length > 2 )
						{
							IntPtr pidl = PInvoke.CreatePIDL( idl );
							try
							{
								IntPtr pidlRltv = ShellAPI.ILFindLastID( pidl );

								uint sfgao = (uint)( SFGAO.FOLDER | SFGAO.STREAM );
								if( W32.S_OK == sf.GetAttributesOf( 1, new IntPtr[] { pidlRltv }, ref sfgao ) && ( sfgao & (uint)SFGAO.FOLDER ) != 0 )
								{
									if( ( sfgao & (uint)SFGAO.STREAM ) != 0 )
									{
										// can be compressed folder
										string pathCF = ShellAPI.GetDisplayName( sf, pidlRltv );
										if( !String.IsNullOrEmpty( pathCF ) && File.Exists( pathCF ) )
										{
											string ext = Path.GetExtension( pathCF );
											if( !String.IsNullOrEmpty( ext ) )
											{
												lstExts.Add( ext.ToLower() );
												continue;
											}
										}
									}

									fFolder = true;
									continue;
								}

								string path = ShellAPI.GetDisplayName( sf, pidlRltv );
								if( !String.IsNullOrEmpty( path ) )
								{
									string ext = Path.GetExtension( path );
									if( !String.IsNullOrEmpty( ext ) )
									{
										lstExts.Add( ext.ToLower() );
									}
								}
							}
							catch
							{
							}
							finally
							{
								if( pidl != IntPtr.Zero )
								{
									Marshal.FreeCoTaskMem( pidl );
								}
							}
						}
					}
				}

				if( lstExts.Count > 0 || fFolder )
				{
					if( index == 0 )
					{
						var idlsToSelect = new List<byte[]>();
						SHCONTF flags = SHCONTF.FOLDERS | SHCONTF.NONFOLDERS | SHCONTF.INCLUDEHIDDEN;
						if( IsSevenOrLater )
						{
							flags |= SHCONTF.INCLUDESUPERHIDDEN;
						}

						if( W32.S_OK == sf.EnumObjects( IntPtr.Zero, (int)flags, out enumIDList ) )
						{
							IntPtr pidl;
							IntPtr fetched;
							while( 0 == enumIDList.Next( 1, out pidl, out fetched ) )		// definition of IEnumIDList.Next in QTPluginLib.dll is wrong, 3rd argument
							{
								try
								{
									string path = ShellAPI.GetDisplayName( sf, pidl );
									if( fFolder )
									{
										uint sfgao = (uint)( SFGAO.FOLDER | SFGAO.STREAM );
										if( 0 == sf.GetAttributesOf( 1, new IntPtr[] { pidl }, ref sfgao ) && ( sfgao & (uint)SFGAO.FOLDER ) != 0 )
										{
											if( ( sfgao & (uint)SFGAO.STREAM ) == 0 || !File.Exists( path ) )
											{
												idlsToSelect.Add( PInvoke.GetIDListData( pidl ) );
												continue;
											}
										}
									}
									if( !String.IsNullOrEmpty( path ) )
									{
										string ext = Path.GetExtension( path );
										if( !String.IsNullOrEmpty( ext ) && lstExts.Contains( ext.ToLower() ) )
										{
											idlsToSelect.Add( PInvoke.GetIDListData( pidl ) );
										}
									}
								}
								catch
								{
								}
								finally
								{
									if( pidl != IntPtr.Zero )
									{
										Marshal.FreeCoTaskMem( pidl );
									}
								}
							}
						}

						if( idlsToSelect.Count > 0 )
						{
							tab.SelectedIDLs = idlsToSelect;
						}
					}
					else if( !fFolder )
					{
						// filter
						string str = String.Empty;
						foreach( var s in lstExts )
						{
							str += ( str.Length > 0 ? "|" : String.Empty ) + Regex.Escape( s ) + "$";
						}
						this.pluginServer.InvokeCommand( QCommand.Filter, "/" + str + "/", (int)tab.View );
					}
				}
				else
				{
					// clear selection
					tab.SelectedIDLs = new List<byte[]>();
					System.Media.SystemSounds.Beep.Play();
				}
			}
			catch
			{
			}
			finally
			{
				if( sf != null )
				{
					Marshal.ReleaseComObject( sf );
				}

				if( enumIDList != null )
				{
					Marshal.ReleaseComObject( enumIDList );
				}
			}
		}

		private void Update()
		{
			var selection = false;
			var view = this.pluginServer.FocusedView;
			if( view != QTPlugin.View.None )
			{
				var tab = this.pluginServer.GetSelectedTab( view );
				if( tab != null )
				{
					var sel = tab.SelectedIDLs;
					selection = sel != null && sel.Count > 0;
				}
			}
			this.pluginServer.UpdateItem( this, selection, false );
		}

		private static IShellFolder ShellFolderFromTab( ITab tab )
		{
			IntPtr pidl = PInvoke.CreatePIDL( tab.IDL );
			if( pidl != IntPtr.Zero )
			{
				IShellFolder sf;
				object o;
				if( W32.S_OK == PInvoke.SHBindToObject( null, pidl, IntPtr.Zero, PInvoke.IID_IShellFolder, out o ) )
				{
					sf = o as IShellFolder;
					if( sf != null )
					{
						return sf;
					}
					if( o != null )
					{
						Marshal.ReleaseComObject( o );
					}
				}
			}
			return null;
		}

		private void pluginServer_NavigationComplete( object sender, PluginEventArgs e )
		{
			this.Update();
		}

		private void pluginServer_SettingsChanged( object sender, PluginEventArgs e )
		{
			this.Update();
		}

		private void pluginServer_SelectionChanged( object sender, PluginEventArgs e )
		{
			this.Update();
		}

		private void ViewEventSource_ActiveViewChanged( object sender, PluginEventArgs e )
		{
			this.Update();
		}
	}

	[Plugin( PluginType.Background, typeof( Localizer ), 2, Version = "2.1.0.0" )]
	public class SelectionInfoToStatusBar : IBarDropButton
	{
		// 2.1.0.0			plugin lib 1.3.0.0, supports extra views. Remove blocking ui thread...!
		
		// NO STATUS BAR in Windows8 and later.

		private IPluginServer pluginServer;
		private IShellBrowser shellBrowser;
		private Localizer localizer;

		private volatile bool CancellationPending;
		private volatile int nThread;
		private volatile bool fClosed;
		private volatile int sessionID;
		private static bool IsJa = System.Globalization.CultureInfo.CurrentCulture.Parent.Name == "ja", fInitialized;
		internal static string[] PropertyDisplayNames = { "Size", "Modified", "Created", "Attributes", IsJa ? Resource.currentDriveInfo_ja : Resource.currentDriveInfo,
																									   IsJa ? Resource.currentFolderInfo_ja : Resource.currentFolderInfo, 
																									   IsJa ? Resource.sizeInBytes_ja : Resource.sizeInBytes,
																									   IsJa ? Resource.foldersize_ja : Resource.foldersize,
																									   IsJa ? Resource.enableForNetwork_ja : Resource.enableForNetwork };

		internal static StatusInfoKind InfoKind;

		
		public SelectionInfoToStatusBar()
		{
			if( !fInitialized )
			{
				fInitialized = true;
				InitializePropertyStrings();				
			}
		}

		private static void InitializePropertyStrings()
		{
			Guid FMTID_Storage = new Guid( "B725F130-47EF-101A-A5F1-02608C9EEBAC " );
			int PID_STG_SIZE = 12;
			int PID_STG_ATTRIBUTES = 13;
			int PID_STG_WRITETIME = 14;
			int PID_STG_CREATETIME = 15;

			string strSize, strAttr, strMod, strCr;

			strSize = PropertyDescription.GetDisplayName( new PROPERTYKEY
			{
				fmtid = FMTID_Storage,
				pid = PID_STG_SIZE
			} );
			if( !String.IsNullOrEmpty( strSize ) )
			{
				PropertyDisplayNames[0] = strSize;
			}
			strMod = PropertyDescription.GetDisplayName( new PROPERTYKEY
			{
				fmtid = FMTID_Storage,
				pid = PID_STG_WRITETIME
			} );
			if( !String.IsNullOrEmpty( strMod ) )
			{
				PropertyDisplayNames[1] = strMod;
			}
			strCr = PropertyDescription.GetDisplayName( new PROPERTYKEY
			{
				fmtid = FMTID_Storage,
				pid = PID_STG_CREATETIME
			} );
			if( !String.IsNullOrEmpty( strCr ) )
			{
				PropertyDisplayNames[2] = strCr;
			}
			strAttr = PropertyDescription.GetDisplayName( new PROPERTYKEY
			{
				fmtid = FMTID_Storage,
				pid = PID_STG_ATTRIBUTES
			} );
			if( !String.IsNullOrEmpty( strAttr ) )
			{
				PropertyDisplayNames[3] = strAttr;
			}
		}

		#region IPluginClient members

		public void Open( IPluginServer pluginServer, IShellBrowser shellBrowser )
		{
			this.pluginServer = pluginServer;
			this.shellBrowser = shellBrowser;

			this.localizer = new Localizer();
			this.localizer.SetKey( 2 );

			ReadSettings();

			this.pluginServer.ViewEventSource.NavigationComplete += new PluginEventHandler( pluginServer_NavigationComplete );
			this.pluginServer.ViewEventSource.SelectionChanged += new PluginEventHandler( pluginServer_SelectionChanged );
			this.pluginServer.ViewEventSource.ActiveViewChanged += new PluginEventHandler( ViewEventSource_ActiveViewChanged );
			this.pluginServer.ViewEventSource.TabChanged += new PluginEventHandler( ViewEventSource_TabChanged );
		}

		public bool QueryShortcutKeys( out string[] actions )
		{
			actions = new string[] { this.localizer.KeyShortcutStr[0] };		// Abort
			return true;
		}

		public void Close( EndCode endCode )
		{
			this.CancellationPending = true;
			this.Wait();

			if( endCode == EndCode.WindowClosed )
			{
				this.fClosed = true;
				SelectionInfoToStatusBar.SaveSettings();
			}
		}

		public bool HasOption
		{
			get
			{
				return true;
			}
		}

		public void OnMenuItemClick( MenuType menuType, string menuText, ITab tab )
		{
		}

		public void OnOption()
		{
			using( Form_SelectionInfoSB fs = new Form_SelectionInfoSB() )
			{
				fs.ShowDialog();
			}
		}

		public void OnShortcutKeyPressed( int index )
		{
			bool fRunning = this.nThread > 0;
			this.CancellationPending = true;
			if( fRunning )
			{
				this.shellBrowser.SetStatusTextSB( Localizer.StatusInfoMenu[5] );	// Aborted.
			}
		}

		#endregion

		#region IBarButton members

		public Image GetImage( bool fLarge )
		{
			return fLarge ? Resource.SelectionInfoToStatusBar_large : Resource.SelectionInfoToStatusBar_small;
		}

		public void InitializeItem()
		{
		}

		public void OnButtonClick()
		{
		}

		public bool ShowTextLabel
		{
			get
			{
				return false;
			}
		}

		public string Text
		{
			get
			{
				return this.localizer.Name;
			}
		}

		#endregion

		#region IBarDropButton members

		public bool IsSplitButton
		{
			get
			{
				return false;
			}
		}

		public void OnDropDownOpening( ToolStripDropDownMenu menu )
		{
			if( menu.Items.Count == 0 )
			{
				menu.SuspendLayout();

				menu.ShowImageMargin = false;
				menu.ShowCheckMargin = true;

				var kinds = new StatusInfoKind[] { StatusInfoKind.Size, StatusInfoKind.Modified, StatusInfoKind.Created, StatusInfoKind.Attributes, StatusInfoKind.DriveInfo, StatusInfoKind.FolderInfo, StatusInfoKind.SizeInByte, StatusInfoKind.NoFolderSize, StatusInfoKind.Network };

				for( int i = 0; i < PropertyDisplayNames.Length; i++ )
				{
					ToolStripMenuItem tsmi = new ToolStripMenuItem( PropertyDisplayNames[i] );
					tsmi.Checked = InfoKind.HasFlag( kinds[i] ) ^ kinds[i] == StatusInfoKind.NoFolderSize;
					tsmi.Tag = kinds[i];

					menu.Items.Add( tsmi );

					if( kinds[i] == StatusInfoKind.FolderInfo )
					{
						menu.Items.Add( new ToolStripSeparator() );
					}
				}
				menu.ResumeLayout();
			}
			else
			{
				for( int i = 0; i < menu.Items.Count; i++ )
				{
					ToolStripMenuItem tsmi = menu.Items[i] as ToolStripMenuItem;
					if( tsmi != null )
					{
						StatusInfoKind kind = (StatusInfoKind)tsmi.Tag;
						tsmi.Checked = InfoKind.HasFlag( kind ) ^ kind == StatusInfoKind.NoFolderSize;
					}
				}
			}
		}

		public void OnDropDownItemClick( ToolStripItem item, MouseButtons mouseButton )
		{
			this.CancellationPending = true;

			var tsmi = item as ToolStripMenuItem;
			if( tsmi != null )
			{
				StatusInfoKind kind = (StatusInfoKind)tsmi.Tag;

				tsmi.Checked = !tsmi.Checked;
				if( tsmi.Checked ^ kind == StatusInfoKind.NoFolderSize )
				{
					InfoKind |= kind;
				}
				else
				{
					InfoKind &= ~kind;
				}
				SaveSettings();

				var view = this.pluginServer.FocusedView;
				if( view == QTPlugin.View.None )
				{
					view = QTPlugin.View.Default;
				}
				this.StartCalculating( view );
			}
		}

		#endregion
		
		private void Wait()
		{
			int timeOut = 0;
			while( this.nThread > 0 )
			{
				Thread.Sleep( 50 );
				if( ++timeOut > 20 )	// 1 sec max.
				{
					break;
				}
			}
		}

		/// <summary>
		/// Fired when navigation completed.
		/// Abort getting size.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pluginServer_NavigationComplete( object sender, PluginEventArgs e )
		{
			this.CancellationPending = true;

			// Current Drive Info
			if( InfoKind.HasFlag( StatusInfoKind.DriveInfo ) || InfoKind.HasFlag( StatusInfoKind.FolderInfo ) )
			{
				Task.Factory.StartNew( () => Thread.Sleep( 350 ) ).ContinueWith( ( t ) => this.SetCurrentDriveFolderInfoText( true, e.View ), TaskScheduler.FromCurrentSynchronizationContext() );
			}
			else
			{
				Task.Factory.StartNew( () => Thread.Sleep( 350 ) ).ContinueWith( ( t ) => this.StartCalculating( e.View ), TaskScheduler.FromCurrentSynchronizationContext() );
			}
		}

		/// <summary>
		/// Fired when selection changed in Folder View.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pluginServer_SelectionChanged( object sender, PluginEventArgs e )
		{
			this.StartCalculating( e.View );
		}

		private void ViewEventSource_TabChanged( object sender, PluginEventArgs e )
		{
			if( e.View != QTPlugin.View.Default )
			{
				this.StartCalculating( e.View );
			}
		}

		private void ViewEventSource_ActiveViewChanged( object sender, PluginEventArgs e )
		{
			var view = this.pluginServer.FocusedView;
			if( view == QTPlugin.View.None )
			{
				view = QTPlugin.View.Default;
			}

			this.StartCalculating( view );
		}

		/// <summary>
		/// 
		/// </summary>
		private void StartCalculating( QTPlugin.View view )
		{
			if( InfoKind == 0 )
			{
				return;
			}

			// StatusBar existance 
			IntPtr hwndStatusBar;
			if( W32.S_OK != this.shellBrowser.GetControlWindow( W32.FCW_STATUS, out hwndStatusBar ) || hwndStatusBar == IntPtr.Zero )
			{
				return;
			}

			this.CancellationPending = true;
			this.Wait();
			this.CancellationPending = false;

			try
			{
				var tab = this.pluginServer.GetSelectedTab( view );
				if( tab == null )
				{
					return;
				}

				var selection = tab.SelectedPaths;
				if( selection != null )
				{
					if( selection.Count == 0 )
					{
						// no selection, display drive info
						if( InfoKind.HasFlag( StatusInfoKind.DriveInfo ) || InfoKind.HasFlag( StatusInfoKind.FolderInfo ) )
						{
							Task.Factory.StartNew( () => Thread.Sleep( 100 ) ).ContinueWith( ( t ) => this.SetCurrentDriveFolderInfoText( false, view ), TaskScheduler.FromCurrentSynchronizationContext() );
						}
						else
						{
							// item count
							this.shellBrowser.SetStatusTextSB( String.Format( Localizer.ItemString[0], tab.ItemCount ) );
						}
						return;
					}

					bool fAllDrive = true;
					foreach( var path in selection )
					{
						if( String.IsNullOrEmpty( path ) || path.Length != 3 )
						{
							fAllDrive = false;
							break;
						}
					}
					if( fAllDrive )
					{
						this.SetDriveInfoText( selection );
						return;
					}

					this.shellBrowser.SetStatusTextSB( Localizer.StatusInfoMenu[3] );	// Getting selection size...

					var ts = TaskScheduler.FromCurrentSynchronizationContext();					
					Task.Factory.StartNew( () => this.GetInfo( selection, ts ) );
				}
			}
			catch
			{
				this.shellBrowser.SetStatusTextSB( Localizer.StatusInfoMenu[4] );	//Failed.
			}
		}

		/// <summary>
		/// Callback method, calculates selection size.
		/// This runs in Thread Pool.
		/// </summary>
		/// <param name="paths"></param>
		private void GetInfo( IList<string> paths, TaskScheduler ts )
		{
			this.nThread++;
			bool fSelectedOne = paths.Count == 1;
			SelectionData selectionData = new SelectionData();

			foreach( var path in paths )
			{
				try
				{
					if( String.IsNullOrEmpty( path ) || path.Length == 3 || ( !InfoKind.HasFlag( StatusInfoKind.Network ) && path.StartsWith( @"\\" ) ) )
					{
						continue;
					}

					if( this.CancellationPending )
					{
						break;
					}

					DirectoryInfo di = new DirectoryInfo( path );
					if( di.Exists )
					{
						selectionData.FolderCount++;

						if( InfoKind.HasFlag( StatusInfoKind.Size ) && !InfoKind.HasFlag( StatusInfoKind.NoFolderSize ) )
						{
							selectionData.TotalSize += GetDirectorySize( di );
						}

						if( InfoKind.HasFlag( StatusInfoKind.Modified ) )
						{
							DateTime dt = di.LastWriteTime;

							if( selectionData.ModifiedDateTime < dt )
							{
								selectionData.ModifiedDateTime = dt;
							}

							if( selectionData.ModifiedDateTimeMinimum > dt )
							{
								selectionData.ModifiedDateTimeMinimum = dt;
							}
						}

						if( InfoKind.HasFlag( StatusInfoKind.Created ) )
						{
							DateTime dt = di.CreationTime;

							if( selectionData.CreationDateTime < dt )
							{
								selectionData.CreationDateTime = dt;
							}

							if( selectionData.CreationDateTimeMinimum > dt )
							{
								selectionData.CreationDateTimeMinimum = dt;
							}
						}

						if( fSelectedOne && InfoKind.HasFlag( StatusInfoKind.Attributes ) )
						{
							selectionData.Attributes = di.Attributes;
						}
					}
					else
					{
						FileInfo fi = new FileInfo( path );
						if( fi.Exists )
						{
							selectionData.FileCount++;

							if( InfoKind.HasFlag( StatusInfoKind.Size ) )
							{
								selectionData.TotalSize += fi.Length;
							}

							if( InfoKind.HasFlag( StatusInfoKind.Modified ) )
							{
								DateTime dt = fi.LastWriteTime;

								if( selectionData.ModifiedDateTime < dt )
								{
									selectionData.ModifiedDateTime = dt;
								}

								if( selectionData.ModifiedDateTimeMinimum > dt )
								{
									selectionData.ModifiedDateTimeMinimum = dt;
								}
							}

							if( InfoKind.HasFlag( StatusInfoKind.Created ) )
							{
								DateTime dt = fi.CreationTime;

								if( selectionData.CreationDateTime < dt )
								{
									selectionData.CreationDateTime = dt;
								}

								if( selectionData.CreationDateTimeMinimum > dt )
								{
									selectionData.CreationDateTimeMinimum = dt;
								}
							}

							if( fSelectedOne && InfoKind.HasFlag( StatusInfoKind.Attributes ) )
							{
								selectionData.Attributes = fi.Attributes;
							}
						}
					}
				}
				catch
				{
				}
			}

			if( !this.CancellationPending && !this.fClosed )
			{
				Task.Factory.StartNew( () =>
				{
					try
					{
						if( !this.CancellationPending && !this.fClosed )
						{
							this.ShowResult( selectionData );
						}						
					}
					catch
					{
					}
				}, CancellationToken.None, TaskCreationOptions.None, ts );
			}
			this.nThread--;
		}

		/// <summary>
		/// Callback from thread pool.
		/// This runs in Explorer thread.
		/// </summary>
		private void ShowResult( SelectionData selectionData )
		{
			if( selectionData != null )
			{
				string str = String.Empty;
				if( !this.CancellationPending )
				{
					bool fSelectedOne = ( selectionData.FolderCount + selectionData.FileCount ) == 1;
					bool fSelectionItemCountDisplayed = false;

					// Size
					if( InfoKind.HasFlag( StatusInfoKind.Size ) && selectionData.TotalSize > -1 )
					{
						if( selectionData.FolderCount > 0 )
						{
							str += selectionData.FolderCount + Localizer.StatusInfoMenu[0] + MakeS( selectionData.FolderCount ) + ", ";
						}

						if( selectionData.FileCount > 0 )
						{
							str += selectionData.FileCount + Localizer.StatusInfoMenu[1] + MakeS( selectionData.FileCount ) + ", ";
						}

						str += ShellAPI.StrFormatByteSize( selectionData.TotalSize );
						if( InfoKind.HasFlag( StatusInfoKind.SizeInByte ) && selectionData.TotalSize >= 1024 )
						{
							str += " (" + selectionData.TotalSize + Localizer.StatusInfoMenu[2] + MakeS( selectionData.TotalSize ) + ")";
						}

						fSelectionItemCountDisplayed = true;
					}

					if( InfoKind.HasFlag( StatusInfoKind.Modified ) )
					{
						if( str.Length > 0 )
						{
							str += ",  ";
						}

						str += PropertyDisplayNames[1] + ": " + ( fSelectedOne ? String.Empty : selectionData.ModifiedDateTimeMinimum.ToString() + " - " ) + selectionData.ModifiedDateTime.ToString();
					}

					if( InfoKind.HasFlag( StatusInfoKind.Created ) )
					{
						if( str.Length > 0 )
						{
							str += ",  ";
						}

						str += PropertyDisplayNames[2] + ": " + ( fSelectedOne ? String.Empty : selectionData.CreationDateTimeMinimum.ToString() + " - " ) + selectionData.CreationDateTime.ToString();
					}

					if( InfoKind.HasFlag( StatusInfoKind.Attributes ) && fSelectedOne )
					{
						if( str.Length > 0 )
						{
							str += ",  ";
						}

						str += PropertyDisplayNames[3] + ": " + selectionData.Attributes.ToString();
					}

					if( InfoKind.HasFlag( StatusInfoKind.DriveInfo ) )
					{
						try
						{
							string strDrive = this.pluginServer.SelectedTab.Address.Path;
							if( !strDrive.StartsWith( "::" ) )
							{
								DriveInfo di = new DriveInfo( strDrive );
								if( di.IsReady )
								{
									if( str.Length > 0 )
									{
										str += ", ";
									}

									str += ShellAPI.StrFormatByteSize( di.TotalFreeSpace ) + "/" + ShellAPI.StrFormatByteSize( di.TotalSize ) + " (" + di.Name + ")";
								}
							}
						}
						catch
						{
						}
					}

					if( !fSelectionItemCountDisplayed && !fSelectedOne )
					{
						int c = selectionData.FolderCount + selectionData.FileCount;
						str = String.Format( Localizer.ItemString[2], c ) + ", " + str;
					}
				}

				this.shellBrowser.SetStatusTextSB( str );
			}
		}

		/// <summary>
		/// if drives are selected, show drive info.
		/// </summary>
		/// <param name="address">Paths of drives.</param>
		private void SetDriveInfoText( IList<string> paths )
		{
			try
			{
				long lTotalFreeSpace = 0;
				long lTotalSize = 0;
				int cDrive = 0;

				DateTime dtMod = DateTime.MinValue;
				DateTime dtCrt = DateTime.MinValue;
				FileAttributes fa = 0;

				bool fSelectedOne = paths.Count == 1;

				foreach( var path in paths )
				{
					if( !String.Equals( path, @"a:\", StringComparison.OrdinalIgnoreCase ) &&
						!String.Equals( path, @"b:\", StringComparison.OrdinalIgnoreCase ) &&
						( InfoKind.HasFlag( StatusInfoKind.Network ) || !path.StartsWith( @"\\" ) ) )
					{
						DriveInfo dr = new DriveInfo( path );
						if( dr.IsReady )
						{
							cDrive++;

							if( InfoKind.HasFlag( StatusInfoKind.Size ) )
							{
								lTotalFreeSpace += dr.TotalFreeSpace;
								lTotalSize += dr.TotalSize;
							}

							if( fSelectedOne )
							{
								if( InfoKind.HasFlag( StatusInfoKind.Modified ) || InfoKind.HasFlag( StatusInfoKind.Created ) || InfoKind.HasFlag( StatusInfoKind.Attributes ) )
								{
									DirectoryInfo di = new DirectoryInfo( path );
									if( di.Exists )
									{
										if( InfoKind.HasFlag( StatusInfoKind.Modified ) )
										{
											dtMod = di.LastWriteTime;
										}

										if( InfoKind.HasFlag( StatusInfoKind.Created ) )
										{
											dtCrt = di.CreationTime;
										}

										if( InfoKind.HasFlag( StatusInfoKind.Attributes ) )
										{
											fa = di.Attributes;
										}
									}
								}
							}
						}
					}
				}

				if( cDrive > 1 )
				{
					this.shellBrowser.SetStatusTextSB( String.Format( Localizer.ItemString[0], cDrive ) + ", " +
													   Localizer.ItemString[4] + ": " + ShellAPI.StrFormatByteSize( lTotalFreeSpace ) + " " + Localizer.ItemString[3] + ShellAPI.StrFormatByteSize( lTotalSize ) );
				}
				else if( cDrive == 1 )
				{
					string strInfo = String.Empty;

					if( InfoKind.HasFlag( StatusInfoKind.Size ) )
					{
						strInfo += Localizer.ItemString[4] + ": " + ShellAPI.StrFormatByteSize( lTotalFreeSpace ) + " " + Localizer.ItemString[3] + ShellAPI.StrFormatByteSize( lTotalSize );
					}

					if( InfoKind.HasFlag( StatusInfoKind.Modified ) )
					{
						if( strInfo.Length > 0 )
						{
							strInfo += ",  ";
						}
						strInfo += PropertyDisplayNames[1] + ": " + dtMod.ToString();
					}

					if( InfoKind.HasFlag( StatusInfoKind.Created ) )
					{
						if( strInfo.Length > 0 )
						{
							strInfo += ",  ";
						}
						strInfo += PropertyDisplayNames[2] + ": " + dtCrt.ToString();
					}

					if( InfoKind.HasFlag( StatusInfoKind.Attributes ) )
					{
						if( strInfo.Length > 0 )
						{
							strInfo += ",  ";
						}
						strInfo += PropertyDisplayNames[3] + ": " + fa.ToString();
					}

					if( strInfo.Length > 0 )
					{
						this.shellBrowser.SetStatusTextSB( strInfo );
					}
				}
			}
			catch
			{
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private void SetCurrentDriveFolderInfoText( bool fNavigationComplete, QTPlugin.View view )
		{
			uint SVGIO_SELECTION = 0x00000001;
			uint SVGIO_ALLVIEW	 = 0x00000002;
			IShellView shellView = null;
			try
			{
				var tab = this.pluginServer.GetSelectedTab( view );
				if( tab != null )
				{
					string path = tab.Address.Path;
					if( !path.StartsWith( "::" ) )
					{
						if( W32.S_OK == this.shellBrowser.QueryActiveShellView( out shellView ) )
						{
							var folderView = shellView as IFolderView;
							if( folderView != null )
							{
								int c;
								if( fNavigationComplete && W32.S_OK == folderView.ItemCount( SVGIO_SELECTION, out c ) && c > 0 )
								{
									return;
								}

								string str = String.Empty;
								if( InfoKind.HasFlag( StatusInfoKind.FolderInfo ) )
								{
									try
									{
										DirectoryInfo di = new DirectoryInfo( path );
										long l = 0;
										foreach( var item in di.EnumerateFiles() )
										{
											l += item.Length;
										}
										str += ", " + ShellAPI.StrFormatByteSize( l );
										if( InfoKind.HasFlag( StatusInfoKind.SizeInByte ) && l >= 1024 )
										{
											str += " (" + l + Localizer.StatusInfoMenu[2] + MakeS( l ) + ")";
										}
									}
									catch
									{
									}
								}
								if( InfoKind.HasFlag( StatusInfoKind.DriveInfo ) )
								{
									try
									{
										DriveInfo di = new DriveInfo( path );
										if( di.IsReady )
										{
											str += ", " + Localizer.ItemString[4] + " " + ShellAPI.StrFormatByteSize( di.TotalFreeSpace ) + "/" + ShellAPI.StrFormatByteSize( di.TotalSize ) + " (" + di.Name + ")";
										}
									}
									catch
									{
									}
								}

								if( !String.IsNullOrEmpty( str ) )
								{
									folderView.ItemCount( SVGIO_ALLVIEW, out c );
									this.shellBrowser.SetStatusTextSB( String.Format( Localizer.ItemString[c > 1 ? 0 : 1], c ) + str );
								}
							}
						}
					}
				}
			}
			catch
			{
			}
			finally
			{
				if( shellView != null )
				{
					Marshal.ReleaseComObject( shellView );
				}
			}
		}

		/// <summary>
		/// Get selection size recursively.
		/// </summary>
		/// <param name="di"></param>
		/// <returns></returns>
		private long GetDirectorySize( DirectoryInfo di )
		{
			if( this.CancellationPending )
			{
				return -1;
			}

			long lSize = 0;

			try
			{
				foreach( DirectoryInfo diSub in di.EnumerateDirectories() )//di.GetDirectories() )
				{
					if( this.CancellationPending )
					{
						return -1;
					}

					try
					{
						lSize += GetDirectorySize( diSub );
					}
					catch
					{
						// access error, etc.
					}
				}

				foreach( FileInfo fiSub in di.EnumerateFiles() ) //di.GetFiles() )
				{
					if( this.CancellationPending )
					{
						return -1;
					}

					try
					{
						lSize += fiSub.Length;
					}
					catch
					{
						// access error, etc.
					}
				}
			}
			catch
			{
				// enumeration error, etc.
				lSize = -1;
			}

			return lSize;
		}

		private static string MakeS( long c )
		{
			return ( !IsJa && c > 1 ) ? "s" : String.Empty;
		}

		sealed class SelectionData
		{
			public int FolderCount;
			public int FileCount;
			public long TotalSize;

			public DateTime ModifiedDateTime;
			public DateTime ModifiedDateTimeMinimum = DateTime.MaxValue;
			public DateTime CreationDateTime;
			public DateTime CreationDateTimeMinimum = DateTime.MaxValue;

			public FileAttributes Attributes;
		}

		#region Settings

		internal static void SaveSettings()
		{
			// shit in order to be compatible

			byte b = 0, b2 = 0;
			if( InfoKind.HasFlag( StatusInfoKind.Size ) )
			{
				b |= 0x80;
			}
			if( InfoKind.HasFlag( StatusInfoKind.Modified ) )
			{
				b |= 0x40;
			}
			if( InfoKind.HasFlag( StatusInfoKind.Created ) )
			{
				b |= 0x20;
			}
			if( InfoKind.HasFlag( StatusInfoKind.Attributes ) )
			{
				b |= 0x10;
			}
			if( InfoKind.HasFlag( StatusInfoKind.DriveInfo ) )
			{
				b |= 0x08;
			}
			if( InfoKind.HasFlag( StatusInfoKind.SizeInByte ) )
			{
				b |= 0x04;
			}
			if( InfoKind.HasFlag( StatusInfoKind.Network ) )
			{
				b |= 0x02;
			}
			if( InfoKind.HasFlag( StatusInfoKind.NoFolderSize ) )
			{
				b |= 0x01;
			}
			if( InfoKind.HasFlag( StatusInfoKind.FolderInfo ) )
			{
				b2 |= 0x1;
			}


			using( RegistryKey rk = Registry.CurrentUser.CreateSubKey( CONSTANTS.REGISTRY_PLUGINSETTINGS + @"\Quizo\SelectionInfoToStatusBar" ) )
			{
				rk.SetValue( "Config", new byte[] { b, b2, 0, 0 } );
			}
		}

		internal static StatusInfoKind ReadSettings()
		{
			InfoKind = 0;

			using( RegistryKey rk = Registry.CurrentUser.OpenSubKey( CONSTANTS.REGISTRY_PLUGINSETTINGS + @"\Quizo\SelectionInfoToStatusBar" ) )
			{
				if( rk != null )
				{
					byte[] bytes = rk.GetValue( "Config" ) as byte[];
					if( bytes != null && bytes.Length > 1 )
					{
						if( ( bytes[0] & 0x80 ) != 0 )
						{
							InfoKind |= StatusInfoKind.Size;
						}
						if( ( bytes[0] & 0x40 ) != 0 )
						{
							InfoKind |= StatusInfoKind.Modified;
						}
						if( ( bytes[0] & 0x20 ) != 0 )
						{
							InfoKind |= StatusInfoKind.Created;
						}
						if( ( bytes[0] & 0x10 ) != 0 )
						{
							InfoKind |= StatusInfoKind.Attributes;
						}
						if( ( bytes[0] & 0x08 ) != 0 )
						{
							InfoKind |= StatusInfoKind.DriveInfo;
						}
						if( ( bytes[0] & 0x04 ) != 0 )
						{
							InfoKind |= StatusInfoKind.SizeInByte;
						}
						if( ( bytes[0] & 0x02 ) != 0 )
						{
							InfoKind |= StatusInfoKind.Network;
						}
						if( ( bytes[0] & 0x01 ) != 0 )
						{
							InfoKind |= StatusInfoKind.NoFolderSize;
						}
						if( ( bytes[1] & 0x01 ) != 0 )
						{
							InfoKind |= StatusInfoKind.FolderInfo;
						}
					}
				}
			}

			return InfoKind;
		}

		public static void Uninstall()
		{
			try
			{
				using( RegistryKey rk = Registry.CurrentUser.OpenSubKey( CONSTANTS.REGISTRY_PLUGINSETTINGS + @"\Quizo", true ) )
				{
					if( rk != null )
					{
						rk.DeleteSubKeyTree( "SelectionInfoToStatusBar" );
					}
				}
			}
			catch
			{
			}
		}

		#endregion
	}

	[Plugin( PluginType.Background, typeof( Localizer ), 3, Version = "1.3.0.0" )]
	public class EditAttributes : IBarDropButton
	{
		// 1.3.0.0		plugin lib 1.3.0.0, supports extra views.

		private IPluginServer pluginServer;
		private Localizer localizer;

		#region IPluginClient members

		public void Open( IPluginServer pluginServer, IShellBrowser shellBrowser )
		{
			this.pluginServer = pluginServer;
			this.pluginServer.ViewEventSource.SelectionChanged += new PluginEventHandler( pluginServer_SelectionChanged );
			this.pluginServer.ViewEventSource.NavigationComplete += new PluginEventHandler( pluginServer_NavigationComplete );
			this.pluginServer.ViewEventSource.ActiveViewChanged += new PluginEventHandler( ViewEventSource_ActiveViewChanged );
			this.pluginServer.SettingsChanged += new PluginEventHandler( pluginServer_SettingsChanged );
			this.pluginServer.ShortcutKeyPressed += new PluginKeyEventHandler( pluginServer_ShortcutKeyPressed );

			this.localizer = new Localizer();
			this.localizer.SetKey( 3 );
		}

		public bool QueryShortcutKeys( out string[] actions )
		{
			actions = this.localizer.KeyShortcutStr;
			return true;
		}

		public void Close( EndCode endCode )
		{			
		}

		public bool HasOption
		{
			get
			{
				return false;
			}
		}

		public void OnMenuItemClick( MenuType menuType, string menuText, ITab tab )
		{
		}

		public void OnOption()
		{
		}

		public void OnShortcutKeyPressed( int index )
		{
		}


		#endregion

		#region IBarButton members

		public Image GetImage( bool fLarge )
		{
			return fLarge ? Resource.EditAttributes_large : Resource.EditAttributes_small;
		}

		public void InitializeItem()
		{
		}

		public void OnButtonClick()
		{			
		}

		public bool ShowTextLabel
		{
			get
			{
				return false;
			}
		}

		public string Text
		{
			get
			{
				return this.localizer.Name;
			}
		}

		#endregion

		#region IBarDropButton members

		public bool IsSplitButton
		{
			get
			{
				return false;
			}
		}

		public void OnDropDownOpening( ToolStripDropDownMenu menu )
		{
			if( menu.Items.Count == 0 )
			{
				menu.SuspendLayout();
				for( int i = 0; i < this.localizer.KeyShortcutStr.Length; i++ )
				{
					var tsmi = new ToolStripMenuItem( this.localizer.KeyShortcutStr[i] );
					tsmi.Tag = i;
					menu.Items.Add( tsmi );

					if( i == 2 || i == 5 )
					{
						var tss = new ToolStripSeparator();
						tss.Enabled = false;
						menu.Items.Add( tss );
					}
				}
				menu.ResumeLayout();
			}
		}

		public void OnDropDownItemClick( ToolStripItem item, MouseButtons mouseButton )
		{
			if( item is ToolStripMenuItem )
			{
				this.EditSelectedItems( (int)item.Tag );
			}
		}

		#endregion
		
		private void pluginServer_NavigationComplete( object sender, PluginEventArgs e )
		{
			this.UpdateButton();
		}

		private void pluginServer_SelectionChanged( object sender, PluginEventArgs e )
		{
			this.UpdateButton();
		}

		private void pluginServer_SettingsChanged( object sender, PluginEventArgs e )
		{
			this.UpdateButton();
		}

		private void pluginServer_ShortcutKeyPressed( object sender, PluginKeyEventArgs e )
		{
			if( !e.Repeat )
			{
				this.EditSelectedItems( e.Index );
			}
		}

		private void ViewEventSource_ActiveViewChanged( object sender, PluginEventArgs e )
		{
			this.UpdateButton();
		}


		private void EditSelectedItems( int index )
		{
			var view = this.pluginServer.FocusedView;
			if( view == QTPlugin.View.None )
			{
				view = QTPlugin.View.Default;
			}
			var tab = this.pluginServer.GetSelectedTab( view );
			if( tab == null )
			{
				return;
			}

			var sel = tab.SelectedPaths;
			if( sel != null && sel.Count > 0 )
			{
				foreach( var path in sel )
				{
					try
					{
						if( !String.IsNullOrEmpty( path ) )
						{
							FileSystemInfo fsi = null;
							FileInfo fi = new FileInfo( path );
							if( fi.Exists )
							{
								fsi = fi;
							}
							else
							{
								DirectoryInfo di = new DirectoryInfo( path );
								if( di.Exists )
								{
									fsi = di;
								}
							}

							if( fsi != null )
							{
								switch( index )
								{
									case 0:
										// toggle sys
										fsi.Attributes = ( fsi.Attributes & FileAttributes.System ) != 0 ? fsi.Attributes & ~FileAttributes.System : fsi.Attributes | FileAttributes.System;
										break;
									case 1:
										// add sys
										fsi.Attributes |= FileAttributes.System;
										break;
									case 2:
										// remove sys
										fsi.Attributes &= ~FileAttributes.System;
										break;
									case 3:
										// toggle read-only
										fsi.Attributes = ( fsi.Attributes & FileAttributes.ReadOnly ) != 0 ? fsi.Attributes & ~FileAttributes.ReadOnly : fsi.Attributes | FileAttributes.ReadOnly;
										break;
									case 4:
										// add ro
										fsi.Attributes |= FileAttributes.ReadOnly;
										break;
									case 5:
										// remove ro
										fsi.Attributes &= ~FileAttributes.ReadOnly;
										break;
									case 6:
										// toggle h
										fsi.Attributes = ( fsi.Attributes & FileAttributes.Hidden ) != 0 ? fsi.Attributes & ~FileAttributes.Hidden : fsi.Attributes | FileAttributes.Hidden;
										break;
									case 7:
										// add h
										fsi.Attributes |= FileAttributes.Hidden;
										break;
									case 8:
										// remove h
										fsi.Attributes &= ~FileAttributes.Hidden;
										break;
								}
							}
						}
					}
					catch
					{
					}
				}
			}
		}

		private void UpdateButton()
		{
			var view = this.pluginServer.FocusedView;
			if( view == QTPlugin.View.None )
			{
				view = QTPlugin.View.Default;
			}
			var tab = this.pluginServer.GetSelectedTab( view );

			var sel = tab == null ? null : tab.SelectedIDLs;
			var fSel = sel != null && sel.Count > 0;
			this.pluginServer.UpdateItem( this, fSel, false );
		}
	}
	

	[Flags]
	enum StatusInfoKind
	{
		Size		 = 0x0001,
		Modified	 = 0x0002,
		Created		 = 0x0004,
		Attributes	 = 0x0008,
		DriveInfo	 = 0x0010,
		Network		 = 0x0020,
		NoFolderSize = 0x0040,
		FolderInfo	 = 0x0080,
		SizeInByte	 = 0x10000000,
	}
}
