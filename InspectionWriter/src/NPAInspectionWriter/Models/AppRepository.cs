using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NPAInspectionWriter.iOS.Models;
using NPAInspectionWriter.Helpers;
using Xamarin.Forms;
using NPAInspectionWriter.Extensions;

namespace NPAInspectionWriter.Models
{
    public class AppRepository : ExtendedSQLiteConnectionWithLock
    {
        private static AppRepository _instance { get; set; }
        public static AppRepository Instance
        {
            get{
                if (_instance == null)
                {
                    var dbFactory = DependencyService.Get<IDatabaseConnectionFactory>();
					_instance = new AppRepository(dbFactory);
                }
                return _instance;
            }
        }

        public PrinterModel SelectedPrinter { get; private set; }
        public IEnumerable<InspectionImage> InspectionImages { get; set; }

        public void Initialize()
        {
            Debug.WriteLine("DB Init");
        }

        public AppRepository( IDatabaseConnectionFactory databaseFactory) :
            base( databaseFactory.GetPlatform(), databaseFactory.GetConnectionString( Constants.DatabaseName ) )
        {
            InitializeTables();
        }

        private readonly Type[] tables = new Type[]
        {
            typeof( CRWriterUser ),
            typeof( CurrentObject ),
            typeof( InspectionMasterRecord ),
            typeof( InspectionTypeRecord ),
            typeof( LocalInspection ),
            typeof( LocalInspectionCategory ),
            typeof( LocalInspectionMaster ),
            typeof( OfflineUserAuthentication ),
            typeof( Picture ),
            typeof( VehicleAlertRecord ),
            typeof( VehicleRecord ),
            typeof( VehicleRecordAvailableInspectionMasterRecord ),
            typeof( VehicleRecordAvailableInspectionTypeRecord ),
            typeof( VinAlertRecord ),
        };

        private async void InitializeTables()
        {
            await CreateTablesAsync( tables );

            if( await Table<InspectionTypeRecord>().CountAsync() != 4 )
            {
                var types = new List<InspectionTypeRecord>()
                {
                    new InspectionTypeRecord()
                    {
                        Id = 1,
                        DisplayName = "Pre-Inspection"
                    },
                    new InspectionTypeRecord()
                    {
                        Id = 2,
                        DisplayName = "Post-Inspection"
                    },
                    new InspectionTypeRecord()
                    {
                        Id = 3,
                        DisplayName = "Redemption"
                    },
                    new InspectionTypeRecord()
                    {
                        Id = 4,
                        DisplayName = "Transfer Inspection"
                    }
                };

                await InsertOrReplaceAllAsync( types );
            }
        }

        public async Task DropAndReplaceTables()
        {
            foreach( var table in tables )
            {
                await DropTableAsync( table );
            }

            await CreateTablesAsync( tables );
        }

        public async Task<int> DeleteInspection( LocalInspection inspection )
        {
            inspection = await CompleteInspection( inspection, isSummery: false, includeVehicle: false );
            int count = 0;

            count += await DeleteMasterAndCategories( inspection );
            count += await ExecuteAsync( "DELETE FROM Picture WHERE InspectionId = ?", inspection.InspectionId );
            count += await DeleteAsync( inspection );
            return count;
        }

        public async Task<int> DeleteMasterAndCategories( LocalInspection inspection )
        {
            int count = 0;
            if( inspection.Master != null )
            {
                count += await ExecuteAsync( "DELETE FROM LocalInspectionCategory WHERE LocalInspectionMasterId = ?", inspection.Master.Id );
                count += await ExecuteAsync( "DELETE FROM LocalInspectionMaster WHERE InspectionId = ?", inspection.InspectionId );
            }
            return count;
        }

        #region Add Current Object

        public async Task<int> AddCurrentObjectAsync( string key, object value ) =>
            await AddCurrentObjectAsync( key, value.ToString() );

