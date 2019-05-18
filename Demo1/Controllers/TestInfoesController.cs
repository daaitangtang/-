using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Demo1.Models;

namespace Demo1.Controllers
{
    public class TestInfoesController : Controller
    {
        private StudyEntities3 db = new StudyEntities3();

        // GET: TestInfoes
        public ActionResult Index()
        {
            return View(db.TestInfo.ToList());
        }

        // GET: TestInfoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TestInfo testInfo = db.TestInfo.Find(id);
            if (testInfo == null)
            {
                return HttpNotFound();
            }
            return View(testInfo);
        }

        // GET: TestInfoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TestInfoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "test_id,test_name,test_time,test_info")] TestInfo testInfo)
        {
            if (ModelState.IsValid)
            {
                db.TestInfo.Add(testInfo);
                db.SaveChanges();
                TempData["changeMessage"] = "新建试题成功";
                TempData["jump"] = "/TestInfoes/Index";
                return RedirectToAction("Index", "Teacher");
            }
            TempData["changeMessage"] = "新建试题失败";
            TempData["jump"] = "/TestInfoes/Create";
            return RedirectToAction("Index", "Teacher");
        }

        // GET: TestInfoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TestInfo testInfo = db.TestInfo.Find(id);
            if (testInfo == null)
            {
                return HttpNotFound();
            }
            return View(testInfo);
        }

        // POST: TestInfoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "test_id,test_name,test_time,test_info")] TestInfo testInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(testInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Teacher");
            }
            return RedirectToAction("Index", "Teacher");
        }

        // GET: TestInfoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TestInfo testInfo = db.TestInfo.Find(id);
            if (testInfo == null)
            {
                return HttpNotFound();
            }
            db.TestInfo.Remove(testInfo);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // POST: TestInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TestInfo testInfo = db.TestInfo.Find(id);
            db.TestInfo.Remove(testInfo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
