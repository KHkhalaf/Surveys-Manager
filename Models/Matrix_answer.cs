using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyMonkey_Project.Models
{
    class Matrix_answer : Multiable_answer
    {

        int id;
        public Matrix_answer(int bound) : base(bound)
        {

        }
        public int ID_proprities
        {
            get { return id; }
            set { id = value; }
        }

    }
}
