using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using hw_3_30.Models;
using hw_3_30b.data;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace hw_3_30.Controllers
{
    public class CookiesController : Controller
    {
        public IActionResult Index()
        {
            SimpleAdDb db = new SimpleAdDb();
            SessionViewModel vm = new SessionViewModel();
            vm.Posts = db.GetPosts();
            List<string> ids = new List<string>();
            if (Request.Cookies["postIds"] != null)
            {
                ids = Request.Cookies["postIds"].Split(',').ToList();         //setting a string to the result of the cookie
                                                                                 //splitting that string into a list of sep #s
                foreach(string num in ids)                          
                {                                           //parsing the nums and adding them to the list of ints that will go to index.cshtml
                    vm.Ids.Add(int.Parse(num));                        
                }
            }            
            return View(vm);
        }
        public IActionResult NewAd()
        {
            return View();
        }
        [HttpPost]
        public IActionResult NewAd(Post post)
        {
            SimpleAdDb db = new SimpleAdDb();
            db.AddPost(post);
            string postIds = "";                                            
            if (Request.Cookies["postIds"] != null)          
            {
                postIds += Request.Cookies["postIds"];              //getting the previous cookie
                postIds += ",";                                     //adding a comma to the end of it (not to the new one after)
            }
            postIds += $"{post.Id}";                                //adding the newest id to the string
            Response.Cookies.Append("postIds", postIds);            //overriding the cookie with the new string
            return Redirect("/cookies/index");
        }

        [HttpPost]
        public IActionResult DeleteAd(int postId)
        {
            SimpleAdDb db = new SimpleAdDb();
            db.DeleteAd(postId);                                           //i don't know if it's necessary to remove the id from the cookie
            List<string> ids = new List<string>();
            List<int> postIds = new List<int>();
            if (Request.Cookies["postIds"] != null)               //if id is still on the cookie, it shouldn't do any harm
            {
                ids = Request.Cookies["postIds"].Split(',').ToList();
                foreach (string num in ids)
                {
                    postIds.Add(int.Parse(num));                            //filling a new list of ints with the string
                }
                postIds.Remove(postId);                                 //removing the deleted id
                string result = "";
                for(int i = 0; i < postIds.Count; i++)
                {
                    result += $"{postIds[i]}";                             //making a new string with no comma at the end
                    if (i < postIds.Count - 1)
                    {
                        result += ",";
                    }
                }
                Response.Cookies.Append("postIds", result);            //overriding the cookie with the new string
            }
            return Redirect("/cookies/index");
        }
    }
}
