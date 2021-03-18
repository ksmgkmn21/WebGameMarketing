using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebGameMarketing.Controllers;
using WebGameMarketing.Models.EntitiyFramwork;

namespace WebGameMarketing.App_Start
{
    
    
    public class StoreController : Controller
    {
        // GET: Market

        WebMarketingEntities db = new WebMarketingEntities();
      

        public ActionResult Store()
        {
            var popular = (from s in db.Games orderby s.Vote select s).ToList();
            var best = (from s in db.Games orderby s.Purchase_count select s).ToList();
            var model = new Tuple<List<Games>, List<Games>>(popular, best);
            return View(model);
        }


        [Authorize]
        public ActionResult StoreCart()
        {
            if (GlobalVariables.ActiveUser != null)
            {
                Users user = GlobalVariables.ActiveUser;
                var model = db.Games
                        .SqlQuery("select g.* from Games as g , Scart as Sc where " + user.ID + "= Sc.Users_id and g.ID = Sc.Game_id")
                        .ToList();
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
                
            
        }


        public ActionResult StoreCatalog(string id)
        {
           var  model = db.Games.ToList();
            List<string> genres = new List<string>{"Sports", "Fighting", "Racing", "Action", "Puzzle", "Simulation", "Role-Playing", "Platform", "Misc", "Shooter" };
            if (id != null && id != ".")
            {
                model = (from s in db.Games where s.Genre  == id select s).ToList();
                if(!genres.Contains(id))
                {
                    model = db.Games.Where(x => x.Name.Contains(id)).ToList();
                }
            }
            
            return View(model);
        }

        [HttpPost]
        [ActionName("Search")]
        public ActionResult Search(String search)
        {
            if (ModelState.IsValid && search != null)
            {

                if (db.Games.Where(x => x.Name.Contains(search)).ToList() == null)
                    return RedirectToAction("StoreCatalog");
                return RedirectToAction("StoreCatalog/" + search, "Store");
            }
            else
            {
                return RedirectToAction("StoreCatalog");
            }
        }


        
        


       
        public ActionResult StoreProduct(int Id)//gameID
        {
            //int id = Convert.ToInt32(Id);
            var game = db.Games.Find(Id);
            var sameGenre = (from s in db.Games where s.Genre == game.Genre select s).ToList();
            var review = (from s in db.Review where s.Game_id == Id select s).ToList();
           
            Belongs_To index = db.Belongs_To.Find(Id);
            
            var model = new Tuple<Games, List<Games>, List<Review>, string>(game, sameGenre, review, db.Publisher.Find(index.Belongs_id).Publisher_Name);

            return View(model);
        }



        public ActionResult RemoveCart(int Id)
        {
            var temp = db.Scart.Find(GlobalVariables.ActiveUser.ID, Id);

            if (temp == null)
                return RedirectToAction("StoreCart");
            db.Scart.Remove(temp);
            db.SaveChanges();
            return RedirectToAction("StoreCart");
        }

        public ActionResult AddCart(int Id)
        {
            if(GlobalVariables.ActiveUser == null)
                return RedirectToAction("Login", "User");


            var temp = db.Scart.Find(GlobalVariables.ActiveUser.ID, Id);
            var temp2 = db.Library.Find(GlobalVariables.ActiveUser.ID, Id);
            if (temp == null && temp2 == null)
            {
                Scart s = new Scart
                {
                    Users_id = GlobalVariables.ActiveUser.ID,
                    Game_id = Id
                };
                db.Scart.Add(s);
            }
            db.SaveChanges();
            return RedirectToAction("StoreCart");
        }


        public ActionResult UpdateLibrary()
        {
            var temp = db.Scart.ToList();
            foreach (Scart item in temp)
            {
               
                Library s = new Library
                {
                    Users_id = GlobalVariables.ActiveUser.ID,
                    Game_id = item.Game_id
                };
                db.Scart.Remove(item);
                db.Library.Add(s);
            }
            db.SaveChanges();
            return RedirectToAction("Profile", "User");
        }

        [HttpPost]
        [ActionName("AddComment")]
        public ActionResult AddComment(Review temp)
        {
            if(ModelState.IsValid && temp != null)
            {
                if(db.Review.Find(GlobalVariables.ActiveUser.ID, temp.Game_id) == null)
                {
                    Review s = new Review
                    {
                        Vote = temp.Vote,
                        Comment = temp.Comment,
                        Users_id = GlobalVariables.ActiveUser.ID,
                        Game_id = temp.Game_id
                    };
                    db.Review.Add(s);
                   
                    db.SaveChanges();
                   
                    return RedirectToAction("StoreProduct/" + temp.Game_id, "Store");
                }
                else
                {
                    ViewBag.Mesaj = "You have already reviewed !!";
                    return RedirectToAction("StoreProduct/" + temp.Game_id, "Store");
                }

            }
            else
            {
                ViewBag.Mesaj = "Your Review could not be sumbited !!";
                return RedirectToAction("StoreProduct/" + temp.Game_id, "Store");
            }
        }
    }
}