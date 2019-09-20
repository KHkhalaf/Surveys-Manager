using System.Collections.Generic;

namespace SurveyMonkey_Project.Models
{
    class Filter_by_Date
    {
        private LinkedList<User> list_user;

        //public Filter_by_Date(AnalaysesQuestion analaysesQuestion, string S_date, string E_date)
        //{
        //    this.analaysesQuestion = analaysesQuestion;
        //    list_user = new LinkedList<User>();
        //    this.S_date = S_date;
        //    this.E_date = E_date;
        //}
        public Filter_by_Date()
        {

        }
        public LinkedList<User> list_user_properties
        {
            get { return this.list_user; }
            set { this.list_user = value; }
        }
        public List<Multiable_answer> Filter_Date(long S_date, long E_date, List<Multiable_answer> listMultibleAnswer)
        {
                for (int i = 0; i < listMultibleAnswer.Count;)
                {
                long date_answer = long.Parse(listMultibleAnswer[i].date_Property);
                if (date_answer < S_date || E_date < date_answer)
                    listMultibleAnswer.RemoveAt(i);
                else
                    i++;
                }
                return listMultibleAnswer;
        }

        public List<MultiChoiceAnswercs> Filter_Date(long S_date, long E_date, List<MultiChoiceAnswercs> listMultibleAnswer)
        {
            for (int i = 0; i < listMultibleAnswer.Count;)
            {
                long date_answer = long.Parse(listMultibleAnswer[i].date_Property);
                if (date_answer < S_date || E_date < date_answer)
                    listMultibleAnswer.RemoveAt(i);
                else
                    i++;
            }
            return listMultibleAnswer;
        }
    }
}
