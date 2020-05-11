using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using hw_3_30.Models;
using hw_3_30b.data;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace hw_3_30.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            SimpleAdDb db = new SimpleAdDb();
            SessionViewModel vm = new SessionViewModel();
            vm.Posts = db.GetPosts();
            if (HttpContext.Session.Get<List<int>>("adIds") != null)
            {
                vm.Ids = HttpContext.Session.Get<List<int>>("adIds");
            }
            return View(vm);
        }
        public IActionResult NewAd()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewAd(Post post)
        {                                                    //post came with a name, a phone number, and text
            SimpleAdDb db = new SimpleAdDb();
            db.AddPost(post);                                //now my post has the id, too
            List<int> adIds = HttpContext.Session.Get<List<int>>("adIds");
            if (adIds == null)
            {
                adIds = new List<int>();
            }
            adIds.Add(post.Id);
            HttpContext.Session.Set("adIds", adIds);
            return Redirect("/home/index");
        }

        [HttpPost]
        public IActionResult DeleteAd(int postId)
        {
            SimpleAdDb db = new SimpleAdDb();
            db.DeleteAd(postId);
            if (HttpContext.Session.Get<List<int>>("adIds") != null)
            {
                HttpContext.Session.Get<List<int>>("adIds").Remove(postId);
            }
            return Redirect("/home/index");
        }
    }


    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            string value = session.GetString(key);

            return value == null ? default(T) :
                JsonConvert.DeserializeObject<T>(value);
        }
    }
}

