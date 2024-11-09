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

namespace StudentsTbl
{
    public partial class Form1 : Form
    {
        private StudentManager _studentManager;
        public Form1()
        {
            InitializeComponent();
            _studentManager = new StudentManager();
        }




        private void btnLoad_Click(object sender, EventArgs e)
        {

            try
            {
                
                var students = _studentManager.GetAllStudents();//Load table
                dataGridView1.DataSource = students;//Connect text to datagridview
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
       

        private void btnInsert_Click(object sender, EventArgs e)
        {
            //Represents textboxes input
            int id = int.Parse(txtID.Text);
            string name = txtName.Text;
            int age = int.Parse(txtAge.Text);
            string course = txtCourse.Text;
            _studentManager.AddStudent(id, name, age, course);//Insert student to the table
            MessageBox.Show("Student successfully added");
          btnLoad_Click(null,null);//Refresh after insert
        }


        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //Represents textboxes input
            int studentIDupdate = int.Parse(txtID.Text);
            string nameupdate = txtName.Text;
            int age = int.Parse(txtAge.Text);
            string course = txtCourse.Text;
            _studentManager.UpdateStudent(studentIDupdate, nameupdate, age, course);//Update 
            MessageBox.Show("Student successfully added");//Display successfull when student has been updated to the table
            btnLoad_Click(null, null);//Refreshes table after deleting
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int studentID= int.Parse(txtID.Text);
            _studentManager.DeleteStudent(studentID);//Delete student at specified ID
            MessageBox.Show("Student successfully deleted");//Display message successfull when the student has been deleted
            btnLoad_Click(null, null);//Refreshes table after deleting
        }
        // CellClick Event to handle deleting a student when clicking on a DataGridView row
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
          
        }

        private void btnGenerateSummary_Click(object sender, EventArgs e)
        {
            _studentManager.GenerateSummary();//Generate summary report
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
