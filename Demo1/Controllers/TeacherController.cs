using Demo1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Demo1.Controllers
{
    public class TeacherController : Controller
    {
        private StudyEntities3 db = new StudyEntities3();
        // GET: Teacher

        [ValidateInput(false)]
        public ActionResult Index(FormCollection fc)
        {
            var content = fc["editor"];
            TempData.Keep("email");
            TempData.Keep("imagePath");

            if (TempData["changeMessage"] != null)
            {
                TempData["message"] = TempData["changeMessage"];
            }
            if (TempData["jump"] != null)
            {
                TempData["jumpLink"] = TempData["jump"];
            }
            if (Session["username"] == null)
            {
                TempData["message"] = "请登录";
                return RedirectToAction("Index", "Home");
            }
            return View("index");
        }

        public ActionResult SignOut()
        {
            Session["username"] = null;
            TempData["email"] = null;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ToAddTeather()
        {
            return View("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,username,password,type,email,phone,imagePath,school,registime")] User user)
        {
            if (ModelState.IsValid)
            {
                db.User.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        public ActionResult StudentGrade()
        {
            var users = db.User.ToList().Where(o => o.type == 0).ToList();
            var studentGrades = db.Student_grade.ToList();
            var testInfos = db.TestInfo.ToList();
            List<StudentGradeModel> studentGradeModels = new List<StudentGradeModel>();
            foreach(var studentGrade in studentGrades)
            {
                User user = users.Where(o => o.id == studentGrade.userid).FirstOrDefault();
                TestInfo testInfo = testInfos.Where(o => o.test_id == studentGrade.test_id).FirstOrDefault();
                if(user != null)
                {
                    studentGradeModels.Add(new StudentGradeModel
                    {
                        user = user,
                        studentGrade = studentGrade,
                        testInfo = testInfo
                    });
                }
            }
            return View(studentGradeModels);
        }

        public ActionResult ToTestAnswer(int test_id, int userid)
        {
            if (test_id <= 0 || userid <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var needQuestion = db.Question.Where(o => o.test_id == test_id).ToList();
            ViewData["QuestionCount"] = needQuestion.Count();
            TestInfo testInfo = db.TestInfo.Find(test_id);
            OnlineTestModel onlineTestModel = new OnlineTestModel
            {
                TestInfo = testInfo,
                Questions = needQuestion
            };
            ViewData["userid"] = userid;

            return View("TestAnswer", onlineTestModel);
        }

        [HttpPost]
        public ActionResult TestAnswer(int test_id, int userid)
        {
            if (test_id <= 0 || userid <= 0)
            {
                return Json(false);
            }
            List<OnlineTestResultModel> result = new List<OnlineTestResultModel>();
            var questions = db.Question.Where(o => o.test_id == test_id).ToList();
            var stuAnswers = db.Student_test.Where(o => o.user_id == userid).ToList();
            var stuGrade = db.Student_grade.Where(o => o.userid == userid && o.test_id == test_id).FirstOrDefault();
            foreach (var question in questions)
            {
                var studentAnswer = stuAnswers.Where(o => o.question_id == question.question_id).FirstOrDefault();
                if (studentAnswer != null)
                {
                    OnlineTestResultModel onlineTestResultModel = new OnlineTestResultModel
                    {
                        Score = stuGrade.grade,
                        UserAnswer = studentAnswer.answer,
                        RealAnswer = question.question_answer,
                        IsTrue = false
                    };
                    if (studentAnswer.answer.Equals(question.question_answer))
                    {
                        onlineTestResultModel.IsTrue = true;
                    }
                    result.Add(onlineTestResultModel);
                }
            }
            return Json(result);
        }
    }
}