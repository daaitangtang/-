using Demo1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Demo1.Controllers
{
    public class HomeController : Controller
    {
        private StudyEntities3 db = new StudyEntities3();

        public ActionResult Index()
        {
            return View("Login");
        }

        [HttpPost]
        public ActionResult Login(string username,string password)
        {
            User user = db.User.Where(o => o.username == username && o.password == password).FirstOrDefault();
            if (user != null)
            {
                Session["username"] = username;
                Session["userid"] = user.id;
                TempData["email"] = user.email;
                TempData["imagePath"] = "/../Resource/img/" + user.imagePath;
                if (user.type == 0)
                {
                    return RedirectToAction("Index", "Stu");
                }
                if(user.type == 1)
                {
                    return RedirectToAction("Index", "Teacher");
                }
                else
                {
                    return RedirectToAction("Index", "Admin");
                }
                
            }
            TempData["message"] = "账号密码错误";
            return RedirectToAction("Index");
        }

        public ActionResult ToRegister()
        {
            return View("Register");
        }

        [HttpPost]
        public ActionResult Register([Bind(Include = "username,password,type,email,phone,imagePath,school,registime")] User user)
        {
            if (ModelState.IsValid)
            {
                var res = db.User.ToList();
                var result = db.User.Where(o => o.username == user.username).FirstOrDefault();
                if(result != null)
                {
                    TempData["message"] = "用户:" + user.username + "已存在";
                    return View();
                }
                user.registime = DateTime.Now;
                user.type = 0;
                db.User.Add(user);
                try
                {
                    db.SaveChanges();
                }
                catch(Exception e)
                {

                }
                return RedirectToAction("Index","Home");
            }

            return View();
        }

        public ActionResult SignOut()
        {
            Session["username"] = null;
            Session["userid"] = null;
            TempData["email"] = null;
            return RedirectToAction("Index", "Home");
        }

    }
}