using System;
using System.Runtime.InteropServices;
using System.Text;
using QTPlugin.Interop;

namespace QuizoPlugins
{
	static class ShellAPI
	{
		public static readonly Guid IID_IShellFolder = new Guid( "000214E6-0000-0000-C000-000000000046" );

		[DllImport( "shell32.dll" )]
		public static extern int SHBindToParent( IntPtr pidl, [In, MarshalAs( UnmanagedType.LPStruct )] Guid riid, out IShellFolder ppv, out IntPtr ppidlLast );

		[DllImport( "shell32.dll" )]
		public static extern IntPtr ILFindLastID( IntPtr pidl );

		public static string GetDisplayName( IShellFolder shellFolder, IntPtr pIDLLast )
		{
			StringBuilder sb = new StringBuilder( 260 );
			STRRET pSTRRET;

			if( 0 == shellFolder.GetDisplayNameOf( pIDLLast, (uint)SHGDNF.FORPARSING, out pSTRRET ) )
			{
				PInvoke.StrRetToBuf( ref pSTRRET, pIDLLast, sb, (uint)sb.Capacity );
			}

			return sb.ToString();
		}

		[DllImport( "shlwapi.dll", CharSet = CharSet.Ansi )]
		private static extern IntPtr StrFormatByteSize64A( long qdw, [Out, MarshalAs( UnmanagedType.LPStr, SizeParamIndex = 2 )] StringBuilder pszBuf, int cchBuf );

		public static string StrFormatByteSize( long lSize )
		{
			StringBuilder sb = new StringBuilder( 260 );
			StrFormatByteSize64A( lSize, sb, sb.Capacity );
			return sb.ToString();
		}

		[DllImport( "propsys.dll" )]
		public static extern int PSGetPropertyDescription( ref PROPERTYKEY keyType, [In, MarshalAs( UnmanagedType.LPStruct )] Guid riid, [Out, MarshalAs( UnmanagedType.IUnknown )] out object ppv );

		[DllImport( "shlwapi.dll", CharSet = CharSet.Unicode )]
		public static extern int SHLoadIndirectString( string pszSource, StringBuilder pszOutBuf, int cchOutBuf, IntPtr ppvReserved );

	}

	[Flags]
	enum SFGAO : uint
	{
		CANCOPY = 0x00000001,	// DROPEFFECT_COPY // Objects can be copied   (0x1)
		CANMOVE = 0x00000002,	// DROPEFFECT_MOVE // Objects can be moved   (0x2)
		CANLINK = 0x00000004,	// DROPEFFECT_LINK // Objects can be linked   (0x4)
		STORAGE = 0x00000008,	// supports BindToObject(IID_IStorage)
		CANRENAME = 0x00000010,	// Objects can be renamed
		CANDELETE = 0x00000020,	// Objects can be deleted
		HASPROPSHEET = 0x00000040,	// Objects have property sheets
		DROPTARGET = 0x00000100,	// Objects are drop target
		SYSTEM = 0x00001000,	// System object (Windows 7 and later.)
		ENCRYPTED = 0x00002000,	// Object is encrypted (use alt color)
		ISSLOW = 0x00004000,	// 'Slow' object
		GHOSTED = 0x00008000,	// Ghosted icon
		LINK = 0x00010000,	// Shortcut (link)
		SHARE = 0x00020000,	// Shared
		READONLY = 0x00040000,	// Read-only
		HIDDEN = 0x00080000,	// Hidden object
		NONENUMERATED = 0x00100000,	// Is a non-enumerated object (should be hidden)
		NEWCONTENT = 0x00200000,	// Should show bold in explorer tree
		STREAM = 0x00400000,	// Supports BindToObject(IID_IStream)
		STORAGEANCESTOR = 0x00800000,	// May contain children with SFGAO_STORAGE or SFGAO_STREAM
		VALIDATE = 0x01000000,	// Invalidate cached information (may be slow)
		REMOVABLE = 0x02000000,	// Is this removeable media?
		COMPRESSED = 0x04000000,	// Object is compressed (use alt color)
		BROWSABLE = 0x08000000,	// Supports IShellFolder, but only implements CreateViewObject() (non-folder view)
		FILESYSANCESTOR = 0x10000000,	// May contain children with SFGAO_FILESYSTEM
		FOLDER = 0x20000000,	// Support BindToObject(IID_IShellFolder)
		FILESYSTEM = 0x40000000,	// Is a win32 file system object (file/folder/root)
		HASSUBFOLDER = 0x80000000,	// May contain children with SFGAO_FOLDER (may be slow)
	}

