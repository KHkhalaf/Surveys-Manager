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
    public partial class SharedSurveys : TabbedPage
    {
        private IFolder folder;
        private IFile file;
        private ControlFile controlFile = new ControlFile();
        private string username;
        private List<Survey> Sharedsurveys = new List<Survey>();
        private List<Survey> OldsharedSurveys = new List<Survey>();

        public SharedSurveys()
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
                    S_S.Set_UrlApi("ReceiveMySurveysShared/" + username);
                    Sharedsurveys = await S_S.GetSurveysAsync();

                    ExistenceCheckResult Resultfolder = await folder.CheckExistsAsync("SharedSurveys");
                    if (Resultfolder == ExistenceCheckResult.FolderExists)
                    {
                        folder = await folder.CreateFolderAsync("SharedSurveys", CreationCollisionOption.OpenIfExists);
                        file = await folder.CreateFileAsync("SharedSurveys", CreationCollisionOption.OpenIfExists);
                        content = await file.ReadAllTextAsync();
                        OldsharedSurveys = Serializable_Survey.deserialize(content).ToList();
                    }
                    if (OldsharedSurveys.Count < Sharedsurveys.Count)
                    {
                        var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
                        player.Load("NotificationReceive.mp3");
                        player.Play();

                        Plugin.LocalNotifications.CrossLocalNotifications.Current.Show(Lang.Resource.responsedٍ_Sh_Surveys, Lang.Resource.BodyResponsedٍ_Sh_Surveys, 1);
                    }

                    folder = FileSystem.Current.LocalStorage;
                    folder = await folder.CreateFolderAsync("SharedSurveys", CreationCollisionOption.ReplaceExisting);
                    file = await folder.CreateFileAsync("SharedSurveys", CreationCollisionOption.ReplaceExisting);

                    content = Serializable_Survey.serialize(Sharedsurveys);
                    await file.WriteAllTextAsync(content);
                    Full_Lists_Surveys();
                }
                else
                {
                    ExistenceCheckResult Resultfolder = await folder.CheckExistsAsync("SharedSurveys");
                    if (Resultfolder == ExistenceCheckResult.FolderExists)
                    {
                        ActivityIndicator.IsVisible = true;
                        ActivityIndicator.IsRunning = true;
                        contentpageEducation.IsVisible = false;

                        folder = await folder.CreateFolderAsync("SharedSurveys", CreationCollisionOption.OpenIfExists);
                        file = await folder.CreateFileAsync("SharedSurveys", CreationCollisionOption.OpenIfExists);
                        content = await file.ReadAllTextAsync();
                        Sharedsurveys = Serializable_Survey.deserialize(content).ToList();
                        Full_Lists_Surveys();
                    }
                }
                ActivityIndicator.IsRunning = false;
                ActivityIndicator.IsVisible = false;
                await Task.Delay(200);
                contentpageEducation.IsVisible = true;
        }
        private void Full_Lists_Surveys()
        {
            List<Survey> ListSurveyEducation = new List<Survey>();
            List<Survey> ListSurveySport = new List<Survey>();
            List<Survey> ListSurveyMarketing = new List<Survey>();
            List<Survey> ListSurveyOther = new List<Survey>();
            foreach (Survey S in Sharedsurveys)
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

            await Navigation.PushAsync(new DetailsSurveyWithAnalyze(e));
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
