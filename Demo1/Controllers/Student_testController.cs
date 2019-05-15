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
    public class Student_testController : Controller
    {
        private StudyEntities3 db = new StudyEntities3();

        // GET: Student_test
        public ActionResult Index()
        {
            return View(db.Student_test.ToList());
        }

        // GET: Student_test/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student_test student_test = db.Student_test.Find(id);
            if (student_test == null)
            {
                return HttpNotFound();
            }
            return View(student_test);
        }

        // GET: Student_test/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Student_test/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,user_id,question_id,answer")] Student_test student_test)
        {
            if (ModelState.IsValid)
            {
                db.Student_test.Add(student_test);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(student_test);
        }

        // GET: Student_test/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student_test student_test = db.Student_test.Find(id);
            if (student_test == null)
            {
                return HttpNotFound();
            }
            return View(student_test);
        }

        // POST: Student_test/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,user_id,question_id,answer")] Student_test student_test)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student_test).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student_test);
        }

        // GET: Student_test/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student_test student_test = db.Student_test.Find(id);
            if (student_test == null)
            {
                return HttpNotFound();
            }
            return View(student_test);
        }

        // POST: Student_test/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student_test student_test = db.Student_test.Find(id);
            db.Student_test.Remove(student_test);
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
