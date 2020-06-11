using System.Collections.Generic;

namespace OnlineForum.Models {
    public class User
    {
        public         int        Id       { get; set; }
        public         string     Name     { get; set; }
        public         string     Password { get; set; }
        public         bool       IsAdmin  { get; set; }
        public virtual List<Post> Posts    { get; set; }
    }
}