        public async Task<int> AddCurrentObjectAsync( string key, string value )
        {
            if( string.IsNullOrWhiteSpace( value ) )
            {
                Debug.WriteLine( $"The value provided for '{key}' is null, empty or whitespace.");
                return 0;
            }

            var obj = new CurrentObject()
            {
                Key = key,
                ObjectValue = value
            };

            return await InsertOrReplaceAsync( obj );
        }

        public async Task<int> AddCurrentObjectAsync( LocalInspection inspection ) =>
            await AddCurrentObjectAsync( inspection, Constants.CurrentInspectionKey, inspection.InspectionId );

        internal void SetPrinter(PrinterModel selectedPrinter)
        {
            SelectedPrinter = selectedPrinter;
        }

        public async Task<int> AddCurrentObjectAsync( VehicleRecord vehicle ) =>
            await AddCurrentObjectAsync( vehicle, Constants.CurrentVehicleKey, vehicle.Id );

        public async Task<int> AddCurrentObjectAsync( CRWriterUser user ) =>
            await AddCurrentObjectAsync( user, Constants.CurrentUserKey, user.AccountId );

        private async Task<int> AddCurrentObjectAsync( object obj, string key, object id )
        {
            try
            {
                int currentObject = await AddCurrentObjectAsync( key, id );
                int insertObject = 0;

                if( obj is VehicleRecord )
                {
                    var vehicle = obj as VehicleRecord;
                    Settings.Current.CurrentVehicleId = vehicle.Id;
                    insertObject += await InsertOrReplaceWithChildrenAsync( vehicle );
                    insertObject += await SetVehicleAlerts( vehicle );
                    insertObject += await SetVINAlerts( vehicle );

                    if( !(vehicle.AvailableInspections.Count() == 0))
                    {
                        insertObject += await InsertOrReplaceAllWithChildrenAsync( vehicle.AvailableInspections );
                    }
                }
                else if( obj is LocalInspection )
                {
                    var inspection = obj as LocalInspection;
                    Settings.Current.CurrentInspectionId = inspection.InspectionId;
                    insertObject += await InsertOrReplaceWithChildrenAsync( inspection );

                }
                else
                    insertObject += await InsertOrReplaceAsync( obj );

                if( currentObject == 0 ) Debug.WriteLine( $"There was a problem adding '{key}' - with value '{id}'");
                if( insertObject == 0 ) Debug.WriteLine( $"There was a problem inserting or replacing the object for '{key}'");

                return currentObject + insertObject;
            }
            catch( Exception e )
            {
                Debug.WriteLine( e );
                return 0;
            }
        }

        private async Task<int> SetVehicleAlerts( VehicleRecord vehicle )
        {
            if( vehicle == null || vehicle.VehicleAlerts.Count() == 0 ) return 0;
            int count = 0;
            await ExecuteAsync( "DELETE FROM VehicleAlertRecord WHERE VehicleRecordId = ?", vehicle.Id );

            foreach( var alert in vehicle.VehicleAlerts )
            {
                alert.VehicleRecordId = vehicle.Id;
                count += await InsertAsync( alert );
            }

            return count;
        }

        private async Task<int> SetVINAlerts( VehicleRecord vehicle )
        {
            if( vehicle == null || vehicle.VinAlerts.Count() == 0 ) return 0;
            int count = 0;

            await ExecuteAsync( "DELETE FROM VinAlertRecord WHERE VehicleId = ?", vehicle.Id );
            foreach( var alert in vehicle.VinAlerts )
            {
                alert.VehicleId = vehicle.Id;
                count += await InsertAsync( alert );
            }

            return count;
        }

        #endregion

        #region Clear Current Objects

        public int ClearAllCurrentObjects() =>
            DeleteAll<CurrentObject>();

        public int ClearCurrentObjects( params string[] keys )
        {
            IEnumerable<CurrentObject> objects = TableSynchronous<CurrentObject>().Where( o => keys.Contains( o.Key ) ).ToList();

            int count = 0;

            foreach( var item in objects )
                count += Delete( item );

            return count;
        }

