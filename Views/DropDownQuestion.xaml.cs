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
    public partial class DropDownQuestion : ContentPage
    {
        private IFolder folder;
        private IFile file;
        private List<Entry> entries;
        private ObservableCollection<Survey> ListSurveys;
        private Survey survey;
        private Multible _questionSelected;
        private int _index;
        private int count_Visable = 2;
        private ControlFile controlFile = new ControlFile();
        private string username;
        public DropDownQuestion(ItemTappedEventArgs itemtapped, Question questionSelected, int index)
        {
            InitializeComponent();
            survey = itemtapped.Item as Survey;
            _questionSelected = questionSelected as Multible;
            BindingContext = _questionSelected;
            entries = new List<Entry>();
            _index = index;
        }

        protected override void OnAppearing()
        {
            questionEntry.Focus();
            if (_questionSelected != null)
            {
                entry1.Text = _questionSelected.answer[0];
                entry2.Text = _questionSelected.answer[1];
                if (_questionSelected.answer[_questionSelected.answer.Length - 1].Equals("Other"))
                {
                    switchOtherAnswer.IsToggled = true;
                    for (int i = 2; i < _questionSelected.answer.Length - 1; i++)
                        stackAnswerChoice.Children.Add(GetElements.GetAnswerChoics(_questionSelected.answer[i]));
                }
                else
                {
                    for (int i = 2; i < _questionSelected.answer.Length; i++)
                        stackAnswerChoice.Children.Add(GetElements.GetAnswerChoics(_questionSelected.answer[i]));
                }

            }
            base.OnAppearing();
        }

        protected override bool OnBackButtonPressed()
        {
            ClosePage();
            return true;
        }
        private async void SaveData_Activated(object sender, EventArgs e)
        {
            int count = stackAnswerChoice.Children.Count;
            for (int i = 0; i < count; i++)
            {
                Grid G = stackAnswerChoice.Children.ElementAt(i) as Grid;
                Entry E = G.Children.ElementAt(0) as Entry;
                if (String.IsNullOrWhiteSpace(E.Text))
                {
                    E.IsVisible = false;
                    continue;
                }
                if (E.IsVisible && E.Text.Contains("~"))
                {
                    checkinputs.Text = Lang.Resource.check_inputsSignin3;
                    return;
                }
                if (!String.IsNullOrWhiteSpace(E.Text) && G.IsVisible)
                    entries.Add(E);

            }
            List<string> choices = new List<string>();
            for (int i = 0; i < entries.Count; i++)
                if (entries[i].IsVisible)
                    choices.Add(entries[i].Text);
            if (switchOtherAnswer.IsToggled)
                choices.Add("Other");
            string[] answers = new string[choices.Count];
            for (int i = 0; i < choices.Count; i++)
                answers[i] = choices[i];
            Multible question = new Multible(TypeQuestion.DropDown, questionEntry.Text, answers.Length);

            question.require_Ans = switchRequireAnswer.IsToggled;
            question.Create_question(questionEntry.Text, answers);

            //// Store Data in File Of Survey
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

                        (S.Questions_Property.ElementAt(_index) as Multible).Type = TypeQuestion.DropDown;
                        (S.Questions_Property.ElementAt(_index) as Multible).require_Ans = switchRequireAnswer.IsToggled;
                        (S.Questions_Property.ElementAt(_index) as Multible).bound = answers.Length;
                        (S.Questions_Property.ElementAt(_index) as Multible).Create_question(questionEntry.Text, answers);
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
            stackquestions.BackgroundColor = Color.FromHex("e5e5e5");
            var res = await DisplayAlert(Lang.Resource.titleMessage, Lang.Resource.bodyMessage, Lang.Resource.btnOkMessage, Lang.Resource.btnCancelMessage);
            if (res)
                await Navigation.PopModalAsync();
            stackquestions.BackgroundColor = Color.FromHex("ffffff");
        }
        private void ClosePage()
        {
            closePageRecognizer_Tapped(new object(), new EventArgs());
        }
        private void DeleteAnswerChoice_Activated(object sender, EventArgs e)
        {
            if (count_Visable > 2)
            {
                if ((sender as Image).Source.ToString() == "cancel1")
                    grid1.IsVisible = false;
                else
                    grid2.IsVisible = false;
                count_Visable--;
            }
        }

        private void AddAnswerChoiceRecognizer_Tapped(object sender, EventArgs e)
        {
            stackAnswerChoice.Children.Add(GetElements.GetAnswerChoics(""));
            count_Visable++;
        }
    }
}
