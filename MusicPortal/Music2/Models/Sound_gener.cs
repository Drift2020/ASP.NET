namespace Music2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Sound_gener
    {
        public int? Id_sound { get; set; }

        public int? Id_gener { get; set; }

        public int Id { get; set; }

        public virtual Genre Genre { get; set; }

        public virtual Sound Sound { get; set; }
    }
}
