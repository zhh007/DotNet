using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF6DemoFluent.Models
{
    public class FamilyMember
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool? Sex { get; set; }
        public DateTime? Birthday { get; set; }

        public virtual ICollection<FamilyMember> Parents { get; set; }
        public virtual ICollection<FamilyMember> Children { get; set; }

        public FamilyMember()
        {
            this.Parents = new List<FamilyMember>();
            this.Children = new List<FamilyMember>();
        }
    }
}
