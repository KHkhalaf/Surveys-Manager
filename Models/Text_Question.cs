using SurveyMonkey_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyMonkey_Project.Models
{
    public class Text_Question:Question
    {
        public int Id { get; set; }
        public TypeQuestion type { get; set; }
        public string question { get; set; }
        public bool requireAnswer { get; set; }
        public string answer { get; set; }
        public Multiable_answer multiAnswer { get; set; }
        public LinkedList<Multiable_answer> list_multi_answers { get; set; }
        public Text_Question() : base() { }
       public  Text_Question(TypeQuestion typeQuestion, string _question) : base(typeQuestion, _question) { }
        public void Initialize(TypeQuestion _type, string _question, bool require_Ans, string _answer)
        {
            type = _type;
            question = _question;
            requireAnswer = require_Ans;
            answer = _answer;
            list_multi_answers = new LinkedList<Multiable_answer>();
        }
        public int Id_Property
        {
            get { return Id; }
            set { Id = value; }
        }
        public int ID_Property
        {
            get { return base.Id; }
            set { base.Id = value; }
        }
        public TypeQuestion typeProperty
        {
            get { return type; }
            set { type = value; }
        }
        public string questionProperty
        {
            get { return question; }
            set { question = value; }
        }
        public bool requireAnsProperty
        {
            get { return requireAnswer; }
            set { requireAnswer = value; }
        }
        public string answerProperty
        {
            get { return answer; }
            set { answer = value; }
        }
        public Multiable_answer multiAnswer_Properity
        {
            get { return multiAnswer; }
            set { multiAnswer = value; }
        }
        public LinkedList<Multiable_answer> list_multi_answers_properity
        {
            get { return this.list_multi_answers; }
            set { this.list_multi_answers = value; }
        }
        public void Answer_question(string answer, string date, User user)
        {
            multiAnswer = new Multiable_answer(0);
            //multiable_Answers.Answer_Property = multiable_Answer.Answer_Property;
            multiAnswer.Choosen_Answer_Property = answer;
            multiAnswer.date_Property = date;
            multiAnswer.user_Property = user;
        }

            public void Edit_Question(Text_Question questionText)
        {
            this.question = questionText.question;
            this.requireAnsProperty = questionText.requireAnsProperty;
        }
    }
}
