using SurveyMonkey_Project.Models;
using System.Collections.Generic;

namespace SurveyMonkey_Project
{
    class Filter_by_Age : Filter

    {
        public Filter_by_Age()
        {
        }
        public List<Multiable_answer> Filter_Age(int minAge,int maxAge, List<Multiable_answer> listMultibleAnswer)
        {

            for (int i = 0; i < listMultibleAnswer.Count;)
            {
                if (listMultibleAnswer[i].user.Age > maxAge || listMultibleAnswer[i].user.Age < minAge)
                    listMultibleAnswer.RemoveAt(i);
                else
                    i++;
            }
            return listMultibleAnswer;
        }


        public List<User>[] Filter_Age(int minAge, int maxAge, List<User>[] Users_answers)
        {

            for (int i = 0; i < Users_answers.Length; i++)
            {
                for (int j = 0; j < Users_answers[i].Count;)
                    if (Users_answers[i][j].Age > maxAge || Users_answers[i][j].Age < minAge)
                        Users_answers[i].RemoveAt(j);
                    else
                        j++;
            }
            return Users_answers;
        }
    }
}
