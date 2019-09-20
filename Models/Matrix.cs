using SurveyMonkey_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyMonkey_Project.Models
{
    class Matrix : Multible
    {
        private Multible[] multiable_question;
        private int bound2;

        public Multible[] multiable_question_property
        {
            get { return multiable_question; }
            set { this.multiable_question = value; }
        }
        public int bound2_Property
        {
            get { return bound2; }
            set { this.bound2 = value; }
        }

        public Matrix(TypeQuestion type, string[] title, int boundAnswer, int boundQustion) : base(type, title[0], boundAnswer)
        {
            this.bound2 = boundQustion;
            multiable_question = new Multible[boundQustion];
            for (int i = 0; i < boundQustion; i++)
            {
                multiable_question[i] = new Multible(type, title[i], boundAnswer);
            }
        }


        public void Answer_question(string[] answer, string date, User user)
        {
            for (int i = 0; i < this.multiable_question.Length; i++)
            {
                int x = multiable_Answer.Answer_Property.Length;
                Multiable_answer matrix_Answer = new Multiable_answer(x);
                base.multiable_Answer = new Multiable_answer(x);
                multiable_Answer.Choosen_Answer_Property = answer[i];
                multiable_Answer.user_Property = user;
                multiable_Answer.date_Property = date;
                multiable_Answer.Id_proprities = i;
                base.List_List_multiable_Answer_properity.AddFirst(multiable_Answer);
                foreach (Multiable_answer m in List_List_multiable_Answer_properity) ;
            }

        }
        public void Create_question(string[] answer, string[] titles)
        {
            base.List_multiable_Answer = new LinkedList<Multiable_answer>();
            for (int i = 0; i < this.multiable_question.Length; i++)
            {
                this.multiable_question[i].Create_question(titles[i]);
                this.multiable_question[i].multiable_Answer_propiretes.Answer_Property = answer;
            }
        }
        public Question Getinfo()
        {
            return this;
        }
        public Matrix Edit_question(Matrix matrix)
        {
            this.answer = matrix.answer;
            this.title = matrix.title;
            return this;
        }
    }
}
