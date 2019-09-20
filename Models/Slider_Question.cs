using SurveyMonkey_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyMonkey_Project.Models
{
    public class Slider_Question : Question
    {
        public int Id { get; set; }
        public int counter { get; set; }
        public int min_value { get; set; }
        public int max_value { get; set; }
        public bool requireAns { set; get; }
        public Multiable_answer multiable_Answer { get; set; }
        public LinkedList<Multiable_answer> List_multiable_Answer { get; set; }

        public Slider_Question(TypeQuestion type, string title) : base(type, title)
        {
            List_multiable_Answer = new LinkedList<Multiable_answer>();
            min_value = 0;
            Max_Value_Property = 10;
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
        public int Max_Value_Property
        {
            get { return this.max_value; }
            set { this.max_value = value; }
        }
        public int Min_Value_Property
        {
            get { return this.min_value; }
            set { this.min_value = value; }
        }
        public int Counter_Property
        {
            get { return this.counter; }
            set { this.counter = value; }
        }
        public Multiable_answer multiable_Answer_Properity
        {
            get { return multiable_Answer; }
            set { multiable_Answer = value; }
        }
        public LinkedList<Multiable_answer> List_multiable_Answer_properity
        {
            get { return this.List_multiable_Answer; }
            set { this.List_multiable_Answer = value; }
        }
        public void Answer_question(int answer, string date, User user)
        {
            counter = answer;
            multiable_Answer = new Multiable_answer(0);
            multiable_Answer.Choosen_Answer_Property = answer.ToString();
            multiable_Answer.user_Property = user;
            multiable_Answer.date_Property = date;
        }

        public void Create_question( int min, int max, bool requireAns)
        {
            this.min_value = min;
            this.max_value = max;
            this.requireAns = requireAns;
        }
        public void Edit_question(Slider_Question slider)
        {
            this.title = slider.title;
            this.min_value = slider.min_value;
            this.max_value = slider.max_value;
            this.requireAns = slider.requireAns;
        }
    }
}
