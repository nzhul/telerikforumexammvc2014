using ForumSystem.Data.Common.Repository;
using ForumSystem.Data.Models;
using ForumSystem.Web.Areas.Administration.Models;
using ForumSystem.Web.Infrastructure;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace ForumSystem.Web.Areas.Administration.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDeletableEntityRepository<Post> posts;

        private readonly ISanitizer sanitizer;

        public HomeController(IDeletableEntityRepository<Post> posts, ISanitizer sanitizer)
        {
            this.posts = posts;
            this.sanitizer = sanitizer;
        }


        // GET: Administration/Home
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult ReadAllPosts([DataSourceRequest] DataSourceRequest request)
        {
            var result = this.posts.All().Select(x => new PostViewModelKendo
            {
                Id = x.Id,
                AuthorName = x.Author.UserName,
                Content = x.Content,
                CreatedOn = x.CreatedOn,
                IsDeleted = x.IsDeleted,
                ModifiedOn = x.ModifiedOn,
                Title = x.Title

            });

            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdatePost([DataSourceRequest] DataSourceRequest request, PostViewModelKendo post)
        {
            var postDb = this.posts.GetById(post.Id);

            postDb.Content = this.sanitizer.Sanitize(post.Content);
            postDb.Title = post.Title;
            postDb.ModifiedOn = DateTime.Now;
            this.posts.SaveChanges();

            post.ModifiedOn = postDb.ModifiedOn;

            return Json(new[] { post }.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DestroyPost([DataSourceRequest] DataSourceRequest request, PostViewModelKendo post)
        {
            this.posts.Delete(post.Id);
            this.posts.SaveChanges();
            return Json(new[] { post }.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreatePost([DataSourceRequest] DataSourceRequest request, PostViewModelKendo post)
        {
            var userId = this.User.Identity.GetUserId();
            var newPost = new Post
            {
                AuthorId = userId,
                Content = this.sanitizer.Sanitize(post.Content),
                CreatedOn = DateTime.Now,
                Title = post.Title
            };
            this.posts.Add(newPost);
            this.posts.SaveChanges();

            post.AuthorName = this.User.Identity.GetUserName();

            return Json(new[] { post }.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }


    }
}