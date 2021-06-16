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
using System.Windows.Threading;
using IWshRuntimeLibrary;
using File = System.IO.File;

namespace DesktopShortcut
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private const string ConfigurationExt = ".config";

        protected override void OnStartup(StartupEventArgs e)
        {
            DispatcherUnhandledException +=OnDispatcherUnhandledException;
            base.OnStartup(e);
            Create(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None));
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var message = e.Exception?.Message;
            var caption = GetType().Assembly.GetName().Name;
            var button = MessageBoxButton.OK;
            var icon = MessageBoxImage.Error;
            MessageBox.Show(message, caption, button, icon);
        }

        private void Create(Configuration configuration)
        {
            var settings = configuration.AppSettings.Settings;
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var applicationPathKey = "applicationPath";
            if (!settings.AllKeys.Contains(applicationPathKey))
            {
                throw new Exception("请配置应用程序路径");
            }
            
            var applicationPath = Path.Combine(baseDirectory, settings["applicationPath"].Value);
            var applicationConfigurationPath = applicationPath + ConfigurationExt;
            // 复制应用程序并改名
            var applicationName = settings["applicationName"].Value;
            var exeName = applicationName + Path.GetExtension(applicationPath);
            var exePath = Path.Combine(baseDirectory, exeName);
            var exeConfigurationName = exeName + ConfigurationExt;
            var exeConfigurationPath = Path.Combine(baseDirectory, exeConfigurationName);
            File.Copy(applicationPath, exePath);
            File.Copy(applicationConfigurationPath, exeConfigurationPath);
            // 生存快捷方式
            var description = settings["description"].Value;
            var desktopDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            var shortcutPath = Path.Combine(desktopDirectory, $"{applicationName}.lnk");
            var shell = new WshShell();
            var shortcut = (IWshShortcut) shell.CreateShortcut(shortcutPath);
            shortcut.TargetPath = exePath;
            shortcut.WorkingDirectory = Path.GetDirectoryName(exePath);
            shortcut.WindowStyle = 1;
            shortcut.Description = description;
            shortcut.Save();
        }
    }
}
