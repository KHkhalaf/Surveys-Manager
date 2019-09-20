using SurveyMonkey_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyMonkey_Project.Models
{
    public class Question
    {
        public static int ID{ set; get; }
        public string title { set; get; }
        public TypeQuestion Type { set; get; }
        public int Id { set; get; }
        public Question() { }
        public Question(TypeQuestion type,string title)
        {
            this.Type = type;
            Id = ++ID;
            this.title = title;
        }
        //public int ID_properity
        //{
        //    get { return Id; }
        //    set { Id = value; }
        //}
        public TypeQuestion typeQuestion_properties
        {
            get { return this.Type; }
            set { this.Type = value; }
        }
        public string title_property
        {
            get { return this.title; }
            set { this.title = value; }
        }
        public void Create_question(string title)
        {
            this.title = title;
        }


    }
}
