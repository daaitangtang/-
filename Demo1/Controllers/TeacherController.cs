using Demo1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

    }
}