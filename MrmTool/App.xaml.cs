using Microsoft.System;
using MrmTool.Resources;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Resources;
using XamlHostingKit;

namespace MrmTool
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            var window = XamlWindow.Current;
            DispatcherHelper.SetSynchronizationContext();
            CustomXamlResourceLoader.Current = new XamlResourceLoader();

            window.Title = "MrmTool";

            Frame frame = new();
            frame.Navigate(typeof(MainPage));
            window.Content = frame;
            window.Show();
        }
    }
}
