using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
namespace SurveyMonkey_Project.Models
{
    class Serializable_Account
    {
        static public string serialize(List<User> Registers)
        {
            string Result = Registers.Count.ToString() + "~";
            for (int i = 0; i < Registers.Count; i++)
            {
                User R = new User(
                    Registers.ElementAt(i).UserName_Property,
                    Registers.ElementAt(i).Password_property,
                    Registers.ElementAt(i).Email_Property,
                    Registers.ElementAt(i).Age_property,
                    Registers.ElementAt(i).Country_propety,
                    Registers.ElementAt(i).Date_Property);
                Result += R.UserName_Property + "~" + R.Password_property + "~"
                       +  R.Email_Property + "~" + R.Age_property + "~"
                       +  R.Country_propety + "~" + R.Date_Property + "~";
            }
            return Result;
        }
        static public List<User> deserialize(string Content)
        {
            List<User> Registers = new List<User>();
            int iter = 0;
            char i = Content[iter];
            string count = "";
            while (i != '~')
            {
                count += i.ToString();
                iter++;
                i = Content[iter];
            }
            int Count = Int32.Parse(count);
            for (int j = 0; j < Count; j++)
            {
                iter++;
                i = Content[iter];
                string username = "";
                string password = "";
                string email = "";
                string age = "";
                string country = "";
                string date = "";
                while (i != '~')
                {
                    username += i.ToString();
                    iter++;
                    i = Content[iter];
                }
                iter++;
                i = Content[iter];
                while (i != '~')
                {
                    password += i.ToString();
                    iter++;
                    i = Content[iter];
                }
                iter++;
                i = Content[iter];
                while (i != '~')
                {
                    email += i.ToString();
                    iter++;
                    i = Content[iter];
                }
                iter++;
                i = Content[iter];
                while (i != '~')
                {
                    age += i.ToString();
                    iter++;
                    i = Content[iter];
                }
                iter++;
                i = Content[iter];
                while (i != '~')
                {
                    country += i.ToString();
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

                User Register = new User(username,password,email,Int32.Parse(age),country,date);
                    Registers.Add(Register);
            }
            return Registers;
        }
    }
}
