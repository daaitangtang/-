using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Demo1.Controllers
{
    public class UEditorController : Controller
    {
        [ValidateInput(false)]
        // GET: UEditor
        public ActionResult Index(FormCollection fc)
        {
            var content = fc["editor"];
            return View();
        }


    }
}