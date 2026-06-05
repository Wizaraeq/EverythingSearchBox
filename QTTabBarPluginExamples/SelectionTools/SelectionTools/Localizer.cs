using System;
using System.Globalization;
using System.Text;
using QTPlugin;

namespace QuizoPlugins
{
	static class StringResources
	{
		public static string[] Names, Descriptions, KeyShortcuts, Tooltips, StatusInfoMenu;
		public static string[] ItemString;

		static StringResources()
		{
			char[] SEPCHAR = new char[] { ';' };

			if( CultureInfo.CurrentCulture.Parent.Name == "ja" )
			{
				Names = Resource.name_ja.Split( SEPCHAR );
				Descriptions = Resource.dcpt_ja.Split( SEPCHAR );
				KeyShortcuts = Resource.keys_ja.Split( SEPCHAR );
				Tooltips = Resource.tooltip_ja.Split( SEPCHAR );

				StatusInfoMenu = Resource.selSize_ja.Split( SEPCHAR );
			}
			else
			{
				Names = Resource.name.Split( SEPCHAR );
				Descriptions = Resource.dcpt.Split( SEPCHAR );
				KeyShortcuts = Resource.keys.Split( SEPCHAR );
				Tooltips = Resource.tooltip.Split( SEPCHAR );

				StatusInfoMenu = Resource.selSize.Split( SEPCHAR );
			}

			ItemString = new string[5];
			StringBuilder sb = new StringBuilder( 260 );
			if( W32.S_OK != ShellAPI.SHLoadIndirectString( "@explorerframe.dll,-41232", sb, sb.Capacity, IntPtr.Zero ) )		// "%1!d! items"
			{
				sb.Append( "%1!d! items" );
			}
			ItemString[0] = sb.ToString().Replace( "%1!d!", "{0}" );
			sb.Clear();
			if( W32.S_OK != ShellAPI.SHLoadIndirectString( "@explorerframe.dll,-41233", sb, sb.Capacity, IntPtr.Zero ) )		// "%1!d! item"
			{
				sb.Append( "%1!d! item" );
			}
			ItemString[1] = sb.ToString().Replace( "%1!d!", "{0}" );
			sb.Clear();
			if( W32.S_OK != ShellAPI.SHLoadIndirectString( "@explorerframe.dll,-41234", sb, sb.Capacity, IntPtr.Zero ) )		// "%1!d! items selected"
			{
				sb.Append( "%1!d! items selected" );
			}
			ItemString[2] = sb.ToString().Replace( "%1!d!", "{0}" );

			sb.Clear();
			if( W32.S_OK != ShellAPI.SHLoadIndirectString( "@shell32.dll,-9306", sb, sb.Capacity, IntPtr.Zero ) )		// "Total Size"
			{
				sb.Append( "Total Size" );
			}
			ItemString[3] = sb.ToString() + ": ";
			sb.Clear();
			if( W32.S_OK != ShellAPI.SHLoadIndirectString( "@shell32.dll,-9307", sb, sb.Capacity, IntPtr.Zero ) )		// "Free Space"
			{
				sb.Append( "Free Space" );
			}
			ItemString[4] = sb.ToString();

		}
	}

	sealed class Localizer : LocalizedStringProvider2
	{
		private string parent;
		private int iKey;

		public Localizer()
		{
			this.parent = CultureInfo.CurrentCulture.Parent.Name;
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
				return StringResources.Names[this.iKey];
			}
		}

		public override DateTime LastUpdate
		{
			get
			{
				return new DateTime( 2014, 12, 14 );
			}
		}

		public override string SupportURL
		{
			get
			{
				return "https://qttabbar.backlog.jp/";
			}
		}

		public override void SetKey( int iKey )
		{
			this.iKey = iKey;
		}

		public string[] KeyShortcutStr
		{
			get
			{
				switch( this.iKey )
				{
					case 0:
						return new string[] { StringResources.KeyShortcuts[0] };

					case 1:
						return new string[] { StringResources.KeyShortcuts[1], StringResources.KeyShortcuts[2] };

					case 2:
						return new string[] { StringResources.KeyShortcuts[3] };

					case 3:
						return new string[] { StringResources.KeyShortcuts[4], StringResources.KeyShortcuts[5], StringResources.KeyShortcuts[6],
											  StringResources.KeyShortcuts[7], StringResources.KeyShortcuts[8], StringResources.KeyShortcuts[9],
											  StringResources.KeyShortcuts[10], StringResources.KeyShortcuts[11], StringResources.KeyShortcuts[12] };

					default:
						return new string[0];
				}
			}
		}

		public string TooltipStr
		{
			get
			{
				return StringResources.Tooltips[this.iKey];
			}
		}

		public static string[] StatusInfoMenu
		{
			get
			{
				return StringResources.StatusInfoMenu;
			}
		}

		/// <summary>
		/// {0} items, {0} item, {0} items selected, Total Size: , Free Space: 
		/// </summary>
		public static string[] ItemString
		{
			get
			{
				return StringResources.ItemString;
			}
		}
	}
}