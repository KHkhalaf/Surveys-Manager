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
    public partial class DetailsSurvey : ContentPage
    {
        private bool rotated = false;
        private IFolder folder;
        private IFile file;
        private ObservableCollection<Survey> ListSurveys;
        private Survey survey;
        private ItemTappedEventArgs _survey;
        private List<Question> listQuestions = new List<Question>();
        private Question question_selected;
        private int index = 0;
        private ControlFile controlFile = new ControlFile();
        private string username;
        public DetailsSurvey(ItemTappedEventArgs selected_survey)
        {
            InitializeComponent();
            BindingContext = selected_survey.Item as Survey;
            survey = selected_survey.Item as Survey;
            _survey = selected_survey;
            listQuestions = survey.Questions_Property;
        }
        protected override void OnAppearing()
        {
            Device.BeginInvokeOnMainThread(async () => {

                ActivityIndicator.IsRunning = true;
                stackQuestions.IsVisible = false;

                folder = FileSystem.Current.LocalStorage;
                username = await controlFile.GetUserName();
                folder = await folder.CreateFolderAsync("foldersurveys" + username, CreationCollisionOption.OpenIfExists);
                file = await folder.CreateFileAsync("filesurveys" + username, CreationCollisionOption.OpenIfExists);

                string content = await file.ReadAllTextAsync();
                ListSurveys = Serializable_Survey.deserialize(content);

                foreach (Survey S in ListSurveys)
                {
                    if (S.Id == survey.Id)
                    {
                        DrowItems(S);
                    }
                }
                await Task.Delay(500);
                ActivityIndicator.IsRunning = false;
                stackQuestions.IsVisible = true;
            });

        }
        protected override bool OnBackButtonPressed()
        {
            if (PopupEditTitlesurvey.IsVisible)
            {
                Close_PopUp();
                return true;
            }
            return false;
        }

        private void DrowItems(Survey S)
        {
            if (S.Questions_Property.Count == 0)
            {
                labelNoNQuestions.IsVisible = true;
                return;
            }
            labelNoNQuestions.IsVisible = false;
            while (stackQuestions.Children.Count > 1)
                stackQuestions.Children.RemoveAt(1);
            for (int i = 0; i < S.Questions_Property.Count(); i++)
                AddQuestion(S.Questions_Property, i);
            survey.Questions_Property = S.Questions_Property;
        }
        private async void EditTitle_Recognizer_Tapped(object sender, EventArgs e)
        {
            Title.Focus();
            contentpage.Opacity = 0.3;
            PopupEditTitlesurvey.IsVisible = true;
            await PopupEditTitlesurvey.ScaleTo(1.5, 200, Easing.Linear);
            await PopupEditTitlesurvey.ScaleTo(1.2, 200, Easing.Linear);
        }

        private async void ImageRecognizer_Tapped(object sender, EventArgs e)
        {
            if (!rotated)
            {
                await ((Image)imageplus).ScaleTo(0.4, 20, Easing.Linear);
                await ((Image)imageplus).RotateTo(230, 20, Easing.Linear);
                await ((Image)sender).ScaleTo(0.5, 20, Easing.Linear);
                Translate_Label(text);
                await Task.Delay(50);
                Translate_Label(dropdown);
                await Task.Delay(50);
                Translate_Label(multichoice);
                await Task.Delay(50);
                Translate_Label(slider);
                rotated = true;
            }
            else
            {
                await ((Image)sender).ScaleTo(0.4, 20, Easing.Linear);
                await ((Image)sender).RotateTo(0, 20, Easing.Linear);
                await ((Image)sender).ScaleTo(0.5, 20, Easing.Linear);
                De_Translate_Label(text);
                await Task.Delay(50);
                De_Translate_Label(dropdown);
                await Task.Delay(50);
                De_Translate_Label(multichoice);
                await Task.Delay(50);
                De_Translate_Label(slider);
                rotated = false;
            }
        }

        private async void Translate_Label(Label label)
        {
            label.IsVisible = true;
            await label.TranslateTo(0, 0, 100);
            await label.TranslateTo(0, -20, 100);
            await label.TranslateTo(0, 0, 200);
        }

        private async void De_Translate_Label(Label label)
        {
            await label.TranslateTo(0, 0, 100);
            await label.TranslateTo(0, 20, 100);
            await label.TranslateTo(0, 0, 200);
            label.IsVisible = false;
        }

        private async void Save(object sender, EventArgs e)
        {
            if (!CheckNetwork.Check_Connectivity())
                return;

            stackQuestions.Opacity = 0.5;
            ActivityIndicator.IsRunning = true;

            folder = FileSystem.Current.LocalStorage;
            folder = await folder.CreateFolderAsync("foldersurveys" + username, CreationCollisionOption.OpenIfExists);
            file = await folder.CreateFileAsync("filesurveys" + username, CreationCollisionOption.OpenIfExists);

            string content = await file.ReadAllTextAsync();
            ListSurveys = Serializable_Survey.deserialize(content);
            foreach (Survey S in ListSurveys)
            {
                if (S.Id == survey.Id)
                {
                    S.Title_Property = Title.Text;
                    S.Description_Property = Description.Text;

                    survey = S;
                    SurveysServices S_S = new SurveysServices();
                    S_S.Set_UrlApi("EditSurveys/" + username);
                    await S_S.PutSurveysAsync(survey);

                    break;
                }
            }
            title.Text = Title.Text;
            description.Text = Description.Text;
            content = Serializable_Survey.serialize(ListSurveys.ToList());
            await file.WriteAllTextAsync(content);
            Cancel(sender, e);

            await Task.Delay(500);
            ActivityIndicator.IsRunning = false;
            stackQuestions.Opacity = 1;

        }

        private void Cancel(object sender, EventArgs e)
        {
            Close_PopUp();
        }
        private async void Close_PopUp()
        {
            await PopupEditTitlesurvey.ScaleTo(1.3, 200, Easing.Linear);
            await PopupEditTitlesurvey.ScaleTo(1, 200, Easing.Linear);
            PopupEditTitlesurvey.IsVisible = false;
            checkinputs.Text = "";
            contentpage.Opacity = 1;
        }
        private async void TextQuestionRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new TextQuestion(_survey, new Question(), index));
            await imageplus.RotateTo(0, 20, Easing.Linear);
            text.IsVisible = false;
            dropdown.IsVisible = false;
            multichoice.IsVisible = false;
            slider.IsVisible = false;
            rotated = false;
        }

        private async void DropDownQuestionRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new DropDownQuestion(_survey, new Question(), index));
            await imageplus.RotateTo(0, 20, Easing.Linear);
            text.IsVisible = false;
            dropdown.IsVisible = false;
            multichoice.IsVisible = false;
            slider.IsVisible = false;
            rotated = false;
        }

        private async void SliderQuestionRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new SliderQuestion(_survey, new Question(), index));
            await imageplus.RotateTo(0, 20, Easing.Linear);
            text.IsVisible = false;
            dropdown.IsVisible = false;
            multichoice.IsVisible = false;
            slider.IsVisible = false;
            rotated = false;
        }

        private async void MultichoiceQuestionRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new MultiChoiceQuestion(_survey, new Question(), index));
            await imageplus.RotateTo(0, 20, Easing.Linear);
            text.IsVisible = false;
            dropdown.IsVisible = false;
            multichoice.IsVisible = false;
            slider.IsVisible = false;
            rotated = false;
        }

        
        private async void Delete_Item(int i)
        {
            var res = await DisplayAlert(Lang.Resource.titleMessage, Lang.Resource.DetailsDeleting, Lang.Resource.btnOkMessage, Lang.Resource.btnCancelMessage);
            if (res && CheckNetwork.Check_Connectivity())
            {
                stackQuestions.Children.RemoveAt(i + 1);
                survey.Questions_Property.RemoveAt(i);

                folder = FileSystem.Current.LocalStorage;

                folder = await folder.CreateFolderAsync("foldersurveys" + username, CreationCollisionOption.OpenIfExists);
                file = await folder.CreateFileAsync("filesurveys" + username, CreationCollisionOption.OpenIfExists);

                string content = await file.ReadAllTextAsync();
                ListSurveys = Serializable_Survey.deserialize(content);
                foreach (Survey S in ListSurveys)
                {
                    if (S.Id == survey.Id)
                    {
                        S.Questions_Property = survey.Questions_Property;

                        SurveysServices S_S = new SurveysServices();
                        S_S.Set_UrlApi("EditSurveys/" + username);
                        await S_S.PutSurveysAsync(survey);

                        DrowItems(S);
                    }
                }
                content = Serializable_Survey.serialize(ListSurveys.ToList());
                await file.WriteAllTextAsync(content);

            }
        }
        private void Visible_Images_Edit_or_Delete()
        {
            for (int i = 1; i < stackQuestions.Children.Count; i++)
            {
                ((stackQuestions.Children.ElementAt(i) as StackLayout).Children.ElementAt(0) as Image).IsVisible = true;
            }
        }
        private void Disable_Images_Edit_or_Delete()
        {
            for (int i = 1; i < stackQuestions.Children.Count; i++)
            {
                ((stackQuestions.Children.ElementAt(i) as StackLayout).Children.ElementAt(0) as Image).Source = "uncheckedcheckbox.png";
                ((stackQuestions.Children.ElementAt(i) as StackLayout).Children.ElementAt(0) as Image).StyleId = "uncheck";
                ((stackQuestions.Children.ElementAt(i) as StackLayout).Children.ElementAt(0) as Image).IsVisible = false;
            }
        }
        private void AddQuestion(List<Question> listQuestions, int i)
        {
            if (listQuestions.ElementAt(i) is Text_Question)
            {
                Text_Question question = (Text_Question)listQuestions.ElementAt(i);
                stackQuestions.Children.Add(GetElements.GetTextQuestion(i + 1 + "- " + question.questionProperty + " ?", false, true));
                (((stackQuestions.Children[i + 1] as StackLayout).Children[(stackQuestions.Children[i + 1] as StackLayout).Children.Count - 1] as StackLayout).Children[0] as Button).Clicked += (sender, e) => {

                    question_selected = survey.Questions_Property.ElementAt(i);
                    Navigation.PushModalAsync(new TextQuestion(_survey, question_selected, i));
                };
                (((stackQuestions.Children[i + 1] as StackLayout).Children[(stackQuestions.Children[i + 1] as StackLayout).Children.Count - 1] as StackLayout).Children[1] as Button).Clicked += (sender, e) => {
                    Delete_Item(i);
                };
            }
            else if (listQuestions.ElementAt(i) is Multible)
            {
                Multible question = (Multible)listQuestions.ElementAt(i);
                if (question.Type.Equals(TypeQuestion.Multible))
                {
                    stackQuestions.Children.Add(GetElements.GetMultiChoices(false, i + 1 + "- " + question.title_property + " ?", question.answer, false, true));
                    (((stackQuestions.Children[i + 1] as StackLayout).Children[(stackQuestions.Children[i + 1] as StackLayout).Children.Count - 1] as StackLayout).Children[0] as Button).Clicked += (sender, e) =>
                    {

                        question_selected = survey.Questions_Property.ElementAt(i);
                        Navigation.PushModalAsync(new MultiChoiceQuestion(_survey, question_selected, i));
                    };
                    (((stackQuestions.Children[i + 1] as StackLayout).Children[(stackQuestions.Children[i + 1] as StackLayout).Children.Count - 1] as StackLayout).Children[1] as Button).Clicked += (sender, e) => {
                        Delete_Item(i);
                    };
                }
                else if (question.Type.Equals(TypeQuestion.MultiChoice))
                {
                    stackQuestions.Children.Add(GetElements.GetMultiChoices(true, i + 1 + "- " + question.title_property + " ?", question.answer, false, true));
                    (((stackQuestions.Children[i + 1] as StackLayout).Children[(stackQuestions.Children[i + 1] as StackLayout).Children.Count - 1] as StackLayout).Children[0] as Button).Clicked += (sender, e) =>
                    {

                        question_selected = survey.Questions_Property.ElementAt(i);
                        Navigation.PushModalAsync(new MultiChoiceQuestion(_survey, question_selected, i));
                    };
                    (((stackQuestions.Children[i + 1] as StackLayout).Children[(stackQuestions.Children[i + 1] as StackLayout).Children.Count - 1] as StackLayout).Children[1] as Button).Clicked += (sender, e) => {
                        Delete_Item(i);
                    };
                }
                else if (question.Type.Equals(TypeQuestion.DropDown))
                {
                    stackQuestions.Children.Add(GetElements.GetDropDownQuestion(i + 1 + "- " + question.title_properties + " ?", question.answer.ToList(), false, true));
                    (((stackQuestions.Children[i + 1] as StackLayout).Children[(stackQuestions.Children[i + 1] as StackLayout).Children.Count - 1] as StackLayout).Children[0] as Button).Clicked += (sender, e) => {

                        question_selected = survey.Questions_Property.ElementAt(i);
                        Navigation.PushModalAsync(new DropDownQuestion(_survey, question_selected, i));
                    };
                    (((stackQuestions.Children[i + 1] as StackLayout).Children[(stackQuestions.Children[i + 1] as StackLayout).Children.Count - 1] as StackLayout).Children[1] as Button).Clicked += (sender, e) => {
                        Delete_Item(i);
                    };
                }
            }
            else if (listQuestions.ElementAt(i) is Slider_Question)
            {
                Slider_Question question = (Slider_Question)listQuestions.ElementAt(i);
                stackQuestions.Children.Add(
                    GetElements.GetSlider(question.Min_Value_Property, question.Max_Value_Property, i + 1 + "- " + question.title_property + " ?", false, true)
                    );
                (((stackQuestions.Children[i + 1] as StackLayout).Children[(stackQuestions.Children[i + 1] as StackLayout).Children.Count - 1] as StackLayout).Children[0] as Button).Clicked += (sender, e) => {

                    question_selected = survey.Questions_Property.ElementAt(i);
                    Navigation.PushModalAsync(new SliderQuestion(_survey, question_selected, i));
                };
                (((stackQuestions.Children[i + 1] as StackLayout).Children[(stackQuestions.Children[i + 1] as StackLayout).Children.Count - 1] as StackLayout).Children[1] as Button).Clicked += (sender, e) => {
                    Delete_Item(i);
                };
            }
        }
    }
}
