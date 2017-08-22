using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace UniversityRegistrar.Models
{
  public class Department
  {
    private int _id;
    private string _name;

    public Department(string name, int Id = 0)
    {
      _id = Id;
      _name = name;
    }

    public int GetId()
    {
      return _id;
    }

    public string GetName()
    {
      return _name;
    }

    public override bool Equals(System.Object otherDepartment)
    {
      if (!(otherDepartment is Department))
      {
        return false;
      }
      else
      {
        Department newDepartment = (Department) otherDepartment;
        bool idEquality = (this.GetId() == newDepartment.GetId());
        bool nameEquality = (this.GetName() == newDepartment.GetName());
        return (idEquality && nameEquality);
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
       cmd.CommandText = @"INSERT INTO departments (name) VALUES (@departmentName);";

       MySqlParameter nameParameter = new MySqlParameter();
       nameParameter.ParameterName = "@departmentName";
       nameParameter.Value = this._name;
       cmd.Parameters.Add(nameParameter);

       cmd.ExecuteNonQuery();
       _id = (int) cmd.LastInsertedId;
       conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
     }

     public static List<Department> GetAll()
     {
       List<Department> allDepartments = new List<Department> {};
       MySqlConnection conn = DB.Connection();
       conn.Open();
       var cmd = conn.CreateCommand() as MySqlCommand;
       cmd.CommandText = @"SELECT * FROM departments;";
       var rdr = cmd.ExecuteReader() as MySqlDataReader;
       while(rdr.Read())
       {
         int departmentId = rdr.GetInt32(0);
         string departmentName = rdr.GetString(1);
         Department newDepartment = new Department(departmentName, departmentId);
         allDepartments.Add(newDepartment);
       }
       conn.Close();
       if (conn != null)
       {
         conn.Dispose();
       }
       return allDepartments;
     }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM departments;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static Department Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM departments WHERE id = @thisId;";

      MySqlParameter idParameter = new MySqlParameter();
      idParameter.ParameterName = "@thisId";
      idParameter.Value = id;
      cmd.Parameters.Add(idParameter);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int departmentId = 0;
      string departmentName = "";

      while (rdr.Read())
      {
        departmentId = rdr.GetInt32(0);
        departmentName = rdr.GetString(1);
      }
      Department foundDepartment= new Department(departmentName, departmentId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return foundDepartment;
    }

    // public void Delete()
    // {
    //   MySqlConnection conn = DB.Connection();
    //   conn.Open();
    //
    //   MySqlCommand cmd = new MySqlCommand("DELETE FROM departments WHERE id = @DepartmentId; DELETE FROM departments_students WHERE course_id = @DepartmentId;", conn);
    //   MySqlParameter departmentIdParameter = new MySqlParameter();
    //   departmentIdParameter.ParameterName = "@DepartmentId";
    //   departmentIdParameter.Value = this.GetId();
    //
    //   cmd.Parameters.Add(departmentIdParameter);
    //   cmd.ExecuteNonQuery();
    //
    //   if (conn != null)
    //   {
    //     conn.Close();
    //   }
    // }

    // public void AddStudent(Student newStudent)
    // {
    //   MySqlConnection conn = DB.Connection();
    //   conn.Open();
    //   var cmd = conn.CreateCommand() as MySqlCommand;
    //   cmd.CommandText = @"INSERT INTO departments (course_id, student_id) VALUES (@DepartmentId, @StudentId);";
    //
    //   MySqlParameter studentIdParameter = new MySqlParameter();
    //   studentIdParameter.ParameterName = "@StudentId";
    //   studentIdParameter.Value = newStudent.GetId();
    //   cmd.Parameters.Add(studentIdParameter);
    //
    //   MySqlParameter departmentIdParameter = new MySqlParameter();
    //   departmentIdParameter.ParameterName = "@DepartmentId";
    //   departmentIdParameter.Value = _id;
    //   cmd.Parameters.Add(departmentIdParameter);
    //
    //   cmd.ExecuteNonQuery();
    //   conn.Close();
    //   if (conn != null)
    //   {
    //       conn.Dispose();
    //   }
    // }


    public List<Student> GetStudents()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT students.*
        FROM students WHERE department_id = @departmentId;";

      MySqlParameter departmentIdParameter = new MySqlParameter();
      departmentIdParameter.ParameterName = "@departmentId";
      departmentIdParameter.Value = _id;
      cmd.Parameters.Add(departmentIdParameter);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Student> allStudents = new List<Student> {};
        while(rdr.Read())
        {
          int thisStudentId = rdr.GetInt32(0);
          string studentName = rdr.GetString(1);
          DateTime enrollmentDate = rdr.GetDateTime(2);
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
