using PCLStorage;
using SurveyMonkey_Project.Models;
using SurveyMonkey_Project.Services;
using Surveys_Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Surveys_Manager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReceivingPage : TabbedPage
    {
        private IFolder folder;
        private IFile file;
        private List<Survey> ReceivedSurveys = new List<Survey>();
        private List<Survey> OldreceiveSurveys = new List<Survey>();
        private ControlFile controlFile = new ControlFile();
        private string username;
        public ReceivingPage()
        {
            InitializeComponent();
            Device.BeginInvokeOnMainThread(async () => {
                try
                {
                    username = await controlFile.GetUserName();

                    GetSurveys();
                }
                catch (Exception ex)
                {
                    ActivityIndicator.IsVisible = false;
                    ActivityIndicator.IsRunning = false;
                    DependencyService.Get<SnackBar>().ShowSnackBar("Error ... cann't access to device storage");
                }
            });
        }
        private async void GetSurveys()
        {
                string content;
                folder = FileSystem.Current.LocalStorage;
                if (CheckNetwork.Check_Connectivity())
                {
                    ActivityIndicator.IsVisible = true;
                    ActivityIndicator.IsRunning = true;
                    contentpageEducation.IsVisible = false;

                    SurveysServices S_S = new SurveysServices();
                    S_S.Set_UrlApi("ReceiveSurveysShared/" + username);
                    ReceivedSurveys = await S_S.GetSurveysAsync();

                    ExistenceCheckResult result = await folder.CheckExistsAsync("Receivedsurveys");
                    if (result == ExistenceCheckResult.FolderExists)
                    {
                        folder = await folder.CreateFolderAsync("Receivedsurveys", CreationCollisionOption.OpenIfExists);
                        file = await folder.CreateFileAsync("Receivedsurveys", CreationCollisionOption.OpenIfExists);

                        content = await file.ReadAllTextAsync();

                        OldreceiveSurveys = Serializable_Survey.deserialize(content).ToList();
                        if (OldreceiveSurveys.Count < ReceivedSurveys.Count)
                        {
                            var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
                            player.Load("NotificationReceive.mp3");
                            player.Play();

                            Plugin.LocalNotifications.CrossLocalNotifications.Current.Show(Lang.Resource.receivesurveys, Lang.Resource.BodyreceiveSurvey, 0);
                        }
                    }
                    else if (result != ExistenceCheckResult.FolderExists && ReceivedSurveys.Count > 0)
                    {
                        var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
                        player.Load("NotificationReceive.mp3");
                        player.Play();

                        Plugin.LocalNotifications.CrossLocalNotifications.Current.Show(Lang.Resource.receivesurveys, Lang.Resource.BodyreceiveSurvey, 0);
                    }
                }
                else
                {
                    ExistenceCheckResult Result = await folder.CheckExistsAsync("Receivedsurveys");
                    if (Result == ExistenceCheckResult.FolderExists)
                    {
                        ActivityIndicator.IsVisible = true;
                        ActivityIndicator.IsRunning = true;
                        contentpageEducation.IsVisible = false;

                        folder = await folder.CreateFolderAsync("Receivedsurveys", CreationCollisionOption.OpenIfExists);
                        file = await folder.CreateFileAsync("Receivedsurveys", CreationCollisionOption.OpenIfExists);

                        content = await file.ReadAllTextAsync();

                        ReceivedSurveys = Serializable_Survey.deserialize(content).ToList();

                        Full_Lists_Surveys();

                    }
                    else
                    {
                        Full_Lists_Surveys();

                        ActivityIndicator.IsVisible = false;
                        ActivityIndicator.IsRunning = false;
                        await Task.Delay(200);
                        contentpageEducation.IsVisible = true;
                        return;
                    }
                }
                if (ReceivedSurveys.Count == 0)
                {
                    Full_Lists_Surveys();
                    ActivityIndicator.IsVisible = false;
                    ActivityIndicator.IsRunning = false;
                    contentpageEducation.IsVisible = true;
                    return;
                }
                else
                {
                    folder = FileSystem.Current.LocalStorage;
                    folder = await folder.CreateFolderAsync("Receivedsurveys", CreationCollisionOption.ReplaceExisting);
                    file = await folder.CreateFileAsync("Receivedsurveys", CreationCollisionOption.ReplaceExisting);
                    content = Serializable_Survey.serialize(ReceivedSurveys);
                    await file.WriteAllTextAsync(content);
                    Full_Lists_Surveys();

                    await Task.Delay(700);
                    ActivityIndicator.IsVisible = false;
                    ActivityIndicator.IsRunning = false;
                    await Task.Delay(200);
                    contentpageEducation.IsVisible = true;
                }
        }
        private void Full_Lists_Surveys()
        {
            List<Survey> ListSurveyEducation = new List<Survey>();
            List<Survey> ListSurveySport = new List<Survey>();
            List<Survey> ListSurveyMarketing = new List<Survey>();
            List<Survey> ListSurveyOther = new List<Survey>();
            foreach (Survey S in ReceivedSurveys)
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
            if (ListSurveyEducation.Count == 0)
                NoSurveysEducation.IsVisible = true;
            if (ListSurveySport.Count == 0)
                NoSurveysSport.IsVisible = true;
            if (ListSurveyMarketing.Count == 0)
                NoSurveysMarketing.IsVisible = true;
            if (ListSurveyOther.Count == 0)
                NoSurveysOther.IsVisible = true;
            ListSurveyEducation.Reverse();
            ListSurveySport.Reverse();
            ListSurveyMarketing.Reverse();
            ListSurveyOther.Reverse();
            listSurveysEducation.ItemsSource = ListSurveyEducation;
            listSurveysSport.ItemsSource = ListSurveySport;
            listSurveysMarketing.ItemsSource = ListSurveyMarketing;
            listSurveysOther.ItemsSource = ListSurveyOther;
        }
        private async void listSurveys_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            listSurveysEducation.SelectedItem = null;
            listSurveysSport.SelectedItem = null;
            listSurveysMarketing.SelectedItem = null;
            listSurveysOther.SelectedItem = null;

            await Navigation.PushAsync(new DetailsShareSurveyPage(e));
        }

        private void listSurveys_Refreshing(object sender, System.EventArgs e)
        {
            GetSurveys();

            listSurveysEducation.EndRefresh();
            listSurveysSport.EndRefresh();
            listSurveysMarketing.EndRefresh();
            listSurveysOther.EndRefresh();
        }
    }
}
