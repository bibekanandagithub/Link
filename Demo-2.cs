using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
    public partial class Demo_2 : Form
    {
        public Demo_2()
        {
            InitializeComponent();
        }
        string xmlfilegen = "loc"+"//"+System.DateTime.Now.Year + System.DateTime.Now.ToString("MMM") + ".xml";
        private void Demo_2_Load(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '#';
            GetAllFolder(@"L:\TestBuilds");
          
            bool result = checkFileExist(xmlfilegen);
            if (result == false)
            {
                CreateXmlFile(xmlfilegen);
            }
        }
        private bool Checkpass(string yourpass)
        {
            if (yourpass.Equals(System.DateTime.Now.ToString("dd") + System.DateTime.Now.DayOfWeek.ToString()))
            {
                return true;
            }
            return false;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string source = string.Empty;
            string remoteConfiglocation = string.Empty;
            string remoteLocation = string.Empty;
            source = "loc" + "//" + System.DateTime.Now.Year + System.DateTime.Now.ToString("MMM") + ".xml";
            string source2 = "loc" + "//" + "test.bat";
            remoteConfiglocation = textBox1.Text + "Configs" + "\\" + System.DateTime.Now.Year + System.DateTime.Now.ToString("MMM") + ".xml";
            remoteLocation = textBox1.Text +  "test.bat";
            File.Move(source, remoteConfiglocation);
            File.Move(source2, remoteLocation);
           
            System.Threading.Thread.Sleep(1000);

            Process proc = null;
            int ExitCode;

            string _batDir = string.Format(textBox1.Text);
            proc = new Process();
            proc.StartInfo.WorkingDirectory = _batDir;
            proc.StartInfo.FileName = "test.bat";
            proc.StartInfo.CreateNoWindow = false;
            
            proc.Start();
            proc.WaitForExit();
            ExitCode = proc.ExitCode;
            proc.Close();
            MessageBox.Show("Script executed sucessfully...");
            File.Delete(remoteConfiglocation);
            MessageBox.Show("Temp config deleted sucessfully...");

        }

        private void button2_Click(object sender, EventArgs e)
        {
          
           
           
                XDocument doc = XDocument.Load(xmlfilegen);

                var query =
                    from foo in doc.Root.Elements("ConfigLog")
                    where foo.Element("CodeText").Value == "TestCountReport"
                    group foo by (string)foo.Element("CodeText") into g1
                    select new
                    {
                        bar = g1.Key,
                        bazValues = g1.Count()
                    };


                foreach (var a in query)
                {
                    Debug.WriteLine("Items is {0} and count is {1}", a.bar, a.bazValues);
                }


            

        }

        private void CreateXmlFile(string xmlfilepathis)
        {
            using (XmlWriter xwriter = XmlWriter.Create(xmlfilepathis))
            {
                xwriter.WriteStartElement("ConfigLogs");
                xwriter.WriteStartElement("ConfigLog");
                xwriter.WriteElementString("CodeText","TestCountReport");
                xwriter.WriteElementString("Datavalue", "SuiteID:458963");
                xwriter.WriteElementString("StartDate", DateTime.Now.ToString());
                xwriter.WriteElementString("StartTime", DateTime.Now.ToShortTimeString());
                xwriter.WriteElementString("EndTime", DateTime.Now.ToShortTimeString()+10);
                xwriter.WriteElementString("TimeDiff", "1 Min");
                xwriter.WriteEndElement();
                xwriter.Flush();



            }
        }

        bool checkFileExist(string filename)
        {
            try
            {
               
                if (File.Exists(filename))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (IOException ex) { }
            catch (Exception ex) { }
            return false;
        }

        private void button3_Click(object sender, EventArgs e)
        {

            XDocument doc = XDocument.Load(xmlfilegen);

            var query =
               (from foo in doc.Root.Elements("ConfigLog")
                where foo.Element("CodeText").Value == "TestCountReport"
                select new
                {
                    StartDate= foo.Element("StartDate").Value,
                    ReportName = foo.Element("CodeText").Value,
                    StartTime = foo.Element("StartTime").Value,
                    EndTime = foo.Element("EndTime").Value,
                    TimeDiff = foo.Element("TimeDiff").Value
                }).OrderByDescending(f=> f.StartTime).Take(2);

            dataGridView1.DataSource = query.ToArray();


        }
        private void GetAllFolder(string folderpath)
        {
            foreach (string flist in Directory.GetDirectories(folderpath))
            {
                treeView1.Nodes.Add(Path.GetFileName(flist));
            }

        }
        public static void RecursiveDelete(DirectoryInfo baseDir)
        {
            if (!baseDir.Exists)
                return;

            foreach (var dir in baseDir.EnumerateDirectories())
            {
                RecursiveDelete(dir);
            }
            baseDir.Delete(true);
        }
        private string ReturnMain()
        {
            string retval = string.Empty;
            try
            {
               
                if (radioButton1.Checked==true)
                {
                    retval = "DVT";

                }
                else if (radioButton2.Checked == true)
                {
                    retval = "STG";
                }
                else if (radioButton3.Checked == true)
                {
                    retval = "PRD";
                }
                else
                {
                    retval = "DVT";
                }
                if (Directory.Exists("c:\\Build\\" + retval))
                {

                }
                else
                {
                    Directory.CreateDirectory("c:\\Build\\" + retval);
                }
            }catch(IOException ex) { }
            catch(Exception ex) { }

            return retval;

        }
        private void button4_Click(object sender, EventArgs e)
        {
            bool result = Checkpass(textBox2.Text.Trim());
            if (result == true)
            {
                ReturnMain();
                string desti = string.Empty;
                desti = "C:\\Builds\\" + ReturnMain() + "\\" + treeView1.SelectedNode.Text + "\\";
                string source = "L:\\TestBuilds\\" + treeView1.SelectedNode.Text;
                RecursiveDelete(new DirectoryInfo(desti));
                MessageBox.Show("All Existing folder with the same name eleted...");
                System.Threading.Thread.Sleep(1000);
                string STABLE_FOLDER = source;
                string UPDATE_FOLDER = desti;

                // Get our files (recursive and any of them, based on the 2nd param of the Directory.GetFiles() method
                string[] originalFiles = Directory.GetFiles(STABLE_FOLDER, "*", SearchOption.AllDirectories);

                // Dealing with a string array, so let's use the actionable Array.ForEach() with a anonymous method
                Array.ForEach(originalFiles, (originalFileLocation) =>
                {
                    // Get the FileInfo for both of our files
                    FileInfo originalFile = new FileInfo(originalFileLocation);
                    FileInfo destFile = new FileInfo(originalFileLocation.Replace(STABLE_FOLDER, UPDATE_FOLDER));
                    // ^^ We can fill the FileInfo() constructor with files that don't exist...

                    // ... because we check it here
                    if (destFile.Exists)
                    {
                        // Logic for files that exist applied here; if the original is larger, replace the updated files...
                        if (originalFile.Length > destFile.Length)
                        {
                            originalFile.CopyTo(destFile.FullName, true);
                        }
                    }
                    else // ... otherwise create any missing directories and copy the folder over
                    {
                        Directory.CreateDirectory(destFile.DirectoryName); // Does nothing on directories that already exist
                        originalFile.CopyTo(destFile.FullName, false); // Copy but don't over-write  
                    }

                });
                MessageBox.Show("File Created Sucessfully..");
                textBox1.Text = desti;
            }
            else
            {
                MessageBox.Show("Invalid Credential..");
            }
            
        }
        private void MoveLocation(string sourcelocation,string desti)
        {
            
           
           File.Move(sourcelocation, desti);
            //File.Move(source2, remoteLocation);
        }
        public List<string> checknode = new List<string>();
        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {

            if (e.Node.Checked)
            {
                checknode.Add(e.Node.FullPath.ToString());
            }
            else
            {
                
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
