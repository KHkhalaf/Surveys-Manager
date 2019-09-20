
using SurveyMonkey_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyMonkey_Project.Models
{
    class Serializable_Questions
    {
        // Serialize Text Questions 
        static public string serialize_TextQuestion(Text_Question Q)
        {
            string Result = "";
            Result += Q.typeProperty + "~"
                   + Q.questionProperty + "~"
                   + Q.requireAnsProperty + "~"
                   + SERIALIZE_multiable_Answer(Q.multiAnswer_Properity) + "~"
                   + SERIALIZE_List_multiable_Answer(Q.list_multi_answers_properity.ToList());
            return Result;
        }
        // Deserialize Text Questions 
        static public KeyValuePair<Text_Question, int> deserialize_Text_Question(string Content, int iter)
        {
            string question = "";
            string requireAns = "";
            iter++;
            char i = Content[iter];

            while (i != '~')
            {
                question += i.ToString();
                iter++;
                i = Content[iter];
            }
            iter++;
            i = Content[iter];
            while (i != '~')
            {
                requireAns += i.ToString();
                iter++;
                i = Content[iter];
            }

            iter++;
            KeyValuePair<Multiable_answer, int> Pair = DESERIALIZE_Multiable_answer(Content, iter);
            Multiable_answer multiable_answer = Pair.Key;
            iter = Pair.Value;
            if (Content[iter].Equals('~'))
                iter++;
            i = Content[iter];
            string count_answers = "";
            while (i != '~')
            {
                count_answers += i.ToString();
                iter++;
                i = Content[iter];
            }
            LinkedList<Multiable_answer> multiable_Answers = new LinkedList<Multiable_answer>();
            for (int j = 0; j < Int32.Parse(count_answers); j++)
            {
                iter++;
                Pair = DESERIALIZE_Multiable_answer(Content, iter);
                iter = Pair.Value;
                multiable_Answers.AddLast(Pair.Key);
            }
            bool req = true;
            if (!requireAns.Contains("T"))
                req = false;
            Text_Question q = new Text_Question();
            q.Initialize(TypeQuestion.Text, question, req, "");
            q.multiAnswer_Properity = multiable_answer;
            q.list_multi_answers_properity = multiable_Answers;
            return new KeyValuePair<Text_Question, int>(q, iter);
        }

        // Serialize Multible Questions 
        static public string serialize_MultiChoice(Multible Q)
        {
            string Result = "";
            Result += Q.typeQuestion_properties + "~"
                   + Q.title_property + "~"
                   + Q.require_Ans.ToString() + "~"
                   + SERIALIZE_answer(Q.answer)
                   + SERIALIZE_multichoiceAnswers(Q.multiChoiceAnswercs) + "~"
                   + SERIALIZE_ListmultichoiceAnswer(Q.list_multiChoice_properties.ToList())
                   + SERIALIZE_multiable_Answer(Q.multiable_Answer) + "~"
                   + SERIALIZE_List_multiable_Answer(Q.List_List_multiable_Answer_properity.ToList());
            return Result;
        }
        static public string SERIALIZE_ListmultichoiceAnswer(List<MultiChoiceAnswercs> multiList)
        {
            string Result = multiList.Count.ToString() + "~";
            foreach (MultiChoiceAnswercs M_C in multiList)
            {
                Result += SERIALIZE_multichoiceAnswers(M_C) + "~";
            }
            return Result;
        }
        static public string SERIALIZE_multichoiceAnswers(MultiChoiceAnswercs multiChoicesAns)
        {

            if (multiChoicesAns == null)
                return " ";
            string Result = "";
            Result += SERIALIZE_answer(multiChoicesAns.chossenanswer_prperties)
                   + multiChoicesAns.date_Property + "~"
                   + multiChoicesAns.user_Property.UserName_Property + "~"
                   + multiChoicesAns.user_Property.Email_Property + "~"
                   + multiChoicesAns.user_Property.Password_property + "~"
                   + multiChoicesAns.user_Property.Age_property.ToString() + "~"
                   + multiChoicesAns.user_Property.Country_propety;
            return Result;
        }
        static public KeyValuePair<MultiChoiceAnswercs, int> DESERIALIZE_multichoiceAnswer(string Content, int iter)
        {
            string date = "", UserName = "", Email = "", Password = "", Age = "", Country = "";

            KeyValuePair<string[], int> pair = DESERILIZE_answer(Content, iter);
            if (pair.Key == null)
                return new KeyValuePair<MultiChoiceAnswercs, int>(null, pair.Value);
            string[] answers = pair.Key;
            iter = pair.Value;
            char i = Content[iter];
            iter++;
            i = Content[iter];
            while (i != '~')
            {
                date += i.ToString();
                iter++;
                i = Content[iter];
            }
            iter++;
            i = Content[iter];
            while (i != '~')
            {
                UserName += i.ToString();
                iter++;
                i = Content[iter];
            }
            iter++;
            i = Content[iter];
            while (i != '~')
            {
                Email += i.ToString();
                iter++;
                i = Content[iter];
            }
            iter++;
            i = Content[iter];
            while (i != '~')
            {
                Password += i.ToString();
                iter++;
                i = Content[iter];
            }
            iter++;
            i = Content[iter];
            while (i != '~')
            {
                Age += i.ToString();
                iter++;
                i = Content[iter];
            }
            iter++;
            i = Content[iter];
            while (i != '~')
            {
                Country += i.ToString();
                iter++;
                i = Content[iter];
            }
            MultiChoiceAnswercs multiable_answer = new MultiChoiceAnswercs(answers.Length);
            User user = new User(UserName, Password, Email, Int32.Parse(Age), Country, date);
            multiable_answer.user_Property = user;
            multiable_answer.date_Property = date;
            multiable_answer.chossenanswer_prperties = answers;
            return new KeyValuePair<MultiChoiceAnswercs, int>(multiable_answer, iter);
        }
        static public string SERIALIZE_answer(string[] answer)
        {
            string Result = answer.Length.ToString() + "~";
            for (int i = 0; i < answer.Length; i++)
            {
                Result += answer[i] + "~";
            }
            return Result;
        }
        static public KeyValuePair<string[], int> DESERILIZE_answer(string Content, int iter)
        {
            string bound = "";
            char i = Content[iter];
            while (i != '~')
            {
                bound += i.ToString();
                iter++;
                i = Content[iter];
            }
            iter++;
            i = Content[iter];
            if (bound == " ")
                return new KeyValuePair<string[], int>(null, iter);
            string[] answers = new string[Int32.Parse(bound)];
            for (int j = 0; j < Int32.Parse(bound); j++)
            {
                string answer = "";
                while (i != '~')
                {
                    answer += i.ToString();
                    iter++;
                    i = Content[iter];
                }
                iter++;
                i = Content[iter];
                answers[j] = answer;
            }
            return new KeyValuePair<string[], int>(answers, iter);
        }
        static public string SERIALIZE_multiable_Answer(Multiable_answer multiable_Answer)
        {
            if (multiable_Answer == null || multiable_Answer.user_Property == null)
                return " ";
            string Result = "";
            Result += multiable_Answer.Choosen_Answer_Property + "~"
                   + multiable_Answer.date_Property + "~"
                   + multiable_Answer.user_Property.UserName_Property + "~"
                   + multiable_Answer.user_Property.Email_Property + "~"
                   + multiable_Answer.user_Property.Password_property + "~"
                   + multiable_Answer.user_Property.Age_property.ToString() + "~"
                   + multiable_Answer.user_Property.Country_propety;
            return Result;
        }
        static public string SERIALIZE_List_multiable_Answer(List<Multiable_answer> multiable_Answers)
        {
            string Result = multiable_Answers.Count.ToString() + "~";
            foreach (Multiable_answer M_A in multiable_Answers)
            {
                Result += SERIALIZE_multiable_Answer(M_A) + "~";
            }
            return Result;
        }
        // Deserialize Multible Questions 
        static public KeyValuePair<Multible, int> deserialize_MultibleChoice(string Content, int iter, string type)
        {
            string title = "", require = "";

            iter++;
            char i = Content[iter];
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
                require += i.ToString();
                iter++;
                i = Content[iter];
            }
            iter++;
            KeyValuePair<string[], int> pair = DESERILIZE_answer(Content, iter);
            string[] answers = pair.Key;
            int bound = answers.Length;
            iter = pair.Value;
            KeyValuePair<MultiChoiceAnswercs, int> Pair = DESERIALIZE_multichoiceAnswer(Content, iter);
            iter = Pair.Value;
            if (Content[iter] == '~')
                iter++;
            i = Content[iter];
            string count_answers1 = "";
            while (i != '~')
            {
                count_answers1 += i.ToString();
                iter++;
                i = Content[iter];
            }
            LinkedList<MultiChoiceAnswercs> multiable_Answers1 = new LinkedList<MultiChoiceAnswercs>();
            for (int j = 0; j < Int32.Parse(count_answers1); j++)
            {
                iter++;
                Pair = DESERIALIZE_multichoiceAnswer(Content, iter);
                iter = Pair.Value;
                multiable_Answers1.AddLast(Pair.Key);
            }
            iter++;
            KeyValuePair<Multiable_answer, int> Pair1 = DESERIALIZE_Multiable_answer(Content, iter);
            iter = Pair1.Value;
            TypeQuestion _type = TypeQuestion.Multible;
            if (type.Contains("MultiChoice"))
                _type = TypeQuestion.MultiChoice;
            else if (type.Contains("DropDown"))
                _type = TypeQuestion.DropDown;
            Multible multible = new Multible(_type, title, bound);
            multible.answer = answers;
            multible.bound = bound;
            multible.multiChoiceAnswercs_properties = Pair.Key;
            multible.list_multiChoice_properties = multiable_Answers1;
            multible.multiable_Answer = Pair1.Key;
            // iter--;
            if (require.Contains("True"))
                multible.require_Ans = true;
            else
                multible.require_Ans = false;
            if (Content[iter].Equals('~'))
                iter++;
            i = Content[iter];
            string count_answers = "";
            while (i != '~')
            {
                count_answers += i.ToString();
                iter++;
                i = Content[iter];
            }
            LinkedList<Multiable_answer> multiable_Answers = new LinkedList<Multiable_answer>();
            for (int j = 0; j < Int32.Parse(count_answers); j++)
            {
                iter++;
                Pair1 = DESERIALIZE_Multiable_answer(Content, iter);
                iter = Pair1.Value;
                multiable_Answers.AddLast(Pair1.Key);
            }
            multible.List_List_multiable_Answer_properity = multiable_Answers;
            return new KeyValuePair<Multible, int>(multible, iter);
        }
        static public KeyValuePair<Multiable_answer, int> DESERIALIZE_Multiable_answer(string Content, int iter)
        {
            string Choosen_Answer = "", date = "", UserName = "",
                   Email = "", Password = "", Age = "", Country = "";

            //KeyValuePair<string[], int> pair = DESERILIZE_answer(Content, iter);
            //if (pair.Key == null)
            //    return new KeyValuePair<Multiable_answer, int>(null, pair.Value);
            //string[] answers = pair.Key;
            //iter = pair.Value;
            char i = Content[iter];
            if (i == ' ')
                return new KeyValuePair<Multiable_answer, int>(null, 2 + iter);
            while (i != '~')
            {
                Choosen_Answer += i.ToString();
                iter++;
                i = Content[iter];
            }
            iter++;
            i = Content[iter];
            while (i != '~')
            {
                date += i.ToString();
                iter++;
                i = Content[iter];
            }
            iter++;
            i = Content[iter];
            while (i != '~')
            {
                UserName += i.ToString();
                iter++;
                i = Content[iter];
            }
            iter++;
            i = Content[iter];
            while (i != '~')
            {
                Email += i.ToString();
                iter++;
                i = Content[iter];
            }
            iter++;
            i = Content[iter];
            while (i != '~')
            {
                Password += i.ToString();
                iter++;
                i = Content[iter];
            }
            iter++;
            i = Content[iter];
            while (i != '~')
            {
                Age += i.ToString();
                iter++;
                i = Content[iter];
            }
            iter++;
            i = Content[iter];
            while (i != '~')
            {
                Country += i.ToString();
                iter++;
                i = Content[iter];
            }
            //iter++;
            Multiable_answer multiable_answer = new Multiable_answer(0);
            User user = new User(UserName, Password, Email, Int32.Parse(Age), Country, date);
            multiable_answer.user_Property = user;
            multiable_answer.Choosen_Answer_Property = Choosen_Answer;
            multiable_answer.date_Property = date;
            return new KeyValuePair<Multiable_answer, int>(multiable_answer, iter);
        }

        //// Serialize DropDown Questions
        //static public string serialize_DropDown(DropDown Q)
        //{

        //    string content = Q.choices_Property.Count + "~";
        //    for(int i = 0; i < Q.choices_Property.Count; i++)
        //    {
        //        content += Q.choices_Property.ElementAt(i) + "~";
        //    }
        //    string Result = "";
        //    Result += Q.type_Property + "~"
        //           + Q.ID_Property + "~"
        //           + Q.questionText_Property + "~"
        //           + content
        //           + Q.answer_Property + "~"
        //           + Q.require_Ans_Property.ToString() + "~"
        //           + SERIALIZE_multiable_Answer(Q.multiAnswer_Property) + "~"
        //           + SERIALIZE_List_multiable_Answer(Q.list_multi_answers_Property.ToList());
        //    return Result;
        //}
        //// Deserialize DropDown Questions
        //static public KeyValuePair<DropDown, int> deserialize_DropDown(string Content, int iter)
        //{
        //    string questionText = "", answer = "" , require = "",ID = "";
        //    List<string> choices = new List<string>();
        //    iter++;
        //    char i = Content[iter];
        //    while (i != '~')
        //    {
        //        ID += i.ToString();
        //        iter++;
        //        i = Content[iter];
        //    }

        //    iter++;
        //    i = Content[iter];
        //    while (i != '~')
        //    {
        //        questionText += i.ToString();
        //        iter++;
        //        i = Content[iter];
        //    }
        //    iter++;
        //    i = Content[iter];
        //    string countChoices = "";
        //    while (i != '~')
        //    {
        //        countChoices += i.ToString();
        //        iter++;
        //        i = Content[iter];
        //    }
        //    for(int j = 0; j < Int32.Parse(countChoices); j++)
        //    {
        //        iter++;
        //        i = Content[iter];
        //        string choice = "";
        //        while (i != '~')
        //        {
        //            choice += i.ToString();
        //            iter++;
        //            i = Content[iter];
        //        }
        //        choices.Add(choice);
        //    }
        //    iter++;
        //    i = Content[iter];
        //    while (i != '~')
        //    {
        //        answer += i.ToString();
        //        iter++;
        //        i = Content[iter];
        //    }
        //    iter++;
        //    i = Content[iter];
        //    while (i != '~')
        //    {
        //        require += i.ToString();
        //        iter++;
        //        i = Content[iter];
        //    }
        //    iter++;
        //    KeyValuePair<Multiable_answer, int> Pair = DESERIALIZE_Multiable_answer(Content, iter);
        //    Multiable_answer multiable_answer = Pair.Key;
        //    iter = Pair.Value;
        //    i = Content[iter];
        //    string count_answers = "";
        //    while (i != '~')
        //    {
        //        count_answers += i.ToString();
        //        iter++;
        //        i = Content[iter];
        //    }
        //    LinkedList<Multiable_answer> multiable_Answers = new LinkedList<Multiable_answer>();
        //    for (int j = 0; j < Int32.Parse(count_answers); j++)
        //    {
        //        iter++;
        //        Pair = DESERIALIZE_Multiable_answer(Content, iter);
        //        iter = Pair.Value;
        //        multiable_Answers.AddLast(Pair.Key);
        //    }
        //    bool _require = false;
        //    if (require.Contains("True"))
        //        _require = true;
        //    DropDown dropDown = new DropDown(TypeQuestion.DropDown, questionText);
        //    dropDown.Initialize(TypeQuestion.DropDown, questionText, choices, answer, _require);
        //    dropDown.multiAnswer_Property = multiable_answer;
        //    dropDown.list_multi_answers_Property = multiable_Answers;
        //    dropDown.ID_Property = Int32.Parse(ID);
        //    return new KeyValuePair<DropDown, int>(dropDown, iter);
        //}

        // Serialize Slider Questions
        static public string serialize_Slider(Slider_Question Q)
        {
            string Result = "";
            Result += Q.typeQuestion_properties + "~"
                   + Q.ID_Property + "~"
                   + Q.title_property + "~"
                   + Q.Counter_Property.ToString() + "~"
                   + Q.Min_Value_Property.ToString() + "~"
                   + Q.Max_Value_Property.ToString() + "~"
                   + Q.requireAns.ToString() + "~"
                   + SERIALIZE_multiable_Answer(Q.multiable_Answer_Properity) + "~"
                   + SERIALIZE_List_multiable_Answer(Q.List_multiable_Answer_properity.ToList());
            return Result;
        }
        // Deserialize DropDown Questions
        static public KeyValuePair<Slider_Question, int> deserialize_Slider(string Content, int iter)
        {
            string questionText = "", Counter = "", Min_Value = "", Max_Value = "", requireAns = "", ID = "";
            List<string> choices = new List<string>();
            iter++;
            char i = Content[iter];
            while (i != '~')
            {
                ID += i.ToString();
                iter++;
                i = Content[iter];
            }
            iter++;
            i = Content[iter];
            while (i != '~')
            {
                questionText += i.ToString();
                iter++;
                i = Content[iter];
            }
            iter++;
            i = Content[iter];
            while (i != '~')
            {
                Counter += i.ToString();
                iter++;
                i = Content[iter];
            }
            iter++;
            i = Content[iter];
            while (i != '~')
            {
                Min_Value += i.ToString();
                iter++;
                i = Content[iter];
            }
            iter++;
            i = Content[iter];
            while (i != '~')
            {
                Max_Value += i.ToString();
                iter++;
                i = Content[iter];
            }
            iter++;
            i = Content[iter];
            while (i != '~')
            {
                requireAns += i.ToString();
                iter++;
                i = Content[iter];
            }
            iter++;
            KeyValuePair<Multiable_answer, int> Pair = DESERIALIZE_Multiable_answer(Content, iter);
            Multiable_answer multiable_answer = Pair.Key;
            iter = Pair.Value;
            if (Content[iter].Equals('~'))
                iter++;
            i = Content[iter];
            string count_answers = "";
            while (i != '~')
            {
                count_answers += i.ToString();
                iter++;
                i = Content[iter];
            }
            LinkedList<Multiable_answer> multiable_Answers = new LinkedList<Multiable_answer>();
            for (int j = 0; j < Int32.Parse(count_answers); j++)
            {
                iter++;
                Pair = DESERIALIZE_Multiable_answer(Content, iter);
                iter = Pair.Value;
                multiable_Answers.AddLast(Pair.Key);
            }
            Slider_Question slider = new Slider_Question(TypeQuestion.Slider, questionText);
            slider.Counter_Property = Int32.Parse(Counter);
            slider.List_multiable_Answer_properity = multiable_Answers;
            bool require = true;
            if (!requireAns.Contains("True"))
                require = false;
            slider.ID_Property = Int32.Parse(ID);
            slider.Create_question(Int32.Parse(Min_Value), Int32.Parse(Max_Value), require);
            slider.multiable_Answer_Properity = multiable_answer;
            return new KeyValuePair<Slider_Question, int>(slider, iter);
        }


        //static public string serialize(List<Question> Questions)
        //{
        //    string Result = Questions.Count.ToString() + "~";
        //    for (int i = 0; i < Questions.Count; i++)
        //    {
        //        Question q = new Question(Questions.ElementAt(i).Type_Property, Questions.ElementAt(i).ID_Property);
        //        Result += q.GetType() + "~" + q.ID_Property + "~";
        //    }
        //    return Result;
        //}
        //static public List<Question> deserialize(string Content)
        //{
        //    List<Question> questions = new List<Question>();
        //    int iter = 0;
        //    char i = Content[iter];
        //    string count = "";
        //    while (i != '~')
        //    {
        //        count += i.ToString();
        //        iter++;
        //        i = Content[iter];
        //    }
        //    int Count = Int32.Parse(count);
        //    for (int j = 0; j < Count; j++)
        //    {
        //        iter++;
        //        i = Content[iter];
        //        string typequestion = "";
        //        string id = "";
        //        while (i != '~')
        //        {
        //            typequestion += i.ToString();
        //            iter++;
        //            i = Content[iter];
        //        }
        //        iter++;
        //        i = Content[iter];
        //        while (i != '~')
        //        {
        //            id += i.ToString();
        //            iter++;
        //            i = Content[iter];
        //        }
        //if (typequestion.Contains("MultiChoice"))
        //{
        //    Question question = new Question(TypeQuestion.MultiChoice, Int32.Parse(id));
        //    questions.Add(question);
        //}
        //else if (typequestion.Contains("DropDown"))
        //{
        //    Question question = new Question(TypeQuestion.DropDown, Int32.Parse(id));
        //    questions.Add(question);
        //}
        //else if (typequestion.Contains("CommentBox"))
        //{
        //    Question question = new Question(TypeQuestion.CommentBox, Int32.Parse(id));
        //    questions.Add(question);
        //}
        //else if (typequestion.Contains("TextDescription"))
        //{
        //    Question question = new Question(TypeQuestion.TextDescription, Int32.Parse(id));
        //    questions.Add(question);
        //}
        //    }
        //    return questions;
        //}
    }
}
