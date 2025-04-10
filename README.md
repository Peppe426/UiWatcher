### 📦 **UIWatcher** – Inspect UI Elements on Windows in Real Time

**UIWatcher** is a lightweight .NET tool that lets you inspect and monitor UI elements under the mouse cursor using Windows UI Automation. It’s ideal for developers building or testing WinForms, WPF, or other desktop applications.

#### 🔍 Features
- Real-time mouse tracking
- CTRL-key triggered element inspection
- Extracts:
  - Automation ID
  - Name, ClassName, Framework ID
  - Control Type & Localized Control Type
  - Bounding Rectangle
  - Enabled state

#### ⚙️ Built With
- .NET
- P/Invoke (`user32.dll`)
- COM interop via `UIAutomationClient`

#### 🚀 Use Case
- UI debugging and exploration
- Test automation helper
- Accessibility checks
