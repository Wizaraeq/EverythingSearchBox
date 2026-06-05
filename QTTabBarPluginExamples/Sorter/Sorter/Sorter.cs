using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;

using QTPlugin;
using QTPlugin.Interop;
using System.Drawing;

namespace Sorter
{
	[Plugin( PluginType.Background, typeof( Localizer ) )]
	public class Sorter : IBarDropButton
	{
		private const int COUNT_OF_STRING_RESOURCES = 5;

		private IPluginServer pluginServer;
		private IShellBrowser shellBrowser;
		private BitVector32 enabledProperties;
		private const int DEFAULTDATA = 255;
		private bool fClearMenuItems;

		private static PROPERTYKEY propertyKey_NULL					= new PROPERTYKEY();
		private static PROPERTYKEY propertyKey_ItemNameDisplay		= new PROPERTYKEY( new Guid( "{B725F130-47EF-101A-A5F1-02608C9EEBAC}" ), 10 );
		private static PROPERTYKEY propertyKey_DateModified			= new PROPERTYKEY( new Guid( "{B725F130-47EF-101A-A5F1-02608C9EEBAC}" ), 14 );
		private static PROPERTYKEY propertyKey_DateCreated			= new PROPERTYKEY( new Guid( "{B725F130-47EF-101A-A5F1-02608C9EEBAC}" ), 15 );
		private static PROPERTYKEY propertyKey_ItemType				= new PROPERTYKEY( new Guid( "{B725F130-47EF-101A-A5F1-02608C9EEBAC}" ), 4 );
		private static PROPERTYKEY propertyKey_Size					= new PROPERTYKEY( new Guid( "{B725F130-47EF-101A-A5F1-02608C9EEBAC}" ), 12 );
		private static PROPERTYKEY propertyKey_FileAttributes		= new PROPERTYKEY( new Guid( "{B725F130-47EF-101A-A5F1-02608C9EEBAC}" ), 13 );
		private static PROPERTYKEY propertyKey_Keywords				= new PROPERTYKEY( new Guid( "{F29F85E0-4FF9-1068-AB91-08002B27B3D9}" ), 5 );
		private static PROPERTYKEY propertyKey_ItemDate				= new PROPERTYKEY( new Guid( "{F7DB74B4-4287-4103-AFBA-F1B13DCD75CF}" ), 100 );
		private static PROPERTYKEY propertyKey_Author				= new PROPERTYKEY( new Guid( "{F29F85E0-4FF9-1068-AB91-08002B27B3D9}" ), 4 );
		private static PROPERTYKEY propertyKey_Comment				= new PROPERTYKEY( new Guid( "{F29F85E0-4FF9-1068-AB91-08002B27B3D9}" ), 6 );
		private static PROPERTYKEY propertyKey_FileDescription		= new PROPERTYKEY( new Guid( "{0CEF7D53-FA64-11D1-A203-0000F81FEDEE}" ), 3 );
		private static PROPERTYKEY propertyKey_Rating				= new PROPERTYKEY( new Guid( "{64440492-4C8B-11D1-8B70-080036B11A03}" ), 9 );
		private static PROPERTYKEY propertyKey_Subject				= new PROPERTYKEY( new Guid( "{F29F85E0-4FF9-1068-AB91-08002B27B3D9}" ), 3 );
		private static PROPERTYKEY propertyKey_Title				= new PROPERTYKEY( new Guid( "{F29F85E0-4FF9-1068-AB91-08002B27B3D9}" ), 2 );
		private static PROPERTYKEY propertyKey_Music_Artist			= new PROPERTYKEY( new Guid( "{56A3372E-CE9C-11D2-9F0E-006097C686F6}" ), 2 );
		private static PROPERTYKEY propertyKey_Music_AlbumArtist	= new PROPERTYKEY( new Guid( "{56A3372E-CE9C-11D2-9F0E-006097C686F6}" ), 13 );
		private static PROPERTYKEY propertyKey_Music_AlbumTitle		= new PROPERTYKEY( new Guid( "{56A3372E-CE9C-11D2-9F0E-006097C686F6}" ), 4 );
		private static PROPERTYKEY propertyKey_Music_Composer		= new PROPERTYKEY( new Guid( "{64440492-4C8B-11D1-8B70-080036B11A03}" ), 19 );
		private static PROPERTYKEY propertyKey_Music_Genre			= new PROPERTYKEY( new Guid( "{56A3372E-CE9C-11D2-9F0E-006097C686F6}" ), 11 );
		private static PROPERTYKEY propertyKey_Music_TrackNumber	= new PROPERTYKEY( new Guid( "{56A3372E-CE9C-11D2-9F0E-006097C686F6}" ), 7 );
		private static PROPERTYKEY propertyKey_Media_Duration		= new PROPERTYKEY( new Guid( "{64440490-4C8B-11D1-8B70-080036B11A03}" ), 3 );
		private static PROPERTYKEY propertyKey_Media_Year			= new PROPERTYKEY( new Guid( "{56A3372E-CE9C-11D2-9F0E-006097C686F6}" ), 5 );
		private static PROPERTYKEY propertyKey_Image_Dimensions		= new PROPERTYKEY( new Guid( "{6444048F-4C8B-11D1-8B70-080036B11A03}" ), 13 );

