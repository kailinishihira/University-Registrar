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
    private int _departmentId;


    public Student(string name, DateTime enrollmentDate, int departmentId = 0, int id = 0)
    {
      _name = name;
      _enrollmentDate = enrollmentDate;
      _departmentId = departmentId;
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
    public void SetDepartmentId(int departmentId)
    {
      _departmentId = departmentId;
    }
    public int GetDepartmentId()
    {
      return _departmentId;
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
        bool departmentIdEquality = (this.GetDepartmentId() == otherStudent.GetDepartmentId());
        return (idEquality && nameEquality && enrollmentDateEquality && departmentIdEquality);
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
      cmd.CommandText = @"INSERT INTO students  (name, enrollment_date, department_id) VALUES (@name, @enrollmentDate, @department_id);";
      MySqlParameter nameParameter = new MySqlParameter();
      nameParameter.ParameterName = "@name";
      nameParameter.Value = this._name;
      cmd.Parameters.Add(nameParameter);

      MySqlParameter enrollmentDateParameter = new MySqlParameter();
      enrollmentDateParameter.ParameterName = "@enrollmentDate";
      enrollmentDateParameter.Value = this._enrollmentDate;
      cmd.Parameters.Add(enrollmentDateParameter);

      MySqlParameter departmentIdParameter = new MySqlParameter();
      departmentIdParameter.ParameterName = "@department_id";
      departmentIdParameter.Value = this._departmentId;
      cmd.Parameters.Add(departmentIdParameter);

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
        int departmentId = rdr.GetInt32(3);
        Student newStudent = new Student(StudentName, enrollmentDate, departmentId, StudentId);
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
      int departmentId = 0;

      while (rdr.Read())
      {
        StudentId = rdr.GetInt32(0);
        StudentName = rdr.GetString(1);
        enrollmentDate = rdr.GetDateTime(2);
        departmentId = rdr.GetInt32(3);
      }
      Student newStudent = new Student(StudentName, enrollmentDate, departmentId, StudentId);
      return newStudent;
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = new MySqlCommand("DELETE FROM students WHERE id = @StudentId; DELETE FROM courses_students WHERE student_id = @StudentId;", conn);
      MySqlParameter studentIdParameter = new MySqlParameter();
      studentIdParameter.ParameterName = "@StudentId";
      studentIdParameter.Value = this.GetId();

      cmd.Parameters.Add(studentIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

   public List<Course> GetCourses()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT courses.*
          FROM students
        JOIN courses_students ON (students.id = courses_students.student_id)
        JOIN courses ON (courses_students.course_id = courses.id)
        WHERE students.id = @StudentId;";

      MySqlParameter studentIdParameter = new MySqlParameter();
      studentIdParameter.ParameterName = "@StudentId";
      studentIdParameter.Value = _id;
      cmd.Parameters.Add(studentIdParameter);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Course> allCourses = new List<Course> {};
      while(rdr.Read())
        {
          int thisCourseId = rdr.GetInt32(0);
          string courseName = rdr.GetString(1);
          int departmentId = rdr.GetInt32(2);
          Course foundCourse = new Course(courseName, departmentId, thisCourseId);
          allCourses.Add(foundCourse);
        }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allCourses;
    }

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
