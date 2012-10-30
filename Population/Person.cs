using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Population
{
    public class Person
    {
        //public Person()
        //{
        //    firstName = "";
        //    middleName = "";
        //    lastName = "";
        //    favouriteColour = Color.Azure;
        //    birthdate = System.DateTime.Now;
        //}
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string lastName { get; set; }
        public DateTime birthdate { get; set; }
        public Color favouriteColour { get; set; }
        public string quote { get; set; }
    }
}
