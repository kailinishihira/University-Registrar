using Microsoft.AspNetCore.Mvc;
using UniversityRegistrar.Models;
using System.Collections.Generic;
using System;

namespace UniversityRegistrar.Controllers
{
  public class HomeController : Controller
  {
    [HttpGet("/")]
    public ActionResult Index()
    {
      return View();
    }

    [HttpGet("/courses")]
    public ActionResult Courses()
    {
        List<Course> allCourses = Course.GetAll();
        return View(allCourses);
    }

    [HttpGet("/students")]
    public ActionResult Students()
    {
        List<Student> allStudents = Student.GetAll();
        return View(allStudents);
    }

    [HttpGet("/courses/new")]
    public ActionResult CourseForm()
    {
        return View();
    }
    [HttpPost("/courses/new")]
    public ActionResult CourseCreate()
    {
        Course newCourse = new Course(Request.Form["course-name"]);
        newCourse.Save();
        List<Course> allCourses = Course.GetAll();
        return View("Courses", allCourses);
    }

//NEW CATEGORY
    [HttpGet("/students/new")]
    public ActionResult StudentForm()
    {
        return View();
    }

    [HttpPost("/students/new")]
    public ActionResult StudentCreate()
    {

        Student newStudent = new Student(Request.Form["student-name"], DateTime.Parse(Request.Form["enrollmentDate"]));

        newStudent.Save();
        List<Student> allStudents = Student.GetAll();
        return View("Students", allStudents);
    }

    [HttpGet("/students/{id}")]
    public ActionResult StudentDetail(int id)
    {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Student selectedStudent= Student.FindStudentById(id);
        List<Course> studentCourses = selectedStudent.GetCourses();
        List<Course> allCourses = Course.GetAll();
        model.Add("student", selectedStudent);
        model.Add("studentCourses", studentCourses);
        model.Add("allCourses", allCourses);
        return View( model);

    }

    [HttpGet("/courses/{id}")]
    public ActionResult CourseDetail(int id)
    {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Course selectedCourse = Course.Find(id);
        List<Student> courseStudents = selectedCourse.GetStudents();
        List<Student> allStudents = Student.GetAll();
        model.Add("selectedCourse", selectedCourse);
        model.Add("courseStudents", courseStudents);
        model.Add("allStudents", allStudents);
        return View(model);
    }

//AD CATEGORY TO TASK
    [HttpPost("/students/add_course")]
    public ActionResult StudentAddCourse()
    {
        Student student = Student.FindStudentById(Int32.Parse(Request.Form["student-id"]));

        Course course = Course.Find(Int32.Parse(Request.Form["course-id"]));
        student.AddCourse(course);

        Dictionary<string, object> model = new Dictionary<string, object>();
        Student selectedStudent = Student.FindStudentById(student.GetId());
        List<Course> studentCourses = selectedStudent.GetCourses();
        List<Course> allCourses = Course.GetAll();
        model.Add("student", selectedStudent);
        model.Add("studentCourses", studentCourses);
        model.Add("allCourses", allCourses);
        return View("StudentDetail", model);
    }

//ADD TASK TO CATEGORY
    [HttpPost("courses/add_student")]
    public ActionResult CourseAddStudent()
    {
      Course course = Course.Find(Int32.Parse(Request.Form["course-id"]));
      Student student = Student.FindStudentById(Int32.Parse(Request.Form["student-id"]));

      course.AddStudent(student);

      Dictionary<string, object> model = new Dictionary<string, object>();
      Course selectedCourse = Course.Find(course.GetId());
      List<Student> courseStudents = selectedCourse.GetStudents();
      List<Student> allStudents = Student.GetAll();
      model.Add("selectedCourse", selectedCourse);
      model.Add("courseStudents", courseStudents);
      model.Add("allStudents", allStudents);
      return View("CourseDetail", model);
    }
    [HttpGet("/courses/delete/{id}")]
    public ActionResult DeleteCourse(int id)
    {
      Course course = Course.Find(id);
      course.Delete();

      return RedirectToAction("Courses");
    }

    [HttpGet("/students/delete/{id}")]
    public ActionResult DeleteStudent(int id)
    {
      Student student = Student.FindStudentById(id);
      student.Delete();

      return RedirectToAction("Students");
    }

  }
}
