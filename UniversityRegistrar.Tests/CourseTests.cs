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
      // Task.DeleteAll();
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
  //
  //   [TestMethod]
  //   public void Test_AddTask_AddsTaskToCategory()
  //   {
  //     //Arrange
  //     Category testCategory = new Category("Household chores");
  //     testCategory.Save();
  //
  //     Task testTask = new Task("Mow the lawn");
  //     testTask.Save();
  //
  //     Task testTask2 = new Task("Water the garden");
  //     testTask2.Save();
  //
  //     //Act
  //     testCategory.AddTask(testTask);
  //     testCategory.AddTask(testTask2);
  //
  //     List<Task> result = testCategory.GetTasks();
  //     List<Task> testList = new List<Task>{testTask, testTask2};
  //
  //     //Assert
  //     CollectionAssert.AreEqual(testList, result);
  //   }
  //
  //   [TestMethod]
  //   public void GetTasks_ReturnsAllCategoryTasks_TaskList()
  //   {
  //     //Arrange
  //     Category testCategory = new Category("Household chores");
  //     testCategory.Save();
  //
  //     Task testTask1 = new Task("Mow the lawn");
  //     testTask1.Save();
  //
  //     Task testTask2 = new Task("Buy plane ticket");
  //     testTask2.Save();
  //
  //     //Act
  //     testCategory.AddTask(testTask1);
  //     List<Task> savedTasks = testCategory.GetTasks();
  //     List<Task> testList = new List<Task> {testTask1};
  //
  //     //Assert
  //     CollectionAssert.AreEqual(testList, savedTasks);
  //   }
  //
  //   [TestMethod]
  //   public void Delete_DeletesCategoryAssociationsFromDatabase_CategoryList()
  //   {
  //     //Arrange
  //     Task testTask = new Task("Mow the lawn");
  //     testTask.Save();
  //
  //     string testName = "Home stuff";
  //     Category testCategory = new Category(testName);
  //     testCategory.Save();
  //
  //     //Act
  //     testCategory.AddTask(testTask);
  //     testCategory.Delete();
  //
  //     List<Category> resultTaskCategories = testTask.GetCategories();
  //     List<Category> testTaskCategories = new List<Category> {};
  //
  //     //Assert
  //     CollectionAssert.AreEqual(testTaskCategories, resultTaskCategories);
  //   }
  //
  }
}
