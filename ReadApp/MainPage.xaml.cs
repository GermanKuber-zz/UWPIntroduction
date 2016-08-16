using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ReadApp
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Button_OnClick(object sender, RoutedEventArgs e)
        {
            var notice = ((FrameworkElement)sender).DataContext as NoticeModel;

            if (notice != null)
                await((MainPageData)this.DataContext).SendEmailAsync(notice);
        }
    }
}
