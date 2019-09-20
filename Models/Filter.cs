using System.Collections.Generic;

namespace SurveyMonkey_Project.Models
{
    class Filter
    {
        protected AnalaysesQuestion analaysesQuestion;
        protected LinkedList<User> list_user;
        public LinkedList<User> list_user_properties
        {
            get { return this.list_user; }
        }
        //public Filter(AnalaysesQuestion analaysesQuestion)
        //{
        //    this.analaysesQuestion = analaysesQuestion;
        //    list_user = new LinkedList<User>();
        //}
        public Filter()
        {
            list_user = new LinkedList<User>();
        }
        public AnalaysesQuestion analaysesQuestion_properties
        {
            get { return this.analaysesQuestion; }
        }
    }

}
