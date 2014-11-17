namespace ForumSystem.Data.Migrations
{
    using ForumSystem.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        Random rand = new Random();
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;

        }

        protected override void Seed(ApplicationDbContext context)
        {
            this.AddInitialPostsWithTags(context);
            this.AddInitialFeedBacks(context);
        }

        private void AddInitialFeedBacks(ApplicationDbContext context)
        {
            if (!context.FeedBacks.Any())
            {
                ApplicationUser user = new ApplicationUser() { UserName = "TolkovaSiMojete", Email = "TolkovaSiMojete@test.com" };
                for (int i = 0; i < 10; i++)
                {
                    var feedBack = new FeedBack()
                    {
                        Author = user,
                        Content = "FeedBack content " + i,
                        CreatedOn = DateTime.Now,
                        Title = "FeedBack Title " + i
                    };

                    context.FeedBacks.Add(feedBack);
                }
                context.SaveChanges();
            }
        }

        private void AddInitialPostsWithTags(ApplicationDbContext context)
        {
            if (!context.Posts.Any())
            {
                ApplicationUser user = new ApplicationUser() { UserName = "TestUser", Email = "TestMail@test.com" };
                for (int i = 1; i < 11; i++)
                {
                    var post = new Post();
                    post.Author = user;
                    post.Content = "Simple content of the post " + i;
                    post.CreatedOn = DateTime.Now;
                    post.Title = "Title of the post " + i;

                    var tagsCount = rand.Next(3, 6);
                    for (int j = 0; j < tagsCount; j++)
                    {
                        var newTag = new Tag();
                        newTag.Name = "tag" + j;
                        newTag.CreatedOn = DateTime.Now;
                        newTag.Posts.Add(post);
                        post.Tags.Add(newTag);
                    }

                    var positiveVotesCount = rand.Next(3, 30);
                    for (int j = 0; j < positiveVotesCount; j++)
                    {
                        var newPositiveVote = new Vote();

                        newPositiveVote.Post = post;
                        newPositiveVote.VotedBy = user;
                        newPositiveVote.IsPositive = true;
                        post.PositiveVotes.Add(newPositiveVote);
                    }

                    var negativeVotesCount = rand.Next(3, 10);
                    for (int j = 0; j < negativeVotesCount; j++)
                    {
                        var newNegativeVote = new Vote();

                        newNegativeVote.Post = post;
                        newNegativeVote.VotedBy = user;
                        newNegativeVote.IsPositive = false;
                        post.NegativeVotes.Add(newNegativeVote);
                    }

                    context.Posts.Add(post);
                }


                context.SaveChanges();
            }
        }
    }
}
