﻿// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.42000
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace NPAInspectionWriter.Resx {
    using System;
    using System.Reflection;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ModuleResources {
        
        private static System.Resources.ResourceManager resourceMan;
        
        private static System.Globalization.CultureInfo resourceCulture;
        
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ModuleResources() {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager {
            get {
                if (object.Equals(null, resourceMan)) {
                    System.Resources.ResourceManager temp = new System.Resources.ResourceManager("NPAInspectionWriter.Resx.ModuleResources", typeof(ModuleResources).GetTypeInfo().Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        internal static string AppNameNotPresent {
            get {
                return ResourceManager.GetString("AppNameNotPresent", resourceCulture);
            }
        }
        
        internal static string AppVersionNotPresent {
            get {
                return ResourceManager.GetString("AppVersionNotPresent", resourceCulture);
            }
        }
        
        internal static string LoginBtnText {
            get {
                return ResourceManager.GetString("LoginBtnText", resourceCulture);
            }
        }
        
        internal static string OhSnapAlertTitle {
            get {
                return ResourceManager.GetString("OhSnapAlertTitle", resourceCulture);
            }
        }
        
        internal static string OkBtnText {
            get {
                return ResourceManager.GetString("OkBtnText", resourceCulture);
            }
        }
        
        internal static string PasswordPlaceholder {
            get {
                return ResourceManager.GetString("PasswordPlaceholder", resourceCulture);
            }
        }
        
        internal static string UserNamePlaceholder {
            get {
                return ResourceManager.GetString("UserNamePlaceholder", resourceCulture);
            }
        }
    }
}