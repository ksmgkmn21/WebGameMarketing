using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebGameMarketing.Models;
using WebGameMarketing.Models.EntitiyFramwork;

namespace WebGameMarketing.Controllers
{

    public static class GlobalVariables
    {
        public static Users ActiveUser { get; set; }
    }

    [Authorize]
    public class UserController : Controller
    {
        // GET: User
        WebMarketingEntities db = new WebMarketingEntities();


        
        public ActionResult Profile()
        {
            
            if (GlobalVariables.ActiveUser != null)
            {
                Users user = GlobalVariables.ActiveUser;


                var ActiveUserGames = db.Games
                        .SqlQuery("select g.* from Games as g , Library as L where " +  user.ID + "= L.Users_id and g.ID = L.Game_id")
                        .ToList();
              
                var games = (from s in db.Games orderby s.Release_date descending select s).ToList();
                foreach (Games item in ActiveUserGames)
                    games.Remove(item);
                var model = new Tuple<List<Games>, List<Games>, Users>(games, ActiveUserGames, user);
                return View(model);

            }
            else
            {
                return RedirectToAction("Login");
            }

        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            var model = db.Users.ToList();
            return View(model);
        }


        [HttpPost]
        [ActionName("Login")]
        [AllowAnonymous]
        public ActionResult Login(Users user)
        {
            
            var temp = db.Users.FirstOrDefault(x => x.UserName == user.UserName && x.Password == user.Password);
            if (temp != null)
            {
                FormsAuthentication.SetAuthCookie(temp.UserName, false);
                GlobalVariables.ActiveUser = temp;
               
                return RedirectToAction("Home", "Home");
            }
            else
            {
                ViewBag.Mesaj = "Invalid Username or Password";
                return View();
            }
           
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            GlobalVariables.ActiveUser = null;
            return RedirectToAction("Login");
        }

    }
}