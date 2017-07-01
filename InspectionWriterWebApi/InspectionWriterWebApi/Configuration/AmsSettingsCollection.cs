using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IdentityModel.Protocols.WSTrust;
using System.Linq;
using System.Web;

namespace InspectionWriterWebApi.Configuration
{
    public class AmsSettingsCollection : ConfigurationElementCollection
    {
        public AmsSettingsCollection()
        {

        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.AddRemoveClearMap; }
        }

        protected override ConfigurationElement CreateNewElement(string elementName)
        {
            return new AmsSettingElement(elementName);
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new AmsSettingElement();
        }
        
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AmsSettingElement) element).Name;
        }

        public new string AddElementName
        {
            get { return base.AddElementName; }
            set { base.AddElementName = value; }
        }

        public new string ClearElementName
        {
            get { return base.ClearElementName; }
            set { base.ClearElementName = value; }
        }

        public new string RemoveElementName
        {
            get { return base.RemoveElementName; }
        }

        public new int Count
        {
            get { return base.Count; }
        }

        public AmsSettingElement this[int index]
        {
            get { return (AmsSettingElement) BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public AmsSettingElement this[string settingName]
        {
            get { return (AmsSettingElement) BaseGet(settingName); }
        }

        public int IndexOf(AmsSettingElement setting)
        {
            return BaseIndexOf(setting);
        }

        public void Add(AmsSettingElement setting)
        {
            BaseAdd(setting);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(AmsSettingElement setting)
        {
            if (BaseIndexOf(setting) >= 0)
            {
                BaseRemove(setting.Name);
            }
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string settingName)
        {
            BaseRemove(settingName);
        }

        public void Clear()
        {
            BaseClear();
        }
    }
}