using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using New_site.Models;
using Microsoft.AspNet.Identity;

namespace New_site.Controllers
{
    public class UserDetailsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: UserDetails
        public ActionResult Index()
        {
            return View(db.Models.ToList());
        }


        // GET: UserDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Model model = db.Models.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // GET: UserDetails/Create
        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();

            var existingModel = db.Models.FirstOrDefault(s => s.UserID == userId);

            if (existingModel == null)
            {
                existingModel = new Model();
            }

            var user = db.Users.FirstOrDefault(s => s.Id == userId);

            if (user != null)
            {
                existingModel.Email = user.Email;
            }

            return View(existingModel);
        }

        // POST: UserDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Tara_forma_scurta,Tara,Localitate,Organizatie,Departament,Domeniu,Email,Private_password")] Model model)
        {

            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();

                var existingModel = db.Models.FirstOrDefault(s => s.UserID == userId);

                if (existingModel == null)
                {
                    model.UserID = userId;
                    db.Models.Add(model);
                }
                else
                {
                    if (existingModel != null)
                    {
                        //update
                        existingModel.Localitate = model.Localitate;
                        existingModel.Tara = model.Tara;
                        existingModel.Tara_forma_scurta = model.Tara_forma_scurta;
                        existingModel.Domeniu = model.Domeniu;
                        existingModel.Departament = model.Departament;
                        existingModel.Organizatie = model.Organizatie;
                        existingModel.Private_password = model.Private_password;
                    }
                }

                db.SaveChanges();

                return RedirectToAction("Create");
            }


            return View(model);
        }

        // GET: UserDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Model model = db.Models.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: UserDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Tara_forma_scurta,Tara,Localitate,Organizatie,Departament,Domeniu,Email,Private_password")] Model model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: UserDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Model model = db.Models.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: UserDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Model model = db.Models.Find(id);
            db.Models.Remove(model);
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
