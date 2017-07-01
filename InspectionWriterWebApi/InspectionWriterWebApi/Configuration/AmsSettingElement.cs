using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Web;
using InspectionWriterWebApi.Models;

namespace InspectionWriterWebApi.Configuration
{
    public class AmsSettingElement : ConfigurationElement
    {
        public AmsSettingElement() { }

        public AmsSettingElement(string name)
        {
            Name = name;
        }

        public AmsSettingElement(string name, bool enabled)
        {
            Name = name;
            Enabled = enabled;
        }

        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string) this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("enabled", DefaultValue = true, IsRequired = false, IsKey = false)]
        public bool Enabled
        {
            get { return (bool) this["enabled"]; }
            set { this["enabled"] = value; }
        }

        [ConfigurationProperty("type", DefaultValue = "System.String", IsRequired = false, IsKey = false)]
        public string Type
        {
            get { return (string) this["type"]; }
            set { this["type"] = value; }
        }
    }
}