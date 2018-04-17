using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace AutomationHelp
{
    public partial class xsd_help : Form
    {
        string fileName = string.Empty;
        public xsd_help()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string check = ValidateXML();
            MessageBox.Show(check);
        }

        private string ValidateXML()
        {
            try
            {
                string xmlpath = "PathStore.xml";
                string xsdpath = "PathStore.xsd";
                XmlReaderSettings rsetting = new XmlReaderSettings();
                rsetting.ValidationType = ValidationType.Schema;
                rsetting.Schemas.Add(null, XmlReader.Create(xsdpath));
                XmlReader xmlreader = XmlReader.Create(xmlpath, rsetting);
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(xmlreader);

                return "Validation Sucess";
            }catch(Exception ex)
            {
                return ex.Message;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            BookHelp<Books> blist = new BookHelp<Books>();

            Books b = new Books { BookAuthor="Juneja",BookID=100,BookName="Accounting"};
            Books bb = new Books { BookAuthor = "Tanjena", BookID = 102, BookName = "Accounting" };
            blist.Add(b);
            blist.Add(bb);

            Books b1 = blist.GetAll().SingleOrDefault(bw => bw.BookID == 100);
            blist.Remove(b1);

            foreach(var test in blist.GetAll())
            {
                MessageBox.Show(test.BookID + " " + test.BookName + "  " + test.BookAuthor);
            }


        }

        private void button3_Click(object sender, EventArgs e)
        {
            Action d = () => MessageBox.Show("Hello Action");
            d();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            #region ReadFileFormLocalDirectory
            Dictionary<string, string> collectiondictionary = new Dictionary<string, string>();
            XmlDocument pathretrivedoc = new XmlDocument();
            pathretrivedoc.Load("pathStore.xml");
            XmlNodeList collectionofnodes = pathretrivedoc.DocumentElement.SelectNodes("locations/DVT/Location");
            foreach (XmlNode xcollectnode in collectionofnodes)
            {

                #endregion

                string storepath = null;
                storepath = xcollectnode.InnerText.ToString();

                
                foreach (string one in Directory.EnumerateFiles(storepath, "*.xml"))
                {
                    XmlDocument xdoc = new XmlDocument();
                    xdoc.Load(one);
                    XmlNodeList columnnode = xdoc.DocumentElement.SelectNodes("Tests/Test");

                    foreach (XmlNode xnode in columnnode)
                    {

                        // collectiondictionary.Add(new Random().Next().ToString(), xnode.InnerText.ToString());
                        AddToDictionaryContainsKey(collectiondictionary, xnode.Attributes["id"].Value, xnode.InnerText.ToString());
                    }
                }
            }

            foreach(KeyValuePair<string,string> cols in collectiondictionary)
            {
                MessageBox.Show(cols.Key + "  " + cols.Value);
            }

        }

        public void AddToDictionaryContainsKey(Dictionary<string, string> myDictionary, string myKey, string myValue)
        {
            try
            {
                if (!myDictionary.ContainsKey(myKey))
                {
                    myDictionary.Add( myKey , myValue);
                    InsertData( myKey, myValue, "LocalCollection.xml");
                }
            }catch(Exception ex)
            {

            }
        }

        public void InsertData(string Testcaseid,string TestcaseName,string path)
        {

            XmlDocument doc3 = new XmlDocument();
            doc3.Load(path);
            XmlNode root1 = doc3.DocumentElement;
            //Create a new attrtibute.  
            XmlElement elem = doc3.CreateElement("Test");
            XmlAttribute attr = doc3.CreateAttribute("ID");
            attr.Value = Testcaseid;
            elem.Attributes.Append(attr);
            elem.InnerText = TestcaseName;
            //Add the node to the document.  
            root1.InsertAfter(elem, root1.LastChild);
            doc3.Save(path);
            // changed to

        }

        public void RemoveDuplicate(string path)
        {
            XDocument xDoc = XDocument.Parse(path);
            xDoc.Root.Elements("annotation")
                     .SelectMany(s => s.Elements("image")
                                       .GroupBy(g => g.Attribute("location").Value)
                                       .SelectMany(m => m.Skip(1))).Remove();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            string _Producttype = "TestClosingInsight";
            string _TestSetting = "LUIPUI.TestSettings";
            string _class = "TestClosingInsight.Test.CiPortalTest";
            string _Subject = "LUI ClosingCollab Portal"; ;
            string _dll = "TestClosingInsight.dll";
            List<ForTestCase> li = new List<ForTestCase>();
            li.AddRange(new ForTestCase[] { new ForTestCase {
                TestcaseID = 345677, TestcaseName = "Testing_server" } ,
                new ForTestCase { TestcaseID = 345678, TestcaseName = "Testing_server_tow" },
                new ForTestCase { TestcaseID = 345679, TestcaseName = "Testing_server_three" }
            });
            
            using (XmlWriter writer = XmlWriter.Create("xmlconfigstore.xml"))
            {
                
             //   writer.WriteStartDocument(true);
                writer.WriteComment("this is dynamically generated config file");
                writer.WriteStartElement("Configuration");
                writer.WriteElementString("ProductType", _Producttype);
                writer.WriteElementString("TestSetting", _TestSetting);
                writer.WriteElementString("Class", _class);
                writer.WriteElementString("Subject", _Subject);
                writer.WriteElementString("dll", _dll);
                writer.WriteStartElement("exportParameterTest");
                writer.WriteEndElement();
                writer.WriteElementString("email", "elnovio.amie@gmail.com");
                writer.WriteElementString("SuiteID","");
                writer.WriteStartElement("Tests");


                foreach(ForTestCase f in li)
                {
                    writer.WriteStartElement("test");
                    writer.WriteAttributeString("id", f.TestcaseID.ToString());
                    writer.WriteString(f.TestcaseName.ToString());
                    writer.WriteEndElement();
                }

              



                writer.WriteEndElement();
                 
                writer.WriteEndDocument();

            }
        }
    }
    public class ForTestCase
    {
        public int TestcaseID { set; get; }
        public string TestcaseName { set; get; }
    }
}
