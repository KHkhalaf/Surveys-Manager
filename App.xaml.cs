using CrossPieCharts.FormsPlugin.Abstractions;
using PCLStorage;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using SurveyMonkey_Project.Models;
using SurveyMonkey_Project.Services;
using Surveys_Manager.Models;
using Surveys_Manager.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Surveys_Manager
{
    public partial class App : Application
    {
        private IFile file;
        private IFolder folder;

        public List<Survey> Sharedsurveys;

        public List<Survey> OldsharedSurveys;

        public App()
        {
            InitializeComponent();

            MainPage = new Home();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            base.OnStart();
            CrossConnectivity.Current.ConnectivityChanged += HandleConnectivityChanged;
        }
        private async void HandleConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.IsConnected)
            {
                string Content;
                // Post Responsed Surveys
                folder = FileSystem.Current.LocalStorage;
                List<Survey> surveys = new List<Survey>();
                ExistenceCheckResult result = await folder.CheckExistsAsync("ShareResponsedSurveys");
                if (result == ExistenceCheckResult.FolderExists)
                {
                    folder = await folder.CreateFolderAsync("ShareResponsedSurveys", CreationCollisionOption.OpenIfExists);
                    file = await folder.CreateFileAsync("ShareResponsedSurveys", CreationCollisionOption.OpenIfExists);

                    Content = await file.ReadAllTextAsync();
                    if (String.IsNullOrEmpty(Content) || String.IsNullOrWhiteSpace(Content))
                        return;
                    surveys = Serializable_Survey.deserialize(Content).ToList();
                    foreach (Survey S in surveys)
                    {
                        SurveysServices S_S = new SurveysServices();
                        S_S.Set_UrlApi("ShareResponsedSurveys/");
                        await S_S.PostSurveysAsync(S);
                    }
                    folder = FileSystem.Current.LocalStorage;
                    folder = await folder.GetFolderAsync("ShareResponsedSurveys");
                    await folder.DeleteAsync();
                }

                // Receive Surveys
                ReceiveSurveys();
                // Receive Responsed Surveys
                ReceiveResponsedSurveys();
            }
        }
        private async void ReceiveSurveys()
        {
            SurveysServices S_S1 = new SurveysServices();
            ControlFile controlFile = new ControlFile();
            string username = await controlFile.GetUserName();
            S_S1.Set_UrlApi("ReceiveSurveysShared/" + username);
            List<Survey> ReceivedSurveys = await S_S1.GetSurveysAsync();
            ExistenceCheckResult result = await folder.CheckExistsAsync("Receivedsurveys");
            if (result == ExistenceCheckResult.FolderExists)
            {
                folder = await folder.CreateFolderAsync("Receivedsurveys", CreationCollisionOption.OpenIfExists);
                file = await folder.CreateFileAsync("Receivedsurveys", CreationCollisionOption.OpenIfExists);

                string Content = await file.ReadAllTextAsync();
                List<Survey> OldreceiveSurveys = Serializable_Survey.deserialize(Content).ToList();
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
                folder = FileSystem.Current.LocalStorage;
                folder = await folder.CreateFolderAsync("Receivedsurveys", CreationCollisionOption.ReplaceExisting);
                file = await folder.CreateFileAsync("Receivedsurveys", CreationCollisionOption.ReplaceExisting);
                string content = Serializable_Survey.serialize(ReceivedSurveys);
                await file.WriteAllTextAsync(content);
        }
        private async void ReceiveResponsedSurveys() {

            SurveysServices S_S = new SurveysServices();
            ControlFile controlFile = new ControlFile();
            string username = await controlFile.GetUserName();
            S_S.Set_UrlApi("ReceiveMySurveysShared/" + username);
            Sharedsurveys = await S_S.GetSurveysAsync();

            ExistenceCheckResult Resultfolder = await folder.CheckExistsAsync("SharedSurveys");
            if (Resultfolder == ExistenceCheckResult.FolderExists)
            {
                folder = await folder.CreateFolderAsync("SharedSurveys", CreationCollisionOption.OpenIfExists);
                file = await folder.CreateFileAsync("SharedSurveys", CreationCollisionOption.OpenIfExists);
                string Content = await file.ReadAllTextAsync();
                OldsharedSurveys = Serializable_Survey.deserialize(Content).ToList();
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

            string content = Serializable_Survey.serialize(Sharedsurveys);
            await file.WriteAllTextAsync(content);

        }
        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
