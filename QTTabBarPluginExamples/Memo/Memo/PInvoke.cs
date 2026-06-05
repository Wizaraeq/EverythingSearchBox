using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace QuizoPlugin
{
	static class PInvoke
	{
		[DllImport( "user32.dll", CharSet = CharSet.Auto )]
		public static extern IntPtr SendMessage( IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam );

		[DllImport( "user32.dll" )]
		public static extern bool SetWindowPos( IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags );
		//#define SWP_NOSIZE          0x0001
		//#define SWP_NOMOVE          0x0002
		//#define SWP_NOZORDER        0x0004
		//#define SWP_NOREDRAW        0x0008
		//#define SWP_NOACTIVATE      0x0010
		//#define SWP_FRAMECHANGED    0x0020  /* The frame changed: send WM_NCCALCSIZE */
		//#define SWP_SHOWWINDOW      0x0040
		//#define SWP_HIDEWINDOW      0x0080
		//#define SWP_NOCOPYBITS      0x0100
		//#define SWP_NOOWNERZORDER   0x0200  /* Don't do owner Z ordering */
		//#define SWP_NOSENDCHANGING  0x0400  /* Don't send WM_WINDOWPOSCHANGING */

		//#define SWP_DRAWFRAME       SWP_FRAMECHANGED
		//#define SWP_NOREPOSITION    SWP_NOOWNERZORDER

		//#if(WINVER >= 0x0400)
		//#define SWP_DEFERERASE      0x2000
		//#define SWP_ASYNCWINDOWPOS  0x4000
		//#endif /* WINVER >= 0x0400 */


		//#define HWND_TOP        ((HWND)0)
		//#define HWND_BOTTOM     ((HWND)1)
		//#define HWND_TOPMOST    ((HWND)-1)
		//#define HWND_NOTOPMOST  ((HWND)-2)

		[DllImport( "user32.dll" )]
		private static extern int SetWindowLong( IntPtr hWnd, GWL nIndex, uint dwNewLong );

		[DllImport( "user32.dll", EntryPoint = "SetWindowLongPtr" )]
		private static extern IntPtr SetWindowLongPtr64( IntPtr hWnd, GWL nIndex, IntPtr dwNewLong );

		public static IntPtr SetWindowLongPtr( IntPtr hWnd, GWL nIndex, IntPtr dwNewLong )
		{
			if( IntPtr.Size == 8 )
			{
				return SetWindowLongPtr64( hWnd, nIndex, dwNewLong );
			}
			else
			{
				return new IntPtr( SetWindowLong( hWnd, nIndex, (uint)dwNewLong ) );
			}
		}

		[DllImport( "user32.dll" )]
		private static extern int GetWindowLong( IntPtr hWnd, GWL nIndex );

		[DllImport( "user32.dll", EntryPoint = "GetWindowLongPtr" )]
		private static extern IntPtr GetWindowLongPtr64( IntPtr hWnd, GWL nIndex );

		public static IntPtr GetWindowLongPtr( IntPtr hWnd, GWL nIndex )
		{
			if( IntPtr.Size == 8 )
			{
				return GetWindowLongPtr64( hWnd, nIndex );
			}
			else
			{
				return new IntPtr( GetWindowLong( hWnd, nIndex ) );
			}
		}

		/// <summary>
		/// represents a bitwise OR operation of IntPtr and unsigned integer
		/// </summary>
		public static IntPtr Ptr_OP_OR( IntPtr ptr, uint u )
		{
			if( IntPtr.Size == 8 )
			{
				return (IntPtr)(long)( ( (ulong)ptr ) | u );
			}
			else
			{
				return (IntPtr)(int)( ( (uint)ptr ) | u );
			}
		}

	}

	enum GWL
	{
		//WNDPROC		= ( -4 ),
		//HINSTANCE	= ( -6 ),
		HWNDPARENT = ( -8 ),
		STYLE = ( -16 ),
		EXSTYLE = ( -20 ),
		//USERDATA	= ( -21 ),
		//ID			= ( -12 ),
	}

	static class W32
	{
		public const uint TBS_TRANSPARENTBKGND = 0x1000;	// vista-

	}
}
