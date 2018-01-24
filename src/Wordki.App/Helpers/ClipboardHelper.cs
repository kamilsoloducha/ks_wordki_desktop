using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Wordki.Helpers {
  public class ClipboardHelper {

    private IntPtr hWndNextViewer;
    private HwndSource hWndSource;
    private Window window;
    public delegate void OnNewClipboardDelegate(string pText);
    public OnNewClipboardDelegate OnNewClipboard { get; set; }
    public Boolean IsRunning { get; set; }

    public ClipboardHelper(Window pWindow) {
      window = pWindow;
      IsRunning = false;
    }

    public void InitCBViewer() {
      WindowInteropHelper wih = new WindowInteropHelper(window);
      hWndSource = HwndSource.FromHwnd(wih.Handle);

      hWndSource.AddHook(WinProc);
      hWndNextViewer = Win32.SetClipboardViewer(hWndSource.Handle);
      IsRunning = true;
    }

    public void CloseCBViewer() {
      Win32.ChangeClipboardChain(hWndSource.Handle, hWndNextViewer);
      hWndNextViewer = IntPtr.Zero;
      hWndSource.RemoveHook(WinProc);
      IsRunning = false;
    }

    private IntPtr WinProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) {
      switch (msg) {
        case Win32.WM_CHANGECBCHAIN:
          if (wParam == hWndNextViewer) {
            hWndNextViewer = lParam;
          } else if (hWndNextViewer != IntPtr.Zero) {
            Win32.SendMessage(hWndNextViewer, msg, wParam, lParam);
          }
          break;
        case Win32.WM_DRAWCLIPBOARD:
          LoggerSingleton.LogInfo(Clipboard.GetText());
          if (OnNewClipboard != null) {
            OnNewClipboard(Clipboard.GetText());
          }
          Win32.SendMessage(hWndNextViewer, msg, wParam, lParam);
          break;
      }
      return IntPtr.Zero;
    }
  }

  internal static class Win32 {
    internal const int WM_DRAWCLIPBOARD = 0x0308;

    internal const int WM_CHANGECBCHAIN = 0x030D;

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
  }
}
