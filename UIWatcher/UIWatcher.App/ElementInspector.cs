using System.Runtime.InteropServices;
using UIAutomationClient;

namespace UIWatcher
{
    class ElementInspector
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr WindowFromPoint(POINT Point);

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT pt);

        private static readonly CUIAutomation Automation = new();

        public static UIElementInfo? InspectElementUnderCursor()
        {
            if (!GetCursorPos(out POINT pt))
                return null;

            IntPtr hwnd = WindowFromPoint(pt);
            if (hwnd == IntPtr.Zero)
                return null;

            try
            {
                IUIAutomationElement element = Automation.ElementFromHandle(hwnd);
                if (element == null) return null;

                var rect = element.CurrentBoundingRectangle;
                return new UIElementInfo
                {
                    Name = element.CurrentName,
                    AutomationId = element.CurrentAutomationId,
                    ControlType = element.CurrentControlType,
                    ClassName = element.CurrentClassName,
                    LocalizedControlType = element.CurrentLocalizedControlType,
                    BoundingRectangle = (rect.left, rect.top, rect.right, rect.bottom),
                    FrameworkId = element.CurrentFrameworkId,
                    IsEnabled = element.CurrentIsEnabled != 0
                };
            }
            catch
            {
                return null;
            }
        }
    }
}
