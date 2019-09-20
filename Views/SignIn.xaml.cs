using PCLStorage;
using SurveyMonkey_Project.Models;
using SurveyMonkey_Project.Services;
using Surveys_Manager.Models;
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
    public partial class SignIn : ContentPage
    {
        private IFolder folder;
        private IFile file;
        public SignIn()
        {
            InitializeComponent();
            email.Focus();
        }

        private async void SignIn_clicked(object sender, EventArgs e)
        {
            if (ActivityIndicator.IsRunning)
                return;
            List<User> register = new List<User>();
            var Email = email.Text;
            var Password = password.Text;
            if (String.IsNullOrWhiteSpace(Email) || String.IsNullOrWhiteSpace(Password))
            {
                check_inputs.Text = Lang.Resource.check_inputsSignin1;
                return;
            }
            if (Password.Length > 8)
            {
                check_inputs.Text = Lang.Resource.check_inputsSignin2;
                return;
            }
            if (Password.Contains("~") || Email.Contains("~"))
            {
                check_inputs.Text = Lang.Resource.check_inputsSignin3;
                return;
            }
            var emailpattern = "^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$";
            if (!String.IsNullOrWhiteSpace(Email) && !(Regex.IsMatch(Email, emailpattern)))
            {
                check_inputs.Text = Lang.Resource.check_inputsSignin4;
                return;
            }
            if (!CheckNetwork.Check_Connectivity())
                return;

            ActivityIndicator.IsVisible = true;
            ActivityIndicator.IsRunning = true;
            contentpage.Opacity = 0.5;

            RegistersServices R_S = new RegistersServices();
            List<User> registers = await R_S.GetRegistersBykeywordAsync(Email.Substring(0, Email.IndexOf(".")));
            if (registers != null)
            {
                if (registers.Count > 0 && registers.ElementAt(0).Email_Property == Email)
                {
                    FileAndFolderUser(registers.ElementAt(0));
                    await Task.Delay(500);
                    await Navigation.PopToRootAsync();
                }
                else
                {
                    ActivityIndicator.IsVisible = false;
                    ActivityIndicator.IsRunning = false;
                    contentpage.Opacity = 1;
                    check_inputs.Text = Lang.Resource.check_inputsSignin5;
                    return;
                }
            }
            await Task.Delay(500);
            ActivityIndicator.IsVisible = false;
            ActivityIndicator.IsRunning = false;
            contentpage.Opacity = 1;
        }
        private async void FileAndFolderUser(User register)
        {
            // Folder And File  User
            folder = FileSystem.Current.LocalStorage;
            List<User> registers = new List<User>();
            registers.Add(register);
            string serialize = Serializable_Account.serialize(registers);
            folder = await folder.CreateFolderAsync("folderUser", CreationCollisionOption.ReplaceExisting);
            file = await folder.CreateFileAsync("fileUser", CreationCollisionOption.ReplaceExisting);
            await file.WriteAllTextAsync(serialize);
        }
    }
}
