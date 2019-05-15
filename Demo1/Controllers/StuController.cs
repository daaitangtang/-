using Demo1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
            return View(db.TestInfo.ToList());
        }
    }
}