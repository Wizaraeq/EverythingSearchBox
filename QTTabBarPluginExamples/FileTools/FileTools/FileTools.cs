using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using QTPlugin;
using QTPlugin.Interop;

namespace QuizoPlugins
{
	/// <summary>
	/// Cut button
	/// </summary>
	[Plugin( PluginType.Interactive, typeof( Localizer ), 0, Version = "1.3.0.0" )]
	public class CutButton : IBarButton
	{
		// 1.3.0.0		supports views.

		private IPluginServer pluginServer;
		private string[] ResStr;
		
		#region IPluginClient Members

		public void Open( IPluginServer pluginServer, IShellBrowser shellBrowser )
		{
			this.pluginServer = pluginServer;
			this.ResStr = new string[] { StringResources.ButtonNames[0] };

			this.pluginServer.ViewEventSource.SelectionChanged += new PluginEventHandler( pluginServer_SelectionChanged );
			this.pluginServer.ViewEventSource.NavigationComplete += new PluginEventHandler( pluginServer_NavigationComplete );
			this.pluginServer.ViewEventSource.ActiveViewChanged += new PluginEventHandler( ViewEventSource_ActiveViewChanged );
			this.pluginServer.SettingsChanged += new PluginEventHandler( pluginServer_SettingsChanged );
		}

		public bool QueryShortcutKeys( out string[] actions )
		{
			actions = null;
			return false;
		}

		public void Close( EndCode code )
		{
			this.pluginServer = null;
		}

		public void OnShortcutKeyPressed( int index )
		{
		}

		public void OnMenuItemClick( MenuType menuType, string menuText, ITab tab )
		{
		}

		public bool HasOption
		{
			get
			{
				return false;
			}
		}

		public void OnOption()
		{
		}

		#endregion

		#region IBarButton Members

		public void InitializeItem()
		{
		}

		public Image GetImage( bool fLarge )
		{
			return fLarge ? Resource.CutButton_large : Resource.CutButton_small;
		}

		public void OnButtonClick()
		{
			var view = this.pluginServer.FocusedView;
			if( view == QTPlugin.View.None )
			{
				view = QTPlugin.View.Default;
			}

			this.pluginServer.InvokeCommand( QCommand.Cut, (int)view );
		}

		public bool ShowTextLabel
		{
			get
			{
				return true;
			}
		}

		public string Text
		{
			get
			{
				return this.ResStr[0];
			}
		}

		#endregion

		private void pluginServer_NavigationComplete( object sender, PluginEventArgs e )
		{
			this.Update();
		}

		private void pluginServer_SelectionChanged( object sender, PluginEventArgs e )
		{
			this.Update();
		}

		private void pluginServer_SettingsChanged( object sender, PluginEventArgs e )
		{
			this.Update();
		}

		private void ViewEventSource_ActiveViewChanged( object sender, PluginEventArgs e )
		{
			this.Update();
		}

		private void Update()
		{
			Helper.EnableIfFocusedViewHasSelection( this.pluginServer, this );
		}
	}

	/// <summary>
	/// Copy button
	/// </summary>
	[Plugin( PluginType.Interactive, typeof( Localizer ), 1, Version = "1.3.0.0" )]
	public class CopyButton : IBarButton
	{
		// 1.3.0.0		supports views.

		private IPluginServer pluginServer;
		private string[] ResStr;

		#region IPluginClient Members

		public void Open( IPluginServer pluginServer, IShellBrowser shellBrowser )
		{
			this.pluginServer = pluginServer;
			this.ResStr = new string[] { StringResources.ButtonNames[1] };

			this.pluginServer.ViewEventSource.NavigationComplete += new PluginEventHandler( pluginServer_NavigationComplete );
			this.pluginServer.ViewEventSource.SelectionChanged += new PluginEventHandler( pluginServer_SelectionChanged );
			this.pluginServer.ViewEventSource.ActiveViewChanged += new PluginEventHandler( ViewEventSource_ActiveViewChanged );
			this.pluginServer.SettingsChanged += new PluginEventHandler( pluginServer_SettingsChanged );
		}

		public bool QueryShortcutKeys( out string[] actions )
		{
			actions = null;
			return false;
		}

		public void Close( EndCode code )
		{
			this.pluginServer = null;
		}

		public void OnShortcutKeyPressed( int index )
		{
		}

