using System;
using System.Linq;
using Foundation;

namespace NPAInspectionWriter.iOS.Extensions
{
    public static class NSUserDefaultsExtensions
    {
        public static void AddPrefernceSpecifierObserver( this NSUserDefaults defaults, Action action )
        {
            defaults.AddObserver( "PreferenceSpecifiers", NSKeyValueObservingOptions.New, ( observedChange ) =>
            {
                action?.Invoke();
            } );
        }

        public static NSDictionary[] GetPreferenceSpecifiers( this NSUserDefaults defaults )
        {
            return NSArray.FromArray<NSDictionary>( defaults[ "PreferenceSpecifiers" ] as NSArray );
        }

        public static NSDictionary GetPreferenceItem( this NSUserDefaults defaults, string key )
        {
            var preferenceItems = defaults.GetPreferenceSpecifiers();
            return preferenceItems?.FirstOrDefault( x => x[ "Key" ]?.ToString() == key );
        }

        public static NSObject GetPreferenceItemDefaultValue( this NSUserDefaults defaults, string key )
        {
            return defaults.GetPreferenceItem( key )?[ "DefaultValue" ];
        }

        public static NSObject GetPreferenceItemDefaultValue( this NSDictionary preferenceItem )
        {
            return preferenceItem[ "DefaultValue" ];
        }

        public static string StringFromPreferenceKey( this NSUserDefaults defaults, string key )
        {
            return defaults.GetPreferenceItemDefaultValue( key )?.ToString() ?? string.Empty;
        }

        public static int IntFromPreferenceKey( this NSUserDefaults defaults, string key, int defaultValue = 0 )
        {
            try
            {
                return int.Parse( defaults.GetPreferenceItemDefaultValue( key ).ToString() );
            }
            catch( Exception e )
            {
                System.Diagnostics.Debug.WriteLine( e );
                return defaultValue;
            }
        }

        public static T GetValueFromPreferenceKey<T>( this NSUserDefaults defaults, string key, T defaultValue = default( T ) )
        {
            try
            {
                return ( T )Convert.ChangeType( defaults.GetPreferenceItemDefaultValue( key ).ToString(), typeof( T ) );
            }
            catch( Exception )
            {
                return defaultValue;
            }
        }

        public static bool BoolFromPreferenceKey( this NSUserDefaults defaults, string key )
        {
            try
            {
                switch( defaults.GetPreferenceItemDefaultValue( key ).ToString() )
                {
                    case null:
                    case "":
                    case "0":
                        return false;
                    case "1":
                        return true;
                    default:
                        return bool.Parse( defaults.GetPreferenceItemDefaultValue( key ).ToString() );
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