		private static PROPERTYKEY[] propertyKeys = { propertyKey_ItemNameDisplay, propertyKey_DateModified, propertyKey_DateCreated, propertyKey_ItemType, propertyKey_Size,
													  propertyKey_FileAttributes, propertyKey_Keywords, propertyKey_ItemDate, propertyKey_Author, propertyKey_Comment,
													  propertyKey_FileDescription, propertyKey_Rating, propertyKey_Subject, propertyKey_Title, propertyKey_Music_Artist,
													  propertyKey_Music_AlbumArtist, propertyKey_Music_AlbumTitle, propertyKey_Music_Composer, propertyKey_Music_Genre, propertyKey_Music_TrackNumber,
													  propertyKey_Media_Duration, propertyKey_Media_Year, propertyKey_Image_Dimensions };


		#region IPluginClient members 

		public void Open( IPluginServer pluginServer, IShellBrowser shellBrowser )
		{
			this.pluginServer = pluginServer;
			this.shellBrowser = shellBrowser;

			this.ReadSettings();
		}

		public void Close( EndCode endCode )
		{
		}

		public bool QueryShortcutKeys( out string[] actions )
		{
			actions = StringResources.KeyShortcuts;
			return true;
		}

		public void OnShortcutKeyPressed( int index )
		{
			PROPERTYKEY propKey = propertyKey_NULL;
			if( -1 < index && index < propertyKeys.Length )
			{
				propKey = propertyKeys[index];
			}

			this.ToggleSort( propKey );
		}

		public void OnMenuItemClick( MenuType menuType, string menuText, ITab tab )
		{
		}

		public bool HasOption
		{
			get
			{
				return true;
			}
		}

		public void OnOption()
		{
			using( SettingForm sf = new SettingForm( enabledProperties.Data ) )
			{
				if( DialogResult.OK == sf.ShowDialog() )
				{
					int[] arr = sf.CheckedIndices;

					enabledProperties = new BitVector32( 0 );
					for( int i = 0; i < arr.Length; i++ )
					{
						enabledProperties[1 << arr[i]] = true;
					}

					this.SaveSettings();

					this.fClearMenuItems = true;
				}
			}
		}

		#endregion

		#region IBarButton members

		public System.Drawing.Image GetImage( bool fLarge )
		{
			return fLarge ? Resource.Sorter_large : Resource.Sorter_small;
		}

		public void OnButtonClick()
		{
		}

