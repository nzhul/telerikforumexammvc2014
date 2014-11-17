namespace ForumSystem.Web.ViewModels.Home
{
    using ForumSystem.Data.Models;
    using ForumSystem.Web.Infrastructure.Mapping;
using System.Collections.Generic;

    public class IndexBlogPostViewModel : IMapFrom<Post>
    {
        private IEnumerable<Tag> tags;
        private IEnumerable<Vote> positiveVotes;
        private IEnumerable<Vote> negativeVotes;
        public IndexBlogPostViewModel()
        {
            this.tags = new HashSet<Tag>();
            this.positiveVotes = new HashSet<Vote>();
            this.negativeVotes = new HashSet<Vote>();
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public bool CanVotePositively { get; set; }
        public bool CanVoteNegatively { get; set; }

        public virtual IEnumerable<Vote> PositiveVotes
        {
            get { return this.positiveVotes; }
            set { this.positiveVotes = value; }
        }
        public virtual IEnumerable<Vote> NegativeVotes
        {
            get { return this.negativeVotes; }
            set { this.negativeVotes = value; }
        }
        public virtual IEnumerable<Tag> Tags
        {
            get { return this.tags; }
            set { this.tags = value; }
        }
    }
    //force commit
}