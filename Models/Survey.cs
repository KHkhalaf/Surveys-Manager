using SurveyMonkey_Project.Models;
using Surveys_Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SurveyMonkey_Project.Models
{
    public class Survey
    {
        public int Id { get; set; }
        public List<Question> Questions { get; set; }
        public LinkedList<Question> Q_list { get; set; }
        public ListsOfQuestions ListsOfQuestions { get; set; }
        public string User { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Date_publish { get; set; }
        public int Number_of_Participants { get; set; }
        public TypeSurvey type { get; set; }
        public TypeSurvey type_Property
        {
            get { return type; }
            set { type = value; }
        }
        public int Id_Property
        {
            get { return Id; }
            set { Id = value; }
        }

        public List<Question> Questions_Property
        {
            get { return Questions; }
            set { Questions = value; }
        }
        public LinkedList<Question> Q_list_properity
        {
            get { return Q_list; }
            set { Q_list = value; }
        }
        public string User_Property
        {
            get { return User; }
            set { User = value; }
        }
        public string Title_Property
        {
            get { return Title; }
            set { this.Title = value; }
        }
        public string Description_Property
        {
            get { return Description; }
            set { this.Description = value; }
        }
        public string Date_Property
        {
            get { return Date_publish; }
            set { Date_publish = value; }
        }

        public Survey(List<Question> questions, string user, string title, string description, string date_publish)
        {
            this.Questions = questions;
            this.User  = user;
            this.Title = title;
            this.Description = description;
            this.Date_publish = date_publish;
        }
        public void create(string title, string user, string description)
        {
            this.Title = title;
            this.Description = description;
            this.User  = user;
        }
        public void addQuestion(Question question)
        {
            Questions.Add(question);
        }
        public void Delete_question()
        {

            //Questions.Remove(Current_question);

        }

        public void Edit_question()
        {

            foreach (Question i in Questions)
            {
               // if (i.get_id() == Current_question.get_id())
                    Questions.Remove(i);
            }
            //Current_question.Edit_question();
            //Questions.Add(Current_question);
            //Questions.Remove(temp);
        }

        public static explicit operator Survey(ItemTappedEventArgs v)
        {
            throw new NotImplementedException();
        }
    }
}
