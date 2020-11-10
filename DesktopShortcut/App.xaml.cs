using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using IWshRuntimeLibrary;

namespace DesktopShortcut
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Create(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None));
        }

        private void Create(Configuration configuration)
        {
            var settings = configuration.AppSettings.Settings;
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var applicationPath = Path.Combine(baseDirectory, settings["applicationPath"].Value);
            var applicationConfiguration = ConfigurationManager.OpenExeConfiguration(applicationPath);
            var applicationName = applicationConfiguration.AppSettings.Settings["applicationName"].Value;
            var description = settings["description"].Value;
            var desktopDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            var shortcutPath = Path.Combine(desktopDirectory, $"{applicationName}.lnk");
            var shell = new WshShell();
            var shortcut = (IWshShortcut) shell.CreateShortcut(shortcutPath);
            shortcut.TargetPath = applicationPath;
            shortcut.WorkingDirectory = Path.GetDirectoryName(applicationPath);
            shortcut.WindowStyle = 1;
            shortcut.Description = description;
            shortcut.Save();
        }
    }
}
