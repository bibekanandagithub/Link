using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace AutomationHelp
{
    public partial class Demo : Form
    {
        public Demo()
        {
            InitializeComponent();
        }
        XmlSerializer xs = null;
        List<Student> ls = null;
        private void button1_Click(object sender, EventArgs e)
        {
            FileStream fs = new FileStream("test.xml", FileMode.Create, FileAccess.ReadWrite);
            ls = new List<Student>
            {
                new Student{ ID=1,Name="Bibeka",Age=34},
                new Student{ ID=2,Name="Gul",Age=44}

            };
            


            xs.Serialize(fs, ls);
            fs.Close();
        }

        private void Demo_Load(object sender, EventArgs e)
        {
            ls = new List<Student>();
            xs = new XmlSerializer(typeof(List<Student>));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FileStream fs = new FileStream("test.xml", FileMode.Open, FileAccess.Read);
            ls =(List<Student>) xs.Deserialize(fs);
            dataGridView1.DataSource = ls;
            
            fs.Close();

        }
    }

    public class Student
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public int Age { set; get; }
    }
}
