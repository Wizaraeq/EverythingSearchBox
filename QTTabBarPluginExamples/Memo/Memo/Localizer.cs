using System;
using System.Drawing;
using System.Globalization;
using QTPlugin;

namespace QuizoPlugin
{
	sealed class Localizer : LocalizedStringProvider2
	{
		public static string[] StringResources, MenuStrings;
		public static bool fJa;

		static Localizer()
		{
			if( CultureInfo.CurrentCulture.Parent.Name == "ja" )
			{
				fJa = true;
				StringResources = Resource.str_ja.Split( new char[] { ';' } );
				MenuStrings = Resource.menu_ja.Split( new char[] { ';' } );
			}
			else
			{
				StringResources = Resource.str_en.Split( new char[] { ';' } );
			}
		}

		public override string Name
		{
			get
			{
				return fJa ? "フォルダー メモ" : "Folder Memo";
			}
		}

		public override string Author
		{
			get
			{
				return "Quizo";
			}
		}

		public override DateTime LastUpdate
		{
			get
			{
				return new DateTime( 2014, 5, 31 );
			}
		}

		public override string SupportURL
		{
			get
			{
				return "https://qttabbar.backlog.jp/projects/Q";
			}
		}

		public override string Description
		{
			get
			{
				return fJa ? "フォルダーごとにメモを保存できるようになります。メモのあるフォルダではメモウィンドウが自動的に表示されます。タブのコンテキスト メニューからメモを作成してください。" :
							 "You can make a note for each folder. Memo window is displayed if the current folder has memo. Start to make a memo by click 'Show folder memo' in tab context menu.";
			}
		}

		public override void SetKey( int iKey )
		{
			
		}

		public static Font CreateFont()
		{
			if( fJa )
			{
				return new Font( "Meiryo UI", 9F );
			}
			else
			{
				return new Font( "Segoe UI", 9F );
			}
		}
	}
}
