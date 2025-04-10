using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using UIWatcher;

class Program
{
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT { public int X, Y; }

    [DllImport("user32.dll")] static extern bool GetCursorPos(out POINT lpPoint);
    [DllImport("user32.dll")] static extern short GetAsyncKeyState(int vKey);
    const int VK_CONTROL = 0x11;

    static async Task Main()
    {
        using var cts = new CancellationTokenSource();

        Task mouseTask = Task.Run(() => TrackMouseMovement(100, OnMouseMoved, cts.Token));
        Task ctrlTask = Task.Run(() => TrackCtrlPressed(100, OnCtrlPressed, cts.Token));

        Console.WriteLine("Tracking started. Press Enter to stop...");
        Console.ReadLine();
        cts.Cancel();

        await Task.WhenAll(mouseTask, ctrlTask);
    }

    static void TrackMouseMovement(int intervalMs, Action<POINT> onMove, CancellationToken token)
    {
        GetCursorPos(out POINT lastPos);
        while (!token.IsCancellationRequested)
        {
            if (GetCursorPos(out POINT currentPos) && !PointsAreEqual(currentPos, lastPos))
            {
                lastPos = currentPos;
                onMove(currentPos);
            }
            token.WaitHandle.WaitOne(intervalMs);
        }
    }

    static void TrackCtrlPressed(int intervalMs, Action onCtrlPressed, CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            if ((GetAsyncKeyState(VK_CONTROL) & 0x8000) != 0)
            {
                onCtrlPressed();
            }
            token.WaitHandle.WaitOne(intervalMs);
        }
    }

    static bool PointsAreEqual(POINT a, POINT b) => a.X == b.X && a.Y == b.Y;

    static void OnMouseMoved(POINT pos) =>
        Console.WriteLine($"Mouse moved to: X={pos.X}, Y={pos.Y}");

    static void OnCtrlPressed()
    {
        Console.WriteLine("[Ctrl Pressed]");
        var elementInfo = ElementInspector.InspectElementUnderCursor();
        if (elementInfo == null) return;

        Console.WriteLine("Element under cursor:");
        Console.WriteLine($"- Name: {elementInfo.Name ?? "[none]"}");
        Console.WriteLine($"- AutomationId: {elementInfo.AutomationId ?? "[none]"}");
        Console.WriteLine($"- ControlType: {elementInfo.ControlType}");
        Console.WriteLine($"- ClassName: {elementInfo.ClassName ?? "[none]"}");
        Console.WriteLine($"- LocalizedControlType: {elementInfo.LocalizedControlType ?? "[none]"}");
        Console.WriteLine($"- BoundingRectangle: {elementInfo.BoundingRectangle.Left}, {elementInfo.BoundingRectangle.Top}, {elementInfo.BoundingRectangle.Right}, {elementInfo.BoundingRectangle.Bottom}");
        Console.WriteLine($"- FrameworkId: {elementInfo.FrameworkId ?? "[none]"}");
        Console.WriteLine($"- IsEnabled: {elementInfo.IsEnabled}");
    }
}
