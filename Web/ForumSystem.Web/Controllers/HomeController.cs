namespace ForumSystem.Web.Controllers
{
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using ForumSystem.Data.Common.Repository;
    using ForumSystem.Data.Models;
    using ForumSystem.Web.ViewModels.Home;
    using System.Linq;
    using Microsoft.AspNet.Identity;

    public class HomeController : Controller
    {
        private readonly IDeletableEntityRepository<Post> posts;

        public HomeController(IDeletableEntityRepository<Post> posts)
        {
            this.posts = posts;
        }

        public ActionResult Index()
        {
            //var model = this.posts.All().Project().To<IndexBlogPostViewModel>();
            var viewModel = this.posts.All()
                .OrderByDescending(x => x.CreatedOn)
                .Select(x => new IndexBlogPostViewModel
                {
                    Title = x.Title,
                    Tags = x.Tags,
                    Id = x.Id,
                    NegativeVotes = x.NegativeVotes,
                    PositiveVotes = x.PositiveVotes,
                    CanVoteNegatively = true,
                    CanVotePositively = true
                }).ToList();

            var userId = this.User.Identity.GetUserId();
            for (int i = 0; i < viewModel.Count(); i++)
            {
                for (int j = 0; j < viewModel[i].PositiveVotes.Count(); j++)
                {
                    foreach (var vote in viewModel[i].PositiveVotes)
                    {
                        if (vote.VotedById == userId)
                        {
                            viewModel[i].CanVotePositively = false;
                        }
                    }
                }
            }

            for (int i = 0; i < viewModel.Count(); i++)
            {
                for (int j = 0; j < viewModel[i].NegativeVotes.Count(); j++)
                {
                    foreach (var vote in viewModel[i].NegativeVotes)
                    {
                        if (vote.VotedById == userId)
                        {
                            viewModel[i].CanVoteNegatively = false;
                        }
                    }
                }
            }

            return this.View(viewModel);
        }

        public ActionResult VotePositive(int id)
        {
            var userId = this.User.Identity.GetUserId();
            var thePost = this.posts.GetById(id);
            foreach (var vote in thePost.PositiveVotes)
            {
                if (vote.VotedById == userId)
                {
                    var positive = thePost.PositiveVotes.Count;
                    var negative = thePost.NegativeVotes.Count;
                    var result = positive - negative;
                    TempData["cannotvote"] = "You cannot vote again";
                    return Content(result.ToString());
                }
            }
            
            var newPositiveVote = new Vote()
            {
                IsPositive = true,
                VotedById = userId,
                Post = thePost
            };
            thePost.PositiveVotes.Add(newPositiveVote);
            this.posts.SaveChanges();

            var postVotesAfterVote = this.posts.GetById(id);
            var positiveVotes = postVotesAfterVote.PositiveVotes.Count;
            var negativeVotes = postVotesAfterVote.NegativeVotes.Count;
            var votingResult = positiveVotes - negativeVotes;
            return Content(votingResult.ToString());
        }

        public ActionResult VoteNegative(int id)
        {
            var userId = this.User.Identity.GetUserId();
            var thePost = this.posts.GetById(id);
            foreach (var vote in thePost.NegativeVotes)
            {
                if (vote.VotedById == userId)
                {
                    var positive = thePost.PositiveVotes.Count;
                    var negative = thePost.NegativeVotes.Count;
                    var result = positive - negative;
                    TempData["cannotvote"] = "You cannot vote again";
                    return Content(result.ToString());
                }
            }

            var newNegativeVote = new Vote()
            {
                IsPositive = false,
                VotedById = userId,
                Post = thePost
            };
            thePost.NegativeVotes.Add(newNegativeVote);
            this.posts.SaveChanges();

            var postVotesAfterVote = this.posts.GetById(id);
            var positiveVotes = postVotesAfterVote.PositiveVotes.Count;
            var negativeVotes = postVotesAfterVote.NegativeVotes.Count;
            var votingResult = positiveVotes - negativeVotes;
            return Content(votingResult.ToString());
        }

    }
}