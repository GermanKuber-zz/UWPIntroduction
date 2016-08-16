using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TestAsync
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            btnClick.Content = "Click !";
            for (int i = 0; i < 99999; i++)
            {
                btnClick.Content = $"Click ! - {i}";
            }
        }

        private async void btnClickAsynx_Click(object sender, RoutedEventArgs e)
        {
            await AsyncClick();
        }
        private async Task AsyncClick()
        {
            await Task.Run(() =>
            {
                for (int i = 0; i < 999999; i++)
                    Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                        () =>
                        {
                            if (i % 100 == 0)
                            {
                                btnClickAsync.Content = $"Click Async ! - {i}";
                            }

                        });
            });
        }
    }
}
