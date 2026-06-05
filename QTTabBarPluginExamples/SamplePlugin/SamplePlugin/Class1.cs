using System;
using System.Collections.Generic;
using System.Text;
using QTPlugin;

namespace SamplePlugin
{
	[Plugin( PluginType.Interactive, Author = "Quizo", Name = "サンプルプラグイン", Version = "0.9.0.0", Description = "サンプル プラグインです" )]
	public class SampleButton : IBarButton
	{
		#region IPluginClient メンバ

		private IPluginServer pluginServer;

		public void Open( IPluginServer pluginServer, QTPlugin.Interop.IShellBrowser shellBrowser )
		{
			this.pluginServer = pluginServer;

			this.pluginServer.RegisterMenu( this, MenuType.Both, "サンプルプラグインが追加したメニュー", true );
			this.pluginServer.RegisterMenu( this, MenuType.Both, "サンプルプラグインが追加したメニュー2", true );
		}

		public void Close( EndCode endCode )
		{
			// ボタンバーからボタンが取り除かれたり、ウィンドウが閉じたときに呼ばれます。
		}

		public bool QueryShortcutKeys( out string[] actions )
		{
			actions = new string[] { "1番目のショートカットキー", "2番目のショートカットキー" };
			return true;
		}

		public bool HasOption
		{
			get
			{
				// このプラグインがオプションを持つなら true を返す。   
				return false;
			}
		}

		public void OnMenuItemClick( MenuType menuType, string menuText, ITab tab )
		{
			System.Windows.Forms.MessageBox.Show( "タブの現在のパスは" + tab.Address.Path );
		}

		public void OnOption()
		{
			// QT TabBar のオプションウィンドウで、「プラグインのオプション」ボタンが押されると呼ばれます。
		}

		public void OnShortcutKeyPressed( int index )
		{
			System.Windows.Forms.MessageBox.Show( ( index + 1 ) + "番目のショートカットキーが押されました" );
		}

		#endregion

		#region IBarButton メンバ

		public void InitializeItem()
		{
			// ボタンバーにこのボタンが追加されるたびに呼ばれます。
		}

		public System.Drawing.Image GetImage( bool fLarge )
		{
			return fLarge ? Resource.SampleButton_large : Resource.SampleButton_small;
		}

		public void OnButtonClick()
		{
			this.pluginServer.ExecuteCommand( Commands.SetModalState, true );

			System.Windows.Forms.DialogResult dr = System.Windows.Forms.MessageBox.Show( "サンプル ボタンがクリックされました。\n1階層上に行きますか？",
													"サンプル プラグイン",
													System.Windows.Forms.MessageBoxButtons.OKCancel );

			if( dr == System.Windows.Forms.DialogResult.OK )
				this.pluginServer.ExecuteCommand( Commands.GoUpOneLevel, null );

			this.pluginServer.ExecuteCommand( Commands.SetModalState, false );
		}

		public bool ShowTextLabel
		{
			get
			{
				// ボタンバーの設定が「ボタン名をいくつか表示する」のとき、
				// ボタン名を表示するなら true    
				return false;
			}
		}

		public string Text
		{
			get
			{
				// このボタンのツールチップに表示される文字列です。
				return "サンプル プラグインですよ";
			}
		}

		#endregion
	}
}
