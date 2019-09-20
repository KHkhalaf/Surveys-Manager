using PCLStorage;
using SurveyMonkey_Project.Models;
using SurveyMonkey_Project.Services;
using Surveys_Manager.Models;
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
    public partial class SharingPage : ContentPage
    {
        private List<User> Users = new List<User>();
        private List<User> SelectedUsers = new List<User>();
        private RegistersServices R_S = new RegistersServices();
        private int countSelected = 0;
        private Survey _survey;
        private IFolder folder;
        private IFile file;

        public SharingPage(Survey survey)
        {
            InitializeComponent();
            ActivityIndicator.IsRunning = true;
            contentpage.IsVisible = false;
            GetUsers();
            _survey = survey;
        }
        private async void GetUsers()
        {
            Users = await R_S.GetRegistersAsync();

            folder = FileSystem.Current.LocalStorage;
            folder = await folder.CreateFolderAsync("folderUser", CreationCollisionOption.OpenIfExists);
            file = await folder.CreateFileAsync("fileUser", CreationCollisionOption.OpenIfExists);
            string AccountUser = await file.ReadAllTextAsync();
            User register = Serializable_Account.deserialize(AccountUser).ElementAt(0);
            Users = Users.Where(user => user.Email != register.Email).ToList();

            ListUsers.ItemsSource = Users;
            await Task.Delay(400);
            ActivityIndicator.IsRunning = false;
            contentpage.IsVisible = true;
        }

        private void SelectedItemRecognizer_Tapped(object sender, ItemTappedEventArgs e)
        {

            var stack = sender as StackLayout;
            string username = ((stack.Children[1] as StackLayout).Children[0] as Label).Text;
            string Email = ((stack.Children[1] as StackLayout).Children[1] as Label).Text;
            var register = new User(username, "", Email, 0, "", "");
            if (stack.BackgroundColor == Color.White && (SelectedUsers.Where(user => user.Email == Email)).Count() == 0)
            {
                stack.BackgroundColor = Color.FromHex("bae8f8");
                countSelected++;
                DependencyService.Get<SnackBar>().ShowSnackBar(username + " " + Lang.Resource.selected);
                SelectedUsers.Add(register);
            }
            else
            {
                stack.BackgroundColor = Color.White;
                if (countSelected > 0)
                    countSelected--;
                DeleteDisSelectedUser(Email);
                DependencyService.Get<SnackBar>().ShowSnackBar(username + " " + Lang.Resource.unSelected);
            }
            countUsersSelected.Text = countSelected.ToString();
        }

        private async void Sharing_Clicked(object sender, EventArgs e)
        {
            if (!CheckNetwork.Check_Connectivity())
                return;
            if (countSelected == 0)
            {
                DependencyService.Get<SnackBar>().ShowSnackBar(Lang.Resource.NoAccountsSelected);
                return;
            }

            var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            player.Load("NotificationSend.mp3");
            player.Play();

            await ((Image)sender).ScaleTo(0.8, 50, Easing.Linear);
            await Task.Delay(30);
            await ((Image)sender).ScaleTo(1, 50, Easing.Linear);
            await ((Image)sender).TranslateTo(-85, -200, 200,Easing.CubicIn);
            await ((Image)sender).TranslateTo(0, -750, 100,Easing.CubicIn);

            _survey.Date_Property = DateTime.Now.ToString();
            _survey.Number_of_Participants = SelectedUsers.Count;

            ActivityIndicator.IsRunning = true;
            contentpage.IsVisible = false;


            for (int i = 0; i < SelectedUsers.Count; i++)
            {
                PostSurveysByNames(SelectedUsers.ElementAt(i).UserName_Property);
            }

            await Task.Delay(200);
            ActivityIndicator.IsRunning = false;
            contentpage.IsVisible = true;
            await Navigation.PopModalAsync();
        }
        private void DeleteDisSelectedUser(string email)
        {
            foreach (User user in SelectedUsers)
                if (user.Email == email)
                {
                    SelectedUsers.Remove(user);
                    return;
                }
        }
        private async void PostSurveysByNames(string UserName)
        {
            SurveysServices S_S = new SurveysServices();
            S_S.Set_UrlApi("ShareSurveys/" + UserName);
            await S_S.PostSurveysAsync(_survey);
        }

        private void ListUsers_Refreshing(object sender, EventArgs e)
        {
            GetUsers();
            ListUsers.EndRefresh();
        }
        private void entrySearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var keyWord = entrySearch.Text;
            var Suggestions = Users.Where(
                user => user.UserName.ToLower().Contains(keyWord.ToLower())
            ||  user.Email.ToLower().Contains(keyWord.ToLower())); 
            ListUsers.ItemsSource = Suggestions;
        }
        
        //private void ListUsers_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        //{
        //    var register = e.SelectedItem as User;

        //    if (SelectedUsers.Contains(register))
        //    {
        //        SelectedUsers.Remove(register);
        //        DependencyService.Get<SnackBar>().ShowSnackBar(register.UserName_Property + " " + Lang.Resource.unSelected);
        //        countSelected--;
        //        return;
        //    }
        //    DependencyService.Get<SnackBar>().ShowSnackBar(register.UserName_Property + " " + Lang.Resource.selected);
        //    countSelected++;
        //    SelectedUsers.Add(register);
        //}

    }
}
