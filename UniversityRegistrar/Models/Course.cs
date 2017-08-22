using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace UniversityRegistrar.Models
{
  public class Course
  {
    private int _id;
    private string _name;

    public Course(string name, int Id = 0)
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
       cmd.CommandText = @"INSERT INTO courses (name) VALUES (@courseName);";

       MySqlParameter nameParameter = new MySqlParameter();
       nameParameter.ParameterName = "@courseName";
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

      while (rdr.Read())
      {
        courseId = rdr.GetInt32(0);
        courseName = rdr.GetString(1);
      }
      Course foundCourse= new Course(courseName, courseId);
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

    // public void AddCategory(Category newCategory)
    // {
    //   MySqlConnection conn = DB.Connection();
    //   conn.Open();
    //   var cmd = conn.CreateCommand() as MySqlCommand;
    //   cmd.CommandText = @"INSERT INTO categories_tasks (category_id, task_id) VALUES (@CategoryId, @TaskId);";
    //
    //   MySqlParameter category_id = new MySqlParameter();
    //   category_id.ParameterName = "@CategoryId";
    //   category_id.Value = newCategory.GetId();
    //   cmd.Parameters.Add(category_id);
    //
    //   MySqlParameter task_id = new MySqlParameter();
    //   task_id.ParameterName = "@TaskId";
    //   task_id.Value = _id;
    //   cmd.Parameters.Add(task_id);
    //
    //   cmd.ExecuteNonQuery();
    //   conn.Close();
    //   if (conn != null)
    //   {
    //       conn.Dispose();
    //   }
    // }
    //
    //
    // public List<Category> GetCategories()
    // {
    //   MySqlConnection conn = DB.Connection();
    //   conn.Open();
    //   var cmd = conn.CreateCommand() as MySqlCommand;
    //   cmd.CommandText = @"SELECT categories.*
    //     FROM categories
    //     JOIN categories_tasks ON (categories.id = categories_tasks.category_id)
    //     JOIN tasks ON(tasks.id = categories_tasks.task_id)
    //     WHERE task_id = @taskId;";
    //
    //   MySqlParameter taskIdParameter = new MySqlParameter();
    //   taskIdParameter.ParameterName = "@taskId";
    //   taskIdParameter.Value = _id;
    //   cmd.Parameters.Add(taskIdParameter);
    //
    //   var rdr = cmd.ExecuteReader() as MySqlDataReader;
    //   List<Category> categories = new List<Category> {};
    //     while(rdr.Read())
    //     {
    //       int thisCategoryId = rdr.GetInt32(0);
    //       string categoryName = rdr.GetString(1);
    //       Category foundCategory = new Category(categoryName, thisCategoryId);
    //       categories.Add(foundCategory);
    //     }
    //   conn.Close();
    //   if (conn != null)
    //   {
    //     conn.Dispose();
    //   }
    //   return categories;
    // }


  }
}
