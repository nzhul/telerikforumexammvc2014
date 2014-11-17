using ForumSystem.Data.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumSystem.Data.Models
{
    public class Vote : AuditInfo, IDeletableEntity
    {
        [Key]
        public int ID { get; set; }

        public string VotedById { get; set; }

        public virtual ApplicationUser VotedBy { get; set; }

        public int PostId { get; set; }

        public virtual Post Post { get; set; }

        public bool IsPositive { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
