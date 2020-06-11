using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineForum.Models {
    public class Post
    {
        public int    Id   { get; set; }
        public string Text { get; set; }

        [ForeignKey("PostedOnId")] public virtual Board PostedOn { get; set; }
        [ForeignKey("PostedById")] public virtual User  PostedBy { get; set; }
    }
}