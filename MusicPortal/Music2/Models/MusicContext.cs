using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Music2.Models
{
    public class MusicContext:DbContext
    {
        public MusicContext() : base("DefaultConnection")
        { }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Pwd_salt> Pwd_salts { get; set; }
        public DbSet<Sound> Sounds { get; set; }
        public DbSet<User> Users { get; set; }

    }
}