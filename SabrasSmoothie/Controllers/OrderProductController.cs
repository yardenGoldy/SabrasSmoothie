using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SabrasSmoothie.Models;

namespace SabrasSmoothie.Controllers
{
    public class OrderProductController : Controller
    {
        private OrderProductDbContext db = new OrderProductDbContext();
        private ProductDbContext ProductDB = new ProductDbContext();


        // GET: OrderProduct
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult InitCart()
        {
            return RedirectToAction("Index");
        }

        // GET: OrderProduct/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderProduct orderProduct = db.OrderProducts.Find(id);
            if (orderProduct == null)
            {
                return HttpNotFound();
            }
            return View(orderProduct);
        }

        // GET: OrderProduct/Create
        public ActionResult Create()
        {
            if (Session["Cart"] == null)
            {
                Session["Cart"] = new List<Product>();
            }

            IList<Product> CartProducts = (Session["Cart"] as IList<Product>).Distinct().ToList();
            ViewBag.CartProducts = CartProducts;
            return View();
        }

        // POST: OrderProduct/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderId,ProductId,Quantity")] OrderProduct orderProduct)
        {
            if (ModelState.IsValid)
            {
                db.OrderProducts.Add(orderProduct);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(orderProduct);
        }

        [HttpPost]
        public ActionResult SendParams(List<string> quantity)
        {
            db.Orders.Add(new Order()
            {
                CreationDate = DateTime.Now,
                CustomerId = int.Parse(Session["UserId"].ToString())
            });

            var sessionAsType = (Session["Cart"] as IList<Product>);
            for (int i = 0; i < quantity.Count; i++)
            {
                db.OrderProducts.Add(new OrderProduct()
                {
                    ProductId = sessionAsType[i].Id,
                    Quantity = int.Parse(quantity[i].ToString())
                });
            }
            
            db.SaveChanges();
            Session["Cart"] = new List<Product>();
            return RedirectToAction("Index", "Home");
        }

        // GET: OrderProduct/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderProduct orderProduct = db.OrderProducts.Find(id);
            if (orderProduct == null)
            {
                return HttpNotFound();
            }
            return View(orderProduct);
        }

        // POST: OrderProduct/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderId,ProductId,Quantity")] OrderProduct orderProduct)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderProduct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(orderProduct);
        }

        // GET: OrderProduct/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderProduct orderProduct = db.OrderProducts.Find(id);
            if (orderProduct == null)
            {
                return HttpNotFound();
            }
            return View(orderProduct);
        }

        // POST: OrderProduct/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderProduct orderProduct = db.OrderProducts.Find(id);
            db.OrderProducts.Remove(orderProduct);
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
