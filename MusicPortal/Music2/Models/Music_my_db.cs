namespace Music2.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Music_my_db : DbContext
    {
        public Music_my_db()
            : base("name=Music_my_db")
        {
        }

        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Pwd_salt> Pwd_salt { get; set; }
        public virtual DbSet<Sound> Sounds { get; set; }
        public virtual DbSet<Sound_gener> Sound_gener { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genre>()
                .HasMany(e => e.Sound_gener)
                .WithOptional(e => e.Genre)
                .HasForeignKey(e => e.Id_gener);

            modelBuilder.Entity<Sound>()
                .HasMany(e => e.Sound_gener)
                .WithOptional(e => e.Sound)
                .HasForeignKey(e => e.Id_sound);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Pwd_salt)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.Id_user)
                .WillCascadeOnDelete(false);
        }
    }
}
