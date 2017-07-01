using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace InspectionWriterWebApi.Configuration
{
    public class AmsSettingsSection : ConfigurationSection
    {
        [ConfigurationProperty("keys", IsDefaultCollection = false)]
        public AmsSettingsCollection Keys
        {
            get { return (AmsSettingsCollection) base["keys"]; }
        }

        protected override string SerializeSection(ConfigurationElement parenElement, string name, ConfigurationSaveMode saveMode)
        {
            var s = base.SerializeSection(parenElement, name, saveMode);

            return s;
        }
    }
}