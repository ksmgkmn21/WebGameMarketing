using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebGameMarketing.Models.EntitiyFramwork;

namespace WebGameMarketing.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        WebMarketingEntities db = new WebMarketingEntities();
        public ActionResult Home()
        {

            var popular = (from s in db.Games orderby s.Vote select s).ToList();
            var best = (from s in db.Games orderby s.Purchase_count select s).ToList();
            var newlyGames = (from s in db.Games orderby s.Release_date descending select s).ToList();
            
           
            var model = new Tuple<List<Games>, List<Games>, List<Games>>(popular, best, newlyGames);
            return View(model);
        }
    }
}