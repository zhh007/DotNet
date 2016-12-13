using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDemoTransparentProxy.Config;

namespace UnityDemoTransparentProxy
{
    public class SomeEntityClass : ConfigBase
    {
        [ConfigProperty("31E1DDE9-D028-41DC-B169-BAFC69B63003")]
        public virtual string StringProperty { get; set; }

        public int IntProperty { get; set; }

        public decimal DecimalProperty { get; set; }

        public bool BoolProperty { get; set; }

        public Guid GuidProperty { get; set; }

        public DateTime? DatetimeProperty { get; set; }
    }
}
