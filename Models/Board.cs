using System.Collections.Generic;

namespace OnlineForum.Models {
    public class Board
    {
        public         int        Id      { get; set; }
        public         string     Theme   { get; set; }
        public         string     Text    { get; set; }
        public virtual User       Creator { get; set; }
        public virtual List<Post> Posts   { get; set; }
    }
}