        public async Task<int> ClearCurrentObjectsAsync( params string[] keys )
        {
            IEnumerable<CurrentObject> objects = await Table<CurrentObject>().Where( o => keys.Contains( o.Key ) ).ToListAsync();

            int count = 0;

            foreach( var item in objects )
                count += await DeleteAsync( item );

            return count;
        }

        public async Task<int> ClearAllCurrentObjectsAsync() =>
            await DeleteAllAsync<CurrentObject>();

        #endregion

        #region Get Current Object

        public async Task<string> GetCurrentSettingAsync( string key )
        {
            try
            {
                var result = await Table<CurrentObject>().Where( o => o.Key == key ).FirstOrDefaultAsync();
                if( result == null ) return string.Empty;

                ValidateId( result?.ObjectValue, key );
                return result.ObjectValue;
            }
            catch( Exception e )
            {
                Debug.WriteLine( e );
                return string.Empty;
            }
        }

        public async Task<Guid> GetCurrentSettingAsync( string key, Guid defaultValue = default( Guid ) )
        {
            var result = await GetCurrentSettingAsync( key );
            if( !string.IsNullOrWhiteSpace( result ) )
            {
                return Guid.Parse( result );
            }

            return default( Guid );
        }

        public async Task<InspectionState> GetCurrentSettingAsync( string key, InspectionState defaultValue = InspectionState.Locked )
        {
            try
            {
                return ( InspectionState )Enum.Parse( typeof( InspectionState ), await GetCurrentSettingAsync<string>( key ) );
            }
            catch( Exception e )
            {
                Debug.WriteLine( e );
                return defaultValue;
            }
        }

        public async Task<T> GetCurrentSettingAsync<T>( string key, T defaultValue = default( T ) )
        {
            try
            {
                var result = await Table<CurrentObject>().Where( o => o.Key == key ).FirstOrDefaultAsync();

                if( result == null || string.IsNullOrWhiteSpace( result.ObjectValue ) ) return defaultValue;

                return ( T )Convert.ChangeType( result.ObjectValue, typeof( T ) );
            }
            catch( Exception e )
            {
                Debug.WriteLine( e );
                return defaultValue;
            }

        }

        /// <summary>
        /// Retrieves the current vehicle object
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        /// <exception cref="VehicleNotFoundException">Throws a Vehicle Not Found Exception if no Vehicle Id is stored in the Current Object Table</exception>
        public async Task<VehicleRecord> GetCurrentVehicleAsync()
        {
            // We intentionally want to get the string so we can make sure it's valid.
            string idString = await GetCurrentSettingAsync( Constants.CurrentVehicleKey );
            Guid id;
            if( !string.IsNullOrWhiteSpace( idString ) && Guid.TryParse( idString, out id ) )
            {
                var vehicle = await GetAsync<VehicleRecord>( v => v.Id == id );

                // Get Inspections
                vehicle.AvailableInspections = await GetInspectionsForVehicle( vehicle.Id );

                // Get Available Inspection Types
                vehicle.AvailableInspectionTypes = await QueryAsync<InspectionTypeRecord>(
                    "SELECT * FROM InspectionTypeRecord itr INNER JOIN VehicleRecordAvailableInspectionTypeRecord vrait ON " +
                    "itr.Id = vrait.InspectionTypeId WHERE " +
                    "vrait.VehicleId = ?", vehicle.Id );

                // Get Available Inspection Masters
                vehicle.AvailableMasters = await QueryAsync<InspectionMasterRecord>(
                    "SELECT * FROM InspectionMasterRecord imr INNER JOIN VehicleRecordAvailableInspectionMasterRecord vraimr " +
                    "ON imr.MasterId = vraimr.MasterId " +
                    "WHERE vraimr.VehicleId = ?", vehicle.Id );

                // Get Vehicle Alerts
                vehicle.VehicleAlerts = await Table<VehicleAlertRecord>().Where( x => x.VehicleRecordId == vehicle.Id ).ToListAsync();

                // Get VIN Alerts
                vehicle.VinAlerts = await Table<VinAlertRecord>().Where( x => x.VehicleId == vehicle.Id ).ToListAsync();

                return vehicle;
            }

            return null;
        }

