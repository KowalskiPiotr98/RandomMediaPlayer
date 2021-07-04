using System;
using System.Reflection;
using System.Windows;
using RandomMediaPlayer.Storage;

namespace RandomMediaPlayer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Version AssemblyVersion => Assembly.GetEntryAssembly()?.GetName().Version ?? new Version();
        public static string Version
        {
            get
            {
                var version = AssemblyVersion;
                return $"v{version.Major}.{version.Minor}.{version.Build}"; //I don't know who in their right mind skips "patch", but whatever
            }
        }

        public static App CurrentApp { get; private set; }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            FileStorage.EnsureFileStructureIsPresent();
            CurrentApp = this;
        }

    }
}
