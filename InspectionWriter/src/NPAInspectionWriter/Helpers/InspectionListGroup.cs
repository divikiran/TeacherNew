using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NPAInspectionWriter.Models;
using MvvmHelpers;
namespace NPAInspectionWriter.Helpers
{
    public class InspectionListGroup : ObservableRangeCollection<LocalInspection>
    {
        public InspectionListGroup( IEnumerable<LocalInspection> collection )
        {
            this.AddRange( collection );
            GroupHeader = this.FirstOrDefault()?.GroupHeader;
        }

        public string GroupHeader { get; private set; }

        public static ObservableCollection<InspectionListGroup> GetGroupedCollection( IEnumerable<LocalInspection> collection )
        {
            var groups = collection.Select( x => x.GroupHeader ).Distinct();
            var output = new ObservableCollection<InspectionListGroup>();

            foreach( var group in groups )
            {
                output.Add( new InspectionListGroup( collection.Where( x => x.GroupHeader == group ) ) );
            }

            return output;
        }

        public LocalInspection GetInspectionById( Guid inspectionId ) =>
            this.FirstOrDefault( x => x.InspectionId == inspectionId );

        public bool HasInspection( Guid inspectionId ) =>
            this.Any( x => x.InspectionId == inspectionId );
    }
}
