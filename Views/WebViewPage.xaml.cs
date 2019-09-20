using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Surveys_Manager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WebViewPage : ContentPage
    {
        public WebViewPage(string URL)
        {
            InitializeComponent();
            myWebview.Source = URL;
        }

        private void myWebview_Navigating(object sender, WebNavigatingEventArgs e)
        {
            ActivityIndicator.IsVisible = true;
        }

        private void myWebview_Navigated(object sender, WebNavigatedEventArgs e)
        {
            ActivityIndicator.IsVisible = false;
        }
    }
}
