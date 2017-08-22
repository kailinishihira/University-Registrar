using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace UniversityRegistrar.Models
{
  public class Course
  {
    private int _id;
    private string _name;
    private int _departmentId;

    public Course(string name, int departmentId, int Id = 0)
    {
      _id = Id;
      _name = name;
      _departmentId = departmentId;
    }

    public int GetId()
    {
      return _id;
    }

    public string GetName()
    {
      return _name;
    }
    public int GetDepartmentId()
    {
      return _departmentId;
    }

    public override bool Equals(System.Object otherCourse)
    {
      if (!(otherCourse is Course))
      {
        return false;
      }
      else
      {
        Course newCourse = (Course) otherCourse;
        bool idEquality = (this.GetId() == newCourse.GetId());
        bool nameEquality = (this.GetName() == newCourse.GetName());
        bool departmentIdEquality = (this.GetDepartmentId() == newCourse.GetDepartmentId());
        return (idEquality && nameEquality && departmentIdEquality);
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
       cmd.CommandText = @"INSERT INTO courses (name, department_id) VALUES (@courseName, @departmentId);";

       MySqlParameter nameParameter = new MySqlParameter();
       nameParameter.ParameterName = "@courseName";
       nameParameter.Value = this._name;
       cmd.Parameters.Add(nameParameter);

       MySqlParameter departmentIdParameter = new MySqlParameter();
       departmentIdParameter.ParameterName = "@departmentId";
       departmentIdParameter.Value = this._departmentId;
       cmd.Parameters.Add(departmentIdParameter);

       cmd.ExecuteNonQuery();
       _id = (int) cmd.LastInsertedId;
       conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
     }

     public static List<Course> GetAll()
     {
       List<Course> allCourses = new List<Course> {};
       MySqlConnection conn = DB.Connection();
       conn.Open();
       var cmd = conn.CreateCommand() as MySqlCommand;
       cmd.CommandText = @"SELECT * FROM courses;";
       var rdr = cmd.ExecuteReader() as MySqlDataReader;
       while(rdr.Read())
       {
         int courseId = rdr.GetInt32(0);
         string courseName = rdr.GetString(1);
         int departmentId = rdr.GetInt32(2);
         Course newCourse = new Course(courseName, courseId);
         allCourses.Add(newCourse);
       }
       conn.Close();
       if (conn != null)
       {
         conn.Dispose();
       }
       return allCourses;
     }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM courses;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static Course Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM courses WHERE id = @thisId;";

      MySqlParameter idParameter = new MySqlParameter();
      idParameter.ParameterName = "@thisId";
      idParameter.Value = id;
      cmd.Parameters.Add(idParameter);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int courseId = 0;
      string courseName = "";
      int departmentId = "";

      while (rdr.Read())
      {
        courseId = rdr.GetInt32(0);
        courseName = rdr.GetString(1);
        departmentId = rdr.GetInt32(2);
      }
      Course foundCourse= new Course(courseName, departmentId, courseId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return foundCourse;
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = new MySqlCommand("DELETE FROM courses WHERE id = @CourseId; DELETE FROM courses_students WHERE course_id = @CourseId;", conn);
      MySqlParameter courseIdParameter = new MySqlParameter();
      courseIdParameter.ParameterName = "@CourseId";
      courseIdParameter.Value = this.GetId();

      cmd.Parameters.Add(courseIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public void AddStudent(Student newStudent)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO courses_students (course_id, student_id) VALUES (@CourseId, @StudentId);";

      MySqlParameter studentIdParameter = new MySqlParameter();
      studentIdParameter.ParameterName = "@StudentId";
      studentIdParameter.Value = newStudent.GetId();
      cmd.Parameters.Add(studentIdParameter);

      MySqlParameter courseIdParameter = new MySqlParameter();
      courseIdParameter.ParameterName = "@CourseId";
      courseIdParameter.Value = _id;
      cmd.Parameters.Add(courseIdParameter);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
    }


    public List<Student> GetStudents()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT students.*
        FROM students
        JOIN courses_students ON (students.id = courses_students.student_id)
        JOIN courses ON(courses.id = courses_students.course_id)
        WHERE course_id = @courseId;";

      MySqlParameter courseIdParameter = new MySqlParameter();
      courseIdParameter.ParameterName = "@courseId";
      courseIdParameter.Value = _id;
      cmd.Parameters.Add(courseIdParameter);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Student> allStudents = new List<Student> {};
        while(rdr.Read())
        {
          int thisStudentId = rdr.GetInt32(0);
          string studentName = rdr.GetString(1);
          DateTime enrollmentDate = rdr.GetDateTime(2);
          int departmentId = rdr.GetInt32(3);
          Student foundStudent = new Student(studentName, enrollmentDate, thisStudentId);
          allStudents.Add(foundStudent);
        }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allStudents;
    }


  }
}
