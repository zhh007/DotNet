using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDemo.Configuration;

namespace UnityDemo
{
    public class SomeEntityClass : IConfiguration
    {
        [ConfigurationItem("31E1DDE9-D028-41DC-B169-BAFC69B63003")]
        public virtual string StringProperty { get; set; }

        [ConfigurationItem("53F99EE3-C4C3-4447-BA0D-3BAF952A4EC9")]
        public virtual int IntProperty { get; set; }

        public decimal DecimalProperty { get; set; }

        public bool BoolProperty { get; set; }

        public Guid GuidProperty { get; set; }

        public DateTime? DatetimeProperty { get; set; }
    }
}
