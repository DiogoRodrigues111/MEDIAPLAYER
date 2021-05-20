using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace MEDIA_PLAYER
{
	public class Taskbar
	{
		[DllImport("user32.dll", EntryPoint = "GetSystemMetrics")]
		public static extern int GetSystemMetrics(int which);

		[DllImport("user32.dll")]
		public static extern void
			SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
						 int x, int y, int width, int height, int flags);

		private const int SM_CXSCREEN = 0;
		private const int SM_CYSCREEN = 1;
		private static IntPtr HWND_TOP = IntPtr.Zero;
		private const int SWP_SHOWWINDOW = 64; // 0x0040

		public static int ScreenX
		{
			get { return GetSystemMetrics(SM_CXSCREEN); }
		}

		public static int ScreenY
		{
			get { return GetSystemMetrics(SM_CYSCREEN); }
		}

		public static void SetWinFullScreen(IntPtr hWnd)
		{
			SetWindowPos(hWnd, HWND_TOP, -7, -7, ScreenX *2, ScreenY *2, SWP_SHOWWINDOW);
		}

		public class WindowFullscreenTaskbar
		{
			public static bool IsMaximize = false;
		}
	}
}
