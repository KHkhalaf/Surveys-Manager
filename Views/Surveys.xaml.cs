using PCLStorage;
using SurveyMonkey_Project.Models;
using SurveyMonkey_Project.Services;
using Surveys_Manager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.LocalNotifications;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Surveys_Manager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Surveys : TabbedPage
    {
        private List<Survey> ListSurveys = null;
        private IFolder folder;
        private IFile file;
        private ControlFile controlFile = new ControlFile();
        private string username;
        private string title, description;
        public Surveys()
        {
            InitializeComponent();
            if (ListSurveys == null)
                ListSurveys = new List<Survey>();
        }
        protected override void OnAppearing()
        {
            Get_Surveys();

            base.OnAppearing();
        }
        protected override bool OnBackButtonPressed()
        {
            if (PopupcreatesurveyEducation.IsVisible || PopupcreatesurveySport.IsVisible || PopupcreatesurveyMarketing.IsVisible || PopupcreatesurveyOther.IsVisible)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (PopupcreatesurveyEducation.IsVisible)
                    {
                        await PopupcreatesurveyEducation.ScaleTo(1.3, 200, Easing.Linear);
                        await PopupcreatesurveyEducation.ScaleTo(1, 200, Easing.Linear);
                        PopupcreatesurveyEducation.IsVisible = false;
                        contentpageEducation.Opacity = 1;
                        checkinputsEducation.Text = "";
                        DescriptionEducation.Text = "";
                        TitleEducation.Text = "";
                    }
                    else if (PopupcreatesurveySport.IsVisible)
                    {
                        await PopupcreatesurveySport.ScaleTo(1.3, 200, Easing.Linear);
                        await PopupcreatesurveySport.ScaleTo(1, 200, Easing.Linear);
                        PopupcreatesurveySport.IsVisible = false;
                        contentpageSport.Opacity = 1;
                        checkinputsSport.Text = "";
                        DescriptionSport.Text = "";
                        TitleSport.Text = "";
                    }
                    else if (PopupcreatesurveyMarketing.IsVisible)
                    {
                        await PopupcreatesurveyMarketing.ScaleTo(1.3, 200, Easing.Linear);
                        await PopupcreatesurveyMarketing.ScaleTo(1, 200, Easing.Linear);
                        PopupcreatesurveyMarketing.IsVisible = false;
                        contentpageMarketing.Opacity = 1;
                        checkinputsMarketing.Text = "";
                        DescriptionMarketing.Text = "";
                        TitleMarketing.Text = "";
                    }
                    else if (PopupcreatesurveyOther.IsVisible)
                    {
                        await PopupcreatesurveyOther.ScaleTo(1.3, 200, Easing.Linear);
                        await PopupcreatesurveyOther.ScaleTo(1, 200, Easing.Linear);
                        PopupcreatesurveyOther.IsVisible = false;
                        contentpageOther.Opacity = 1;
                        checkinputsOther.Text = "";
                        DescriptionOther.Text = "";
                        TitleOther.Text = "";
                    }
                });
                return true;
            }
            return false;
        }

        private async void Create_SurveyEducation(object sender, EventArgs e)
        {
            TitleEducation.Focus();
            contentpageEducation.Opacity = 0.3;
            PopupcreatesurveyEducation.BackgroundColor = Color.FromHex("eeebeb");
            PopupcreatesurveyEducation.IsVisible = true;
            await PopupcreatesurveyEducation.ScaleTo(1.5, 200, Easing.Linear);
            await PopupcreatesurveyEducation.ScaleTo(1.2, 200, Easing.Linear);
        }
        private async void Create_SurveySport(object sender, EventArgs e)
        {
            TitleSport.Focus();
            contentpageSport.Opacity = 0.3;
            PopupcreatesurveySport.BackgroundColor = Color.FromHex("eeebeb");
            PopupcreatesurveySport.IsVisible = true;
            await PopupcreatesurveySport.ScaleTo(1.5, 200, Easing.Linear);
            await PopupcreatesurveySport.ScaleTo(1.2, 200, Easing.Linear);
        }
        private async void Create_SurveyMarketing(object sender, EventArgs e)
        {
            TitleMarketing.Focus();
            contentpageMarketing.Opacity = 0.3;
            PopupcreatesurveyMarketing.BackgroundColor = Color.FromHex("eeebeb");
            PopupcreatesurveyMarketing.IsVisible = true;
            await PopupcreatesurveyMarketing.ScaleTo(1.5, 200, Easing.Linear);
            await PopupcreatesurveyMarketing.ScaleTo(1.2, 200, Easing.Linear);
        }
        private async void Create_SurveyOther(object sender, EventArgs e)
        {
            TitleOther.Focus();
            contentpageOther.Opacity = 0.3;
            PopupcreatesurveyOther.BackgroundColor = Color.FromHex("eeebeb");
            PopupcreatesurveyOther.IsVisible = true;
            await PopupcreatesurveyOther.ScaleTo(1.5, 200, Easing.Linear);
            await PopupcreatesurveyOther.ScaleTo(1.2, 200, Easing.Linear);
        }
        private async void Cancel(object sender, EventArgs e)
        {
            if (PopupcreatesurveyEducation.IsVisible)
            {
                await PopupcreatesurveyEducation.ScaleTo(1.3, 200, Easing.Linear);
                await PopupcreatesurveyEducation.ScaleTo(1, 200, Easing.Linear);
                PopupcreatesurveyEducation.IsVisible = false;
                contentpageEducation.Opacity = 1;
                checkinputsEducation.Text = "";
                DescriptionEducation.Text = "";
                TitleEducation.Text = "";
            }
            else if (PopupcreatesurveySport.IsVisible)
            {
                await PopupcreatesurveySport.ScaleTo(1.3, 200, Easing.Linear);
                await PopupcreatesurveySport.ScaleTo(1, 200, Easing.Linear);
                PopupcreatesurveySport.IsVisible = false;
                contentpageSport.Opacity = 1;
                checkinputsSport.Text = "";
                DescriptionSport.Text = "";
                TitleSport.Text = "";
            }
            else if (PopupcreatesurveyMarketing.IsVisible)
            {
                await PopupcreatesurveyMarketing.ScaleTo(1.3, 200, Easing.Linear);
                await PopupcreatesurveyMarketing.ScaleTo(1, 200, Easing.Linear);
                PopupcreatesurveyMarketing.IsVisible = false;
                contentpageMarketing.Opacity = 1;
                checkinputsMarketing.Text = "";
                DescriptionMarketing.Text = "";
                TitleMarketing.Text = "";
            }
            else if (PopupcreatesurveyOther.IsVisible)
            {
                await PopupcreatesurveyOther.ScaleTo(1.3, 200, Easing.Linear);
                await PopupcreatesurveyOther.ScaleTo(1, 200, Easing.Linear);
                PopupcreatesurveyOther.IsVisible = false;
                contentpageOther.Opacity = 1;
                checkinputsOther.Text = "";
                DescriptionOther.Text = "";
                TitleOther.Text = "";
            }
        }

        private void listSurveys_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            listSurveysEducation.SelectedItem = null;
            listSurveysSport.SelectedItem = null;
            listSurveysMarketing.SelectedItem = null;
            listSurveysOther.SelectedItem = null;

            Navigation.PushAsync(new DetailsSurvey(e));
        }

        private async void Share_MenuItem_Clicked(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                var survey = menuItem.BindingContext as Survey;
                var result = await DisplayAlert(Lang.Resource.sharing, Lang.Resource.detailsMessageShareSurvey + survey.Title_Property + Lang.Resource.survey, Lang.Resource.btnCancelMessage, Lang.Resource.btnOkMessage);
                if (result)
                    return;
                if (CheckNetwork.Check_Connectivity())
                    await Navigation.PushModalAsync(new SharingPage(survey));
            }
        }
        private async void Delete_MenuItem_Clicked(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                var survey = menuItem.BindingContext as Survey;
                var result = await DisplayAlert(Lang.Resource.deleting, Lang.Resource.detailsMessageDeleteSurvey + survey.Title_Property + Lang.Resource.survey, Lang.Resource.btnCancelMessage, Lang.Resource.btnOkMessage);
                if (result)
                    return;
                if (CheckNetwork.Check_Connectivity())
                {
                    SurveysServices S_S = new SurveysServices();
                    S_S.Set_UrlApi("DeleteSurvey/" + survey.Id + "/" + survey.User);
                    await S_S.DeleteSurveysAsync();

                    folder = FileSystem.Current.LocalStorage;
                    folder = await folder.CreateFolderAsync("foldersurveys" + username, CreationCollisionOption.OpenIfExists);
                    file = await folder.CreateFileAsync("filesurveys" + username, CreationCollisionOption.OpenIfExists);

                    string content = await file.ReadAllTextAsync();
                    ListSurveys = Serializable_Survey.deserialize(content).ToList();
                    for (int i = 0; i < ListSurveys.Count; i++)
                    {
                        if (ListSurveys[i].Id == survey.Id)
                            ListSurveys.RemoveAt(i);
                    }
                    content = Serializable_Survey.serialize(ListSurveys);
                    await file.WriteAllTextAsync(content);

                    Full_Lists_Surveys();
                }
            }
        }
        private async void Get_Surveys()
        {
            try
            {
                ActivityIndicator.IsRunning = true;
                contentpageEducation.IsVisible = false;

                folder = FileSystem.Current.LocalStorage;
                ExistenceCheckResult Resultfolder = await folder.CheckExistsAsync("folderUser");

                if (Resultfolder != ExistenceCheckResult.FolderExists)
                {
                    await Navigation.PushAsync(new Login());
                    return;
                }

                username = await controlFile.GetUserName();

                Resultfolder = await folder.CheckExistsAsync("foldersurveys" + username);
                if (Resultfolder != ExistenceCheckResult.FolderExists)
                {
                    if (!CheckNetwork.Check_Connectivity())
                    {
                        await Task.Delay(700);
                        ActivityIndicator.IsRunning = false;
                        contentpageEducation.IsVisible = true;
                        return;
                    }
                    SurveysServices S_S1 = new SurveysServices();
                    S_S1.Set_UrlApi("ReceiveMySurveysSaved/" + username);
                    ListSurveys = await S_S1.GetSurveysAsync();

                    if (ListSurveys == null || ListSurveys.Count == 0)
                    {
                        await Task.Delay(700);
                        ActivityIndicator.IsRunning = false;
                        contentpageEducation.IsVisible = true;
                        return;
                    }

                    folder = await folder.CreateFolderAsync("foldersurveys" + username, CreationCollisionOption.ReplaceExisting);
                    file = await folder.CreateFileAsync("filesurveys" + username, CreationCollisionOption.ReplaceExisting);

                    string Content = Serializable_Survey.serialize(ListSurveys);
                    await file.WriteAllTextAsync(Content);

                    stackEducation.IsVisible = false;

                    Full_Lists_Surveys();

                    await Task.Delay(700);
                    ActivityIndicator.IsRunning = false;
                    contentpageEducation.IsVisible = true;
                    return;
                }
                folder = await folder.CreateFolderAsync("foldersurveys" + username, CreationCollisionOption.OpenIfExists);
                file = await folder.CreateFileAsync("filesurveys" + username, CreationCollisionOption.OpenIfExists);

                string content = await file.ReadAllTextAsync();
                ListSurveys = Serializable_Survey.deserialize(content).ToList();

                if (ListSurveys == null || ListSurveys.Count == 0)
                {
                    await Task.Delay(700);
                    ActivityIndicator.IsRunning = false;
                    contentpageEducation.IsVisible = true;
                    return;
                }
                stackEducation.IsVisible = false;

                Full_Lists_Surveys();

                await Task.Delay(700);
                ActivityIndicator.IsRunning = false;
                contentpageEducation.IsVisible = true;
            }
            catch(Exception ex)
            {
                ActivityIndicator.IsVisible = false;
                ActivityIndicator.IsRunning = false;
                DependencyService.Get<SnackBar>().ShowSnackBar("Error ... cann't access to device storage");
            }
        }
        private void listSurveys_Refreshing(object sender, EventArgs e)
        {
            Get_Surveys();

            listSurveysEducation.EndRefresh();
            listSurveysSport.EndRefresh();
            listSurveysMarketing.EndRefresh();
            listSurveysOther.EndRefresh();
        }
        private void SaveSurveyEducation(object sender, EventArgs e)
        {
            title = TitleEducation.Text;
            description = DescriptionEducation.Text;
            Save_Survey(sender, e, TypeSurvey.Education);
        }
        private void SaveSurveySport(object sender, EventArgs e)
        {
            title = TitleSport.Text;
            description = DescriptionSport.Text;
            Save_Survey(sender, e, TypeSurvey.Sport);
        }
        private void SaveSurveyMarketing(object sender, EventArgs e)
        {
            title = TitleMarketing.Text;
            description = DescriptionMarketing.Text;
            Save_Survey(sender, e, TypeSurvey.Marketing);
        }
        private void SaveSurveyOther(object sender, EventArgs e)
        {
            title = TitleOther.Text;
            description = DescriptionOther.Text;
            Save_Survey(sender, e, TypeSurvey.Other);
        }
        private async void Save_Survey(object sender, EventArgs e, TypeSurvey type)
        {
            string content;
            List<Question> list = new List<Question>();

            folder = FileSystem.Current.LocalStorage;

            Survey survey = new Survey(list, username, title, description, (DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss")).Substring(0, DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss").Length - 8));
            survey.type_Property = type;
            ExistenceCheckResult Result = await folder.CheckExistsAsync("foldersurveys" + username);
            if (Result != ExistenceCheckResult.FolderExists)
            {
                folder = await folder.CreateFolderAsync("foldersurveys" + username, CreationCollisionOption.ReplaceExisting);
            }
            else
            {
                folder = await folder.CreateFolderAsync("foldersurveys" + username, CreationCollisionOption.OpenIfExists);
            }
            Result = await folder.CheckExistsAsync("filesurveys" + username);
            if (Result != ExistenceCheckResult.FileExists)
            {
                file = await folder.CreateFileAsync("filesurveys" + username, CreationCollisionOption.ReplaceExisting);
                survey.Id = 1;
                ListSurveys.Add(survey);
                content = Serializable_Survey.serialize(ListSurveys);
                await file.WriteAllTextAsync(content);

                Full_Lists_Surveys();

                if (CheckNetwork.Check_Connectivity())
                {
                    SurveysServices S_S1 = new SurveysServices();
                    S_S1.Set_UrlApi("SaveSurveys/");
                    await S_S1.PostSurveysAsync(survey);
                }
                stackEducation.IsVisible = false;
                Cancel(sender, e);
                return;
            }
            file = await folder.CreateFileAsync("filesurveys" + username, CreationCollisionOption.OpenIfExists);

            content = await file.ReadAllTextAsync();

            ListSurveys = Serializable_Survey.deserialize(content).ToList();
            if (ListSurveys.Count != 0)
                survey.Id = ListSurveys[ListSurveys.Count - 1].Id + 1;
            else
                survey.Id = 1;
            ListSurveys.Add(survey);
            content = Serializable_Survey.serialize(ListSurveys);
            await file.WriteAllTextAsync(content);

            Full_Lists_Surveys();

            if (CheckNetwork.Check_Connectivity())
            {
                SurveysServices S_S = new SurveysServices();
                S_S.Set_UrlApi("SaveSurveys/");
                await S_S.PostSurveysAsync(survey);
            }
            stackEducation.IsVisible = false;
            Cancel(sender, e);
        }

        private void Full_Lists_Surveys()
        {
            List<Survey> ListSurveyEducation = new List<Survey>();
            List<Survey> ListSurveySport = new List<Survey>();
            List<Survey> ListSurveyMarketing = new List<Survey>();
            List<Survey> ListSurveyOther = new List<Survey>();
            foreach (Survey S in ListSurveys)
            {
                if (S.type == TypeSurvey.Education)
                {
                    ListSurveyEducation.Add(S);
                }
                else if (S.type == TypeSurvey.Sport)
                {
                    ListSurveySport.Add(S);
                }
                else if (S.type == TypeSurvey.Marketing)
                {
                    ListSurveyMarketing.Add(S);
                }
                else if (S.type == TypeSurvey.Other)
                {
                    ListSurveyOther.Add(S);
                }
            }
            ListSurveyEducation.Reverse();
            ListSurveySport.Reverse();
            ListSurveyMarketing.Reverse();
            ListSurveyOther.Reverse();
            listSurveysEducation.ItemsSource = ListSurveyEducation;
            listSurveysSport.ItemsSource = ListSurveySport;
            listSurveysMarketing.ItemsSource = ListSurveyMarketing;
            listSurveysOther.ItemsSource = ListSurveyOther;
        }
    }
}
