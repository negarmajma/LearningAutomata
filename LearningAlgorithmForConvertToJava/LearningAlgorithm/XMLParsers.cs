using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace LearningAlgorithm
{
    public static class XMLParsers
    {
        private static string xmlUrl = "../../XMLFile/Student.xml";

        // Parse the xml using XMLDocument class.
        public static StudentsInformation ParseByXMLDocument()
        {
            var students = new StudentsInformation();

            XmlDocument doc = new XmlDocument();
            doc.Load(xmlUrl);

            XmlNode GeneralInformationNode =
                doc.SelectSingleNode("/StudentsInformation/GeneralInformation");
            students.School =
                GeneralInformationNode.SelectSingleNode("School").InnerText;
            students.Department =
                GeneralInformationNode.SelectSingleNode("Department").InnerText;

            XmlNode StudentListNode =
                doc.SelectSingleNode("/StudentsInformation/Studentlist");
            XmlNodeList StudentNodeList =
                StudentListNode.SelectNodes("Student");
            foreach (XmlNode node in StudentNodeList)
            {
                Student aStudent = new Student();
                aStudent.id = Convert.ToInt16(node.Attributes
                    .GetNamedItem("id").Value);
                aStudent.name = node.InnerText;
                aStudent.score = Convert.ToInt16(node.Attributes
                    .GetNamedItem("score").Value);
                aStudent.enrollment =
                    node.Attributes.GetNamedItem("enrollment").Value;
                aStudent.comment =
                    node.Attributes.GetNamedItem("comment").Value;

                students.Studentlist.Add(aStudent);
            }

            return students;
        }

        // Parse the xml using XDocument class.
        public static StudentsInformation ParseByXDocument()
        {
            var students = new StudentsInformation();

            XDocument doc = XDocument.Load(xmlUrl);
            XElement generalElement = doc
                    .Element("StudentsInformation")
                    .Element("GeneralInformation");
            students.School = generalElement.Element("School").Value;
            students.Department = generalElement.Element("Department").Value;

            students.Studentlist = (from c in doc.Descendants("Student")
                                    select new Student()
                                    {
                                        id = Convert.ToInt16(c.Attribute("id").Value),
                                        name = c.Value,
                                        score = Convert.ToInt16(c.Attribute("score").Value),
                                        enrollment = c.Attribute("enrollment").Value,
                                        comment = c.Attribute("comment").Value
                                    }).ToList<Student>();

            return students;
        }
    }




    public  class XMLParsersPacemaker
    {
       // private static string xmlUrl = "../../mainNet3.xml";
     
        // Parse the xml using XMLDocument class.
        public static PlaceInformation ParseByXMLDocument()
        {
            string xmlUrl = "../../result.xml";
            var places = new PlaceInformation();

            XmlDocument doc = new XmlDocument();
            doc.Load(xmlUrl);

            XmlNode PageNode = doc.SelectSingleNode("/workspaceElements/cpnet/page");

          
            XmlNodeList PageNodeList =PageNode.SelectNodes("place");

            foreach (XmlNode node in PageNodeList)
            {
                XmlNode PlaceInfo = doc.SelectSingleNode("/workspaceElements/cpnet/page/place/initmark");
                XmlNodeList PlaceNodeList = PlaceInfo.SelectNodes("text");

                Place age = new Place();
                age.name = node.InnerText;


                places.Placelist.Add(age);
            }

            return places;
        }



        //Write


       public static void ParseByXMLDocumentWrite(string Inivalue)
        {
            string xmlUrl = "../../mainNet3.xml";
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlUrl);

            XmlNode PageNode = doc.SelectSingleNode("/workspaceElements/cpnet/page");
            XmlNodeList PageNodeList = PageNode.SelectNodes("place");
            int i =0;
           
           foreach (XmlNode node in PageNodeList)
            {
                XmlNode PlaceInfo = doc.SelectSingleNode("/workspaceElements/cpnet/page/place/initmark");
                XmlNodeList PlaceNodeList = PlaceInfo.SelectNodes("text");

                XmlNodeList PageNodeList2 = node.SelectNodes("initmark/text");
               if (i==0)
                    PageNodeList2.Item(0).InnerText = Inivalue;
                i = i + 1;             
            }
            doc.Save(xmlUrl);
           
        }



      
        // Parse the xml using XDocument class.
        //public static PlaceInformation ParseByXDocument()
        //{
        //    var places = new PlaceInformation();

        //    XDocument doc = XDocument.Load(xmlUrl);
           

        //    places.Placelist = (from c in doc.Descendants("Place")
        //                            select new Place()
        //                            {
                                        
        //                                name = c.Value,
                                       
        //                            }).ToList<Place>();

        //    return places;
        //}
    }
}
