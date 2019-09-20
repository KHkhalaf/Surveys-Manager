using PCLStorage;
using SurveyMonkey_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Surveys_Manager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Account : ContentPage
    {
        private IFolder folder;
        private IFile file;
        private User register;
        public Account()
        {
            InitializeComponent();
            
            ActivityIndicator.IsRunning = true;
            contentpage.IsVisible = false;
        }
        protected async override void OnAppearing()
        {
            folder = FileSystem.Current.LocalStorage;
            ExistenceCheckResult Resultfolder = await folder.CheckExistsAsync("folderUser");
            if (Resultfolder != ExistenceCheckResult.FolderExists)
            {
                DependencyService.Get<SnackBar>().ShowSnackBar("Error ... Don't have an account");
            }
            try
            {
                folder = FileSystem.Current.LocalStorage;

                folder = await folder.CreateFolderAsync("folderUser", CreationCollisionOption.OpenIfExists);
                file = await folder.CreateFileAsync("fileUser", CreationCollisionOption.OpenIfExists);
                string AccountUser = await file.ReadAllTextAsync();
                register = Serializable_Account.deserialize(AccountUser).ElementAt(0);
                username.Text = register.UserName_Property;
                Email.Text = register.Email_Property;
                Password.Text = register.Password_property;
                age.Text = register.Age_property.ToString();
                country.Text = register.Country_propety;
                date.Text = register.Date_Property;
                await Task.Delay(700);
                ActivityIndicator.IsRunning = false;
                contentpage.IsVisible = true;
            }
            catch(Exception ex)
            {
                ActivityIndicator.IsVisible = false;
                ActivityIndicator.IsRunning = false;
                DependencyService.Get<SnackBar>().ShowSnackBar("Error ... Don't have an account");
            }
            base.OnAppearing();
        }
        private async void Edit_Gesture_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new SignUp(register));
        }
    }
}
