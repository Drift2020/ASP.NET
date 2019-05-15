using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Galery.Models
{
    public class MainModel
    {

   

        [Required]
        public List<Album> six_albums { set; get; }

        [Required]
        public User now_user { set; get; }

    }
}