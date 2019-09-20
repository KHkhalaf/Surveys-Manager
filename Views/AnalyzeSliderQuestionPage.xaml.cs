using CrossPieCharts.FormsPlugin.Abstractions;
using SurveyMonkey_Project;
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
    public partial class AnalyzeSliderQuestionPage : ContentPage
    {
        private Survey _survey;
        private AnalaysesQuestion analyse = new AnalaysesQuestion();
        private int img_clicked = 0;
        private List<Multiable_answer> listMultibleAnswer;
        private int index;
        private string country = Lang.Resource.Syria, PickerStartDate, PickerEndDate;

        public AnalyzeSliderQuestionPage(Survey survey, int i)
        {
            InitializeComponent();
            Question q = survey.Questions.ElementAt(i);
            _survey = survey;

            index = i;
            listMultibleAnswer = ((Slider_Question)_survey.Questions[index]).List_multiable_Answer.ToList();
            //analyse.Analys(q);

            // Initilize The Items Picker
            pickerSelectionCountry.Title = country;
            pickerSelectionCountry.Items.Add(Lang.Resource.Syria);
            pickerSelectionCountry.Items.Add(Lang.Resource.Iraq);
            pickerSelectionCountry.Items.Add(Lang.Resource.Jourdan);
            pickerSelectionCountry.Items.Add(Lang.Resource.USA);
            pickerSelectionCountry.Items.Add(Lang.Resource.UIA);
            pickerSelectionCountry.Items.Add(Lang.Resource.KSA);
            pickerSelectionCountry.Items.Add(Lang.Resource.Algeria);
            pickerSelectionCountry.Items.Add(Lang.Resource.Egypt);
            pickerSelectionCountry.Items.Add(Lang.Resource.Moroco);
            pickerSelectionCountry.Items.Add(Lang.Resource.Libya);
            pickerSelectionCountry.Items.Add(Lang.Resource.Tunisia);
            pickerSelectionCountry.Items.Add(Lang.Resource.Mauritania);

            InitializeData();

            Init(analyse.Average(listMultibleAnswer, index , ((Slider_Question)_survey.Questions[index]).max_value));
        }
        protected override bool OnBackButtonPressed()
        {
            if (PopupFilterByAge.IsVisible)
            {
                ClosePopUpFilterByAge();
                return true;
            }
            else if (PopupFilterByCountry.IsVisible)
            {
                ClosePopUpFilterCountry();
                return true;
            }
            else if (PopupFilterByDate.IsVisible)
            {
                ClosePopUpFilterDate();
                return true;
            }
            return base.OnBackButtonPressed();
        }
        private void Init(int progress)
        {
            var _question = ((Slider_Question)_survey.Questions[index]);
            lblQuestion.Text = "  - " + _question.title + " ? ";
            for (int j = 0; j < listMultibleAnswer.Count; j++)
            {
                stackAnswers.Children.Add(GetElements.GetAnswers_byUsers(_question.List_multiable_Answer.ElementAt(j).choosen_answer, _question.List_multiable_Answer.ElementAt(j).user.UserName));
                stackAnswers.Children.Add(new Label { BackgroundColor = Color.FromHex("a7cefb"), HeightRequest = 2 });
            }
            stackAnswers.Children.Add(new Label { Text = Lang.Resource.RatioSlider, FontAttributes = FontAttributes.Bold, FontSize = 20, TextColor = Color.FromHex("1fe887") });
            
            stackAnswers.Children.Add(
                new Grid
                {
                    Children ={
                                new CrossPieChartView
                                {
                                    Progress = progress,
                                    ProgressColor = Color.Green,
                                    ProgressBackgroundColor = Color.FromHex("#EEEEEEEE"),
                                    StrokeThickness = Device.OnPlatform(10, 20, 80),
                                    Radius = Device.OnPlatform(100, 180, 160),
                                    BackgroundColor = Color.White
                                },
                                new Label
                                {
                                    Text = progress+"%",
                                    Font = Font.BoldSystemFontOfSize(NamedSize.Large),
                                    FontSize = 70,
                                    VerticalOptions = LayoutOptions.Center,
                                    HorizontalOptions = LayoutOptions.Center,
                                    TextColor = Color.Black
                                }
                    }
                }
            );
        }

        private async void filter_Activated(object sender, EventArgs e)
        {
            string[] options = { Lang.Resource.age, Lang.Resource.country, Lang.Resource.Date_signup };
            string Result = await DisplayActionSheet(Lang.Resource.filterBy, Lang.Resource.btnCancelMessage, null, options);

            if (String.IsNullOrWhiteSpace(Result))
                return;
            if (Result.Contains("Age") || Result.Contains("العمر"))
            {
                stackAnswers.Opacity = 0.5;
                img1.Source = "uncheckedradio.png";
                img2.Source = "uncheckedradio.png";
                img3.Source = "uncheckedradio.png";
                PopupFilterByAge.IsVisible = true;
                await PopupFilterByAge.ScaleTo(1.5, 200, Easing.Linear);
                await PopupFilterByAge.ScaleTo(1.2, 200, Easing.Linear);
            }
            else if (Result.Contains("Country") || Result.Contains("البلد"))
            {
                    stackAnswers.Opacity = 0.5;
                    PopupFilterByCountry.IsVisible = true;
                    await PopupFilterByCountry.ScaleTo(1.5, 200, Easing.Linear);
                    await PopupFilterByCountry.ScaleTo(1.2, 200, Easing.Linear);
            }
            else if (Result.Contains("Date") || Result.Contains("تاريخ الإشتراك"))
            {
                stackAnswers.Opacity = 0.5;
                PopupFilterByDate.IsVisible = true;
                await PopupFilterByDate.ScaleTo(1.5, 200, Easing.Linear);
                await PopupFilterByDate.ScaleTo(1.2, 200, Easing.Linear);
            }
        }

        private void FilterByAgeRecognizer_Tapped(object sender, EventArgs e)
        {
            if (sender is Image)
            {
                var img = sender as Image;
                if (img.StyleId.Contains("1"))
                {
                    img1.Source = "checkedradio.png";
                    img2.Source = "uncheckedradio.png";
                    img3.Source = "uncheckedradio.png";
                    img_clicked = 1;
                }
                else if (img.StyleId.Contains("2"))
                {
                    img1.Source = "uncheckedradio.png";
                    img2.Source = "checkedradio.png";
                    img3.Source = "uncheckedradio.png";
                    img_clicked = 2;
                }
                else if (img.StyleId.Contains("3"))
                {
                    img1.Source = "uncheckedradio.png";
                    img2.Source = "uncheckedradio.png";
                    img3.Source = "checkedradio.png";
                    img_clicked = 3;
                }
            }
            else
            {
                var lbl = sender as Label;
                if (lbl.StyleId.Contains("1"))
                {
                    img1.Source = "checkedradio.png";
                    img2.Source = "uncheckedradio.png";
                    img3.Source = "uncheckedradio.png";
                    img_clicked = 1;
                }
                else if (lbl.StyleId.Contains("2"))
                {
                    img1.Source = "uncheckedradio.png";
                    img2.Source = "checkedradio.png";
                    img3.Source = "uncheckedradio.png";
                    img_clicked = 2;
                }
                else if (lbl.StyleId.Contains("3"))
                {
                    img1.Source = "uncheckedradio.png";
                    img2.Source = "uncheckedradio.png";
                    img3.Source = "checkedradio.png";
                    img_clicked = 3;
                }
            }
        }
        
        private void FilterByAgeClicked(object sender, EventArgs e)
        {
            listMultibleAnswer = ((Slider_Question)_survey.Questions[index]).List_multiable_Answer.ToList();
            if (img_clicked == 1)
            {
                listMultibleAnswer = new Filter_by_Age().Filter_Age(20, 30, listMultibleAnswer);
                for (int i = 3; i < stackAnswers.Children.Count;)
                    stackAnswers.Children.RemoveAt(i);
                Init(analyse.Average(listMultibleAnswer, index, ((Slider_Question)_survey.Questions[index]).max_value));
                Cancel_Popup_FilterByAge(sender, e);
            }
            else if (img_clicked == 2)
            {
                listMultibleAnswer = new Filter_by_Age().Filter_Age(30, 40, listMultibleAnswer);
                for (int i = 3; i < stackAnswers.Children.Count;)
                    stackAnswers.Children.RemoveAt(i);
                Init(analyse.Average(listMultibleAnswer, index, ((Slider_Question)_survey.Questions[index]).max_value));
                Cancel_Popup_FilterByAge(sender, e);
            }
            else if (img_clicked == 3)
            {
                listMultibleAnswer = ((Slider_Question)_survey.Questions[index]).List_multiable_Answer.ToList();
                country = Turn_ArabicToEnglish(country);
                InitializeData();
                listMultibleAnswer = new Filter_by_Age().Filter_Age(40, 50, listMultibleAnswer);
                for (int i = 3; i < stackAnswers.Children.Count;)
                    stackAnswers.Children.RemoveAt(i);
                Init(analyse.Average(listMultibleAnswer, index, ((Slider_Question)_survey.Questions[index]).max_value));
                Cancel_Popup_FilterByAge(sender, e);
            }
        }

        private void FilterByCountry_Clicked(object sender, EventArgs e)
        {
            listMultibleAnswer = ((Slider_Question)_survey.Questions[index]).List_multiable_Answer.ToList();
            country = Turn_ArabicToEnglish(country);
            InitializeData();
            listMultibleAnswer = new Filter_by_Country().Filter_Country(country, listMultibleAnswer);
            for (int i = 3; i < stackAnswers.Children.Count;)
                stackAnswers.Children.RemoveAt(i);
            Init(analyse.Average(listMultibleAnswer, index, ((Slider_Question)_survey.Questions[index]).max_value));
            Cancel_Popup_FilterByCountry(sender, e);
        }
        private void FilterByDate_Clicked(object sender, EventArgs e)
        {
            listMultibleAnswer = new Filter_by_Date().Filter_Date(long.Parse(PickerStartDate), long.Parse(PickerEndDate), ((Slider_Question)_survey.Questions[index]).List_multiable_Answer.ToList());
            for (int i = 3; i < stackAnswers.Children.Count;)
                stackAnswers.Children.RemoveAt(i);
            Init(analyse.Average(listMultibleAnswer, index, ((Slider_Question)_survey.Questions[index]).max_value));
            Cancel_Popup_FilterByDate(sender, e);
        }

        private void Cancel_Popup_FilterByAge(object sender, EventArgs e)
        {
            ClosePopUpFilterByAge();
        }

        private async void ClosePopUpFilterByAge()
        {
            await PopupFilterByAge.ScaleTo(1.3, 200, Easing.Linear);
            await PopupFilterByAge.ScaleTo(1, 200, Easing.Linear);
            PopupFilterByAge.IsVisible = false;
            stackAnswers.Opacity = 1;
        }

        private void Cancel_Popup_FilterByCountry(object sender, EventArgs e)
        {
            ClosePopUpFilterCountry();
        }
        private async void ClosePopUpFilterCountry()
        {
            await PopupFilterByCountry.ScaleTo(1.3, 200, Easing.Linear);
            await PopupFilterByCountry.ScaleTo(1, 200, Easing.Linear);
            PopupFilterByCountry.IsVisible = false;
            stackAnswers.Opacity = 1;
        }
        private void Cancel_Popup_FilterByDate(object sender, EventArgs e)
        {
            ClosePopUpFilterDate();
        }
        private async void ClosePopUpFilterDate()
        {
            date2.IsEnabled = false;
            await PopupFilterByDate.ScaleTo(1.3, 200, Easing.Linear);
            await PopupFilterByDate.ScaleTo(1, 200, Easing.Linear);
            PopupFilterByDate.IsVisible = false;
            stackAnswers.Opacity = 1;
        }
        private void pickerSelectionCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            country = pickerSelectionCountry.Items[pickerSelectionCountry.SelectedIndex];
        }
        private string Turn_ArabicToEnglish(string countryArabic)
        {
            if (countryArabic.Equals("سوريا"))
                return "Syria";
            else if (countryArabic.Equals("العراق"))
                return "Iraq";
            else if (countryArabic.Equals("الجمهورية الأردنية الهاشمية"))
                return "Jourdan";
            else if (countryArabic.Equals("الولايات المتحدة الأمريكية"))
                return "USA";
            else if (countryArabic.Equals("الإمارات العربية المتحدة"))
                return "UIA";
            else if (countryArabic.Equals("المملكة العربية السعودية"))
                return "KSA";
            else if (countryArabic.Equals("الجزائر"))
                return "Algeria";
            else if (countryArabic.Equals("مصر"))
                return "Egypt";
            else if (countryArabic.Equals("المغرب"))
                return "Mororco";
            else if (countryArabic.Equals("ليبيا"))
                return "Libya";
            else if (countryArabic.Equals("تونس"))
                return "Tunisia";
            else if (countryArabic.Equals("موريتانيا"))
                return "Mauritania";
            return countryArabic;
        }
        private void InitializeData()
        {
            for (int i = 0; i < listMultibleAnswer.Count; i++)
                listMultibleAnswer[i].user.Country = Turn_ArabicToEnglish(listMultibleAnswer[i].user.Country);
        }
        private void DatePicker_Start_DateSelected(object sender, DateChangedEventArgs e)
        {
            date2.IsEnabled = true;
            date2.MinimumDate = date1.Date;
            PickerStartDate = date1.Date.Year.ToString() + date1.Date.Month.ToString() + date1.Date.Day.ToString();
        }
        private void DatePicker_End_DateSelected(object sender, DateChangedEventArgs e)
        {
            PickerEndDate = date2.Date.Year.ToString() + date1.Date.Month.ToString() + date1.Date.Day.ToString();
        }
    }
}
