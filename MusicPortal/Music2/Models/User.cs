namespace Music2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Pwd_salt = new HashSet<Pwd_salt>();
        }

        public int Id { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        [StringLength(50)]
        public string Pwd { get; set; }

        public int Status { get; set; }

        public bool isActive { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pwd_salt> Pwd_salt { get; set; }
    }
}
