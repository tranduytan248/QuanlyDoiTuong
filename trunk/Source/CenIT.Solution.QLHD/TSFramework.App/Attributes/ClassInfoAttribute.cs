using System;

namespace TSFramework.App.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ClassInfoAttribute : Attribute
    {
        public ClassInfoAttribute(string pluginName, string description)
        {
            PluginName = pluginName;
            Description = description;
        }

        public string PluginName { get; }

        public string Description { get; }
    }
}