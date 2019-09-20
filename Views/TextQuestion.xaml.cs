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

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Surveys_Manager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TextQuestion : ContentPage
    {
        private IFolder folder;
        private IFile file;
        private ObservableCollection<Survey> ListSurveys;
        private Survey survey;
        private Text_Question _questionSelected;
        private int _index;
        private ControlFile controlFile = new ControlFile();
        private string username;
        public TextQuestion(ItemTappedEventArgs itemtapped, Question questionSelected, int index)
        {
            InitializeComponent();
            survey = itemtapped.Item as Survey;
            _questionSelected = questionSelected as Text_Question;
            BindingContext = _questionSelected;
            _index = index;
        }

        protected override void OnAppearing()
        {
            questionEntry.Focus();
            base.OnAppearing();
        }
        protected override bool OnBackButtonPressed()
        {
           ClosePage();
            return true;
        }
        private async void SaveData_Activated(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(questionEntry.Text) || questionEntry.Text.Contains("~"))
            {
                checkinputs.Text = Lang.Resource.check_inputsSignin3;
                return;
            }
            Text_Question question = new Text_Question(TypeQuestion.Text, questionEntry.Text);
            question.Initialize(TypeQuestion.Text, questionEntry.Text, switchAnswer.IsToggled, " ");

            // Store Data in File Of Survey
            folder = FileSystem.Current.LocalStorage;

            username = await controlFile.GetUserName();

            folder = await folder.CreateFolderAsync("foldersurveys" + username, CreationCollisionOption.OpenIfExists);
            file = await folder.CreateFileAsync("filesurveys" + username, CreationCollisionOption.OpenIfExists);

            string content = await file.ReadAllTextAsync();
            ListSurveys = Serializable_Survey.deserialize(content);

            if (_questionSelected != null)
            {
                foreach (Survey S in ListSurveys)
                {
                    if (S.Id == survey.Id)
                    {
                        Text_Question questionText = new Text_Question(TypeQuestion.Text, questionEntry.Text);
                        questionText.Initialize(TypeQuestion.Text, questionEntry.Text, switchAnswer.IsToggled, " ");
                        (S.Questions_Property.ElementAt(_index) as Text_Question).Edit_Question(questionText);
                        survey = S;
                        break;
                    }
                }
            }
            else
            {
                foreach (Survey S in ListSurveys)
                {
                    if (S.Id == survey.Id)
                    {
                        S.Questions_Property.Add(question);

                        survey = S;
                        break;
                    }
                }
            }
            if (CheckNetwork.Check_Connectivity())
            {
                SurveysServices S_S = new SurveysServices();
                S_S.Set_UrlApi("EditSurveys/" + username);
                await S_S.PutSurveysAsync(survey);
            }


            content = Serializable_Survey.serialize(ListSurveys.ToList());

            await file.WriteAllTextAsync(content);

            await Navigation.PopModalAsync();
        }

        private async void closePageRecognizer_Tapped(object sender, EventArgs e)
        {
            bool res = false;
            stackquestions.BackgroundColor = Color.FromHex("e5e5e5");
            res = await DisplayAlert(Lang.Resource.titleMessage, Lang.Resource.bodyMessage, Lang.Resource.btnOkMessage, Lang.Resource.btnCancelMessage);
            if (res)
                await Navigation.PopModalAsync();
            stackquestions.BackgroundColor = Color.FromHex("ffffff");
        }
        private void ClosePage()
        {
            closePageRecognizer_Tapped(new object(), new EventArgs());
        }
    }
}
