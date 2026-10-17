using TerraFX.Interop.Windows;
using Windows.ApplicationModel.Resources.Core;
using WinRT;
using XamlHostingKit;

namespace MrmTool
{
    internal static class Program
    {
        private static App? _xamlApp;

        public static App Application => _xamlApp!;
        public static ResourceMap ResourceMap => XamlApplication.ResourceManager.MainResourceMap;
        public static HWND WindowHandle => (HWND)XamlApplication.MainWindow.WindowHandle.Value;
        public static bool IsWebViewAvailable => XamlApplication.IsWebViewAvailable;

        [STAThread]
        private static void Main()
        {
            ComWrappersSupport.InitializeComWrappers();
            XamlApplication.Start(_ =>
            {
                _xamlApp = new App();
            });
        }

        internal static void Exit()
        {
            XamlApplication.Close();
        }

        /// <summary>Sets the application title to the current file path and its save state.</summary>
        /// <param name="filePath">The path displayed in the title.</param>
        /// <param name="isUnsaved">Whether to append an unsaved-change indicator.</param>
        internal static void SetWindowTitle(string filePath, bool isUnsaved = false)
        {
            XamlApplication.MainWindow.Title = isUnsaved
                ? $"MrmTool - {filePath} *"
                : $"MrmTool - {filePath}";
        }
    }
}