		public void OnMenuItemClick( MenuType menuType, string menuText, ITab tab )
		{
		}

		public bool HasOption
		{
			get
			{
				return false;
			}
		}

		public void OnOption()
		{
		}

		#endregion

		#region IBarButton Members

		public void InitializeItem()
		{
		}

		public Image GetImage( bool fLarge )
		{
			return fLarge ? Resource.CopyButton_large : Resource.CopyButton_small;
		}

		public void OnButtonClick()
		{
			var view = this.pluginServer.FocusedView;
			if( view == QTPlugin.View.None )
			{
				view = QTPlugin.View.Default;
			}

			this.pluginServer.InvokeCommand( QCommand.Copy, (int)view );
		}

		public bool ShowTextLabel
		{
			get
			{
				return true;
			}
		}

		public string Text
		{
			get
			{
				return this.ResStr[0];
			}
		}

		#endregion

		private void pluginServer_NavigationComplete( object sender, PluginEventArgs e )
		{
			this.Update();
		}

		private void pluginServer_SelectionChanged( object sender, PluginEventArgs e )
		{
			this.Update();
		}

		private void pluginServer_SettingsChanged( object sender, PluginEventArgs e )
		{
			this.Update();
		}

		private void ViewEventSource_ActiveViewChanged( object sender, PluginEventArgs e )
		{
			this.Update();
		}

		private void Update()
		{
			Helper.EnableIfFocusedViewHasSelection( this.pluginServer, this );
		}
	}

	/// <summary>
	/// Paste Button
	/// </summary>
	[Plugin( PluginType.Interactive, typeof( Localizer ), 2, Version = "1.3.0.0" )]
	public class PasteButton : IBarButton
	{
		// 1.3.0.0		supports views.

		private IPluginServer pluginServer;
		private string[] ResStr;

		#region IPluginClient Members

		public void Open( IPluginServer pluginServer, IShellBrowser shellBrowser )
		{
			this.pluginServer = pluginServer;
			this.ResStr = new string[] { StringResources.ButtonNames[2] };
		}

		public bool QueryShortcutKeys( out string[] actions )
		{
			actions = null;
			return false;
		}

		public void Close( EndCode code )
		{
			this.pluginServer = null;
		}

		public void OnShortcutKeyPressed( int index )
		{			
		}

		public void OnMenuItemClick( MenuType menuType, string menuText, ITab tab )
		{
			
		}

		public bool HasOption
		{
			get
			{
				return false;
			}
		}

		public void OnOption()
		{			
		}

		#endregion

		#region IBarButton Members

		public void InitializeItem()
		{
		}

		public Image GetImage( bool fLarge )
		{
			return fLarge ? Resource.PasteButton_large : Resource.PasteButton_small;
		}

		public void OnButtonClick()
		{
			bool fFilesInClipboard = false;
			try
			{
				fFilesInClipboard = Clipboard.ContainsFileDropList();
			}
			catch
			{
			}

			if( fFilesInClipboard )
			{
				var view = this.pluginServer.FocusedView;
				if( view == QTPlugin.View.None )
				{
					view = QTPlugin.View.Default;
				}

				this.pluginServer.InvokeCommand( QCommand.Paste, (int)view );
			}
			else
			{
				System.Media.SystemSounds.Beep.Play();
			}
		}

		public bool ShowTextLabel
		{
			get
			{
				return true;
			}
		}

		public string Text
		{
			get
			{
				return this.ResStr[0];
			}
		}

		#endregion
	}

	/// <summary>
	/// Delete button
	/// </summary>
	[Plugin( PluginType.Interactive, typeof( Localizer ), 3, Version = "1.3.0.0" )]
	public class DeleteButton : IBarButton
	{
		// 1.3.0.0		supports views.

		private IPluginServer pluginServer;
		private string[] ResStr;

		#region IPluginClient Members

		public void Open( IPluginServer pluginServer, IShellBrowser shellBrowser )
		{
			this.pluginServer = pluginServer;
			this.ResStr = new string[] { StringResources.ButtonNames[3] };

			this.pluginServer.ViewEventSource.NavigationComplete += new PluginEventHandler( pluginServer_NavigationComplete );
			this.pluginServer.ViewEventSource.SelectionChanged += new PluginEventHandler( pluginServer_SelectionChanged );
			this.pluginServer.ViewEventSource.ActiveViewChanged += new PluginEventHandler( ViewEventSource_ActiveViewChanged );
			this.pluginServer.SettingsChanged += new PluginEventHandler( pluginServer_SettingsChanged );
		}

