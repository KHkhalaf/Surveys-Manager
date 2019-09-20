using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyMonkey_Project.Models
{
    public class MultiChoiceAnswercs : Multiable_answer
    {
        public string[] chossenanswer;

        public MultiChoiceAnswercs(int bound) : base(bound)
        {

        }

        public string[] chossenanswer_prperties
        {
            get { return this.chossenanswer; }
            set { this.chossenanswer = value; }

        }

        public void Answer_question(string[] answer, string date, User user)
        {
            this.chossenanswer = answer;
            base.user_Property = user;
            base.date_Property = date;
        }

    }
}