        public async Task<CRWriterUser> GetCurrentUserAsync()
        {
            // We intentionally want to get the string so we can make sure it's valid.
            string idString = await GetCurrentSettingAsync( Constants.CurrentUserKey );

            Guid id;
            if( !string.IsNullOrWhiteSpace( idString ) && Guid.TryParse( idString, out id ) )
                return await GetAsync<CRWriterUser>( x => x.AccountId == id );

            return null;
        }

        public async Task<LocalInspection> GetCurrentInspectionAsync( bool isSummery = false)
        {
            string idString = await GetCurrentSettingAsync( Constants.CurrentInspectionKey );
            Guid id = default( Guid );
            if( !string.IsNullOrWhiteSpace( idString ) && Guid.TryParse( idString, out id ) )
                return await GetInspectionById( id, isSummery );

            return null;
        }

        public async Task<Guid> GetSelectedPrinterId() =>
            await GetCurrentSettingAsync( Constants.SelectedPrinterKey, default( Guid ) );

        public async Task<InspectionState> GetCurrentInspectionState() =>
            await GetCurrentSettingAsync( Constants.CurrentInspectionState, InspectionState.Locked );

        #endregion

        private void ValidateId( string id, string key )
        {
            if( string.IsNullOrWhiteSpace( id ) )
            {
                var message = $"No value was found for the specified key: {key}";
                Debug.WriteLine( message);
                throw new ArgumentNullException( message: message, innerException: null );
            }
        }

        #region RootObject CRUD Operations

        public async Task<int> InsertOrReplaceWithChildrenAsync( VehicleRecord vehicle )
        {
            if( vehicle == null ) return 0;

            var count = await InsertOrReplaceAsync( vehicle );

            if( vehicle.AvailableInspections?.Count > 0 )
                count += await InsertOrReplaceAllWithChildrenAsync( vehicle.AvailableInspections );

            if( vehicle.AvailableInspectionTypes?.Count > 0 )
                count += await InsertOrReplaceAsync( vehicle.AvailableInspectionTypes );

            if( vehicle.AvailableMasters?.Count > 0 )
                count += await InsertOrReplaceAsync( vehicle.AvailableMasters );

            if( vehicle.VehicleAlerts?.Count > 0 )
                count += await InsertOrReplaceAllAsync( vehicle.VehicleAlerts );

            if( vehicle.VinAlerts?.Count > 0 )
                count += await InsertOrReplaceAllAsync( vehicle.VinAlerts );

            var masterKeys = new List<VehicleRecordAvailableInspectionMasterRecord>();
            if( vehicle.AvailableMasters != null )
            {
                foreach( var master in vehicle.AvailableMasters )
                {
                    if( master == null )
                    {
                        Debug.WriteLine( "We have a null master in our loop..." );
                        continue;
                    }

                    masterKeys.Add( new VehicleRecordAvailableInspectionMasterRecord()
                    {
                        VehicleId = vehicle.Id,
                        MasterId = master.MasterId
                    } );
                }
            }

            var typeKeys = new List<VehicleRecordAvailableInspectionTypeRecord>();
            if( vehicle.AvailableInspectionTypes != null )
            {
                foreach( var type in vehicle.AvailableInspectionTypes )
                {
                    typeKeys.Add( new VehicleRecordAvailableInspectionTypeRecord()
                    {
                        VehicleId = vehicle.Id,
                        InspectionTypeId = type.Id
                    } );
                }
            }

            count += await InsertOrReplaceAllAsync( masterKeys );
            count += await InsertOrReplaceAllAsync( typeKeys );

            return count;
        }

        /// <summary>
        /// Inserts or replaces all local inspection models with children async.
        /// </summary>
        /// <returns>The or replace all with children async.</returns>
        /// <param name="inspections">Inspections.</param>
        public async Task<int> InsertOrReplaceAllWithChildrenAsync( IEnumerable<LocalInspection> inspections )
        {
            int count = 0;

            if( inspections == null || inspections.Count() < 1 ) return count;

            foreach( var inspection in inspections )
            {
                count += await InsertOrReplaceWithChildrenAsync( inspection );
            }

            return count;
        }

