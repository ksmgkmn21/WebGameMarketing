using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebGameMarketing.Models.EntitiyFramwork;

namespace WebGameMarketing.Controllers
{
    public class GalleryController : Controller
    {
        
        // GET: Gallery
        WebMarketingEntities db = new WebMarketingEntities();
        public ActionResult Gallery()
        {

            
            var urls = db.Img_Path.ToList();
            List<Img_Path> rndGames = new List<Img_Path>();

            Random random = new Random();
            for (int i=0; i <4 ; i++)
            {
                int num = random.Next(urls.Count);
                if (!rndGames.Contains(urls[num]))
                {
                    rndGames.Add(urls[num]);
                }
                else
                    i--;
            }
            var newlyGames = (from s in db.Games orderby s.Release_date descending select s).ToList();
            var model = new Tuple<List<Img_Path>, List<Games>>(rndGames, newlyGames);

            return View(model);
        }
    }
}