		public bool QueryShortcutKeys( out string[] actions )
		{
			actions = null;
			return false;
		}

		public void Close( EndCode code )
		{
			this.pluginServer = null;
		}

		public void OnShortcutKeyPressed( int index )
		{
		}

		public void OnMenuItemClick( MenuType menuType, string menuText, ITab tab )
		{
		}

		public bool HasOption
		{
			get
			{
				return false;
			}
		}

		public void OnOption()
		{
		}

		#endregion


		#region IBarButton Members

		public void InitializeItem()
		{
		}

		public Image GetImage( bool fLarge )
		{
			return fLarge ? Resource.DeleteButton_large : Resource.DeleteButton_small;
		}

		public void OnButtonClick()
		{
			var view = this.pluginServer.FocusedView;
			if( view == QTPlugin.View.None )
			{
				view = QTPlugin.View.Default;
			}

			this.pluginServer.InvokeCommand( QCommand.Delete, (int)view );
		}

		public bool ShowTextLabel
		{
			get
			{
				return true;
			}
		}

		public string Text
		{
			get
			{
				return this.ResStr[0];
			}
		}

		#endregion


		private void pluginServer_NavigationComplete( object sender, PluginEventArgs e )
		{
			this.Update();
		}

		private void pluginServer_SelectionChanged( object sender, PluginEventArgs e )
		{
			this.Update();
		}

		private void pluginServer_SettingsChanged( object sender, PluginEventArgs e )
		{
			this.Update();
		}

		private void ViewEventSource_ActiveViewChanged( object sender, PluginEventArgs e )
		{
			this.Update();
		}

		private void Update()
		{
			Helper.EnableIfFocusedViewHasSelection( this.pluginServer, this );
		}
	}

	/// <summary>
	/// CopyTo button
	/// </summary>
	[Plugin( PluginType.Interactive, typeof( Localizer ), 4, Version = "1.3.0.0" )]
	public class CopyToButton : IBarButton
	{
		// 1.3.0.0		supports views.

		private IPluginServer pluginServer;
		private string[] ResStr;

		#region IPluginClient Members

		public void Open( IPluginServer pluginServer, IShellBrowser shellBrowser )
		{
			this.pluginServer = pluginServer;
			this.ResStr = new string[] { StringResources.ButtonNames[4] };

			this.pluginServer.ViewEventSource.SelectionChanged += new PluginEventHandler( pluginServer_SelectionChanged );
			this.pluginServer.ViewEventSource.NavigationComplete += new PluginEventHandler( pluginServer_NavigationComplete );
			this.pluginServer.ViewEventSource.ActiveViewChanged += new PluginEventHandler( ViewEventSource_ActiveViewChanged );
			this.pluginServer.SettingsChanged += new PluginEventHandler( pluginServer_SettingsChanged );
		}

		public bool QueryShortcutKeys( out string[] actions )
		{
			actions = new string[] { this.ResStr[0] };
			return true;
		}

		public void Close( EndCode code )
		{
			this.pluginServer = null;
		}

		public void OnShortcutKeyPressed( int index )
		{
			this.OnButtonClick();
		}

		public void OnMenuItemClick( MenuType menuType, string menuText, ITab tab )
		{
		}

		public bool HasOption
		{
			get
			{
				return false;
			}
		}

		public void OnOption()
		{
		}

		#endregion


		#region IBarButton Members

		public void InitializeItem()
		{
		}

		public Image GetImage( bool fLarge )
		{
			return fLarge ? Resource.CopyToButton_large : Resource.CopyToButton_small;
		}

		public void OnButtonClick()
		{
			var view = this.pluginServer.FocusedView;
			if( view == QTPlugin.View.None )
			{
				view = QTPlugin.View.Default;
			}

			this.pluginServer.InvokeCommand( QCommand.CopyToFolder, (int)view );
		}

		public bool ShowTextLabel
		{
			get
			{
				return true;
			}
		}

		public string Text
		{
			get
			{
				return this.ResStr[0];
			}
		}

		#endregion


		private void pluginServer_NavigationComplete( object sender, PluginEventArgs e )
		{
			this.Update();
		}

