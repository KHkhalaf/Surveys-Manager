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
    public partial class MultiChoiceQuestion : ContentPage
    {
        private IFolder folder;
        private IFile file;
        private ObservableCollection<Survey> ListSurveys;
        private List<Entry> entries;
        private Survey survey;
        private string Type_multi = "Radio";
        private Multible _questionSelected;
        private int _index;
        private ControlFile controlFile = new ControlFile();
        private string username;
        private int count_Visable = 2;
        public MultiChoiceQuestion(ItemTappedEventArgs itemtapped, Question questionSelected, int index)
        {
            InitializeComponent();
            pickerSelectionType.Items.Add(Lang.Resource.typePickerMulti_1);
            pickerSelectionType.Items.Add(Lang.Resource.typePickerMulti_2);
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
                if (_questionSelected.Type == TypeQuestion.MultiChoice)
                {
                    pickerSelectionType.Title = Lang.Resource.typePickerMulti_2;
                    Type_multi = "Multi-select (Check Boxs)";
                }
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
            TypeQuestion type = TypeQuestion.Multible;
            if (!Type_multi.Contains("Radio"))
                type = TypeQuestion.MultiChoice;
            int size = entries.Count;
            if (switchOtherAnswer.IsToggled)
                size++;
            string[] answer = new string[size];
            for (int i = 0; i < entries.Count; i++)
            {
                answer[i] = entries.ElementAt(i).Text;
            }
            if (switchOtherAnswer.IsToggled)
                answer[size - 1] = "Other";
            Multible question = new Multible(type, questionEntry.Text, entries.Count);
            question.require_Ans = switchRequireAnswer.IsToggled;
            question.Create_question(questionEntry.Text, answer);
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
                        Multible multible = new Multible(type, questionEntry.Text, answer.Length);
                        multible.require_Ans = switchRequireAnswer.IsToggled;
                        multible.answer = answer;
                        (S.Questions_Property.ElementAt(_index) as Multible).Type = type;
                        (S.Questions_Property.ElementAt(_index) as Multible).Edit_question(multible);
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

        private void pickerSelectionType_Selected(object sender, EventArgs e)
        {
            Type_multi = pickerSelectionType.Items[pickerSelectionType.SelectedIndex];
            pickerSelectionType.Title = Type_multi;
        }

    }
}
