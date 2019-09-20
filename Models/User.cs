using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyMonkey_Project.Models
{

    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Date { get; set; }
        public int Age { get; set; }
        public string Country { get; set; }
        public User(string UserName, string Password, string Email, int age, string Country, string date)
        {
            this.UserName = UserName;
            this.Password = Password;
            this.Email = Email;
            this.Age = age;
            this.Country = Country;
            this.Date = date;
        }

        public string Country_propety
        {
            set { this.Country = value; }
            get { return Country; }
        }
        public int Age_property
        {
            set { this.Age = value; }
            get { return Age; }
        }
        public string UserName_Property
        {
            get { return UserName; }
            set { this.UserName = value; }
        }
        public string Password_property
        {
            get { return Password; }
            set { this.Password = value; }
        }
        public string Email_Property
        {
            get { return Email; }
            set { this.Email = value; }
        }
        public string Date_Property
        {
            get { return Date; }
            set { this.Date = value; }
        }
        //get
        public User GetInfo()
        {
            return this;
        }


        //post
        public void Edit_post(User user)
        {
            this.UserName = user.UserName;
            this.Password = user.Password;
            this.Email = user.Email;
        }

    }
}


