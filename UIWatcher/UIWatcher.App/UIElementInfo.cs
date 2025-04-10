namespace UIWatcher;

public class UIElementInfo
{
    public string Name { get; set; }
    public string AutomationId { get; set; }
    public int ControlType { get; set; }
    public string ClassName { get; set; }
    public string LocalizedControlType { get; set; }
    public (int Left, int Top, int Right, int Bottom) BoundingRectangle { get; set; }
    public string FrameworkId { get; set; }
    public bool IsEnabled { get; set; }
}