		private void pluginServer_SelectionChanged( object sender, PluginEventArgs e )
		{
			this.Update();
		}

		private void pluginServer_SettingsChanged( object sender, PluginEventArgs e )
		{
			this.Update();
		}

		private void ViewEventSource_ActiveViewChanged( object sender, PluginEventArgs e )
		{
			this.Update();
		}

		private void Update()
		{
			Helper.EnableIfFocusedViewHasSelection( this.pluginServer, this );
		}
	}

	/// <summary>
	/// MoveTo button
	/// </summary>
	[Plugin( PluginType.Interactive, typeof( Localizer ), 5, Version = "1.3.0.0" )]
	public class MoveToButton : IBarButton
	{
		// 1.3.0.0		supports views.

		private IPluginServer pluginServer;
		private string[] ResStr;

		#region IPluginClient Members

		public void Open( IPluginServer pluginServer, IShellBrowser shellBrowser )
		{
			this.pluginServer = pluginServer;
			this.ResStr = new string[] { StringResources.ButtonNames[5] };

			this.pluginServer.ViewEventSource.SelectionChanged += new PluginEventHandler( pluginServer_SelectionChanged );
			this.pluginServer.ViewEventSource.NavigationComplete += new PluginEventHandler( pluginServer_NavigationComplete );
			this.pluginServer.ViewEventSource.ActiveViewChanged += new PluginEventHandler( ViewEventSource_ActiveViewChanged );
			this.pluginServer.SettingsChanged += new PluginEventHandler( pluginServer_SettingsChanged );
		}

		public bool QueryShortcutKeys( out string[] actions )
		{
			actions = new string[] { this.ResStr[0] };
			return true;
		}

		public void Close( EndCode code )
		{
			this.pluginServer = null;
		}

		public void OnShortcutKeyPressed( int index )
		{
			this.OnButtonClick();
		}

		public void OnMenuItemClick( MenuType menuType, string menuText, ITab tab )
		{
		}

		public bool HasOption
		{
			get
			{
				return false;
			}
		}

		public void OnOption()
		{
		}

		#endregion

		#region IBarButton Members

		public void InitializeItem()
		{
		}

		public Image GetImage( bool fLarge )
		{
			return fLarge ? Resource.MoveToButton_large : Resource.MoveToButton_small;
		}

		public void OnButtonClick()
		{
			var view = this.pluginServer.FocusedView;
			if( view == QTPlugin.View.None )
			{
				view = QTPlugin.View.Default;
			}

			this.pluginServer.InvokeCommand( QCommand.MoveToFolder, (int)view );
		}

		public bool ShowTextLabel
		{
			get
			{
				return true;
			}
		}

		public string Text
		{
			get
			{
				return this.ResStr[0];
			}
		}

		#endregion

		private void pluginServer_NavigationComplete( object sender, PluginEventArgs e )
		{
			this.Update();
		}

		private void pluginServer_SelectionChanged( object sender, PluginEventArgs e )
		{
			this.Update();
		}

		private void pluginServer_SettingsChanged( object sender, PluginEventArgs e )
		{
			this.Update();
		}

		private void ViewEventSource_ActiveViewChanged( object sender, PluginEventArgs e )
		{
			this.Update();
		}

		private void Update()
		{
			Helper.EnableIfFocusedViewHasSelection( this.pluginServer, this );
		}
	}

	/// <summary>
	/// Undo Button
	/// </summary>
	[Plugin( PluginType.Interactive, typeof( Localizer ), 6, Version = "1.3.0.0" )]
	public class UndoButton : IBarButton
	{
		// 1.3.0.0		uses InvokeCommand instead of FileOps of its own

		private IPluginServer pluginServer;
		private string[] ResStr;

		#region IPluginClient Members

		public void Open( IPluginServer pluginServer, IShellBrowser shellBrowser )
		{
			this.pluginServer = pluginServer;
			this.ResStr = new string[] { StringResources.ButtonNames[6] };
		}

		public bool QueryShortcutKeys( out string[] actions )
		{
			actions = null;
			return false;
		}

		public void Close( EndCode code )
		{
			this.pluginServer = null;
		}

		public void OnShortcutKeyPressed( int index )
		{

		}

		public void OnMenuItemClick( MenuType menuType, string menuText, ITab tab )
		{

		}

