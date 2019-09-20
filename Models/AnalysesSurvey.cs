using System.Collections.Generic;

namespace SurveyMonkey_Project.Models
{
    class AnalysesSurvey
    {

        protected Survey survey;
        protected List<AnalaysesQuestion> list_analayesquestion;

        public AnalysesSurvey(Survey survey)
        {
            list_analayesquestion = new List<AnalaysesQuestion>();
            this.survey = survey;
        }
        public List<AnalaysesQuestion> list_analayesquestion_properties
        {
            get { return this.list_analayesquestion; }
        }
        public void Analyse()
        {
            foreach (Question question in survey.Q_list_properity)
            {

                AnalaysesQuestion analaysesQuestion = new AnalaysesQuestion();
                analaysesQuestion.Analys(question);
                list_analayesquestion.Add(analaysesQuestion);

            }

        }

    }

}


