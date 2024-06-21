using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace LearningAlgorithm
{
    public class Student
    {
        public int id { get; set; }
        public string name { get; set; }
        public int score { get; set; }
        public string enrollment { get; set; }
        public string comment { get; set; }
    }

    public class StudentsInformation
    {
        public string School { get; set; }
        public string Department { get; set; }
        public List<Student> Studentlist { get; set; }

        public StudentsInformation()
        {
            School = "N/A";
            Department = "N/A";
            Studentlist = new List<Student>();
        }
    }

    public class Place
    {
        public string name { get; set; }
       
    }

    public class PlaceInformation
    {
        public string name { get; set; }
        public List<Place> Placelist { get; set; }

        public PlaceInformation()
        {
            name = "N/A";
            Placelist = new List<Place>();
        }
    }
}
