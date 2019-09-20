
using SurveyMonkey_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyMonkey_Project.Models
{
    class Filter_by_Country : Filter
    {
        
        public List<Multiable_answer> Filter_Country(string country,List<Multiable_answer> listMultibleAnswer)
        {

            for (int i = 0; i < listMultibleAnswer.Count;)
            {
                if (!listMultibleAnswer[i].user.Country.Equals(country))
                    listMultibleAnswer.RemoveAt(i);
                else
                    i++;
            }
            return listMultibleAnswer;
            
        }


        public List<User>[] Filter_Country(string country, List<User>[] listMultibleAnswer)
        {

            for (int i = 0; i < listMultibleAnswer.Length; i++)
            {
                for (int j = 0; j < listMultibleAnswer[i].Count;)
                    if (!listMultibleAnswer[i][j].Country.Equals(country))
                        listMultibleAnswer[i].Remove(listMultibleAnswer[i][j]);
                    else
                        j++;
            }
            return listMultibleAnswer;
        }
     }
}

