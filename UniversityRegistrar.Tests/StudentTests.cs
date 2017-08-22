using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using UniversityRegistrar.Models;

namespace UniversityRegistrar.Tests
{
  [TestClass]
  public class StudentTests : IDisposable
  {
    public void Dispose()
    {
      Student.DeleteAll();
      Course.DeleteAll();
    }

    public StudentTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=university_registrar_test;";
    }

    [TestMethod]
    public void Equals_ReturnsTrueIfDescriptionsAreTheSame_Task()
    {
      //Arrange, Act
      Student firstStudent = new Student("Michael", new DateTime(2017, 09, 20));
      Student secondStudent = new Student("Michael", new DateTime(2017, 09, 20));

      //Assert
      Assert.AreEqual(firstStudent, secondStudent);
    }

    [TestMethod]
    public void GetAll_DatabaseEmptyAtFirst_0()
    {
      //Arrange, Act
      int result = Student.GetAll().Count;

      //Assert
      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Save_SavesToDatabase_StudentList()
    {
      //Arrange
      Student testStudent = new Student("Kaili", new DateTime(2017, 09, 22));

      //Act
      testStudent.Save();
      List<Student> result = Student.GetAll();
      List<Student> testList = new List<Student>{testStudent};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }
//
    [TestMethod]
    public void Save_AssignsIdToObject_Id()
    {
      //Arrange
      Student testStudent = new Student("Michael", new DateTime(2017, 09, 22));

      //Act
      testStudent.Save();
      Student savedStudent = Student.GetAll()[0];

      int result = savedStudent.GetId();
      int testId = testStudent.GetId();

      //Assert
      Assert.AreEqual(testId, result);
    }

    [TestMethod]
    public void FindStudentById_FindsStudentInDatabase_Student()
    {
      //Arrange
      Student testStudent = new Student("Michael", new DateTime(2017, 09, 22));
      testStudent.Save();

      //Act
      Student foundStudent = Student.FindStudentById(testStudent.GetId());

      //Assert
      Assert.AreEqual(testStudent, foundStudent);
    }

      [TestMethod]
      public void AddCourse_AddsCourseToJoinTable_CourseList()
      {
        //Arrange
        Course testCourse = new Course("History");
        testCourse.Save();

        Student testCourse = new Student("Home stuff");
        testStudent.Save();

        //Act
        testTask.AddStudent(testCourse);

        List<Student> result = testTask.GetCategories();
        List<Student> testList = new List<Student>{testStudent};

        //Assert
        CollectionAssert.AreEqual(testList, result);
      }

    // [TestMethod]
    // public void Delete_DeletesStudentAssociationsFromDatabase_TaskList()
    // {
    //   //Arrange
    //   Course testCourse = new Course("History");
    //   testCourse.Save();
    //
    //   Student testStudent = new Student("Kaili", new DateTime(2017,09,  22));
    //   testStudent.Save();
    //
    //   //Act
    //   testStudent.AddCourse(testCourse);
    //   testStudent.Delete();
    //
    //   List<Student> resultCourseStudents = testCourse.GetStudents();
    //   List<Student> testCourseStudents = new List<Student> {};
    //
    //   //Assert
    //   CollectionAssert.AreEqual(testCourseStudents, resultCourseStudents);
    // }

//
//     [TestMethod]
//     public void GetCategories_ReturnsAllTaskCategories_CourseList()
//     {
//       //Arrange
//       Task testTask = new Task("Mow the lawn");
//       testTask.Save();
//
//       Category testCategory1 = new Category("Home stuff");
//       testCategory1.Save();
//
//       Category testCategory2 = new Category("Work stuff");
//       testCategory2.Save();
//
//       //Act
//       testTask.AddCategory(testCategory1);
//       List<Category> result = testTask.GetCategories();
//       List<Category> testList = new List<Category> {testCategory1};
//
//       //Assert
//       CollectionAssert.AreEqual(testList, result);
//     }
//

//
  }
}
