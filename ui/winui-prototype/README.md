# WinUI Prototype for MeGUI

This branch contains a WinUI 3 prototype demonstrating Option A (Windows 11 visual language) and a full-behavior xvid settings page that saves/loads a JSON file in packages/video/xvid/xvid_settings_demo.json.

How to build
- Requires Windows 10/11 with Windows App SDK support and Visual Studio 2022/2023 with .NET 7 workload.
- Open the solution or the csproj at ui/winui-prototype/WinUIPrototype.csproj and build.

What this prototype includes
- MainWindow with NavigationView (left rail), content host and right inspector.
- DesignTokens.xaml with colors and brushes.
- XvidSettingsPage: two-way bound controls to a ViewModel and JSON save/load to demonstrate full behavior.

Next steps
- Swap demo model with the real settings class in packages/video/xvid when ready (adapter will be provided).
- Add more pages for other configuration panels and wire commands to the backend.
