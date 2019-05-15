namespace Music2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Pwd_salt
    {
        public int Id { get; set; }

        public int Id_user { get; set; }

        [Required]
        public string Salt { get; set; }

        public virtual User User { get; set; }
    }
}
