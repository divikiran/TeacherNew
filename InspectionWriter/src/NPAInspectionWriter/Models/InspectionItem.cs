using System;
using Newtonsoft.Json;

namespace NPAInspectionWriter.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class InspectionItem : IEquatable<InspectionItem>
    {
        [JsonProperty(PropertyName = "itemId", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Guid ItemId { get; set; }

        [JsonProperty(PropertyName = "categoryId", Required = Required.Always)]
        public Guid CategoryId { get; set; }

        [JsonProperty(PropertyName = "optionId", Required = Required.AllowNull)]
        public Guid? OptionId { get; set; }

        [JsonProperty(PropertyName = "comments", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ItemComments { get; set; }
        
        [JsonProperty(PropertyName = "itemScore", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int ItemScore { get; set; }

        public bool Equals(InspectionItem other)
        {
            return ItemId.Equals(other.ItemId) && CategoryId.Equals(other.CategoryId) &&
                   ((OptionId.HasValue && other.OptionId.HasValue) && OptionId.Value.Equals(other.OptionId.Value)) &&
                   ItemComments.Equals(other.ItemComments, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            var idHash = ItemId.GetHashCode();
            var catIdHash = CategoryId.GetHashCode();
            var optionIdHash = OptionId.HasValue ? OptionId.GetHashCode() : 1;
            var commentsHash = ItemComments.GetHashCode();

            return idHash * catIdHash * optionIdHash * commentsHash;
        }
    }
}
