using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using System.Windows.Forms;

namespace StudentsTbl
{
    internal class StudentManager
    {
        public string studentFile = @"C:\Users\kutlw\source\repos\StudentsTbl\students.txt";//File path

        public List<Student> GetAllStudents()
        {
            var students = new List<Student>();//Creates a list to hold all students
            if (File.Exists(studentFile))//Check if file exits before attmpting to read
            {

                var lines = File.ReadAllLines(studentFile);//Read all lines from the student file
                foreach (var line in lines.Skip(1))//Skip the first line which is the header
                {
                    var coloumns = line.Split(',');//Split current line by commas into coloumns
                    if (coloumns.Length == 4)//Ensure the line has exactly 4 columns
                    {
                        students.Add(new Student
                        {
                            Id = int.Parse(coloumns[0].Trim()),//Convert the first coloumn ID to an integer
                            Name = coloumns[1].Trim(),//Second coloumn as student nameand remove any surrounding spaces
                            Age = int.Parse((coloumns[2].Trim())),//Convert third coloumn age into an integer and remove spaces
                            Course = coloumns[3].Trim()//Fourth coloumn as the student course and remove any spaces


                        });
                    }


                }
            }
            return students;
        }
        public void SaveAllStudents(List<Student> students)
        {
            var lines = students.Select(s => $"{s.Id},{s.Name},{s.Age},{s.Course}");//Select all lines that include id,name,age and course
            File.WriteAllLines(studentFile, lines);//Saves and write to file 
        }
        public void AddStudent(int id, string name, int age, string course)

        {
            try
            {

               
                if (string.IsNullOrWhiteSpace(id.ToString()) || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(age.ToString()) || string.IsNullOrWhiteSpace(course))//Valdation if all courses have not been filled
                {
                    MessageBox.Show("Please fill in all fields.");
                    return;
                }
                if (age < 18)//If user enters an age less than 18 displays invalid age
                {
                    MessageBox.Show("Invalid age");
                }

                if (!string.Equals(course, "BIT", StringComparison.OrdinalIgnoreCase) || !string.Equals(course, "BCOMP", StringComparison.OrdinalIgnoreCase) || !string.Equals(course, "DIT", StringComparison.OrdinalIgnoreCase))//Display invalid course if it's neither of the courses mentioned
                {
                    MessageBox.Show("Invalid course");
                }
                if (id.ToString().Length != 4)//Student ID must consist of 4 charactes
                {
                    MessageBox.Show("Student ID needs to consist of 4 characters");
                }
                string studentRecord = $"{id},{name},{age},{course}";//Add to student record
                File.AppendAllText(studentFile, studentRecord + Environment.NewLine);//Add new students 
                MessageBox.Show("Student successfully added");//Message to show students successfully added to the record
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }
        public void DeleteStudent(int id)
        {
            try
            {
                var students = GetAllStudents();//Get all students
                for (int i = 0; i < students.Count; i++)//Loop through all students
                {
                    if (students[i].Id == id)
                    {
                        students.RemoveAt(i);//Removes at the specified index
                        break;
                    }
                    if (students[i].Id != id)
                    {
                        MessageBox.Show("Student ID does not exist");
                    }
                }
                if (id.ToString().Length != 4)//Student ID must consist of 4 characters
                {
                    MessageBox.Show("Student ID needs to consist of 4 characters");
                }
                SaveAllStudents(students);//Saves the rest of the other students
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           

            
        }
        public void UpdateStudent(int id, string studentName, int studentAge, string studentCourse)
        {
            try
            {
                var students = GetAllStudents();//Get all students
                var studentUpdate = students.Find(s => s.Id == id);//Takes the id from user input and find the ID in the list 
                if (studentUpdate == null)//Checks if student exists
                {
                   MessageBox.Show("Student ID not found");
                   return;

                }
                if (string.IsNullOrWhiteSpace(id.ToString()) || string.IsNullOrWhiteSpace(studentName) || string.IsNullOrWhiteSpace(studentAge.ToString()) || string.IsNullOrWhiteSpace(studentCourse)) //If either of the fields are not entered display message box
                {
                    MessageBox.Show("Please fill in all fields.");
                    return;
                }
                if (studentAge < 18)//If age is less than 18 display message box
                {
                    MessageBox.Show("Invalid age");
                }

                if (!string.Equals(studentCourse, "BIT", StringComparison.OrdinalIgnoreCase) && !string.Equals(studentCourse, "BCOMP", StringComparison.OrdinalIgnoreCase) && !string.Equals(studentCourse, "DIT", StringComparison.OrdinalIgnoreCase))//If it's neither of the courses display invalid course
                {
                    MessageBox.Show("Invalid course");
                }
                if (id.ToString().Length != 4)
                {
                    MessageBox.Show("Student ID needs to consist of 4 characters");
                }
                   //Instatiates the new values
                    studentUpdate.Name = studentName;
                    studentUpdate.Age = studentAge;
                    studentUpdate.Course = studentCourse;
                    SaveAllStudents(students);
                    MessageBox.Show("Student updated successfully");

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }
        public void GenerateSummary()
        {
            try
            {
                var students = GetAllStudents();//Get all students
                int studentCount = students.Count();//Count number of students
                double average = studentCount > 0 ? students.Average(s => s.Age) : 0;//Ensure theres more than one student if there is calculates the average if there isn't displays 0

                string summaryFilePath = @"C:\Users\kutlw\source\repos\StudentsTbl\summary.txt";//File path for summary.txt

                using (var fs = File.Create(summaryFilePath))//Create file
                {
                    using (var writer = new StreamWriter(fs))
                    {

                        writer.WriteLine($"ID,Name,Age,Course");//Headings
                        foreach (Student student in GetAllStudents())//Copy records from the student file 
                        {
                            writer.WriteLine($"{student.Id}\t,{student.Name}\t,{student.Age}\t,{student.Course}");//Display
                        }
                        writer.WriteLine();
                        writer.WriteLine("Total number of students: " + studentCount);//Display number of students
                        writer.WriteLine("Average students age: " + average);//Average students age

                    }

                }
                MessageBox.Show("Summary generated!");//Message to show the summary has been generated 
                System.Diagnostics.Process.Start(summaryFilePath);//Opens note pad
            }
            catch (Exception ex) { 
            
            MessageBox.Show(ex.Message );
            }
          
        }
    }
}