	enum SHGDNF
	{
		NORMAL = 0,
		INFOLDER = 0x1,
		FOREDITING = 0x1000,
		FORADDRESSBAR = 0x4000,
		FORPARSING = 0x8000
	}

	[Flags]
	enum SHCONTF
	{
		CHECKING_FOR_CHILDREN = 0x00010,
		FOLDERS = 0x00020,
		NONFOLDERS = 0x00040,
		INCLUDEHIDDEN = 0x00080,
		INIT_ON_FIRST_NEXT = 0x00100,
		NETPRINTERSRCH = 0x00200,
		SHAREABLE = 0x00400,
		STORAGE = 0x00800,
		NAVIGATION_ENUM = 0x01000,
		FASTITEMS = 0x02000,
		FLATLIST = 0x04000,
		ENABLE_ASYNC = 0x08000,
		INCLUDESUPERHIDDEN = 0x10000
	}


	static class W32
	{
		public const int S_OK = 0;
		public const int FCW_STATUS = 0x0001;
	}

	static class PropertyDescription
	{
		private static readonly Guid IID_IPropertyDescription = new Guid( "6f79d558-3e96-4549-a1d1-7d75d2288814" );

		public static string GetDisplayName( PROPERTYKEY pkey )
		{
			string str = null;
			object oUnk = null;
			try
			{
				if( W32.S_OK == ShellAPI.PSGetPropertyDescription( ref pkey, IID_IPropertyDescription, out oUnk ) )
				{
					var propertyDescription = oUnk as IPropertyDescription;
					if( propertyDescription != null )
					{
						propertyDescription.GetDisplayName( out str );
					}
				}
			}
			finally
			{
				if( oUnk != null )
				{
					Marshal.ReleaseComObject( oUnk );
				}
			}
			return str;
		}
	}


