﻿using Demo1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Demo1.Controllers
{
    public class StuController : Controller
    {
        private StudyEntities3 db = new StudyEntities3();
        // GET: Main
        public ActionResult Index()
        {
            TempData.Keep("email");
            TempData.Keep("imagePath");
            if (Session["username"] == null)
            {
                TempData["message"] = "请登录";
                return RedirectToAction("Index", "Home");
            }
            return View("index");
        }

        public ActionResult ToChangePwd()
        {
            return View("ChangePwd");
        }

        public ActionResult ChangePwd(string oldPassword, string newPassword)
        {
            string username = Session["username"].ToString();
            var result = db.User.Where(o => o.username == username && o.password.Equals(oldPassword)).FirstOrDefault();
            if(result == null)
            {
                TempData["message"] = "旧密码不正确";
                return RedirectToAction("Index");
            }
            User user = result;
            user.password = newPassword;
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            TempData["message"] = "修改成功";
            return RedirectToAction("Index");
        }

        public ActionResult TestIndex()
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Index", "Stu");
            }
            var testInfos = db.TestInfo.ToList();
            var studentGrade = db.Student_grade.ToList();
            List<StuTestInfoModel> stuTestInfoModels = new List<StuTestInfoModel>();
            //检查该学生是否做过测试
            foreach(var testInfo in testInfos)
            {
                var result = studentGrade.Where(o => o.test_id == testInfo.test_id && o.userid.Equals(Session["userid"])).FirstOrDefault();
                if(result != null)
                {
                    stuTestInfoModels.Add(new StuTestInfoModel
                    {
                        testInfo = testInfo,
                        IsDone = true
                    });
                }
                else
                {
                    stuTestInfoModels.Add(new StuTestInfoModel
                    {
                        testInfo = testInfo,
                        IsDone = false
                    });
                }
            }

            return View(stuTestInfoModels);
        }

        public ActionResult OnlineTest(int? id)
        {
            if (id == null) 
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var needQuestion = db.Question.Where(o => o.test_id == id).ToList();
            ViewData["QuestionCount"] = needQuestion.Count();
            TestInfo testInfo = db.TestInfo.Find(id);
            OnlineTestModel onlineTestModel = new OnlineTestModel
            {
                TestInfo = testInfo,
                Questions = needQuestion
            };
            return View(onlineTestModel);
        }

        [HttpPost]
        public JsonResult OnlineTest(int test_id, string[] Answer, int userid)
        {
            var questions = db.Question.Where(o => o.test_id == test_id).ToList();
            var student_answer = Answer.ToList();
            if (questions.Count() == student_answer.Count())
            {
                double itemScore = 100F / questions.Count();
                double sumScore = 0;

                List<OnlineTestResultModel> result = new List<OnlineTestResultModel>();
                for (int i = 0; i < student_answer.Count(); i++)
                {
                    //将学生考试信息添加到数据库
                    Student_test student_test = new Student_test
                    {
                        user_id = userid,
                        question_id = questions[i].question_id,
                        answer = student_answer[i]
                    };
                    db.Student_test.Add(student_test);

                    //返回给前端考试信息
                    OnlineTestResultModel onlineTestResultModel = new OnlineTestResultModel
                    {
                        UserAnswer = student_answer[i],
                        RealAnswer = questions[i].question_answer,
                        IsTrue = false
                    };

                    //统计分数
                    if (questions[i].question_answer.Equals(student_answer[i]))
                    {
                        sumScore += itemScore;
                        onlineTestResultModel.IsTrue = true;
                    }
                    result.Add(onlineTestResultModel);
                }
                Student_grade student_Grade = new Student_grade
                {
                    userid = userid,
                    test_id = test_id,
                    grade = sumScore
                };
                db.Student_grade.Add(student_Grade);
                db.SaveChanges();

                return Json(result);
            }
            return Json(false);
        }
    }
}