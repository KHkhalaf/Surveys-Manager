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
    public partial class SignUp : ContentPage
    {
        private IFolder folder;
        private IFile file;
        private User register;
        private string country;

        public SignUp(User _register)
        {
            InitializeComponent();
            register = _register;
            InitializeData();
            username.Focus();
            this.BindingContext = register;
            country = Lang.Resource.Syria;
            if (register != null)
            {
                verifypassword.IsEnabled = false;
                password.IsEnabled = false;
            }
        }

        private async void SignUp_clicked(object sender, EventArgs e)
        {
            string userName = username.Text;
            string Email = email.Text;
            string Password = password.Text;
            string Age = age.Text;
            if (String.IsNullOrWhiteSpace(Age) || String.IsNullOrWhiteSpace(Email) || String.IsNullOrWhiteSpace(userName) || String.IsNullOrWhiteSpace(Password))
            {
                check_inputs.Text = Lang.Resource.check_inputsSignin1;
                return;
            }
            if (Password.Length > 8)
            {
                check_inputs.Text = Lang.Resource.check_inputsSignin2;
                return;
            }
            string Check = userName + Email + Password + Age;
            if (Check.Contains("~"))
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
            var confirmpassword = verifypassword.Text;
            if (confirmpassword != Password)
            {
                check_inputs.Text = Lang.Resource.confirmPassword;
                return;
            }
            if (!CheckNetwork.Check_Connectivity())
                return;

            ActivityIndicator.IsVisible = true;
            ActivityIndicator.IsRunning = true;
            contentpage.Opacity = 0.5;

            // Folder Accounts
            folder = FileSystem.Current.LocalStorage;
            ExistenceCheckResult Result = await folder.CheckExistsAsync("folderAccount");
            if (Result != ExistenceCheckResult.FolderExists)
            {
                folder = await folder.CreateFolderAsync("folderAccount", CreationCollisionOption.ReplaceExisting);
            }
            else
            {
                folder = await folder.CreateFolderAsync("folderAccount", CreationCollisionOption.OpenIfExists);
            }

            //file = await folder.GetFileAsync("fileAccount");
            //await file.DeleteAsync();
            //return;

            //string serialize; Register r;
            //Result = await folder.CheckExistsAsync("fileAccount");
            //if (Result != ExistenceCheckResult.FileExists)
            //{
            //    //  File Accounts
            //    file = await folder.CreateFileAsync("fileAccount", CreationCollisionOption.ReplaceExisting);
            //    r = new Register(userName, Password, "khalil", Email, DateTime.Now.ToString());
            //    register.Add(r);
            //    serialize = Serializable_Account.serialize(register);
            //    await file.WriteAllTextAsync(serialize);

            //    // Folder And File  User
            //    FileAndFolderUser(serialize);

            //    await Navigation.PopToRootAsync();
            //    return;
            //}
            //file = await folder.CreateFileAsync("fileAccount", CreationCollisionOption.OpenIfExists);
            //string Accounts = await file.ReadAllTextAsync();
            //if (Accounts == "")
            //{
            //    r = new Register(userName, Password, "khalil", Email, DateTime.Now.ToString());
            //    register.Add(r);
            //    serialize = Serializable_Account.serialize(register);
            //    await file.WriteAllTextAsync(serialize);
            //    // Folder And File  User
            //    FileAndFolderUser(serialize);

            //    await Navigation.PopToRootAsync();
            //    return;
            //}
            //register = Serializable_Account.deserialize(Accounts);
            //foreach (Register R in register)
            //{
            //    if (userName.Equals(R.UserName_Property) || Email.Equals(R.Email_Property))
            //    {
            //        check_inputs.Text = "there are same UserName or Email ...";
            //        return;
            //    }
            //}
            //r = new Register(userName, Password, "khalil", Email, DateTime.Now.ToString());
            //register.Add(r);
            //serialize = Serializable_Account.serialize(register);
            //await file.WriteAllTextAsync(serialize);
            //// Folder And File  User
            //register.Clear();
            //register.Add(r);
            //serialize = Serializable_Account.serialize(register);
            //FileAndFolderUser(serialize);
            
            RegistersServices R_S = new RegistersServices();
            
            string _email = Email.Substring(0, Email.IndexOf("."));
            User user = null;
            List<User> listregisters = new List<User>();
            if (register == null)
            {
                listregisters = await R_S.GetRegistersBykeywordAsync(_email);
                if (listregisters.Count == 0)
                {
                    user = new User(userName, Password, Email, Int32.Parse(Age), country, DateTime.Now.ToString().Substring(0, 10));
                    await R_S.PostRegistersAsync(user);
                    listregisters.Add(user);
                    string serialize = Serializable_Account.serialize(listregisters);
                    FileAndFolderUser(serialize);
                    await Task.Delay(500);
                    await Navigation.PopToRootAsync();
                }
                else
                {
                    check_inputs.Text = Lang.Resource.username_same_email;
                    ActivityIndicator.IsVisible = false;
                    ActivityIndicator.IsRunning = false;
                    contentpage.Opacity = 1;
                    return;
                }
            }
            else
            {
                user = new User(userName, Password, Email, Int32.Parse(Age), country, register.Date);
                R_S.Set_UrlApi("Edit/");
                await R_S.PutRegistersAsync(user);
                listregisters.Add(user);
                string serialize = Serializable_Account.serialize(listregisters);
                FileAndFolderUser(serialize);
                await Task.Delay(500);
                await Navigation.PopModalAsync();
            }
        }

        private async void signin_toolbar_Clicked(object sender, EventArgs e)
        {

            if (!CheckNetwork.Check_Connectivity())
                return;

            //folder = FileSystem.Current.LocalStorage;
            //ExistenceCheckResult Resultfolder = await folder.CheckExistsAsync("folderAccount");
            //if (Resultfolder != ExistenceCheckResult.FolderExists)
            //{
            //    DependencyService.Get<SnackBar>().ShowSnackBar("Don't Have Any Account");
            //    return;
            //}
            //folder = await folder.CreateFolderAsync("folderAccount", CreationCollisionOption.OpenIfExists);
            //file = await folder.CreateFileAsync("fileAccount", CreationCollisionOption.OpenIfExists);

            //string Accounts = await file.ReadAllTextAsync();
            //if (Accounts == "")
            //{
            //    DependencyService.Get<SnackBar>().ShowSnackBar("Don't Have Any Account");
            //    return;
            //}
            //await Navigation.PushAsync(new SignIn());


            RegistersServices R_S = new RegistersServices();
            List<User> listregisters = await R_S.GetRegistersAsync();
            if (listregisters == null)
            {
                DependencyService.Get<SnackBar>().ShowSnackBar(Lang.Resource.NotFoundAccounts);
                return;
            }

            //folder = FileSystem.Current.LocalStorage;
            //ExistenceCheckResult Resultfolder = await folder.CheckExistsAsync("folderAccount");
            //if (Resultfolder != ExistenceCheckResult.FolderExists)
            //{
            //    DependencyService.Get<SnackBar>().ShowSnackBar("Don't Have Any Account");
            //    return;
            //}
            //folder = await folder.CreateFolderAsync("folderAccount", CreationCollisionOption.OpenIfExists);
            //file = await folder.CreateFileAsync("fileAccount", CreationCollisionOption.OpenIfExists);


            await Navigation.PushAsync(new SignIn());
        }

        private async void FileAndFolderUser(string serialize)
        {
            // Folder And File  User
            folder = FileSystem.Current.LocalStorage;
            folder = await folder.CreateFolderAsync("folderUser", CreationCollisionOption.ReplaceExisting);
            file = await folder.CreateFileAsync("fileUser", CreationCollisionOption.ReplaceExisting);
            await file.WriteAllTextAsync(serialize);
        }
        private void InitializeData()
        {
            this.Title = Lang.Resource.titleSignup;
            SignIn.Text = Lang.Resource.textSignin;
            username.Placeholder = Lang.Resource.ph_username;
            email.Placeholder = Lang.Resource.ph_email;
            password.Placeholder = Lang.Resource.ph_password;
            verifypassword.Placeholder = Lang.Resource.ph_Ver_password;
            age.Placeholder = Lang.Resource.ph_age;
            if(register != null)
                pickerSelectionCountry.Title = register.Country;
            else
                pickerSelectionCountry.Title = Lang.Resource.Syria;

            // Initilize The Items Picker
            pickerSelectionCountry.Items.Add(Lang.Resource.Syria);
            pickerSelectionCountry.Items.Add(Lang.Resource.Iraq);
            pickerSelectionCountry.Items.Add(Lang.Resource.Jourdan);
            pickerSelectionCountry.Items.Add(Lang.Resource.USA);
            pickerSelectionCountry.Items.Add(Lang.Resource.UIA);
            pickerSelectionCountry.Items.Add(Lang.Resource.KSA);
            pickerSelectionCountry.Items.Add(Lang.Resource.Algeria);
            pickerSelectionCountry.Items.Add(Lang.Resource.Egypt);
            pickerSelectionCountry.Items.Add(Lang.Resource.Moroco);
            pickerSelectionCountry.Items.Add(Lang.Resource.Libya);
            pickerSelectionCountry.Items.Add(Lang.Resource.Tunisia);
            pickerSelectionCountry.Items.Add(Lang.Resource.Mauritania);

            Enter.Text = Lang.Resource.textEnterSignup;
            last1.Text = Lang.Resource.last1;
            last2.Text = Lang.Resource.last2;
            last3.Text = Lang.Resource.last3;
            last4.Text = Lang.Resource.last4;
            last5.Text = Lang.Resource.last5;
            last6.Text = Lang.Resource.last6;
        }

        private void pickerSelectionCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
           country = pickerSelectionCountry.Items[pickerSelectionCountry.SelectedIndex];
        }
    }
}