        /// <summary>
        /// Inserts the inspection or replaces with children async.
        /// </summary>
        /// <returns>The or replace with children async.</returns>
        /// <param name="inspection">Inspection.</param>
        public async Task<int> InsertOrReplaceWithChildrenAsync( LocalInspection inspection )
        {
            if(inspection == null) { return -1; }
            // Set Inspection Type
            if( inspection.Type == null )
            {
                // Grab Type by Id if the inspection has a type set
                if( inspection.InspectionTypeId > 0 )
                {
                    inspection.Type = ( await QueryAsync<InspectionTypeRecord>(
                        "SELECT itr.* FROM InspectionTypeRecord itr " +
                        "INNER JOIN VehicleRecordAvailableInspectionTypeRecord vraitr " +
                        "ON vraitr.InspectionTypeId = itr.Id " +
                        "WHERE vraitr.InspectionTypeId = ?", inspection.InspectionTypeId ) )?.FirstOrDefault();
                }
                else
                {
                    inspection.Type = ( await QueryAsync<InspectionTypeRecord>(
                        "SELECT itr.* FROM InspectionTypeRecord itr " +
                        "INNER JOIN VehicleRecordAvailableInspectionTypeRecord vraitr " +
                        "ON vraitr.InspectionTypeId = itr.Id " +
                        "WHERE vraitr.VehicleId = ?", inspection.VehicleId ) )?.FirstOrDefault();
                }
            }

            if( inspection.Type != null )
            {
                if( string.IsNullOrWhiteSpace( inspection.InspectionType ) )
                    inspection.InspectionType = inspection.Type.DisplayName;

                if( inspection.InspectionTypeId != inspection.Type.Id ) inspection.InspectionTypeId = inspection.Type.Id;
            }

            // Set Inspection Master
            InspectionMasterRecord master = null;
            if( inspection.MasterId != default( Guid ) )
            {
                master = ( await QueryAsync<InspectionMasterRecord>(
                    "SELECT * FROM InspectionMasterRecord " +
                    "INNER JOIN VehicleRecordAvailableInspectionMasterRecord " +
                    "ON VehicleRecordAvailableInspectionMasterRecord.Masterid = InspectionMasterRecord.MasterId " +
                    "WHERE InspectionMasterRecord.MasterId = ?", inspection.MasterId ) )?.FirstOrDefault();
            }
            else
            {
                master = ( await QueryAsync<InspectionMasterRecord>(
                    "SELECT * FROM InspectionMasterRecord " +
                    "INNER JOIN VehicleRecordAvailableInspectionMasterRecord " +
                    "ON VehicleRecordAvailableInspectionMasterRecord.MasterId = InspectionMasterRecord.MasterId " +
                    "WHERE VehicleRecordAvailableInspectionMasterRecord.VehicleId = ?", inspection.VehicleId ) )?.FirstOrDefault();

            }

            if( master != null )
            {
                if( string.IsNullOrWhiteSpace( inspection.MasterDisplayName ) )
                    inspection.MasterDisplayName = master?.DisplayName;

                if( master.MasterId != inspection.MasterId ) inspection.MasterId = master.MasterId;
            }

            // Save Inspection
            int count = await InsertOrReplaceAsync( inspection );
            count += await InsertOrReplaceWithChildrenAsync( inspection.Master, inspection );
            count += await InsertOrReplaceAllAsync( inspection.Pictures );

            return count;
        }

        public async Task<int> InsertOrReplaceWithChildrenAsync( LocalInspectionMaster master, Guid inspectionId )
        {
            if( master == null ) return 0;

            master.InspectionId = inspectionId;
            int count = await InsertOrReplaceAsync( master );
            int localMasterId = ( await Table<LocalInspectionMaster>()
                    .Where( x => x.InspectionId == inspectionId ).FirstOrDefaultAsync() )?.Id ?? 0;

            foreach( var category in master.Categories )
            {
                category.LocalInspectionMasterId = localMasterId;
                //count += await InsertOrReplaceAsync( category );
            }

            if( master.Categories?.Count > 0 )
                count += await InsertOrReplaceAllAsync( master.Categories );

            return count;
        }

