using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ForumSystem.Web.Areas.Administration.Models
{
    public class PostViewModelKendo
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }
        public string Title { get; set; }

        [AllowHtml]
        public string Content { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime CreatedOn { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}