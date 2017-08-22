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
    public void Equals_ReturnsTrueIfDescriptionsAreTheSame_Student()
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
        Student testStudent = new Student("Michael", new DateTime(2017, 09, 22));
        testStudent.Save();

        Course testCourse1 = new Course("History");
        testCourse1.Save();
        Course testCourse2 = new Course("Physics");
        testCourse2.Save();

        //Act
        testStudent.AddCourse(testCourse1);
        testStudent.AddCourse(testCourse2);

        List<Course> result = testStudent.GetCourses();
        List<Course> testList = new List<Course>{testCourse1, testCourse2};

        //Assert
        CollectionAssert.AreEqual(testList, result);
      }

    [TestMethod]
    public void Delete_DeletesStudentAssociationsFromDatabase_StudentList()
    {
      //Arrange
      Course testCourse = new Course("History");
      testCourse.Save();

      Student testStudent = new Student("Kaili", new DateTime(2017,09,  22));
      testStudent.Save();

      //Act
      testStudent.AddCourse(testCourse);
      testStudent.Delete();

      List<Student> resultCourseStudents = testCourse.GetStudents();
      List<Student> testCourseStudents = new List<Student> {};

      //Assert
      CollectionAssert.AreEqual(testCourseStudents, resultCourseStudents);
    }

    [TestMethod]
    public void GetCourses_ReturnsAllCourseStudents_CourseList()
    {
      //Arrange
      Student testStudent = new Student("Michael", new DateTime(2017, 09, 22));
      testStudent.Save();

      Course testCourse1 = new Course("History");
      testCourse1.Save();

      Course testCourse2 = new Course("Physics");
      testCourse2.Save();

      //Act
      testStudent.AddCourse(testCourse1);
      testStudent.AddCourse(testCourse2);
      List<Course> result = testStudent.GetCourses();
      List<Course> testList = new List<Course> {testCourse1, testCourse2};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }
  }
}