        /// <summary>
        /// Gets the inspections for vehicle.
        /// </summary>
        /// <returns>The inspections for vehicle.</returns>
        /// <param name="vehicleId">Vehicle identifier.</param>
        public async Task<List<LocalInspection>> GetInspectionsForVehicle( Guid vehicleId )
        {
            var inspections = await Table<LocalInspection>().Where( x => x.VehicleId == vehicleId ).ToListAsync();

            foreach( var inspection in inspections )
            {
                await CompleteInspection( inspection, true );
            }

            return inspections;
        }


        /// <summary>
        /// Gets the inspection by id. Including children.
        /// </summary>
        /// <returns>The inspection by identifier.</returns>
        /// <param name="id">Identifier.</param>
        public async Task<LocalInspection> GetInspectionById( Guid id, bool isSummery = false)
        {
            var inspection = await Table<LocalInspection>().Where( x => x.InspectionId == id ).FirstOrDefaultAsync();
            return await CompleteInspection( inspection, isSummery );
        }

        public async Task<IEnumerable<LocalInspection>> GetLocalInspections( bool includeVehicle = false )
        {
            var inspections = await Table<LocalInspection>().ToListAsync();
            return inspections.ForEach( async x => await CompleteInspection( x ) );
        }

        public async Task<IEnumerable<LocalInspection>> GetLocalInspections( Expression<Func<LocalInspection,bool>> expression, bool includeVehicle = false )
        {
            var inspections = await Table<LocalInspection>().Where( expression ).ToListAsync();
            foreach( var i in inspections )
                await CompleteInspection( i, includeVehicle: includeVehicle );

            return inspections;
            //await inspections.ForEachAsync( x => CompleteInspection( x ) );
            //return inspections.ForEach( async x => await CompleteInspection( x ) );
        }

        public async Task<List<Picture>> GetPicturesById(Guid inspectionId)
        {
            var pics = await Table<Picture>().Where(x => x.InspectionId == inspectionId).OrderBy(x => x.Position).ToListAsync();
            return pics;
        }

        /// <summary>
        /// Completes the inspection.
        /// </summary>
        /// <returns>The completed inspection.</returns>
        /// <param name="inspection">Inspection.</param>
        private async Task<LocalInspection> CompleteInspection( LocalInspection inspection, bool isSummery = false, bool includeVehicle = false )
        {
            inspection.Type = await Table<InspectionTypeRecord>().Where( x => x.Id == inspection.InspectionTypeId ).FirstOrDefaultAsync();
            if( includeVehicle )
            {
                inspection.Vehicle = await Table<VehicleRecord>().Where( v => v.Id == inspection.VehicleId ).FirstOrDefaultAsync();
            }

            if (!isSummery)
            {
                inspection.Master = await Table<LocalInspectionMaster>().Where(x => x.InspectionId == inspection.InspectionId).FirstOrDefaultAsync();
                inspection.Pictures = await Table<Picture>().Where(x => x.InspectionId == inspection.InspectionId).OrderBy(x => x.Position).ToListAsync();

                if (inspection.Master != null)
                {
                    inspection.Master.Categories = await Table<LocalInspectionCategory>().Where(x => x.LocalInspectionMasterId == inspection.Master.Id).ToListAsync();
                }
                inspection.SetScore();
            }

            return inspection;
        }

        public async Task<bool> SaveLocalInspection(LocalInspection inspection)
        {
            if (inspection == null) return false;

            await InsertOrReplaceAsync(inspection);

            if(inspection.Master != null)
            {
                await InsertOrReplaceAsync(inspection.Master);
                if (inspection.Master.Categories != null)
                {
                    await InsertOrReplaceAllAsync(inspection.Master.Categories);
                }
            }

            return true;
        }

        #endregion
    }
	
}
