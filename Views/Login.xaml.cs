using SurveyMonkey_Project.Models;
using SurveyMonkey_Project.Services;
using Surveys_Manager.Models;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Surveys_Manager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
        }

        private async void SignUp_clicked(object sender, EventArgs e)
        {
            if (!CheckNetwork.Check_Connectivity())
                return;
            await Navigation.PushAsync(new SignUp(null));
        }

        private async void SignIn_clicked(object sender, EventArgs e)
        {
            if (!CheckNetwork.Check_Connectivity())
                return;

            RegistersServices R_S = new RegistersServices();
            List<User> listregisters = await R_S.GetRegistersAsync();
            if (listregisters == null || listregisters.Count == 0)
            {
                DependencyService.Get<SnackBar>().ShowSnackBar("Don't Have Any Account");
                return;
            }
            await Navigation.PushAsync(new SignIn());
        }
    }
}
