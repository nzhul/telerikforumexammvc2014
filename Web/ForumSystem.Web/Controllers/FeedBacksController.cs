using ForumSystem.Web.InputModels.FeedBacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ForumSystem.Data.Models;
using ForumSystem.Data.Common.Repository;
using ForumSystem.Web.Infrastructure;
using ForumSystem.Web.ViewModels.FeedBack;

namespace ForumSystem.Web.Controllers
{
    public class FeedBacksController : Controller
    {
        private readonly IDeletableEntityRepository<FeedBack> feedbacks;

        private readonly ISanitizer sanitizer;

        public FeedBacksController(IDeletableEntityRepository<FeedBack> feedbacks, ISanitizer sanitizer)
        {
            this.feedbacks = feedbacks;
            this.sanitizer = sanitizer;
        }

        const int PageSize = 4;

        // GET: FeedBacks
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Index(int? id)
        {

            int pageNumber = id.GetValueOrDefault(1);
            var feedBacks = this.feedbacks.All()
                .OrderByDescending(f => f.CreatedOn)
                .Skip((pageNumber - 1) * PageSize).Take(PageSize)
                .Select(f => new FeedBackListItemViewModel
                {
                    Author = f.Author.UserName,
                    Content = f.Content,
                    Title = f.Title,
                    CreatedOn = f.CreatedOn
                }).ToList();

            for (int i = 0; i < feedBacks.Count; i++)
            {
                if (feedBacks[i].Author == null)
                {
                    feedBacks[i].Author = "--Anonimos--";
                }
                feedBacks[i].Content = this.sanitizer.Sanitize(feedBacks[i].Content);
            }

            ViewBag.Pages = Math.Ceiling((double)this.feedbacks.All().Count() / PageSize);
            return View(feedBacks);
        }

        [HttpPost]
        //[Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FeedBackInputModel input)
        {
            if (ModelState.IsValid)
            {
                var userId = "";
                if (this.User.Identity.IsAuthenticated)
                {
                     userId = this.User.Identity.GetUserId();
                }
                else
                {
                     userId = null;
                }
                

                var post = new FeedBack
                {
                    Title = input.Title,
                    Content = this.sanitizer.Sanitize(input.Content),
                    AuthorId = userId,
                    CreatedOn = DateTime.Now
                };

                this.feedbacks.Add(post);
                this.feedbacks.SaveChanges();
                TempData["successFeedBack"] = "You have submited the feedback successfully!";
                return this.Redirect("/");
            }

            var errorModel = new FeedBackDisplayViewModel
            {
                Title = input.Title,
                Content = input.Content
            };

            return this.View(errorModel);
        }
    }
}