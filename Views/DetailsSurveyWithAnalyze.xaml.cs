using SurveyMonkey_Project.Models;
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
    public partial class DetailsSurveyWithAnalyze : ContentPage
    {
        private List<Question> listQuestions = new List<Question>();
        private Survey SelectedSurvey;
        public DetailsSurveyWithAnalyze(ItemTappedEventArgs survey)
        {
            InitializeComponent();
            SelectedSurvey = survey.Item as Survey;
            BindingContext = SelectedSurvey;
            listQuestions = SelectedSurvey.Questions_Property;
            Device.BeginInvokeOnMainThread(async () => {

                ActivityIndicator.IsRunning = true;
                stackQuestions.IsVisible = false;

                for (int i = 0; i < listQuestions.Count; i++)
                {
                    AddQuestion(listQuestions, i);
                    Noquestions.IsVisible = false;
                }
                await Task.Delay(400);
                ActivityIndicator.IsRunning = false;
                stackQuestions.IsVisible = true;
            });
        }
        private void AddQuestion(List<Question> listQuestions, int i)
        {
            if (listQuestions.ElementAt(i) is Text_Question)
            {
                Text_Question question = (Text_Question)listQuestions.ElementAt(i);
                stackQuestions.Children.Add(GetElements.GetTextQuestion(i + 1 + "- " + question.questionProperty + " ?", false, false));
                AddAnalyzeButton(i, TypeQuestion.Text);
            }
            else if (listQuestions.ElementAt(i) is Multible)
            {
                Multible question = (Multible)listQuestions.ElementAt(i);
                if (question.typeQuestion_properties.ToString().Contains("Multible"))
                    stackQuestions.Children.Add(GetElements.GetMultiChoices(false, i + 1 + "- " + question.title_property + " ?", question.answer, false, false));
                else if (question.Type.Equals(TypeQuestion.MultiChoice))
                    stackQuestions.Children.Add(GetElements.GetMultiChoices(true, i + 1 + "- " + question.title_property + " ?", question.answer, false, false));
                else if (question.Type.Equals(TypeQuestion.DropDown))
                    stackQuestions.Children.Add(GetElements.GetDropDownQuestion(i + 1 + "- " + question.title_properties + " ?", question.answer.ToList(), false, false));
                AddAnalyzeButton(i, TypeQuestion.Multible);
            }
            else if (listQuestions.ElementAt(i) is Slider_Question)
            {
                Slider_Question question = (Slider_Question)listQuestions.ElementAt(i);
                stackQuestions.Children.Add(
                    GetElements.GetSlider(question.Min_Value_Property, question.Max_Value_Property, i + 1 + "- " + question.title_property + " ?", false, false)
                    );
                AddAnalyzeButton(i, TypeQuestion.Slider);
            }
        }
        private void AddAnalyzeButton(int i, TypeQuestion type)
        {
            //(stackQuestions.Children[i] as StackLayout).
            var Analyzebtn = new Button
            {
                Text = Lang.Resource.btnAnalyse,
                Image= "analytics.png",
                FontSize=15,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromHex("18d94b"),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                BackgroundColor = Color.FromHex("efefef"),
                WidthRequest = 150
            };
            if (type == TypeQuestion.Text)
            {
                Analyzebtn.Clicked += (sender, e) => {
                    Navigation.PushAsync(new AnalyzeTextQuestionPage(SelectedSurvey, i));
                };
            }
            else if (type == TypeQuestion.Slider)
            {
                Analyzebtn.Clicked += (sender, e) => {
                    Navigation.PushAsync(new AnalyzeSliderQuestionPage(SelectedSurvey, i));
                };
            }
            else
            {
                Analyzebtn.Clicked += (sender, e) => {
                    Navigation.PushAsync(new AnalyzeMultiChoiceQuestionPage(SelectedSurvey, i));
                };
            }
            //TapGestureRecognizer tapText = new TapGestureRecognizer();
            //tapText.Tapped += (sender, e) => {
            //    Navigation.PushModalAsync(new AnalyzeTextQuestionPage());
            //};
            //TapGestureRecognizer tapMuliChoices = new TapGestureRecognizer();
            //tapMuliChoices.Tapped += (sender, e) => {
            //    Navigation.PushModalAsync(new AnalyzeMultiChoiceQuestionPage(SelectedSurvey));
            //};
            //TapGestureRecognizer tapSlider = new TapGestureRecognizer();
            //tapSlider.Tapped += (sender, e) => {
            //    Navigation.PushModalAsync(new AnalyzeSliderQuestionPage());
            //};
            //if (type == TypeQuestion.Text)
            //    Analyzebtn.GestureRecognizers.Add(tapText);
            //else if (type == TypeQuestion.Slider)
            //    Analyzebtn.GestureRecognizers.Add(tapSlider);
            //else
            //    Analyzebtn.GestureRecognizers.Add(tapMuliChoices);
            (stackQuestions.Children[i + 1] as StackLayout).Children.Add(new Label { HeightRequest = 2, BackgroundColor = Color.FromHex("1fe887") });
            (stackQuestions.Children[i + 1] as StackLayout).Children.Add(Analyzebtn);
        }
    }
}
