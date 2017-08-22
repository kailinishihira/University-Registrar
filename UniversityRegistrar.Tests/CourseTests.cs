using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using UniversityRegistrar.Models;

namespace UniversityRegistrar.Tests
{
  [TestClass]
  public class CategoryTests : IDisposable
  {
    public CategoryTests()
    {
        DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=university_registrar_test;";
    }

    public void Dispose()
    {
      Student.DeleteAll();
      Course.DeleteAll();
    }

    [TestMethod]
    public void GetAll_CoursesEmptyAtFirst_0()
    {
      //Arrange, Act
      int result = Course.GetAll().Count;

      //Assert
      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Equals_ReturnsTrueForSameName_True()
    {
      //Arrange, Act
      Course firstCourse = new Course("History");
      Course secondCourse = new Course("History");

      //Assert
      Assert.AreEqual(firstCourse, secondCourse);
    }

  [TestMethod]
    public void Save_DatabaseAssignsIdToCourse_Id()
    {
      //Arrange
      Course testCourse = new Course("History");
      testCourse.Save();

      //Act
      Course savedCourse = Course.GetAll()[0];

      int result = savedCourse.GetId();
      int testId = testCourse.GetId();

      //Assert
      Assert.AreEqual(testId, result);
    }

    [TestMethod]
    public void Save_SavesCourseToDatabase_CourseList()
    {
      //Arrange
      Course testCourse = new Course("History");
      testCourse.Save();

      //Act
      List<Course> result = Course.GetAll();
      List<Course> testList = new List<Course>{testCourse};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void Find_FindsCourseInDatabase_Course()
    {
      //Arrange
      Course testCourse = new Course("History");
      testCourse.Save();

      //Act
      Course foundCourse = Course.Find(testCourse.GetId());

      //Assert
      Assert.AreEqual(testCourse, foundCourse);
    }

    [TestMethod]
    public void Delete_DeletesCourseFromDatabase_CourseList()
    {
      //Arrange
      string name1 = "History";
      Course testCourse1 = new Course(name1);
      testCourse1.Save();

      string name2 = "Physics";
      Course testCourse2 = new Course(name2);
      testCourse2.Save();

      //Act
      testCourse1.Delete();
      List<Course> resultCategories = Course.GetAll();
      List<Course> testCourseList = new List<Course> {testCourse2};

      //Assert
      CollectionAssert.AreEqual(testCourseList, resultCategories);
    }

    [TestMethod]
    public void AddStudent_AddsStudentToJoinTable_StudentList()
    {
      //Arrange
      Course testCourse = new Course("History");
      testCourse.Save();

      Student testStudent = new Student("Kaili", new DateTime(2017, 09, 22));
      testStudent.Save();

      Student testStudent2 = new Student("Michael", new DateTime(2017, 09, 22));
      testStudent2.Save();

      //Act
      testCourse.AddStudent(testStudent);
      testCourse.AddStudent(testStudent2);

      List<Student> result = testCourse.GetStudents();
      List<Student> testList = new List<Student>{testStudent, testStudent2};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void GetStudents_ReturnsAllStudentsForCourse_StudentList()
    {
      //Arrange
      Course testCourse = new Course("History");
      testCourse.Save();

      Student testStudent1 = new Student("Kaili", new DateTime(2017, 09, 22));
      testStudent1.Save();

      Student testStudent2 = new Student("Michael", new DateTime(2017, 09, 22));
      testStudent2.Save();

      //Act
      testCourse.AddStudent(testStudent1);
      testCourse.AddStudent(testStudent2);

      List<Student> savedStudents = testCourse.GetStudents();
      List<Student> testList = new List<Student> {testStudent1, testStudent2};

      //Assert
      CollectionAssert.AreEqual(testList, savedStudents);
    }

    [TestMethod]
    public void Delete_DeletesCourseFromCoursesAndJointTable_CourseList()
    {
      //Arrange
      Student testStudent = new Student("Kaili", new DateTime(2017, 09, 22));
      testStudent.Save();

      Course testCourse = new Course("History");
      testCourse.Save();

      //Act
      testCourse.AddStudent(testStudent);
      testCourse.Delete();

      List<Course> resultStudentCourses = testStudent.GetCourses();
      List<Course> testStudentCourses = new List<Course> {};

      //Assert
      CollectionAssert.AreEqual(testStudentCourses, resultStudentCourses);
    }

  }
}