		public void InitializeItem()
		{
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
				return "Sorter";
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

		public void OnDropDownItemClick( ToolStripItem item, MouseButtons mouseButton )
		{
			int index = Array.IndexOf<string>( StringResources.ColumnNames, item.Text );
			if( -1 < index && index < StringResources.ColumnNames.Length )
			{
				this.ToggleSort( propertyKeys[index] ); 
			}
		}

		public void OnDropDownOpening( ToolStripDropDownMenu menu )
		{
			if( this.fClearMenuItems )
			{
				while( menu.Items.Count > 0 )
				{
					menu.Items[0].Dispose();
				}
				this.fClearMenuItems = false;
			}


			if( menu.Items.Count == 0 )
			{
				int bit = 1;
				foreach( var item in StringResources.ColumnNames )
				{
					if( enabledProperties[bit] )
					{
						ToolStripMenuItem tsmi = new ToolStripMenuItem( item );
						menu.Items.Add( tsmi );
					}
					bit <<= 1;
				}
			}
		}

		#endregion


		private void ReadSettings()
		{
			using( RegistryKey rkPlugin = Registry.CurrentUser.OpenSubKey( CONSTANTS.REGISTRY_PLUGINSETTINGS + "\\Quizo\\Sorter" ) )
			{
				if( rkPlugin != null )
				{
					object o = rkPlugin.GetValue( "Properties", DEFAULTDATA );
					if( o is int )
					{
						enabledProperties = new BitVector32( (int)o );
						return;
					}
				}
			}
			enabledProperties = new BitVector32( DEFAULTDATA );
		}

		private void SaveSettings()
		{
			using( RegistryKey rkPlugin = Registry.CurrentUser.CreateSubKey( CONSTANTS.REGISTRY_PLUGINSETTINGS + "\\Quizo\\Sorter" ) )
			{
				if( rkPlugin != null )
				{
					rkPlugin.SetValue( "Properties", enabledProperties.Data );
				}
			}
		}


		private void ToggleSort( PROPERTYKEY propKey )
		{
			IShellView shellView = null;
			try
			{
				if( 0 == this.shellBrowser.QueryActiveShellView( out shellView ) )
				{
					IFolderView2 folderView = shellView as IFolderView2;
					if( folderView != null )
					{
						SORTCOLUMN[] sortColumns;
						if( GetSortColumns( folderView, out sortColumns ) )
						{
							bool fFound = false;
							SORTCOLUMN sortColumnTargetA = new SORTCOLUMN( propKey, SORTDIRECTION.SORT_ASCENDING );

							for( int i = 0; i < sortColumns.Length; i++ )
							{
								if( propKey.Equals( propertyKey_NULL ) )
								{
									sortColumns[i].Reverse();
									fFound = true;
								}
								else if( sortColumns[i].propkey.Equals( propKey ) )
								{
									sortColumns[i].Reverse();
									fFound = true;
									break;
								}
							}
							if( !fFound )
							{
								sortColumns = new SORTCOLUMN[] { sortColumnTargetA };
							}

							SetSortColumns( folderView, sortColumns );
						}
					}
				}
			}
			catch( Exception ex )
			{
				MessageBox.Show( ex.ToString() );
			}
			finally
			{
				if( shellView != null )
				{
					Marshal.ReleaseComObject( shellView );
				}
			}
		}

		private static bool GetSortColumns( IFolderView2 folderView, out SORTCOLUMN[] sortColumns )
		{
			sortColumns = null;

			int cColumns;
			folderView.GetSortColumnCount( out cColumns );
			if( cColumns > 0 )
			{
				int cbSize = Marshal.SizeOf( typeof( SORTCOLUMN ) );

				IntPtr p = Marshal.AllocCoTaskMem( cbSize * cColumns );
				if( p != IntPtr.Zero )
				{
					try
					{
						// i don't know how to marshal these arguments of GetSortColumns and SetSortColumns without crashing Explorer.
						// so i use pointer
						if( 0 == folderView.GetSortColumns( p, cColumns ) )
						{
							List<SORTCOLUMN> lst = new List<SORTCOLUMN>();
							for( int i = 0; i < cColumns; i++ )
							{
								lst.Add( (SORTCOLUMN)Marshal.PtrToStructure( p + ( cbSize * i ), typeof( SORTCOLUMN ) ) );
							}

							//foreach( SORTCOLUMN sc in lst )
							//{
							//    MessageBox.Show( PropetyKeyToString( sc.propkey ) );
							//}

							sortColumns = lst.ToArray();
							return true;
						}
					}
					finally
					{
						Marshal.FreeCoTaskMem( p );
					}
				}
			}
			return false;
		}

		private static bool SetSortColumns( IFolderView2 folderView, SORTCOLUMN[] sortColumns )
		{
			if( sortColumns != null && sortColumns.Length > 0 )
			{
				int cbSize = Marshal.SizeOf( typeof( SORTCOLUMN ) );

				IntPtr p = Marshal.AllocCoTaskMem( cbSize * sortColumns.Length );
				if( p != IntPtr.Zero )
				{
					try
					{
						for( int i = 0; i < sortColumns.Length; i++ )
						{
							Marshal.StructureToPtr( sortColumns[i], p + ( cbSize * i ), false );
						}

						return 0 == folderView.SetSortColumns( p, sortColumns.Length );
					}
					finally
					{
						Marshal.FreeCoTaskMem( p );
					}
				}
			}
			return false;
		}


		public static Font CreateDefaultFont()
		{
			if( !String.IsNullOrEmpty( StringResources.DefaultFont ) )
			{
				try
				{
					return new Font( StringResources.DefaultFont, 9, FontStyle.Regular, GraphicsUnit.Point, ( (byte)( 0 ) ) );
				}
				catch
				{
				}
			}
			return null;
		}

		private static string PropetyKeyToString( PROPERTYKEY pk )
		{
			int index = Array.IndexOf( propertyKeys, pk );
			if( index != -1 )
			{
				return index.ToString();
			}
			return String.Empty;
		}

		// IColumnManager
	}

