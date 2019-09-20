
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyMonkey_Project.Models
{
    public class Multiable_answer
    {
        public int Id { get; set; }
        public string[] answer { get; set; }
        public string choosen_answer { get; set; }
        public User user { get; set; }
        public string date { get; set; }

        public Multiable_answer(int bound)
        {
            this.answer = new string[bound];
        }
        public int Id_proprities
        {
            get { return Id; }
            set { Id = value; }
        }
        
        public string date_Property
        {
            get { return date; }
            set { date = value; }
        }

        public User user_Property
        {
            get { return this.user; }
            set { this.user = value; }
        }

        public string Choosen_Answer_Property
        {
            get { return choosen_answer; }
            set { this.choosen_answer = value; }
        }

        public string[] Answer_Property
        {
            get { return answer; }
            set { this.answer = value; }
        }

        public void Answer_question(string answer, string date, User user)
        {
            this.choosen_answer = answer;
            this.user = user;
            this.date = date;
        }

    }
}
