using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyMonkey_Project.Models
{
    class Answer
    {

        string choosen_answer;

        public void Answer_question(string answer)
        {
            this.choosen_answer = answer;
        }

    }
}