	sealed class StringResources
	{
		public static string[] ColumnNames, KeyShortcuts, SettingForm;
		public static string Name, Description, DefaultFont;

		static StringResources()
		{
			char[] SEPCHAR = new char[] { ';' };

			if( CultureInfo.CurrentCulture.Parent.Name == "ja" )
			{
				ColumnNames = Resource.sortColumnNames_ja.Split( SEPCHAR );
				KeyShortcuts = Resource.keyShortcutNames_ja.Split( SEPCHAR );
				SettingForm = Resource.settingForm_ja.Split( SEPCHAR );
				Name = Resource.name_ja;
				Description = Resource.description_ja;
				DefaultFont = Resource.DefaultFont_ja;
			}
			else
			{
				ColumnNames = Resource.sortColumnNames_en.Split( SEPCHAR );
				KeyShortcuts = Resource.keyShortcutNames_en.Split( SEPCHAR );
				SettingForm = Resource.settingForm_en.Split( SEPCHAR );
				Name = Resource.name_en;
				Description = Resource.description_en;
				DefaultFont = Resource.DefaultFont_en;
			}
		}
	}

	sealed class Localizer : LocalizedStringProvider
	{
		public Localizer()
		{
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
				return StringResources.Description;
			}
		}

		public override string Name
		{
			get
			{
				return StringResources.Name;
			}
		}

		public override void SetKey( int iKey )
		{			
		}

	}

	// FormatID and propID
	//-------------------------------------------------------------------------------
	// System.ItemNameDisplay			{B725F130-47EF-101A-A5F1-02608C9EEBAC}, 10 
	// System.DateModified				{B725F130-47EF-101A-A5F1-02608C9EEBAC}, 14
	// System.DateCreated				{B725F130-47EF-101A-A5F1-02608C9EEBAC}, 15
	// System.ItemType					{28636AA6-953D-11D2-B5D6-00C04FD918D0}, 11
	// System.FileDescription			{0CEF7D53-FA64-11D1-A203-0000F81FEDEE}, 3
	// System.Size						{B725F130-47EF-101A-A5F1-02608C9EEBAC}, 12
	// System.FileAttributes			{B725F130-47EF-101A-A5F1-02608C9EEBAC}, 13
	// System.Keywords					{F29F85E0-4FF9-1068-AB91-08002B27B3D9}, 5				tag.
	// System.ItemDate					{F7DB74B4-4287-4103-AFBA-F1B13DCD75CF}, 100				For example, for photos this maps to System.Photo.DateTaken.

	// System.Music.AlbumArtist			{56A3372E-CE9C-11D2-9F0E-006097C686F6}, 13
	// System.Music.AlbumTitle			{56A3372E-CE9C-11D2-9F0E-006097C686F6}, 4
	// System.Music.Artist				{56A3372E-CE9C-11D2-9F0E-006097C686F6}, 2
	// System.Music.Composer			{64440492-4C8B-11D1-8B70-080036B11A03}, 19
	// System.Music.Genre				{56A3372E-CE9C-11D2-9F0E-006097C686F6}, 11
	// System.Music.TrackNumber			{56A3372E-CE9C-11D2-9F0E-006097C686F6}, 7
	// System.Media.Duration			{64440490-4C8B-11D1-8B70-080036B11A03}, 3
	// System.Media.Year				{56A3372E-CE9C-11D2-9F0E-006097C686F6}, 5
	// System.Image.Dimensions			{6444048F-4C8B-11D1-8B70-080036B11A03}, 13

		




}
