using SurveyMonkey_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyMonkey_Project.Models
{

    class AnalaysesQuestion
    {
        Question question;
        List<User>[] user;
        List<User>[][] usermatrix;
        TypeQuestion typeQuestion;

        public Question question_properties
        {
            get { return this.question; }
        }
        public List<User>[] user_properities
        {
            get { return this.user; }
            set { this.user = value; }

        }
        public List<User>[][] usermatrix_properities
        {
            get { return this.usermatrix; }
            set { this.usermatrix = value; }
        }
        public TypeQuestion typeQuestion_properties
        {
            set { this.typeQuestion = value; }
            get { return typeQuestion; }
        }
        public List<User>[] Analys(Question question)
        {
            if (question.typeQuestion_properties == TypeQuestion.Multible || question.typeQuestion_properties == TypeQuestion.DropDown)
            {
                this.question = (Multible)question;

                typeQuestion = TypeQuestion.Multible;

                user = Get_Users_Multi(((Multible)question).List_List_multiable_Answer_properity.ToList(), ((Multible)question).answer);
                return this.user;
            }
            else if (question.typeQuestion_properties == TypeQuestion.Slider)
            {
                this.question = (Slider_Question)question;
                typeQuestion = TypeQuestion.Slider;
                user = new List<User>[((Slider_Question)question).Max_Value_Property];

                for (int i = 0; i < user.Length; i++)
                    user[i] = new List<User>();

                foreach (Multiable_answer multiable_Answer in ((Slider_Question)question).List_multiable_Answer_properity)
                {
                    user[int.Parse(multiable_Answer.Choosen_Answer_Property)].Add(multiable_Answer.user_Property);
                }

                return this.user;
            }
            else if (question.typeQuestion_properties == TypeQuestion.Matrix)
            {
                typeQuestion = TypeQuestion.Matrix;
                this.usermatrix = AnalysMatrix((Matrix)question);
            }
            else if (question.typeQuestion_properties == TypeQuestion.MultiChoice)
            {
                typeQuestion = TypeQuestion.MultiChoice;
                this.user = Analys_multi(question);
                return this.user;
            }
            return null;

        }
        public List<User>[][] AnalysMatrix(Matrix question)
        {
            this.question = (Matrix)question;
            usermatrix = new List<User>[question.multiable_question_property.Length][];
            for (int i = 0; i < usermatrix.Length; i++)
                usermatrix[i] = new List<User>[question.multiable_Answer_propiretes.Answer_Property.Length];
            for (int i = 0; i < usermatrix.Length; i++)
            {
                for (int j = 0; j < usermatrix[i].Length; j++)
                {
                    usermatrix[i][j] = new List<User>();
                }

            }

            foreach (Multiable_answer multiable_Answer in (question).List_List_multiable_Answer_properity)
            {

                for (int i = 0; i < usermatrix.Length; i++)
                {
                    if (multiable_Answer.Id_proprities == i)
                        for (int j = 0; j < usermatrix[i].Length; j++)
                        {

                            if ((question.multiable_question_property[i].multiable_Answer_propiretes.Answer_Property[j]).Equals(multiable_Answer.Choosen_Answer_Property))
                            {
                                usermatrix[i][j].Add(multiable_Answer.user_Property);
                            }

                        }
                }
            }
            return this.usermatrix;

        }
        private List<User>[] Analys_multi(Question question)
        {

            this.question = (Multible)question;

            typeQuestion = TypeQuestion.Multible;
            user = Get_UsersMultiChoice(((Multible)question).list_multiChoice_properties.ToList(), ((Multible)question).answer);

            return this.user;

        }
        public int Average(List<Multiable_answer> List_multiable_Answer, int index, int max_value)
        {
            int sum = 0;
            for (int i = 0; i < List_multiable_Answer.Count; i++)
            {
                sum += Int32.Parse(List_multiable_Answer.ElementAt(i).choosen_answer);
            }
            if (List_multiable_Answer.Count == 0)
                return 0;
            return ((sum / List_multiable_Answer.Count) * 100) / max_value;
        }
        public List<User>[] Get_Users_Multi(List<Multiable_answer> List_multiable_Answer, string[]  answers)
        {
            List<User>[] user = new List<User>[answers.Length];
            for (int i = 0; i < user.Length; i++)
                user[i] = new List<User>();
            foreach (Multiable_answer multiable_Answer in List_multiable_Answer)
            {
                for (int i = 0; i < user.Length; i++)
                {
                    if (answers[i].Equals(multiable_Answer.Choosen_Answer_Property))
                    {
                        user[i].Add(multiable_Answer.user_Property);
                    }
                }
            }
            return user;
        }

        public List<User>[] Get_UsersMultiChoice(List<MultiChoiceAnswercs> List_multiable_Answer, string[] answers)
        {
            List<User>[] user = new List<User>[answers.Length];
            for (int i = 0; i < user.Length; i++)
                user[i] = new List<User>();
            foreach (MultiChoiceAnswercs multiable_Answer in List_multiable_Answer)
            {
                for (int i = 0; i < user.Length; i++)
                {
                    if (answers[i].Equals(multiable_Answer.chossenanswer[i]))
                    {
                        user[i].Add(multiable_Answer.user_Property);
                    }
                }
            }
            return user;
        }
    }
}