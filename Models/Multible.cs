using SurveyMonkey_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyMonkey_Project.Models
{
    public class Multible : Question
    {
        public int Id { set; get; }
        public string[] answer { get; set; }
        public string choosen_answer { get; set; }
        public MultiChoiceAnswercs multiChoiceAnswercs { get; set; }
        public LinkedList<MultiChoiceAnswercs> list_multichoice_answer { get; set; }
        public Multiable_answer multiable_Answer { get; set; }
        public LinkedList<Multiable_answer> List_multiable_Answer { get; set; }
        public bool require_Ans { get; set; }
        public int bound { get; set; }

        public Multible(TypeQuestion type, string title, int bound) : base(type, title)
        {

            this.bound = bound;
            this.List_multiable_Answer = new LinkedList<Multiable_answer>();
            this.answer = new string[bound];
            multiable_Answer = new Multiable_answer(bound);
            this.list_multichoice_answer = new LinkedList<MultiChoiceAnswercs>();
        }
        public int ID_Property
        {
            get { return base.Id; }
            set { base.Id = value; }
        }
        public LinkedList<MultiChoiceAnswercs> list_multiChoice_properties
        {
            get { return this.list_multichoice_answer; }
            set { this.list_multichoice_answer = value; }
        }
        public MultiChoiceAnswercs multiChoiceAnswercs_properties
        {
            get { return this.multiChoiceAnswercs; }
            set { this.multiChoiceAnswercs = value; }
        }
        public LinkedList<Multiable_answer> List_List_multiable_Answer_properity
        {
            get { return this.List_multiable_Answer; }
            set { this.List_multiable_Answer = value; }
        }
        public Multiable_answer multiable_Answer_propiretes
        {
            get { return this.multiable_Answer; }
            set { this.multiable_Answer = value; }
        }
        public string choosen_answer_prpiretes
        {
            get { return this.choosen_answer; }
            set { this.choosen_answer = value; }
        }
        public void Answer_question(string answer, string date, User user)
        {
            multiable_Answer = new Multiable_answer(bound);
            //multiable_Answers.Answer_Property = multiable_Answer.Answer_Property;
            multiable_Answer.Choosen_Answer_Property = answer;
            multiable_Answer.date_Property = date;
            multiable_Answer.user_Property = user;
        }
        public void Answer_question(string[] answer, string date, User user)
        {
            multiChoiceAnswercs = new MultiChoiceAnswercs(bound);
            multiChoiceAnswercs.Answer_question(answer, date, user);
        }


        public void Create_question(string title, string[] answer)
        {
            base.Create_question(title);
            this.answer = answer;
        }
        public string title_properties
        {
            get { return base.title; }
            set { base.title = value; }
        }
        public Question Getinfo()
        {
            return this;
        }
        public void Edit_question(Multible multible)
        {
            this.answer = multible.answer;
            this.require_Ans = multible.require_Ans;
        }

    }
}
