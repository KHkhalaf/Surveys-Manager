using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyMonkey_Project.Models
{
    public class ListsOfQuestions
    {
        public List<Text_Question> List_text { get; set; }
        public List<Multible> List_multible { get; set; }
        public List<Slider_Question> List_slider { get; set; }
        public ListsOfQuestions()
        {
            List_text = new List<Text_Question>();
            List_multible = new List<Multible>();
            List_slider = new List<Slider_Question>();
        }
    }
}
