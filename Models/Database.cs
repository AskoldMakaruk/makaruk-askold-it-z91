using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace OnlineForum.Models
{
    public class Database : DbContext
    {
        protected DbSet<User>  Users  { get; set; }
        protected DbSet<Post>  Posts  { get; set; }
        protected DbSet<Board> Boards { get; set; }

        public Database(DbContextOptions<Database> options)
        : base(options)
        {
            Database.EnsureCreated();
        }

        public User AuthUser(string username, string password)
        {
            var user = Users.FirstOrDefault(a => a.Name == username);
            if (user == null)
                return null; //user with this name doesn't exists
            return BCrypt.Net.BCrypt.Verify(password, user.Password) ? user : null;
        }

        public User RegisterUser(string username, string password)
        {
            var user = Users.FirstOrDefault(a => a.Name == username);
            if (user != null)
                return null; //user with this name alredy exists

            user = new User
            {
                Name     = username,
                Password = BCrypt.Net.BCrypt.HashPassword(password)
            };
            Users.Add(user);
            SaveChanges();
            return user;
        }

        public User FindUser(int id) => Users.Find(id);

        public void AddPost(int userId, int boardId, string text)
        {
            var user  = Users.Find(userId);
            var board = Boards.Find(boardId);

            board.Posts.Add(new Post()
            {
                PostedBy = user,
                PostedOn = board,
                Text     = text
            });
            SaveChanges();
        }

        public Board AddBoard(User op, string theme, string text)
        {
            var board = new Board()
            {
                Posts   = new List<Post>(),
                Text    = text,
                Creator = op,
                Theme   = theme
            };
            Boards.Add(board);
            SaveChanges();
            return board;
        }

        public List<Board> GetBoards()
        {
            return Boards.ToList();
        }

        public Board GetBoard(int id)
        {
            return Boards.Include(a => a.Creator).Include(a => a.Posts).FirstOrDefault(a => a.Id == id);
        }
    }
}