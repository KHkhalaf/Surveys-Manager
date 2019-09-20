using SurveyMonkey_Project.Models;
using Surveys_Manager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyMonkey_Project.Models
{
    class Serializable_Survey
    {
        static public string serialize(List<Survey> Surveys)
        {
            string Result = Surveys.Count.ToString() + "~";
            for (int i = 0; i < Surveys.Count; i++)
            {
                Survey S = new Survey(
                    Surveys.ElementAt(i).Questions_Property,
                    Surveys.ElementAt(i).User_Property,
                    Surveys.ElementAt(i).Title_Property,
                    Surveys.ElementAt(i).Description_Property,
                    Surveys.ElementAt(i).Date_Property);
                S.Number_of_Participants = Surveys[i].Number_of_Participants;
                S.type_Property = Surveys[i].type_Property;
                Result += Surveys[i].Id +"~"
                       + serialize_questions(S.Questions_Property)
                       + S.User_Property.ToString() + "~"
                       + S.Title_Property + "~"
                       + S.Description_Property + "~"
                       + S.Number_of_Participants + "~"
                       + S.type_Property +"~"
                       + S.Date_Property + "~";
            }
            return Result;
        }
        static public ObservableCollection<Survey> deserialize(string Content)
        {
            ObservableCollection<Survey> surveys = new ObservableCollection<Survey>();
            List<Question> questions = new List<Question>();
            int iter = 0;
            char i = Content[iter];
            string count_surveys = "";
            while (i != '~')
            {
                count_surveys += i.ToString();
                iter++;
                i = Content[iter];
            }
            int Count_Surveys = Int32.Parse(count_surveys);
            for (int j = 0; j < Count_Surveys; j++)
            {
                string count_questions = "";
                string id = "";
                questions = new List<Question>();
                iter++;
                i = Content[iter];
                while (i != '~')
                {
                    id += i.ToString();
                    iter++;
                    i = Content[iter];
                }
                iter++;
                i = Content[iter];
                while (i != '~')
                {
                    count_questions += i.ToString();
                    iter++;
                    i = Content[iter];
                }
                int Count_Questions = Int32.Parse(count_questions);
                for (int k = 0; k < Count_Questions; k++)
                {
                    iter++;
                    i = Content[iter];
                    string typequestion = "";
                    while (i != '~')
                    {
                        typequestion += i.ToString();
                        iter++;
                        i = Content[iter];
                    }
                    if (typequestion.Contains("Text"))
                    {
                       KeyValuePair<Text_Question, int> Pair = Serializable_Questions.deserialize_Text_Question(Content, iter);
                        questions.Add(Pair.Key);
                        iter = Pair.Value;
                    }
                    else if (typequestion.Contains("Multible") || typequestion.Contains("MultiChoice") || typequestion.Contains("DropDown"))
                    {
                        KeyValuePair<Multible, int> Pair = Serializable_Questions.deserialize_MultibleChoice(Content, iter , typequestion);
                        questions.Add(Pair.Key);
                        iter = Pair.Value;
                    }
                    //else if (typequestion.Contains("DropDown"))
                    //{
                    //    KeyValuePair<DropDown, int> Pair = Serializable_Questions.deserialize_DropDown(Content, iter);
                    //    questions.Add(Pair.Key);
                    //    iter = Pair.Value;
                    //}
                    else if (typequestion.Contains("Slider"))
                    {
                        KeyValuePair<Slider_Question, int> Pair = Serializable_Questions.deserialize_Slider(Content, iter);
                        questions.Add(Pair.Key);
                        iter = Pair.Value;
                    }
                    //else if (typequestion.Contains("TextDescription"))
                    //{
                    //    Question question = new Question(TypeQuestion.TextDescription, Int32.Parse(id));
                    //    questions.Add(question);
                    //}
                }

                iter++;
                i = Content[iter];
                string user = "";
                string title = "";
                string description = "";
                string date_publish = "";
                string number_of = "";
                string type = "";
                while (i != '~')
                {
                    user += i.ToString();
                    iter++;
                    i = Content[iter];
                }
                iter++;
                i = Content[iter];
                while (i != '~')
                {
                    title += i.ToString();
                    iter++;
                    i = Content[iter];
                }
                iter++;
                i = Content[iter];
                while (i != '~')
                {
                    description += i.ToString();
                    iter++;
                    i = Content[iter];
                }
                iter++;
                i = Content[iter];
                while (i != '~')
                {
                    number_of += i.ToString();
                    iter++;
                    i = Content[iter];
                }
                iter++;
                i = Content[iter];
                while (i != '~')
                {
                    type += i.ToString();
                    iter++;
                    i = Content[iter];
                }
                iter++;
                i = Content[iter];
                while (i != '~')
                {
                    date_publish += i.ToString();
                    iter++;
                    i = Content[iter];
                }
                Survey survey = new Survey(questions, user, title, description,date_publish);
                survey.Id = Int32.Parse(id);
                survey.Number_of_Participants = Int32.Parse(number_of);
                TypeSurvey _type = TypeSurvey.Education;
                if (type.Equals("Sport"))
                    _type = TypeSurvey.Sport;
                else if (type.Equals("Marketing"))
                    _type = TypeSurvey.Marketing;
                else if (type.Equals("Other"))
                    _type = TypeSurvey.Other;
                    survey.type_Property = _type;
                surveys.Add(survey);
            }
            return surveys;
        }
        static public string serialize_questions(List<Question> questions)
        {

            string Result = questions.Count.ToString() + "~";
            for (int i = 0; i < questions.Count; i++)
            {
                if (questions.ElementAt(i) is Text_Question)
                {
                    Result += Serializable_Questions.serialize_TextQuestion((Text_Question)questions.ElementAt(i));
                }
                //else if(questions.ElementAt(i) is DropDown)
                //{
                //    Result += Serializable_Questions.serialize_DropDown((DropDown)questions.ElementAt(i));
                //}
                else if(questions.ElementAt(i) is Slider_Question)
                {
                    Result += Serializable_Questions.serialize_Slider((Slider_Question)questions.ElementAt(i));
                }
                else if (questions.ElementAt(i) is Multible)
                {
                    Result += Serializable_Questions.serialize_MultiChoice((Multible)questions.ElementAt(i));
                }
            }
            return Result;
        }

    }
}

