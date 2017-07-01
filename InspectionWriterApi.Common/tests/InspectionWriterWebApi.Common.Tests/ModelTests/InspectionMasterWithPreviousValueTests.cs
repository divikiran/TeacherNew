using System;
using System.Linq;
using InspectionWriterWebApi.Common.Tests.Data;
using InspectionWriterWebApi.Models;
using Xunit;
using Xunit.Abstractions;

namespace InspectionWriterWebApi.Common.Tests
{
    public class InspectionMasterWithPreviousValueTests
    {
        private ITestOutputHelper output { get; }
        private InspectionMaster masterWithPreviousVal { get; }

        public InspectionMasterWithPreviousValueTests( ITestOutputHelper helper )
        {
            output = helper;
            var fileHelper = new FileHelper( output );
            masterWithPreviousVal = fileHelper.LoadEmbeddedJsonFile<InspectionMaster>( "masterWithPreviousVal.json" );
        }

        [Theory]
        [ClassData( typeof( MasterWithPreviousValData ) )]
        public void JsonDeserialize( Guid masterId, string masterDisplayName, bool defaultForCategory, int maxInspectionScore,
            Guid categoryId, string categoryDisplayName, int categoryWeight, int categoryMaxScore, bool categoryRequired,
            Guid? optionId, string optionDisplayName, int optionValue )
        {
            //var assembly = typeof( InspectionMasterWithPreviousValueTests ).Assembly;
            //var resourceName = assembly.GetManifestResourceNames().FirstOrDefault( x => x.EndsWith( "masterWithPreviousVal.json" ) );
            //output.WriteLine( $"Resource: {resourceName}" );

            Assert.NotNull( masterWithPreviousVal );

            // Master Level Values (Shouldn't change from theory to theory...)
            Assert.Equal( masterId, masterWithPreviousVal.MasterId );
            Assert.Equal( masterDisplayName, masterWithPreviousVal.DisplayName );
            Assert.Equal( defaultForCategory, masterWithPreviousVal.DefaultForCategory );
            Assert.Equal( maxInspectionScore, masterWithPreviousVal.MaxInspectionScore );

            //Category Level Values
            Assert.Contains( masterWithPreviousVal.Categories, x => x.CategoryId == categoryId );
            var category = masterWithPreviousVal.Categories.FirstOrDefault( x => x.CategoryId == categoryId );
            Assert.Equal( categoryDisplayName, category.DisplayName );
            Assert.Equal( categoryWeight, category.Weight );
            Assert.Equal( categoryMaxScore, category.MaxScore );
            Assert.Equal( categoryRequired, category.Required );

            // Option Values
            if( optionId == null )
            {
                Assert.Null( category.PreviousValue );
            }
            else
            {
                Assert.NotNull( category.PreviousValue );
                Assert.Contains( category.Options, x => x.OptionId == category.PreviousValue.OptionId );
                var option = category.Options.FirstOrDefault( x => x.OptionId == category.PreviousValue.OptionId );
                Assert.Equal( optionId.Value, option.OptionId );
                Assert.Equal( optionDisplayName, option.DisplayName );
                Assert.Equal( optionValue, option.Value );
            }
        }
    }
}
