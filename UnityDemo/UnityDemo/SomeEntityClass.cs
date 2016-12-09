using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDemo
{
    [NotifyPropertyChanged]
    public class SomeEntityClass : MarshalByRefObject
    {
        public string StringProperty { get; set; }
        public int IntProperty { get; set; }
        public decimal DecimalProperty { get; set; }
        public bool BoolProperty { get; set; }
        public Guid GuidProperty { get; set; }
        public DateTime? DatetimeProperty { get; set; }
    }
}
