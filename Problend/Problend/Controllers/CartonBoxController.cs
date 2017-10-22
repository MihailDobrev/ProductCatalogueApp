using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Problend.Models;
using System.IO;

namespace Problend.Controllers
{
    public class CartonBoxController : Controller
    {
        private ProblendDBContext db = new ProblendDBContext();

        // GET: CartonBox
        [Authorize(Roles = "Admin, Worker")]
        public ActionResult All()
        {
            return View(db.CartonBoxes.ToList());
        }

        // GET: CartonBox/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: CartonBox/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,Name,Weight,Lenght,Width,Height")] CartonBox cartonBox)
        {
            if (ModelState.IsValid)
            {
                List<PalletizationPicture> palletizationPictureList = new List<PalletizationPicture>();
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    if (file != null && file.ContentLength > 0)
                    {
                        var filename = Path.GetFileName(file.FileName);
                        PalletizationPicture PalletizationPicture = new PalletizationPicture()
                        {
                            PictureName = filename,
                            Extension = Path.GetExtension(filename),
                            //Guid = Guid.NewGuid()
                        };


                        palletizationPictureList.Add(PalletizationPicture);
                        var path = Path.Combine(Server.MapPath("~/Images/PalletizationPictures"), PalletizationPicture.PictureName);
                        file.SaveAs(path);
                    }
                }
                cartonBox.PalletizationPictures = palletizationPictureList;

                db.CartonBoxes.Add(cartonBox);
                db.SaveChanges();
                return RedirectToAction("All");
            }

            return View(cartonBox);
        }

        // GET: CartonBox/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartonBox cartonBox = db.CartonBoxes.Find(id);
            if (cartonBox == null)
            {
                return HttpNotFound();
            }
            return View(cartonBox);
        }

        // POST: CartonBox/Edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "CartonBoxId,Name,Weight,Lenght,Width,Height")] CartonBox cartonBoxInput)
        {
            if (ModelState.IsValid)
            {
                List<PalletizationPicture> palletizationPictureList = new List<PalletizationPicture>();
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    if (file != null && file.ContentLength > 0)
                    {
                        var filename = Path.GetFileName(file.FileName);
                        PalletizationPicture PalletizationPicture = new PalletizationPicture()
                        {
                            PictureName = filename,
                            Extension = Path.GetExtension(filename),
                        };


                        palletizationPictureList.Add(PalletizationPicture);
                        var path = Path.Combine(Server.MapPath("~/Images/PalletizationPictures"), PalletizationPicture.PictureName);
                        file.SaveAs(path);
                    }
                }

                var cartonBoxFromDB = db.CartonBoxes.Find(cartonBoxInput.CartonBoxId);

                cartonBoxFromDB.PalletizationPictures.Clear();
                cartonBoxFromDB.PalletizationPictures= palletizationPictureList;
                cartonBoxFromDB.Name = cartonBoxInput.Name;
                cartonBoxFromDB.Weight = cartonBoxInput.Weight;
                cartonBoxFromDB.Lenght = cartonBoxInput.Lenght;
                cartonBoxFromDB.Width = cartonBoxInput.Width;
                cartonBoxFromDB.Height = cartonBoxInput.Height;
                
                db.SaveChanges();
                return RedirectToAction("All");
            }
            return View(cartonBoxInput);
        }

        // GET: CartonBox/Delete/id
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartonBox cartonBox = db.CartonBoxes.Find(id);
            if (cartonBox == null)
            {
                return HttpNotFound();
            }
            return View(cartonBox);
        }

        // POST: CartonBox/Delete/id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            CartonBox cartonBox = db.CartonBoxes
                .Include(x => x.PalletizationPictures)
                .FirstOrDefault(x => x.CartonBoxId == id);
                
            db.CartonBoxes.Remove(cartonBox);
            db.SaveChanges();
            return RedirectToAction("All");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
