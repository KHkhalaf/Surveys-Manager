using Plugin.RestClient;
using SurveyMonkey_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SurveyMonkey_Project.Services
{
    public class SurveysServices
    {
        private const string UrlApi = "http://192.168.43.172/WebApi/api/Surveys/";
        private RestClient<Survey> restClient = new RestClient<Survey>();
        public SurveysServices()
        {
            restClient.WebServiceUrlProperty = UrlApi;
        }
        public void Set_UrlApi(string restUrl)
        {
            restClient.WebServiceUrlProperty += restUrl;
        }

        public async Task<List<Survey>> GetSurveysAsync()
        {
            //restClient.WebServiceUrlProperty += "ReceiveSurveysShared/" + UserName;
            try
            {
                List<Survey> ListSurveys = await restClient.GetAsync();
                if (ListSurveys == null)
                    return new List<Survey>();
                foreach (Survey survey in ListSurveys)
                {
                    survey.Date_Property = survey.Date_Property.Substring(0, survey.Date_Property.IndexOf(":") - 2);
                    for (int i = 0; i < survey.Questions.Count; i++)
                    {
                        if (survey.Questions[i].Type.Equals(TypeQuestion.Text))
                        {
                            survey.Questions[i] = survey.ListsOfQuestions.List_text[0];
                            survey.ListsOfQuestions.List_text.RemoveAt(0);
                        }
                        else if (survey.Questions[i].Type.Equals(TypeQuestion.Multible)
                            || survey.Questions[i].Type.Equals(TypeQuestion.MultiChoice)
                            || survey.Questions[i].Type.Equals(TypeQuestion.DropDown))
                        {
                            survey.Questions[i] = survey.ListsOfQuestions.List_multible[0];
                            if (survey.Questions[i].Type.Equals(TypeQuestion.MultiChoice))
                            {
                                survey.Questions[i].Type = TypeQuestion.MultiChoice;
                            }
                            else if (survey.Questions[i].Type.Equals(TypeQuestion.Multible))
                            {
                                survey.Questions[i].Type = TypeQuestion.Multible;
                            }
                            else
                            {
                                survey.Questions[i].Type = TypeQuestion.DropDown;
                            }
                            survey.ListsOfQuestions.List_multible.RemoveAt(0);
                        }
                        else if (survey.Questions[i].Type.Equals(TypeQuestion.Slider))
                        {
                            survey.Questions[i] = survey.ListsOfQuestions.List_slider[0];
                            survey.ListsOfQuestions.List_slider.RemoveAt(0);
                        }
                    }
                    survey.ListsOfQuestions.List_text = null;
                    survey.ListsOfQuestions.List_multible = null;
                    survey.ListsOfQuestions.List_slider = null;
                }
                return ListSurveys;
            }
            catch(Exception ex)
            {
                DependencyService.Get<SnackBar>().ShowSnackBar(ex.ToString());
                return new List<Survey>();
            }
        }
        public async Task<bool> PostSurveysAsync(Survey survey)
        {
            //restClient.WebServiceUrlProperty += "Share/" + UserName;
            try
            {
                survey.ListsOfQuestions = new ListsOfQuestions();
                foreach (Question Q in survey.Questions)
                {
                    if (Q is Text_Question)
                        survey.ListsOfQuestions.List_text.Add(Q as Text_Question);
                    else if (Q is Multible)
                        survey.ListsOfQuestions.List_multible.Add(Q as Multible);
                    else if (Q is Slider_Question)
                        survey.ListsOfQuestions.List_slider.Add(Q as Slider_Question);
                }
                var Result = await restClient.PostAsync(survey);
                return Result;
            }
            catch(Exception ex)
            {
                DependencyService.Get<SnackBar>().ShowSnackBar(ex.ToString());
                return false;
            }
        }
        public async Task<bool> PutSurveysAsync(Survey survey)
        {
            try
            {
                survey.ListsOfQuestions = new ListsOfQuestions();
                foreach (Question Q in survey.Questions)
                {
                    if (Q is Text_Question)
                        survey.ListsOfQuestions.List_text.Add(Q as Text_Question);
                    else if (Q is Multible)
                        survey.ListsOfQuestions.List_multible.Add(Q as Multible);
                    else if (Q is Slider_Question)
                        survey.ListsOfQuestions.List_slider.Add(Q as Slider_Question);
                }

                var Result = await restClient.PutAsync(survey);
                return Result;
            }
            catch(Exception ex)
            {
                DependencyService.Get<SnackBar>().ShowSnackBar(ex.ToString());
                return false;
            }
        }
        public async Task<bool> DeleteSurveysAsync()
        {
            try
            {
                var Result = await restClient.DeleteAsync();
                return Result;
            }
            catch(Exception ex)
            {
                DependencyService.Get<SnackBar>().ShowSnackBar(ex.ToString());
                return false;
            }
        }
        public async Task<List<Survey>> GetSurveysBykeywordAsync(string keyword)
        {
            try
            {
                var ListSurveys = await restClient.GetBykeywordAsync(keyword);
                return ListSurveys;
            }
            catch(Exception ex)
            {
                DependencyService.Get<SnackBar>().ShowSnackBar(ex.ToString());
                return new List<Survey>();
            }
        }
    }
}
