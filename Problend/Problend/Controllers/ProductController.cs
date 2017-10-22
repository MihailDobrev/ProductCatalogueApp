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
    public class ProductController : Controller
    {
        private ProblendDBContext db = new ProblendDBContext();

        // GET: Product
        [Authorize(Roles = "Admin, Worker")]
        public ActionResult All(string sortOrder, string searchString)
        {

            ViewBag.ClientNameSortParm = String.IsNullOrEmpty(sortOrder) ? "clientName_desc" : "";
            ViewBag.ProductTypeSortParm = sortOrder == "productType" ? "productType_desc" : "productType";
            ViewBag.ProductBrandSortParm = sortOrder == "productBrand" ? "productBrand_desc" : "productBrand";
            ViewBag.RealPackageWeightSortParm = sortOrder == "realPackageWeight" ? "realPackageWeight_desc" : "realPackageWeight";
            ViewBag.ProductNumberSortParm = sortOrder == "productNumber" ? "productNumber_desc" : "productNumber";
            var products = db.Products
                .Include(p => p.CartonBox)
                .Include(p => p.Client);

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Client.Name.Contains(searchString)
                                           || s.ProductType.Contains(searchString)
                                           || s.ProductBrand.Contains(searchString)
                                           || s.ProductNumber.ToString().Contains(searchString)
                                           || s.RealPackageWeight.ToString().Contains(searchString));
            }

            switch (sortOrder)
            {
                case "clientName_desc":
                    products = products.OrderByDescending(s => s.Client.Name);
                    break;
                case "productType":
                    products = products.OrderBy(s => s.ProductType);
                    break;
                case "productType_desc":
                    products = products.OrderByDescending(s => s.ProductType);
                    break;
                case "productBrand":
                    products = products.OrderBy(s => s.ProductBrand);
                    break;
                case "productBrand_desc":
                    products = products.OrderByDescending(s => s.ProductBrand);
                    break;
                case "realPackageWeight":
                    products = products.OrderBy(s => s.RealPackageWeight);
                    break;
                case "realPackageWeight_desc":
                    products = products.OrderByDescending(s => s.RealPackageWeight);
                    break;
                case "productNumber":
                    products = products.OrderBy(s => s.ProductNumber);
                    break;
                case "productNumber_desc":
                    products = products.OrderByDescending(s => s.ProductNumber);
                    break;
                default:
                    products = products.OrderBy(s => s.Client.Name);
                    break;
            }
            
            return View(products.ToList());
        }

        private void PopulateClientsDropDownList(object selectedClient = null)
        {
            var clientsQuery = from c in db.Clients
                               orderby c.Name
                               select c;
            ViewBag.ClientId = new SelectList(clientsQuery, "ClientId", "Name", selectedClient);
        }

        private void PopulateCartonBoxesDropDownList(object selectedCartonBox = null)
        {
            var cartonBoxesQuery = from c in db.CartonBoxes
                                   orderby c.Name
                                   select c;
            ViewBag.CartonBoxId = new SelectList(cartonBoxesQuery, "CartonBoxId", "Name", selectedCartonBox);
        }

        // GET: Product/Details/id
        [Authorize(Roles = "Admin, Worker")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Product/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {

            PopulateClientsDropDownList();
            PopulateCartonBoxesDropDownList();
            return View(new Product());
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "ProductNumber,ProductType,ProductBrand,ClientId,TypeOfUnit,NumberOfUnits,TypeOfPackage,NumberOfItemsInPackage,WeightInputedInPackage,RealPackageWeight,PaperType,PackageSize,CartonBoxID,MeasuredPackagingWeight,NumberOfSticksMeasured,GrossWeightUnit,PiecesOnEuropallet,PalletizationPicturePath,TransportationUnitPicturePath,TransportationUnitLabelPicturePath,UnitLabelPicturePath,PackagePicturePath,DisplayBoxPictureFront,DisplayBoxPictureBack")] Product productInput, IEnumerable<HttpPostedFileBase> images)
        {
            Product product = new Product();

            if (ModelState.IsValid)
            {  
                int fileCounter = 1;

                foreach (var image in images)
                {
                    if (image != null && image.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(image.FileName);
                        var uploadPath = "~/Images/ProductPictures";
                        var path = Path.Combine(Server.MapPath(uploadPath), fileName);
                        image.SaveAs(path);

                        switch (fileCounter)
                        {
                            case 1:
                                product.PalletizationPicturePath = fileName; break;
                            case 2:
                                product.TransportationUnitPicturePath = fileName; break;
                            case 3:
                                product.TransportationUnitLabelPicturePath = fileName; break;
                            case 4:
                                product.UnitLabelPicturePath = fileName; break;
                            case 5:
                                product.PackagePicturePath = fileName; break;
                            case 6:
                                product.DisplayBoxPictureFront = fileName; break;
                            case 7:
                                product.DisplayBoxPictureBack = fileName; break;
                            default:
                                break;
                        }
                        fileCounter++;
                    }
                }

                product.ProductNumber = productInput.ProductNumber;
                product.ProductType = productInput.ProductType;
                product.ProductBrand = productInput.ProductBrand;
                product.ClientId = productInput.ClientId;
                product.TypeOfUnit = productInput.TypeOfUnit;
                product.NumberOfUnits = productInput.NumberOfUnits;
                product.TypeOfPackage = productInput.TypeOfPackage;
                product.NumberOfItemsInPackage = productInput.NumberOfItemsInPackage;
                product.WeightInputedInPackage = productInput.WeightInputedInPackage;
                product.RealPackageWeight = productInput.RealPackageWeight;
                product.PaperType = productInput.PaperType;
                product.PackageSize = productInput.PackageSize;
                product.CartonBoxID = productInput.CartonBoxID;
                product.MeasuredPackagingWeight = productInput.MeasuredPackagingWeight;
                product.NumberOfSticksMeasured = productInput.NumberOfSticksMeasured;
                product.GrossWeightUnit = productInput.GrossWeightUnit;
                product.PiecesOnEuropallet = productInput.PiecesOnEuropallet;
                product.PackagingUsedForUnit = productInput.MeasuredPackagingWeight / productInput.NumberOfSticksMeasured * productInput.NumberOfItemsInPackage / 1000;
                product.PackagingUsedFor1000Pieces = productInput.MeasuredPackagingWeight / productInput.NumberOfSticksMeasured * 1000;
                product.RawMaterialUsedInUnit = productInput.WeightInputedInPackage * productInput.NumberOfItemsInPackage;


                var cartonBox = db.CartonBoxes
                        .Where(x => x.CartonBoxId == productInput.CartonBoxID)
                        .FirstOrDefault();
                var units = productInput.NumberOfUnits;

                if (units == 0)
                {
                    product.GrossWeightOfUnit = product.RawMaterialUsedInUnit + product.PackagingUsedForUnit + cartonBox.Weight;
                    product.GrossWeightCartonBox = product.GrossWeightOfUnit;
                }
                else
                {
                    product.GrossWeightOfUnit = product.RawMaterialUsedInUnit + product.PackagingUsedForUnit + product.GrossWeightUnit;
                    product.GrossWeightCartonBox= product.NumberOfUnits * product.GrossWeightOfUnit + cartonBox.Weight;
                }

                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("All");
            }

            PopulateClientsDropDownList(productInput.ClientId);
            PopulateCartonBoxesDropDownList(productInput.CartonBoxID);
            return View(productInput);
        }

        // GET: Product/Edit/id
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            PopulateClientsDropDownList(product.ClientId);
            PopulateCartonBoxesDropDownList(product.CartonBoxID);
            return View(product);
        }

        // POST: Product/Edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,ProductNumber,ProductType,ProductBrand,ClientId,TypeOfUnit,NumberOfUnits,TypeOfPackage,NumberOfItemsInPackage,WeightInputedInPackage,RealPackageWeight,PaperType,PackageSize,CartonBoxID,MeasuredPackagingWeight,NumberOfSticksMeasured,GrossWeightUnit,PiecesOnEuropallet,PalletizationPicturePath,TransportationUnitPicturePath,TransportationUnitLabelPicturePath,UnitLabelPicturePath,PackagePicturePath,DisplayBoxPictureFront,DisplayBoxPictureBack")] Product productInput, IEnumerable<HttpPostedFileBase> images)
        {
            if (ModelState.IsValid)
            {
                var productFromDB = db.Products.Find(productInput.Id);

                int fileCounter = 1;

                foreach (var image in images)
                {
                    if (image != null && image.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(image.FileName);
                        var uploadPath = "~/Images/ProductPictures";
                        var path = Path.Combine(Server.MapPath(uploadPath), fileName);
                        image.SaveAs(path);

                        switch (fileCounter)
                        {
                            case 1:
                                productFromDB.PalletizationPicturePath = fileName; break;
                            case 2:
                                productFromDB.TransportationUnitPicturePath = fileName; break;
                            case 3:
                                productFromDB.TransportationUnitLabelPicturePath = fileName; break;
                            case 4:
                                productFromDB.UnitLabelPicturePath = fileName; break;
                            case 5:
                                productFromDB.PackagePicturePath = fileName; break;
                            case 6:
                                productFromDB.DisplayBoxPictureFront = fileName; break;
                            case 7:
                                productFromDB.DisplayBoxPictureBack = fileName; break;
                            default:
                                break;
                        }
                        fileCounter++;
                    }
                }

                productFromDB.ProductNumber = productInput.ProductNumber;
                productFromDB.ProductType = productInput.ProductType;
                productFromDB.ProductBrand = productInput.ProductBrand;
                productFromDB.ClientId = productInput.ClientId;
                productFromDB.TypeOfUnit = productInput.TypeOfUnit;
                productFromDB.NumberOfUnits = productInput.NumberOfUnits;
                productFromDB.TypeOfPackage = productInput.TypeOfPackage;
                productFromDB.NumberOfItemsInPackage = productInput.NumberOfItemsInPackage;
                productFromDB.WeightInputedInPackage = productInput.WeightInputedInPackage;
                productFromDB.RealPackageWeight = productInput.RealPackageWeight;
                productFromDB.PaperType = productInput.PaperType;
                productFromDB.PackageSize = productInput.PackageSize;
                productFromDB.CartonBoxID = productInput.CartonBoxID;
                productFromDB.MeasuredPackagingWeight = productInput.MeasuredPackagingWeight;
                productFromDB.NumberOfSticksMeasured = productInput.NumberOfSticksMeasured;
                productFromDB.GrossWeightUnit = productInput.GrossWeightUnit;
                productFromDB.PiecesOnEuropallet = productInput.PiecesOnEuropallet;
                productFromDB.PackagingUsedForUnit = productInput.MeasuredPackagingWeight / productInput.NumberOfSticksMeasured * productInput.NumberOfItemsInPackage / 1000;
                productFromDB.PackagingUsedFor1000Pieces = productInput.MeasuredPackagingWeight / productInput.NumberOfSticksMeasured * 1000;
                productFromDB.RawMaterialUsedInUnit = productInput.WeightInputedInPackage * productInput.NumberOfItemsInPackage;


                var cartonBox = db.CartonBoxes
                        .Where(x => x.CartonBoxId == productInput.CartonBoxID)
                        .FirstOrDefault();
                var units = productInput.NumberOfUnits;

                if (units == 0)
                {
                    productFromDB.GrossWeightOfUnit = productFromDB.RawMaterialUsedInUnit + productFromDB.PackagingUsedForUnit + cartonBox.Weight;
                    productFromDB.GrossWeightCartonBox = productFromDB.GrossWeightOfUnit;
                }
                else
                {
                    productFromDB.GrossWeightOfUnit = productFromDB.RawMaterialUsedInUnit + productFromDB.PackagingUsedForUnit + productFromDB.GrossWeightUnit;
                    productFromDB.GrossWeightCartonBox = productFromDB.NumberOfUnits * productFromDB.GrossWeightOfUnit + cartonBox.Weight;
                }


                db.SaveChanges();
                return RedirectToAction("All");
            }

            PopulateClientsDropDownList(productInput.ClientId);
            PopulateCartonBoxesDropDownList(productInput.CartonBoxID);
            return View(productInput);
        }

        // GET: Product/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)                                                     
            {                                                                   
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);     
            }                                                                   
            Product product = db.Products.Find(id);                             
            if (product == null)                                                
            {                                                                   
                return HttpNotFound();                                          
            }                                                                   
            return View(product);                                                   
        }

        // POST: Product/Delete/id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);                         
            db.Products.Remove(product);                                    
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
