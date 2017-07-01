## Inspection Writer Development

The solution is broken into different logical projects.

## Libraries
- Common Library (Should never reference Prism)
  - Core (Portable) - This code can be accessed from anywhere
  - iOS - This code is generally specific to work for the iOS Platform
  - iOS Unity - This code provides separation in the case that another DI Container is chosen at another time.
  - Shared Libraries - These are shared between the iOS and Android platforms as the code is largely identical except for in a few places where we can take advantage of partial classes to make the necessary specific implementations.
- Module
  - Common Prism Views/ViewModels with a Unity wrapper.
  - Extensions for ILoggerFacade, Category enum, and NavigationParameters
  - Events
  - Debug Logger implementing ILog and ILoggerFacade
  - Navigation helpers including MultiPageNavigationBehavior, IMultiPageNavigationAware, and INavigationService extensions

### Inspection Writer Projects
- Core (Portable) - This code can be reused cross platform and contains the vast majority of code specific to the Inspection Writer.
- iOS - This code provides iOS specific impelemenations. This should generally be a fairly lean codebase.

## Debugging
The iOS platform includes a UdpLogging interface that implements the ILoggerFacade from Prism. This allows us to capture all events from within Prism itself and stream it to a workstation or logging server. You can use Visual Syslog Server, or any other application you like to capture these events.

The platform includes a number of settings to assist in debugging. These include the ability to force the use of the Stage Server, Override with the use of a custom server, update the Host/IP and Port for the Logging Server. These settings can be updated pre-build by modifying the Settings.bundle/Root.plist, and changing the Default Value associated with the setting you need to change.

## InspectionWriter Development

The InspectionWriter has several primary helpers that are important to be aware of.

1) Helpers.Constants - This contains a number of Constants as well as parameters that can be, or must be replaced for production builds. These constants are wrapped in the 'Build Agent Variables' region, and include the API Url and API Version, as well as the HockeyApp key.
2) Helpers.Settings - This class is built as a singleton, and offers several Variables which should persist on the device as well as several more settings which are more session specific.
3) Models.AppRepository
  a) In addition to being the connection class for the local SQLite database, there are a number of helper functions in this class that will help persist your root level Vehicle object and Inspection objects with their children.
  b) You can simulataniously save a Vehicle or Inspection and add it as the Current Object, you may also save a number of items as the Current Object using the ```AddCurrentObjectAsync( ... )``` methods and the objects key in Helper.Constants.
4) Many of the local models have implicit operators or an extension method to help convert to/from the API Model. Additionally the Vehicle and Inspection have an implicit operator that will return it's Guid Id.
