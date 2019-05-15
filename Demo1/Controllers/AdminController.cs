using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Demo1.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
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
            
            return View();
        }

        public ActionResult SignOut()
        {
            Session["username"] = null;
            TempData["email"] = null;
            return RedirectToAction("Index", "Home");
        }
    }
}