		private static IntPtr hwndSysListView32;

		private static IntPtr GetSysListView32( IntPtr hwndShellView )
		{
			EnumChildWindows( hwndShellView, new EnumWndProc( CallbackEnumChildProc_SysListView32 ), IntPtr.Zero );
			return FileOps.hwndSysListView32;
		}

		private static bool CallbackEnumChildProc_SysListView32( IntPtr hwnd, IntPtr lParam )
		{
			StringBuilder sb = new StringBuilder( 255 );
			GetClassName( hwnd, sb, 255 );
			if( sb.ToString() == "SysListView32" )
			{
				FileOps.hwndSysListView32 = hwnd;
				return false;
			}
			else
			{
				FileOps.hwndSysListView32 = IntPtr.Zero;
				return true;
			}
		}
				[DllImport( "user32.dll", CharSet = CharSet.Auto )]
		private static extern IntPtr SendMessage( IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam );

		[DllImport( "user32.dll" )]
		public static extern bool PostMessage( IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam );
		
		[DllImport( "user32.dll", CharSet = CharSet.Auto )]
		private static extern int GetClassName( IntPtr hWnd, StringBuilder lpClassName, int nMaxCount );

		[DllImport( "user32.dll" )]
		private static extern int EnumChildWindows( IntPtr hWndParent, EnumWndProc lpEnumFunc, IntPtr lParam );

		private delegate bool EnumWndProc( IntPtr hwnd, IntPtr lParam );


			const int WM_APPCOMMAND = 0x0319;
			const int APPCOMMAND_COPY = 36;
			const int APPCOMMAND_CUT = 37;
			const int FAPPCOMMAND_OEM = 0x8000;
			const int FAPPCOMMAND_MASK = 0xF000;

			//GET_APPCOMMAND_LPARAM(lParam) ((short)(HIWORD(lParam) & ~FAPPCOMMAND_MASK))
			IntPtr lParam = (IntPtr)( ( FAPPCOMMAND_OEM | ( fCopy ? APPCOMMAND_COPY : APPCOMMAND_CUT ) ) << 16 );
