using Lab21.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lab21.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            CoffeeShopDBEntities1 ORM = new CoffeeShopDBEntities1();

            ViewBag.ItemList = ORM.Items.ToList();
            ViewBag.Message = "GC Coffee Products";

            return View();
        } 

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Admin()
        {
            CoffeeShopDBEntities1 ORM = new CoffeeShopDBEntities1();

            ViewBag.ItemList = ORM.Items.ToList();
            ViewBag.Message = "Administrator Page";

            return View();
        }
        public ActionResult Register()
        {
            
            return View();
        }
        public ActionResult AddNewUser(User newuser)
        {
            CoffeeShopDBEntities1 ORM = new CoffeeShopDBEntities1();

            //Validation
            if (ModelState.IsValid)
                {
              
                ORM.Users.Add(newuser);
                ORM.Entry(newuser).CurrentValues["SignUpDate"] = DateTime.Now;
                ORM.SaveChanges();
                ViewBag.Message = $"Thank you for registering {newuser.FirstName}";
                return View("confirm");
                
                }
            else
                {
                ViewBag.Address = Request.UserHostAddress;
                return View("Error");
                
                }
        }
     
        public ActionResult ShowItemDetails(string ItemName)
        {
            //1 ORM
            CoffeeShopDBEntities1 ORM = new CoffeeShopDBEntities1();

            //2 Locate an item
            Item Found = ORM.Items.Find(ItemName);
            //3 Show Item
            if (Found != null)
            {
                return View(Found);//return view as model
                //or
                //Viewbag.Found = Found;
                //return View;
            }
            else
            {
                ViewBag.ErrorMessage = "Item not Found";
                return View("Error");
            }
        }
        public ActionResult SearchByItemName(string itemname)
        {
            //1 create the ORM
            CoffeeShopDBEntities1 ORM = new CoffeeShopDBEntities1();

            ViewBag.ItemList = ORM.Items.Where(c => c.ItemName.Contains(itemname)).ToList();

            return View("SearchByItemName");
        }

        public ActionResult EditItem(string ItemName)
        {
            CoffeeShopDBEntities1 ORM = new CoffeeShopDBEntities1();

            Item toUpdate = ORM.Items.Find(ItemName);

            return View(toUpdate);
        }

        public ActionResult SaveUpdatedItem(Item updatedItem)
        {
            //1 ORM
            CoffeeShopDBEntities1 ORM = new CoffeeShopDBEntities1();
            //2 Locate customer to update
            Item OldItemRecord = ORM.Items.Find(updatedItem.ItemName);
            if (OldItemRecord != null && ModelState.IsValid)
            {
            //3 Update the Customer
                OldItemRecord.ItemName = updatedItem.ItemName;
                OldItemRecord.ItemDescription = updatedItem.ItemDescription;
                OldItemRecord.ItemQuantity = updatedItem.ItemQuantity;
                OldItemRecord.ItemPrice = updatedItem.ItemPrice;
            //4 This line tells ORM to update
                ORM.Entry(OldItemRecord).State = System.Data.Entity.EntityState.Modified;
            //5 Save to database
                ORM.SaveChanges();
                return RedirectToAction("Admin");
            }
            else
            {
                ViewBag.ErrorMessage = "Item Not Updated";
                return View("Error");
            }
        }
        public ActionResult DeleteItem(string ItemName)
        {
            //1 ORM
            CoffeeShopDBEntities1 ORM = new CoffeeShopDBEntities1();

            //2 Locate customer to delete
            Item Found = ORM.Items.Find(ItemName);
            //3 Remove customer
            if (Found != null)
            {
            //4Delete Orders
                ORM.Items.Remove(Found);
            //5 Save to database
                ORM.SaveChanges();
                return RedirectToAction("Admin");
            }
            else
            {
                ViewBag.ErrorMessage = "Customer Not Found";
                return View("Error");
            }
        }
        public ActionResult AddItem()
        {
            return View();
        }
        public ActionResult AddNewItem(Item n)
        {
            //1 create the ORM
            CoffeeShopDBEntities1 ORM = new CoffeeShopDBEntities1();
            //2 Validation
            if (ModelState.IsValid)
            {
                //3 add the new object to the Item list
                ORM.Items.Add(n);
                //4 save changes to database
                ORM.SaveChanges();
                ViewBag.Message = "Item Added Succesfully";
                ViewBag.ItemList = ORM.Items.ToList();
                return View("Admin");
            }
            //return RedirectToAction("About");
            else
            {
                ViewBag.ErrorMessage = "Item Not Added";
                return View("Error");
            }

        }
    }
}