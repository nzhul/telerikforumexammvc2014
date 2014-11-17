namespace ForumSystem.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using ForumSystem.Data.Common.Models;
    using System.Collections.Generic;

    public class Post : AuditInfo, IDeletableEntity
    {
        private ICollection<Tag> tags;
        private ICollection<Vote> positiveVotes;
        private ICollection<Vote> negativeVotes;
        public Post()
        {
            this.tags = new HashSet<Tag>();
            this.positiveVotes = new HashSet<Vote>();
            this.negativeVotes = new HashSet<Vote>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }

        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }
 
        public string Content { get; set; }


        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<Vote> PositiveVotes
        {
            get
            {
                return this.positiveVotes;
            }
            set
            {
                this.positiveVotes = value;
            }
        }

        public virtual ICollection<Vote> NegativeVotes
        {
            get
            {
                return this.negativeVotes;
            }
            set
            {
                this.negativeVotes = value;
            }
        }

        public virtual ICollection<Tag> Tags
        {
            get
            {
                return this.tags;
            }
            set
            {
                this.tags = value;
            }
        }
    }
}
