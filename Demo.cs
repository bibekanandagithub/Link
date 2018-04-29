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
using Trezorix.Qxr.Controls;
using System.Diagnostics;

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
            



            //ls = new List<Student>();
            //xs = new XmlSerializer(typeof(List<Student>));

            XmlRender.XmlBrowser xml = new XmlRender.XmlBrowser();
            xml.Height = 200;
            xml.Width = 200;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable("Author");
            dt.Columns.Add("Auth_Id", typeof(int));
            dt.Columns.Add("Auth_Name", typeof(string));
            dt.Columns.Add("Auth_Loaction", typeof(string));
            DataRow dr = dt.NewRow();
            dr[0] = 10;
            dr[1] = "Krishna Garad";
            dr[2] = "Hyderabad";
            DataRow dr1 = dt.NewRow();
            dr1[0] = 20;
            dr1[1] = "Mahesh Chand";
            dr1[2] = "Phili";
            DataRow dr2 = dt.NewRow();
            dr2[0] = 30;
            dr2[1] = "Suthish Nair";
            dr2[2] = "Mumbai";
            DataRow dr3 = dt.NewRow();
            dr3[0] = 40;
            dr3[1] = "Amit";
            dr3[2] = "Delhi";
            dt.Rows.Add(dr);
            dt.Rows.Add(dr1);
            dt.Rows.Add(dr2);
            dt.Rows.Add(dr3);
            ds.Tables.Add(dt);
          //  xml.XmlText = ds.GetXml();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            FileStream fs = new FileStream("test.xml", FileMode.Open, FileAccess.Read);
            ls =(List<Student>) xs.Deserialize(fs);
            dataGridView1.DataSource = ls;
            
            fs.Close();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<int> testlist = new List<int>(4);
            var nums = Enumerable.Range(0, 10).ToArray();
            var rnd = new Random();

            // Shuffle the array
            for (int i = 0; i < nums.Length; ++i)
            {
                int randomIndex = rnd.Next(nums.Length);
                int temp = nums[randomIndex];
                nums[randomIndex] = nums[i];
                nums[i] = temp;
            }

            // Now your array is randomized and you can simply print them in order
            //for (int i = 0; i < nums.Length; ++i)
            //    System.Diagnostics.Debug.WriteLine(nums[i]);




            var groupSize = 4;
            // The characters 'a' - 'z'.
            var source = Enumerable.Range(0, 10).Select(i => nums[i]);
            var groups = source
              .Select((x, i) => new { Item = x, Index = i })
              .GroupBy(x => x.Index / groupSize, x => x.Item);
            foreach (var group in groups)
                System.Diagnostics.Debug.WriteLine("{0}: {1}", group.Key, String.Join(", ", group));


        }
        
        
        public static T[] GetRandomArray<T>(T[] array, int size)
        {
            List<T> list = new List<T>();
            T element;
            int tries = 0;
            int maxTries = array.Length;

            while (tries < maxTries && list.Count < size)
            {
                element = array[array.Length];

                if (!list.Contains(element))
                {
                    list.Add(element);
                }
                else
                {
                    tries++;
                }
            }

            if (list.Count > 0)
            {
                return list.ToArray();
            }
            else
            {
                return null;
            }
        }

    }

    public class Student
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public int Age { set; get; }
    }
}
