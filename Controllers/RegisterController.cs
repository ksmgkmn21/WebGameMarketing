using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebGameMarketing.Models.EntitiyFramwork;

namespace WebGameMarketing.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register

        WebMarketingEntities db = new WebMarketingEntities();
        public ActionResult Register()
        {
            var model = db.Users.ToList();
            return View(model);
        }


        [HttpPost]
        [ActionName("Save")]
        public ActionResult Save(Users user)
        {
            var model = db.Users.ToList();
            if (ModelState.IsValid && user != null)
            {
                 foreach(Users item in model)
                  {
                    if(item.UserName == user.UserName)
                    {
                        ViewBag.Mesaj2 = "This Username already exists";
                        return RedirectToAction("Register");
                    }
                  }
                
                    if (user.ID == 0)
                    {
                        db.Users.Add(user);
                    }
                    else
                    {
                        var temp = db.Users.Find(user.ID);
                        if (temp == null)
                        {
                            return HttpNotFound();
                        }
                        else
                        {
                            db.Entry(user).State = System.Data.Entity.EntityState.Modified; //update
                        }
                    }
                    db.SaveChanges();
                    return RedirectToAction("Home", "Home");
                

            }
            else
            {
                ViewBag.Mesaj2 = "Invalid information";
                return RedirectToAction("Register");
            }
        }
      
    }
}