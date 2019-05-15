using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Music2.Models
{
    public class UserModel
    {

        [Required]
        public List<User> users { get; set; }
    }
}