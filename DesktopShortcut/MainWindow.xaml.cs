using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DesktopShortcut
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly FadeManager manager = new FadeManager()
        {
            In = 1,
            Out = 0,
            MillisecondsDelay = 1000
        };

        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            manager.FadeIn(this, FadeIn_Completed);
        }

        private async void FadeIn_Completed(object sender, EventArgs e)
        {
            await Task.Delay(1000);
            manager.FadeOut(this, FadeOut_Completed);
        }

        private void FadeOut_Completed(object sender, EventArgs e)
        {
            Close();
        }
    }

    public class FadeManager
    {
        private readonly FillBehavior behavior = FillBehavior.Stop;
        public double MillisecondsDelay { get; set; }
        public double In { get; set; }
        public double Out { get; set; }

        public void FadeIn(UIElement element, EventHandler completed)
        {
            var animation = new DoubleAnimation()
            {
                From = Out,
                To = In,
                Duration = TimeSpan.FromMilliseconds(MillisecondsDelay),
                FillBehavior = behavior,
            };
            animation.Completed += completed;
            element.BeginAnimation(UIElement.OpacityProperty, animation);
        }

        public void FadeOut(UIElement element, EventHandler completed)
        {
            var animation = new DoubleAnimation()
            {
                From = In,
                To = Out,
                Duration = TimeSpan.FromMilliseconds(MillisecondsDelay),
                FillBehavior = behavior,
            };
            animation.Completed += completed;
            element.BeginAnimation(UIElement.OpacityProperty, animation);
        }
    }
}
