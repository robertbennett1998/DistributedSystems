using System;

namespace DistSysACWClient.Attributes
{
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public class CommandAttribute : Attribute
    {
        public string Description { get; private set; }
        public CommandAttribute(string description="No Description.")
        {
            Description = description;
        }
    }
}