using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wysnan.EIMOnline.Common.Framework.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CodeAttribute : Attribute
    {
        private string prefix { get; set; }

        public CodeAttribute(string prefix)
        {
            this.prefix = prefix;
        }
    }
}