	[ComImport]
	[InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
	[Guid( "6f79d558-3e96-4549-a1d1-7d75d2288814" )]
	interface IPropertyDescription
	{
		[PreserveSig]
		int GetPropertyKey( out PROPERTYKEY pkey );

		[PreserveSig]
		int GetCanonicalName( [MarshalAs( UnmanagedType.LPWStr )] out string ppszName );

		[PreserveSig]
		int GetPropertyType( out short pvartype );					//typedef unsigned short VARTYPE;

		[PreserveSig]
		int GetDisplayName( [MarshalAs( UnmanagedType.LPWStr )] out string ppszName );

		[PreserveSig]
		int GetEditInvitation( [MarshalAs( UnmanagedType.LPWStr )] out string ppszInvite );

		[PreserveSig]
		int GetTypeFlags( int mask, out int ppdtFlags );				//typedef int PROPDESC_TYPE_FLAGS;

		[PreserveSig]
		int GetViewFlags( out int ppdvFlags );						//typedef int PROPDESC_VIEW_FLAGS;

		[PreserveSig]
		int GetDefaultColumnWidth( out int pcxChars );

		[PreserveSig]
		int GetDisplayType( out int pdisplaytype );					//PROPDESC_DISPLAYTYPE, [v1_enum]

		[PreserveSig]
		int GetColumnState( out int pcsFlags );						//typedef DWORD SHCOLSTATEF;

		[PreserveSig]
		int GetGroupingRange( out int pgr );							//PROPDESC_GROUPING_RANGE	[v1_enum]

		[PreserveSig]
		int GetRelativeDescriptionType( out int prdt );				//PROPDESC_RELATIVEDESCRIPTION_TYPE [v1_enum]

		[PreserveSig]
		int GetRelativeDescription( ref  PROPVARIANT propvar1, ref PROPVARIANT propvar2, [MarshalAs( UnmanagedType.LPWStr )] out string ppszDesc1, [MarshalAs( UnmanagedType.LPWStr )]out string ppszDesc2 );

		[PreserveSig]
		int GetSortDescription( out int psd );						//PROPDESC_SORTDESCRIPTION, [v1_enum]

		[PreserveSig]
		int GetSortDescriptionLabel( bool fDescending, [MarshalAs( UnmanagedType.LPWStr )]out string ppszDescription );

		[PreserveSig]
		int GetAggregationType( out int paggtype );					//PROPDESC_AGGREGATION_TYPE , [v1_enum]

		[PreserveSig]
		int GetConditionType( out int pcontype, out int popDefault );	// PROPDESC_CONDITION_TYPE [v1_enum]  , CONDITION_OPERATION [v1_enum]

		[PreserveSig]
		int GetEnumTypeList( ref Guid riid, out object ppv );			// IPropertyEnumTypeList, MSDN is incorrect

		[PreserveSig]
		int CoerceToCanonicalValue( ref PROPVARIANT ppropvar );

		[PreserveSig]
		int FormatForDisplay( ref PROPVARIANT propvar, int pdfFlags, [MarshalAs( UnmanagedType.LPWStr )]out string ppszDisplay );		//typedef int PROPDESC_FORMAT_FLAGS; MSDN is incorrect

		[PreserveSig]
		int IsValueCanonical( ref PROPVARIANT propvar );
	}

	[StructLayout( LayoutKind.Explicit )]
	struct PROPVARIANT
	{
		// 16bytes in x86, 24 in x64
		// minimum implement

		[FieldOffset( 0 )]
		public short vt;

		// Reserved Fields
		//[FieldOffset( 2 )]
		//public short wReserved1;
		//[FieldOffset( 4 )]
		//public short wReserved2;
		//[FieldOffset( 6 )]
		//public short wReserved3;

		[FieldOffset( 8 )]
		public IntPtr pwszVal;

		[FieldOffset( 8 )]
		public int intVal;

		[FieldOffset( 12 )]
		IntPtr ptr;

		/*
		union 
        {
        CHAR cVal;
        UCHAR bVal;
        SHORT iVal;
        USHORT uiVal;
        LONG lVal;
        ULONG ulVal;
        INT intVal;
        UINT uintVal;
        LARGE_INTEGER hVal;
        ULARGE_INTEGER uhVal;
        FLOAT fltVal;
        DOUBLE dblVal;
        VARIANT_BOOL boolVal;
        _VARIANT_BOOL bool;
        SCODE scode;
        CY cyVal;
        DATE date;
        FILETIME filetime;
        CLSID *puuid;
        CLIPDATA *pclipdata;
        BSTR bstrVal;
        BSTRBLOB bstrblobVal;
        BLOB blob;
        LPSTR pszVal;
        LPWSTR pwszVal;
        IUnknown *punkVal;
        IDispatch *pdispVal;
        IStream *pStream;
        IStorage *pStorage;
        LPVERSIONEDSTREAM pVersionedStream;
        LPSAFEARRAY parray;
        CAC cac;
        CAUB caub;
        CAI cai;
        CAUI caui;
        CAL cal;
        CAUL caul;
        CAH cah;
        CAUH cauh;
        CAFLT caflt;
        CADBL cadbl;
        CABOOL cabool;
        CASCODE cascode;
        CACY cacy;
        CADATE cadate;
        CAFILETIME cafiletime;
        CACLSID cauuid;
        CACLIPDATA caclipdata;
        CABSTR cabstr;
        CABSTRBLOB cabstrblob;
        CALPSTR calpstr;
        CALPWSTR calpwstr;
        CAPROPVARIANT capropvar;
        CHAR *pcVal;
        UCHAR *pbVal;
        SHORT *piVal;
        USHORT *puiVal;
        LONG *plVal;
        ULONG *pulVal;
        INT *pintVal;
        UINT *puintVal;
        FLOAT *pfltVal;
        DOUBLE *pdblVal;
        VARIANT_BOOL *pboolVal;
        DECIMAL *pdecVal;
        SCODE *pscode;
        CY *pcyVal;
        DATE *pdate;
        BSTR *pbstrVal;
        IUnknown **ppunkVal;
        IDispatch **ppdispVal;
        LPSAFEARRAY *pparray;
        PROPVARIANT *pvarVal;
        } */

	}

	[StructLayout( LayoutKind.Sequential )]
	struct PROPERTYKEY
	{
		public Guid fmtid;
		public int pid;
	}


}