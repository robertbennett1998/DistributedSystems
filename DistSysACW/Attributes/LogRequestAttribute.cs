using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DistSysACW.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class LogRequestAttribute : Attribute
    {
        public LogRequestAttribute()
        {
        }
    }
}