		public bool HasOption
		{
			get
			{
				return false;
			}
		}

		public void OnOption()
		{
		}

		#endregion

		#region IBarButton Members

		public void InitializeItem()
		{
		}

		public Image GetImage( bool fLarge )
		{
			return fLarge ? Resource.UndoButton_large : Resource.UndoButton_small;
		}

		public void OnButtonClick()
		{
			this.pluginServer.InvokeCommand( QCommand.Undo );
		}

		public bool ShowTextLabel
		{
			get
			{
				return true;
			}
		}

		public string Text
		{
			get
			{
				return this.ResStr[0];
			}
		}

		#endregion
	}

	/// <summary>
	/// Send up button
	/// </summary>
	[Plugin( PluginType.Background, typeof( Localizer ), 7, Version = "1.3.0.0" )]
	public class SendToParentButton : IBarButton
	{
		// 1.3.0.0		supports views.

		private IPluginServer pluginServer;
		private string[] ResStr;

		#region IPluginClient Members

		public void Open( IPluginServer pluginServer, IShellBrowser shellBrowser )
		{
			this.pluginServer = pluginServer;
			this.ResStr = new string[] { StringResources.ButtonNames[7] };

			this.pluginServer.ViewEventSource.SelectionChanged += new PluginEventHandler( pluginServer_SelectionChanged );
			this.pluginServer.ViewEventSource.NavigationComplete += new PluginEventHandler( pluginServer_NavigationComplete );
			this.pluginServer.ViewEventSource.ActiveViewChanged += new PluginEventHandler( ViewEventSource_ActiveViewChanged );
			this.pluginServer.SettingsChanged += new PluginEventHandler( pluginServer_SettingsChanged );
		}

		public bool QueryShortcutKeys( out string[] actions )
		{
			actions = new string[] { this.ResStr[0] };
			return true;
		}

		public void Close( EndCode code )
		{
			if( code != EndCode.Hidden )
			{
				this.pluginServer = null;
			}
		}

		public void OnShortcutKeyPressed( int index )
		{
			if( !this.MoveSelectionToParent( true ) )
			{
				System.Media.SystemSounds.Beep.Play();
			}
		}

		public void OnMenuItemClick( MenuType menuType, string menuText, ITab tab )
		{
		}

		public bool HasOption
		{
			get
			{
				return false;
			}
		}

		public void OnOption()
		{
		}

		#endregion
		
		#region IBarButton Members

		public void InitializeItem()
		{
		}

		public Image GetImage( bool fLarge )
		{
			return fLarge ? Resource.SendToParentButton_large : Resource.SendToParentButton_small;
		}

		public void OnButtonClick()
		{
			if( !this.MoveSelectionToParent( false ) )
			{
				System.Media.SystemSounds.Beep.Play();
			}
		}

		public bool ShowTextLabel
		{
			get
			{
				return true;
			}
		}

		public string Text
		{
			get
			{
				return this.ResStr[0];
			}
		}

		#endregion
		
		private void pluginServer_NavigationComplete( object sender, PluginEventArgs e )
		{
			this.Update();
		}

		private void pluginServer_SelectionChanged( object sender, PluginEventArgs e )
		{
			this.Update();
		}

		private void pluginServer_SettingsChanged( object sender, PluginEventArgs e )
		{
			this.Update();
		}

		private void ViewEventSource_ActiveViewChanged( object sender, PluginEventArgs e )
		{
			this.Update();
		}

		private void Update()
		{
			try
			{
				var tab = this.pluginServer.SelectedTabInFocusedView;
				if( tab != null )
				{
					var path = tab.Path;
					if( tab.IDL.Length != 2 && path != null && path.Length > 3 && !path.StartsWith( "::" ) )
					{
						this.pluginServer.UpdateItem( this, tab.SelectedIDLs.Count > 0 , false );
						return;
					}
				}
			}
			catch
			{
			}
			this.pluginServer.UpdateItem( this, false, false );
		}

		private bool MoveSelectionToParent( bool fByKeyShortcut )
		{
			try
			{
				var tab = this.pluginServer.SelectedTabInFocusedView;
				if( tab == null || tab.IDL == null || tab.IDL.Length < 3 )
				{
					return false;
				}
				var target = Path.GetDirectoryName( tab.Path );
				var paths = tab.SelectedPaths;

				bool fCopy = !fByKeyShortcut && ( File.Exists( target ) ^ Control.ModifierKeys == Keys.Control );		// by File.Exists( target ), detecting compressed folder....tenuki.

				this.pluginServer.InvokeCommand( fCopy ? QCommand.CopyFile : QCommand.MoveFile, paths, target );
				return true;
			}
			catch
			{
			}
			return false;
		}
	}

