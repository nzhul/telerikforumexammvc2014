namespace ForumSystem.Web.InputModels.FeedBacks
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class FeedBackInputModel
    {
        [Required]
        [Display(Name = "Title")]
        [MaxLength(20)]
        public string Title { get; set; }

        [Required]
        [AllowHtml]
        [Display(Name = "Content")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
    }
}
