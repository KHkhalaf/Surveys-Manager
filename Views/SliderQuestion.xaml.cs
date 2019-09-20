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
    public partial class SliderQuestion : ContentPage
    {
        private IFolder folder;
        private IFile file;
        private ObservableCollection<Survey> ListSurveys;
        private Survey survey;
        private Slider_Question _questionSelected;
        private ControlFile controlFile = new ControlFile();
        private string username;
        private int _index;
        public SliderQuestion(ItemTappedEventArgs itemtapped, Question questionSelected, int index)
        {
            InitializeComponent();
            survey = itemtapped.Item as Survey;
            _questionSelected = questionSelected as Slider_Question;
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
            if (!CheckNetwork.Check_Connectivity())
                return;
            string inputs = questionEntry.Text + minvalue.Text + maxvalue.Text;
            if (String.IsNullOrWhiteSpace(inputs) || inputs.Contains("~"))
            {
                checkinputs.Text = Lang.Resource.check_inputsSignin3;
                return;
            }
            Slider_Question question = new Slider_Question(TypeQuestion.Slider, questionEntry.Text);
            question.Create_question(Int32.Parse(minvalue.Text), Int32.Parse(maxvalue.Text), switchRequireAnswer.IsToggled);

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
                        Slider_Question slider = new Slider_Question(TypeQuestion.Slider, questionEntry.Text);

                        slider.title = questionEntry.Text;
                        slider.requireAns = switchRequireAnswer.IsToggled;
                        slider.min_value = Int32.Parse(minvalue.Text);
                        slider.max_value = Int32.Parse(maxvalue.Text);
                        (S.Questions_Property.ElementAt(_index) as Slider_Question).Edit_question(slider);
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
    }
}