	/// <summary>
	/// Properties button
	/// </summary>
	[Plugin( PluginType.Interactive, typeof( Localizer ), 8, Version = "1.3.0.0" )]
	public class PropertiesButton : IBarButton
	{
		// 1.3.0.0		supports views.

		private IPluginServer pluginServer;
		private string[] ResStr;

		#region IPluginClient Members

		public void Open( IPluginServer pluginServer, IShellBrowser shellBrowser )
		{
			this.pluginServer = pluginServer;
			this.ResStr = new string[] { StringResources.ButtonNames[8] };
		}

		public bool QueryShortcutKeys( out string[] actions )
		{
			actions = new string[] { this.ResStr[0] };
			return true;
		}

		public void Close( EndCode code )
		{
			this.pluginServer = null;
		}

		public void OnShortcutKeyPressed( int index )
		{
			this.OnButtonClick();
		}

		public void OnMenuItemClick( MenuType menuType, string menuText, ITab tab )
		{
		}

		public bool HasOption
		{
			get
			{
				return false;
			}
		}

		public void OnOption()
		{
		}

		#endregion

		#region IBarButton Members

		public void InitializeItem()
		{
		}

		public Image GetImage( bool fLarge )
		{
			return fLarge ? Resource.PropertiesButton_large : Resource.PropertiesButton_small;
		}

		public void OnButtonClick()
		{
			var tab = this.pluginServer.SelectedTabInFocusedView;
			if( tab != null )
			{
				var sel = tab.SelectedIDLs;
				if( sel.Count > 0 )
				{
					this.pluginServer.InvokeCommand( QCommand.Properties, (int)tab.View );
				}
				else
				{
					this.pluginServer.InvokeCommand( QCommand.ShowProperties, tab );
				}
			}




			//Address[] addresses;
			//if( this.pluginServer.TryGetSelection( out addresses ) )
			//{
			//    //if( addresses.Length > 0 )
			//    //    FileOps.FileOperation( FileOpActions.Properties, this.pluginServer.ExplorerHandle, null );
			//    //else
			//    //    FileOps.ShowProperties( this.pluginServer );

			//    if( addresses.Length > 0 )
			//    {
			//        FileOps.ShowProperties( addresses, this.pluginServer );
			//    }
			//    else
			//    {
			//        FileOps.ShowProperties( new Address[] { this.pluginServer.SelectedTab.Address }, this.pluginServer );
			//    }
			//}			
		}

		public bool ShowTextLabel
		{
			get
			{
				return true;
			}
		}

		public string Text
		{
			get
			{
				return this.ResStr[0];
			}
		}

		#endregion
	}


	static class Helper
	{
		public static void EnableIfFocusedViewHasSelection( IPluginServer pluginServer, IBarButton barButton )
		{
			var tab = pluginServer.SelectedTabInFocusedView;
			if( tab != null )
			{
				pluginServer.UpdateItem( barButton, tab.SelectedIDLs.Count > 0, false );
			}
			else
			{
				pluginServer.UpdateItem( barButton, false, false );
			}
		}
	}

	sealed class StringResources
	{
		public static string[] ButtonNames, Descriptions;

		static StringResources()
		{
			char[] SEPCHAR =  new char[] { ';' };

			if( CultureInfo.CurrentCulture.Parent.Name == "ja" )
			{
				ButtonNames = Resource.str_ja.Split( SEPCHAR );
				Descriptions = Resource.dcpt_ja.Split( SEPCHAR );
			}
			else
			{
				ButtonNames = Resource.str.Split( SEPCHAR );
				Descriptions = Resource.dcpt.Split( SEPCHAR );
			}
		}
	}

	sealed class Localizer : LocalizedStringProvider
	{
		private int iKey;

		public override void SetKey( int iKey )
		{
			this.iKey = iKey;
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
				return StringResources.Descriptions[this.iKey];
			}
		}

		public override string Name
		{
			get
			{
				return StringResources.ButtonNames[this.iKey];
			}
		}
	}
}
