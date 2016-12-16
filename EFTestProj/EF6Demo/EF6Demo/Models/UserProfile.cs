using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF6Demo.Models
{
    public class UserProfile
    {
        [Key]
        [ForeignKey("User")]
        public int ProfileID { get; set; }
        public string Name { get; set; }
        public bool? Sex { get; set; }
        public DateTime? Birthday { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Mobilephone { get; set; }
        public string Address { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual User User { get; set; }
    }
}
