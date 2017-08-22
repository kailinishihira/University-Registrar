using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace UniversityRegistrar.Models
{
  public class Student
  {
    private int _id;
    private string _name;
    private DateTime _enrollmentDate;


    public Student(string name, DateTime enrollmentDate, int id = 0)
    {
      _name = name;
      _enrollmentDate = enrollmentDate;
      _id = id;
    }

    public int GetId()
    {
      return _id;
    }

    public string GetName()
    {
      return _name;
    }

    public DateTime GetEnrollmentDate()
    {
      return _enrollmentDate;
    }

    public override bool Equals (System.Object otherStudent)
    {
      if (!(otherStudent is Student))
      {
        return false;
      }
      else
      {
        Student newStudent = (Student) otherStudent;
        bool idEquality = (this.GetId() == newStudent.GetId());
        bool nameEquality = (this.GetName() == newStudent.GetName());
        bool enrollmentDateEquality = (this.GetEnrollmentDate() == newStudent.GetEnrollmentDate());
        return (idEquality && nameEquality && enrollmentDateEquality);
      }
    }

    public override int GetHashCode()
    {
      return this.GetId().GetHashCode();
    }


    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO students  (name, enrollment_date) VALUES (@name, @enrollmentDate);";
      MySqlParameter nameParameter = new MySqlParameter();
      nameParameter.ParameterName = "@name";
      nameParameter.Value = this._name;
      cmd.Parameters.Add(nameParameter);

      MySqlParameter enrollmentDateParameter = new MySqlParameter();
      enrollmentDateParameter.ParameterName = "@enrollmentDate";
      enrollmentDateParameter.Value = this._enrollmentDate;
      cmd.Parameters.Add(enrollmentDateParameter);
      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM students;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
//
    public static List<Student> GetAll()
    {
      List<Student> allStudents = new List<Student>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM students;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      while(rdr.Read())
      {
        int StudentId = rdr.GetInt32(0);
        string StudentName = rdr.GetString(1);
        DateTime enrollmentDate = rdr.GetDateTime(2);
        Student newStudent = new Student(StudentName, enrollmentDate, StudentId);
        allStudents.Add(newStudent);
      }
      return allStudents;
    }

    public static Student FindStudentById(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM students WHERE id = (@searchId);";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int StudentId = 0;
      string StudentName = "";
      DateTime enrollmentDate = DateTime.MinValue;

      while (rdr.Read())
      {
        StudentId = rdr.GetInt32(0);
        StudentName = rdr.GetString(1);
        enrollmentDate = rdr.GetDateTime(2);
      }
      Student newStudent = new Student(StudentName, enrollmentDate, StudentId);
      return newStudent;
    }
//
//     public static void DeleteCategory(int id)
//     {
//       MySqlConnection conn = DB.Connection();
//       conn.Open();
//       var cmd = conn.CreateCommand() as MySqlCommand;
//       cmd.CommandText = @"DELETE FROM categories WHERE id = @thisId;";
//
//       MySqlParameter categoryId = new MySqlParameter();
//       categoryId.ParameterName = "@thisId";
//       categoryId.Value = id;
//       cmd.Parameters.Add(categoryId);
//
//       cmd.ExecuteNonQuery();
//     }
//
//     public void Delete()
//     {
//       MySqlConnection conn = DB.Connection();
//       conn.Open();
//
//       MySqlCommand cmd = new MySqlCommand("DELETE FROM categories WHERE id = @CategoryId; DELETE FROM categories_tasks WHERE category_id = @CategoryId;", conn);
//       MySqlParameter categoryIdParameter = new MySqlParameter();
//       categoryIdParameter.ParameterName = "@CategoryId";
//       categoryIdParameter.Value = this.GetId();
//
//       cmd.Parameters.Add(categoryIdParameter);
//       cmd.ExecuteNonQuery();
//
//       if (conn != null)
//       {
//         conn.Close();
//       }
//     }
//
//    public List<Course> GetTasks()
//     {
//       MySqlConnection conn = DB.Connection();
//       conn.Open();
//       var cmd = conn.CreateCommand() as MySqlCommand;
//       cmd.CommandText = @"SELECT tasks.*
//           FROM categories
//         JOIN categories_tasks ON (categories.id = categories_tasks.category_id)
//         JOIN tasks ON (categories_tasks.task_id = tasks.id)
//         WHERE categories.id = @CategoryId;";
//
//       MySqlParameter categoryIdParameter = new MySqlParameter();
//       categoryIdParameter.ParameterName = "@CategoryId";
//       categoryIdParameter.Value = _id;
//       cmd.Parameters.Add(categoryIdParameter);
//
//       var rdr = cmd.ExecuteReader() as MySqlDataReader;
//       List<Task> tasks = new List<Task> {};
//       while(rdr.Read())
//         {
//           int thisTaskId = rdr.GetInt32(0);
//           string taskDescription = rdr.GetString(1);
//           Task foundTask = new Task(taskDescription, thisTaskId);
//           tasks.Add(foundTask);
//         }
//       conn.Close();
//       if (conn != null)
//       {
//         conn.Dispose();
//       }
//       return tasks;
//     }

    public void AddCourse(Course newCourse)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO courses_students (course_id, student_id) VALUES (@CourseId, @StudentId);";

      MySqlParameter courseIdParameter = new MySqlParameter();
      courseIdParameter.ParameterName = "@CourseId";
      courseIdParameter.Value = newCourse.GetId();
      cmd.Parameters.Add(courseIdParameter);

      MySqlParameter studentIdParameter = new MySqlParameter();
      studentIdParameter.ParameterName = "@StudentId";
      studentIdParameter.Value = _id;
      cmd.Parameters.Add(studentIdParameter);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }


  }
}
