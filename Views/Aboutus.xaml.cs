using SurveyMonkey_Project.Models;
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
    public partial class Aboutus : ContentPage
    {
        public Aboutus()
        {
            InitializeComponent();
        }

        private void GoToFaceBook_Recognizer_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (((Label)sender).Text.Contains("Khalil"))
                {
                    Navigation.PushModalAsync(new WebViewPage("https://m.facebook.com/sameera.shaar.9?ref=bookmarks"));
                }
                else if(((Label)sender).Text.Contains("Khalied"))
                {
                    Navigation.PushModalAsync(new WebViewPage("https://m.facebook.com/profile.php?id=100006393662749&ref=content_filter"));
                }
                else if (((Label)sender).Text.Contains("Imad"))
                {
                    Navigation.PushModalAsync(new WebViewPage("https://m.facebook.com/imad.alzayat?fref=nf&ref=bookmarks"));
                }
                else if (((Label)sender).Text.Contains("Dyaa"))
                {
                    Navigation.PushModalAsync(new WebViewPage("https://m.facebook.com/dyaa.tohan?fref=pb&ref=bookmarks"));
                }
                else if (((Label)sender).Text.Contains("Hadi"))
                {
                    Navigation.PushModalAsync(new WebViewPage("https://m.facebook.com/profile.php?id=100014278849402&ref=content_filter"));
                }
            }
            catch(Exception ex)
            {
                DependencyService.Get<SnackBar>().ShowSnackBar(ex.ToString());
            }
        }
    }
}
