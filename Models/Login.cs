using SurveyMonkey_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyMonkey_Project.Models
{
    class Login : User
    {
        Login(string UserName, string Password, string Name, string Email, string date) : base(UserName,Password, Email,0,"", date) { }
    }
}
