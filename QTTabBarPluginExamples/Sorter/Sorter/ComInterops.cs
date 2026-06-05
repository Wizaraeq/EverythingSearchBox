using System;
using System.Runtime.InteropServices;
using System.Security;
using QTPlugin.Interop;

namespace Sorter
{
	[ComImport]
	[InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
	[Guid( "1af3a467-214f-4298-908e-06b03e0b39f9" )]
	[SuppressUnmanagedCodeSecurity]
	interface IFolderView2
	{
		[PreserveSig]
		int GetCurrentViewMode( ref FOLDERVIEWMODE pViewMode );					// __RPC__inout

		[PreserveSig]
		int SetCurrentViewMode( FOLDERVIEWMODE ViewMode );

		[PreserveSig]
		int GetFolder( [In, MarshalAs( UnmanagedType.LPStruct )] Guid riid, out IPersistFolder2 ppv );

		[PreserveSig]
		int Item( int iItemIndex, out IntPtr ppidl );

		[PreserveSig]
		int ItemCount( SVGIO uFlags, out int pcItems );

		[PreserveSig]
		int Items( SVGIO uFlags, [In, MarshalAs( UnmanagedType.LPStruct )] Guid riid, out IEnumIDList ppv );

		[PreserveSig]
		int GetSelectionMarkedItem( out int piItem );

		[PreserveSig]
		int GetFocusedItem( out int piItem );

		[PreserveSig]
		int GetItemPosition( IntPtr pidl, out POINT ppt );					// __RPC__out

		[PreserveSig]
		int GetSpacing( ref POINT ppt );										// __RPC__inout_opt

		[PreserveSig]
		int GetDefaultSpacing( out POINT ppt );								// __RPC__out

		[PreserveSig]
		int GetAutoArrange();

		[PreserveSig]
		int SelectItem( int iItem, uint dwFlags );

		[PreserveSig]
		int SelectAndPositionItems(
			int cidl,
			[In, MarshalAs( UnmanagedType.LPArray, SizeParamIndex = 0 )] IntPtr[] apidl,
			[In, MarshalAs( UnmanagedType.LPArray, SizeParamIndex = 0 )] POINT[] apt,
			int dwFlags );


		// IFolderView2

		[PreserveSig]
		int SetGroupBy( ref PROPERTYKEY key, [MarshalAs( UnmanagedType.Bool )] bool fAscending );

		[PreserveSig]
		int GetGroupBy( out PROPERTYKEY pkey, [MarshalAs( UnmanagedType.Bool )] out bool pfAscending );

		// deprecated as of Windows 7	
		[PreserveSig]
		int SetViewProperty( IntPtr pidl, ref PROPERTYKEY propkey, IntPtr propvar );					// const PROPVARIANT *... ref VARIANT

		// deprecated as of Windows 7
		[PreserveSig]
		int GetViewProperty( IntPtr pidl, ref PROPERTYKEY propkey, out IntPtr ppropvar );				// PROPVARIANT*

		// deprecated as of Windows 7
		[PreserveSig]
		int SetTileViewProperties( IntPtr pidl, [MarshalAs( UnmanagedType.LPWStr )] String pszPropList );

		// deprecated as of Windows 7
		[PreserveSig]
		int SetExtendedTileViewProperties( IntPtr pidl, [MarshalAs( UnmanagedType.LPWStr )] String pszPropList );

		[PreserveSig]
		int SetText( int iType, [MarshalAs( UnmanagedType.LPWStr )] String pwszText );					// [v1_enum] FVTEXTTYPE , FVST_EMPTYTEXT	= 0

		[PreserveSig]
		int SetCurrentFolderFlags( uint dwMask, uint dwFlags );

		[PreserveSig]
		int GetCurrentFolderFlags( out uint pdwFlags );

		[PreserveSig]
		int GetSortColumnCount( out int pcColumns );

		[PreserveSig]
		int SetSortColumns( IntPtr rgSortColumns, int cColumns );

		[PreserveSig]
		int GetSortColumns( IntPtr rgSortColumns, int cColumns );

		[PreserveSig]
		int GetItem( int iItem, [In, MarshalAs( UnmanagedType.LPStruct )] Guid riid, [MarshalAs( UnmanagedType.IUnknown )] out object ppv );

		[PreserveSig]
		int GetVisibleItem( int iStart, [MarshalAs( UnmanagedType.Bool )] bool fPrevious, out int piItem );

		[PreserveSig]
		int GetSelectedItem( int iStart, out int piItem );

		[PreserveSig]
		int GetSelection( [MarshalAs( UnmanagedType.Bool )] bool fNoneImpliesFolder, [MarshalAs( UnmanagedType.IUnknown )] out object ppsia );							// IShellItemArray **

		[PreserveSig]
		int GetSelectionState( IntPtr pidl, out uint pdwFlags );

		[PreserveSig]
		int InvokeVerbOnSelection( [MarshalAs( UnmanagedType.LPWStr )] string pszVerb );

		[PreserveSig]
		int SetViewModeAndIconSize( FOLDERVIEWMODE uViewMode, int iImageSize );

		[PreserveSig]
		int GetViewModeAndIconSize( out FOLDERVIEWMODE puViewMode, out int piImageSize );

		[PreserveSig]
		int SetGroupSubsetCount( uint cVisibleRows );

		[PreserveSig]
		int GetGroupSubsetCount( out uint pcVisibleRows );

		[PreserveSig]
		int SetRedraw( [MarshalAs( UnmanagedType.Bool )] bool fRedrawOn );

		[PreserveSig]
		int IsMoveInSameFolder();

		[PreserveSig]
		int DoRename();

	}

	[StructLayout( LayoutKind.Sequential, Pack = 8 )]
	struct PROPERTYKEY									// == PROPERTYKEY
	{
		public Guid fmtid;
		public int pid;

		public PROPERTYKEY( Guid fmtid, int pid )
		{
			this.fmtid = fmtid;
			this.pid = pid;
		}
	}

	[StructLayout( LayoutKind.Sequential, Pack = 8 )]
	struct SORTCOLUMN
	{
		public PROPERTYKEY propkey;
		public SORTDIRECTION direction;

		public SORTCOLUMN( PROPERTYKEY propkey, SORTDIRECTION direction )
		{
			this.propkey = propkey;
			this.direction = direction;
		}

		public bool IsEmpty
		{
			get
			{
				return this.propkey.fmtid == Guid.Empty;
			}
		}

		public void Reverse()
		{
			this.direction = (SORTDIRECTION)( (int)this.direction * -1 );
		}
	}

	enum FOLDERVIEWMODE
	{
		FVM_AUTO	   = -1,
		FVM_ICON	   = 1,
		FVM_SMALLICON  = 2,
		FVM_LIST	   = 3,
		FVM_DETAILS	   = 4,
		FVM_THUMBNAIL  = 5,
		FVM_TILE	   = 6,
		FVM_THUMBSTRIP = 7,
		FVM_CONTENT    = 8,				// Windows7
	}

	enum SVGIO : uint
	{
		BACKGROUND	= 0x00000000,
		SELECTION	= 0x00000001,
		ALLVIEW		= 0x00000002,
		CHECKED		= 0x00000003,
		TYPE_MASK	= 0x0000000F,
		FLAG_VIEWORDER = 0x80000000
	}

	enum SORTDIRECTION
	{
		SORT_DESCENDING = -1,
		SORT_ASCENDING = 1
	}

}
