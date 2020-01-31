using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;

namespace AddSkills
{
    public partial class Form1 : Form
    {
               
        Course selectedCourse;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtSkill.Focus();            
            this.AcceptButton = btnAdd;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            selectedCourse = (Course)listBoxCourses.SelectedItem;
            
            string skill = txtSkill.Text;
            if(listBoxCourses.SelectedItem == null)
            {
                MessageBox.Show("Select course first");
                txtSkill.Clear();
                txtSkill.Focus();
                return;
            }
            if (selectedCourse.Skills.Contains(skill))            
            {
                MessageBox.Show("You have already added this skill");
                txtSkill.Clear();
                txtSkill.Focus();
                return;

            }
            else if (txtSkill.Text.Length < 1)
            {
                MessageBox.Show("Please enter a skill");
                txtSkill.Clear();
                txtSkill.Focus();
                return;
            }
            else
            {                
                selectedCourse.addSkill(skill);
                listBoxSkills.Items.Add(skill);
                txtSkill.Clear();
                txtSkill.Focus();
            }
            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listBoxCourses.SelectedItem == null)
            {
                MessageBox.Show("Select skill first");
                txtSkill.Clear();
                txtSkill.Focus();
                return;
            }
            else
            {
                string selectedSkill = listBoxSkills.SelectedItem.ToString();
                selectedCourse.removeSkill(selectedSkill);
                listBoxSkills.Items.Remove(selectedSkill);
            }
            
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            serialiseJSON();
            
        }

        private void listBoxCourses_SelectedIndexChanged(object sender, EventArgs e)
        {            
            listBoxSkills.Items.Clear();
            if (listBoxCourses.SelectedItem == null)
            {
                MessageBox.Show("Select a course");
                txtSkill.Clear();
                txtSkill.Focus();
                return;
            }
            else
            {
                selectedCourse = (Course)listBoxCourses.SelectedItem;
                lblSelectedCourse.Text = selectedCourse.Title;
                lblCourseLevel.Text = selectedCourse.Level;

                if (selectedCourse.Skills.Count > 0)
                {
                    foreach (string s in selectedCourse.Skills)
                    {
                        if (listBoxSkills.Items.Contains(s))
                        {
                            return;
                        }
                        else
                            listBoxSkills.Items.Add(s);
                    }
                }
            }

        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            listBoxSkills.Items.Clear();
            listBoxCourses.Items.Clear();

            try
            {
                DialogResult result = openFileDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string file = openFileDialog1.FileName;

                    using (StreamReader r = new StreamReader(file))
                    {
                        string json = r.ReadToEnd();
                        List<Course> courses = JsonConvert.DeserializeObject<List<Course>>(json);

                        if (courses.Count < 1)
                        {
                            MessageBox.Show("This file can't be read");
                            return;
                        }
                        else
                        {
                            listBoxCourses.DisplayMember = "Title";
                            foreach (Course c in courses)
                            {
                                listBoxCourses.Items.Add(c);
                            }
                        }

                    }
                }              
                       
            }
            catch (Exception ex)
            {
                MessageBox.Show("we had a problem: " + ex.Message.ToString());
                return;
            }
        }

        private void serialiseJSON()
        {
            try
            {
                if (listBoxCourses.Items.Count < 1)
                {
                    MessageBox.Show("This will create an empty file you donkey!");
                    return;
                }
                else
                {
                    HashSet<string> uniqueSkills = new HashSet<string>();
                    List<Course> lstcourses = new List<Course>();
                    foreach (Course c in listBoxCourses.Items)
                    {

                        lstcourses.Add(c);
                        foreach(string s in c.Skills)
                        {
                            uniqueSkills.Add(s);
                        }
                    }

                   
                    string jCourses = JsonConvert.SerializeObject(lstcourses, Formatting.Indented);

                    string jSkills = JsonConvert.SerializeObject(uniqueSkills, Formatting.Indented);
                    //
                    //This part was selecting file save destination but for some reason it still saves it in: AddSkills\AddSkills\bin\Debug\
                    //
                    //Stream myStream;
                    //SaveFileDialog saveFileDialog1 = new SaveFileDialog();

                    //saveFileDialog1.Filter = "JSON files (*.JSON)|*.JSON|All files (*.*)|*.*";
                    //saveFileDialog1.FilterIndex = 2;
                    //saveFileDialog1.RestoreDirectory = true;


                    //if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    //{
                    //    if ((myStream = saveFileDialog1.OpenFile()) != null)
                    //    {
                    // Code to write the stream goes here.
                    //string path = @"C:\json\courses.json";
                    //using (var tw = new StreamWriter(path, true))
                    //{
                    //    tw.WriteLine(jCourses.ToString());
                    //    tw.Close();
                    //}

                    System.IO.File.WriteAllText("CoursesWithSkills.json", jCourses);
                    System.IO.File.WriteAllText("UniqueSkills.json", jSkills);
                    MessageBox.Show("JSON files has been successfuly created");
                    //        myStream.Close();
                    //    }
                    //}
                    //string path = @"C:\Users\lukas\source\repos\AddSkills\AddSkills\bin\Debug\";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("we had a problem: " + ex.Message.ToString());
                return;
            }
        }
    }
}
