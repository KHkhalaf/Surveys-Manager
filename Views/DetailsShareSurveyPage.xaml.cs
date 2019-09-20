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
    public partial class DetailsShareSurveyPage : ContentPage
    {
        private List<Question> listQuestions = new List<Question>();
        private Survey SelectedSurvey;
        private IFolder folder;
        private IFile file;

        public DetailsShareSurveyPage(ItemTappedEventArgs survey)
        {
            InitializeComponent();
            SelectedSurvey = survey.Item as Survey;
            BindingContext = SelectedSurvey;
            listQuestions = SelectedSurvey.Questions_Property;
        }
        protected async override void OnAppearing()
        {
            ActivityIndicator.IsRunning = true;
            stackQuestions.IsVisible = false;

            for (int i = 0; i < listQuestions.Count; i++)
            {
                AddQuestion(listQuestions, i);
            }
            await Task.Delay(400);
            ActivityIndicator.IsRunning = false;
            stackQuestions.IsVisible = true;

            base.OnAppearing();
        }

        private async void Save_Activated(object sender, EventArgs e)
        {
            stackQuestions.BackgroundColor = Color.FromHex("e5e5e5");
            var res = await DisplayAlert(Lang.Resource.titleMessage, Lang.Resource.bodyMessageSave , Lang.Resource.btnCancelMessage, Lang.Resource.btnOkMessage);
            stackQuestions.BackgroundColor = Color.FromHex("ffffff");
            if (res)
                return;
            
            ActivityIndicator.IsRunning = true;
            stackQuestions.Opacity = 0.4;

            folder = FileSystem.Current.LocalStorage;
            folder = await folder.CreateFolderAsync("folderUser", CreationCollisionOption.OpenIfExists);
            file = await folder.CreateFileAsync("fileUser", CreationCollisionOption.OpenIfExists);
            string AccountUser = await file.ReadAllTextAsync();
            User register = Serializable_Account.deserialize(AccountUser).ElementAt(0);

            for (int i = 1; i < stackQuestions.Children.Count; i++)
            {
                string date = "";
                // Get Answer From Text Question
                if (((stackQuestions.Children.ElementAt(i) as StackLayout).StyleId.Equals("text")))
                {
                    string answer = ((stackQuestions.Children.ElementAt(i) as StackLayout).Children[2] as Entry).Text;
                    if (String.IsNullOrWhiteSpace(answer) && (SelectedSurvey.Questions[i - 1] as Text_Question).requireAnswer)
                    {
                        DependencyService.Get<SnackBar>().ShowSnackBar(Lang.Resource.the_question + i + Lang.Resource.restTheQuestion);
                        return;
                    }
                    if(!String.IsNullOrWhiteSpace(answer))
                    if (answer.Contains("~"))
                    {
                        DependencyService.Get<SnackBar>().ShowSnackBar(Lang.Resource.the_question + i + Lang.Resource.rest_theTextQuestion);
                        return;
                    }
                    date += DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();
                    (SelectedSurvey.Questions[i - 1] as Text_Question).Answer_question(answer, date, register);
                }
                else if (((stackQuestions.Children.ElementAt(i) as StackLayout).StyleId.Equals("dropdown")))
                {
                    string answer = ((stackQuestions.Children.ElementAt(i) as StackLayout).Children[2] as Picker).Title;
                    if ((String.IsNullOrWhiteSpace(answer) || answer.Equals("Choices")) && (SelectedSurvey.Questions[i - 1] as Multible).require_Ans)
                    {
                        DependencyService.Get<SnackBar>().ShowSnackBar(Lang.Resource.the_question + i + Lang.Resource.restTheQuestion);
                        return;
                    }
                    date += DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();
                    (SelectedSurvey.Questions[i - 1] as Multible).Answer_question(answer, date, register);
                }
                else if (((stackQuestions.Children.ElementAt(i) as StackLayout).StyleId.Equals("multible")))
                {
                    bool answered = false;
                    if ((SelectedSurvey.Questions[i - 1] as Multible).Type.Equals(TypeQuestion.Multible))
                    {
                        string answer = "";
                        for (int j = 0; j < (((stackQuestions.Children[i] as StackLayout).Children[2] as StackLayout).Children[0] as StackLayout).Children.Count; j++)
                        {
                            var stack = (((stackQuestions.Children[i] as StackLayout).Children[2] as StackLayout).Children[0] as StackLayout).Children[j] as StackLayout;
                            var img = stack.Children[0] as Image;
                            if (!img.StyleId.Contains("un"))
                            {
                                answer = (((((stackQuestions.Children[i] as StackLayout).Children[2] as StackLayout).Children[0] as StackLayout).Children[j] as StackLayout).Children[1] as Label).Text;
                                answered = true;
                            }
                        }
                        if (!answered && (SelectedSurvey.Questions[i - 1] as Multible).require_Ans)
                        {
                            DependencyService.Get<SnackBar>().ShowSnackBar(Lang.Resource.the_question + i + Lang.Resource.restTheQuestion);
                            return;
                        }
                        Multiable_answer multiAnswer = new Multiable_answer((((stackQuestions.Children[i] as StackLayout).Children[2] as StackLayout).Children[0] as StackLayout).Children.Count);

                    date += DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();
                        (SelectedSurvey.Questions[i - 1] as Multible).Answer_question(answer, date, register);
                    }
                    else if ((SelectedSurvey.Questions[i - 1] as Multible).Type.Equals(TypeQuestion.MultiChoice))
                    {
                        string[] multichoicesAnswers = new string[(((stackQuestions.Children[i] as StackLayout).Children[2] as StackLayout).Children[0] as StackLayout).Children.Count];
                        for (int j = 0; j < (((stackQuestions.Children[i] as StackLayout).Children[2] as StackLayout).Children[0] as StackLayout).Children.Count; j++)
                        {
                            var stack = (((stackQuestions.Children[i] as StackLayout).Children[2] as StackLayout).Children[0] as StackLayout).Children[j] as StackLayout;
                            var img = stack.Children[0] as Image;
                            if (!img.StyleId.Contains("un"))
                            {
                                multichoicesAnswers[j] = (((((stackQuestions.Children[i] as StackLayout).Children[2] as StackLayout).Children[0] as StackLayout).Children[j] as StackLayout).Children[1] as Label).Text;
                                answered = true;
                            }
                        }
                        if (!answered && (SelectedSurvey.Questions[i - 1] as Multible).require_Ans)
                        {
                            DependencyService.Get<SnackBar>().ShowSnackBar(Lang.Resource.the_question + i + Lang.Resource.restTheQuestion);
                            return;
                        }
                       (SelectedSurvey.Questions[i - 1] as Multible).list_multichoice_answer = new LinkedList<MultiChoiceAnswercs>();
                        MultiChoiceAnswercs multichoices = new MultiChoiceAnswercs(multichoicesAnswers.Length);

                    date += DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();
                        (SelectedSurvey.Questions[i - 1] as Multible).Answer_question(multichoicesAnswers, date, register);
                    }
                }
                else if (((stackQuestions.Children.ElementAt(i) as StackLayout).StyleId.Equals("slider")))
                {
                    int counter = (int)(((stackQuestions.Children[i] as StackLayout).Children[2] as StackLayout).Children[1] as Slider).Value;
                    date += DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();
                    (SelectedSurvey.Questions[i - 1] as Slider_Question).Answer_question(counter, date, register);
                }
            }
            stackQuestions.Opacity = 0.5;
            if (CheckNetwork.Check_Connectivity())
            {
                SurveysServices S_S = new SurveysServices();
                S_S.Set_UrlApi("ShareResponsedSurveys/");
                await S_S.PostSurveysAsync(SelectedSurvey);
            }
            else
            {
                List<Survey> surveys = new List<Survey>();
                folder = FileSystem.Current.LocalStorage;
                ExistenceCheckResult result = await folder.CheckExistsAsync("ShareResponsedSurveys");
                if (result == ExistenceCheckResult.FolderExists)
                {
                    folder = await folder.CreateFolderAsync("ShareResponsedSurveys", CreationCollisionOption.OpenIfExists);

                    file = await folder.CreateFileAsync("ShareResponsedSurveys", CreationCollisionOption.OpenIfExists);
                    string Content = await file.ReadAllTextAsync();
                    if (String.IsNullOrEmpty(Content) || String.IsNullOrWhiteSpace(Content))
                    {
                        surveys.Add(SelectedSurvey);
                        Content = Serializable_Survey.serialize(surveys);
                        await file.WriteAllTextAsync(Content);

                        await Task.Delay(500);
                        await Navigation.PopAsync();
                        return;
                    }

                    surveys = Serializable_Survey.deserialize(Content).ToList();
                    if (surveys.Contains(SelectedSurvey))
                    {
                        foreach (Survey S in surveys)
                        {
                            if (S.Id == SelectedSurvey.Id_Property)
                                S.Questions = SelectedSurvey.Questions;
                        }
                    }
                    else
                        surveys.Add(SelectedSurvey);
                    Content = Serializable_Survey.serialize(surveys);
                    await file.WriteAllTextAsync(Content);
                }
                else
                {
                    folder = await folder.CreateFolderAsync("ShareResponsedSurveys", CreationCollisionOption.ReplaceExisting);

                    file = await folder.CreateFileAsync("ShareResponsedSurveys", CreationCollisionOption.ReplaceExisting);
                    surveys.Add(SelectedSurvey);
                    string content = Serializable_Survey.serialize(surveys);
                    await file.WriteAllTextAsync(content);
                }
            }
            await Task.Delay(500);
            await Navigation.PopAsync();
        }

        private void AddQuestion(List<Question> listQuestions, int i)
        {
            if (listQuestions.ElementAt(i) is Text_Question)
            {
                Text_Question question = (Text_Question)listQuestions.ElementAt(i);
                stackQuestions.Children.Add(GetElements.GetTextQuestion(i + 1 + "- " + question.questionProperty + " ?", true,false));
            }
            else if (listQuestions.ElementAt(i) is Multible)
            {
                Multible question = (Multible)listQuestions.ElementAt(i);
                if (question.typeQuestion_properties.ToString().Contains("Multible"))
                    stackQuestions.Children.Add(GetElements.GetMultiChoices(false, i + 1 + "- " + question.title_property + " ?", question.answer, true, false));
                else if (question.Type.Equals(TypeQuestion.MultiChoice))
                    stackQuestions.Children.Add(GetElements.GetMultiChoices(true, i + 1 + "- " + question.title_property + " ?", question.answer, true, false));
                else if (question.Type.Equals(TypeQuestion.DropDown))
                    stackQuestions.Children.Add(GetElements.GetDropDownQuestion(i + 1 + "- " + question.title_properties + " ?", question.answer.ToList(), true, false));
            }
            else if (listQuestions.ElementAt(i) is Slider_Question)
            {
                Slider_Question question = (Slider_Question)listQuestions.ElementAt(i);
                stackQuestions.Children.Add(
                    GetElements.GetSlider(question.Min_Value_Property, question.Max_Value_Property, i + 1 + "- " + question.title_property + " ?", true, false)
                    );
            }
        }
    }
}
