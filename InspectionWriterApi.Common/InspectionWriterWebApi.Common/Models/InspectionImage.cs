using System;
using Newtonsoft.Json;

namespace InspectionWriterWebApi.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class InspectionImage
    {
        [JsonProperty(PropertyName = "pictureId", Required = Required.Always)]
        public Guid PictureId { get; set; }

        [JsonProperty(PropertyName = "displayOrder", Required = Required.Always)]
        public int DisplayOrder { get; set; }

        [JsonProperty(PropertyName = "isLandscape")]
        public bool IsLandscape { get; set; }

        [JsonProperty(PropertyName = "width")]
        public int? Width { get; set; }

        [JsonProperty(PropertyName = "height")]
        public int? Height { get; set; }

        [JsonIgnore]
        public NPA.CodeGen.Location ImageLocation { get; set; }

        private string imageBaseUrl;
        [JsonProperty(PropertyName = "imageBaseUrl")]
        public string ImageBaseUrl
        {
            get
            {
                return !string.IsNullOrWhiteSpace(ImageLocation?.BaseImageUrl) ? ImageLocation.BaseImageUrl + "?PictureID=" + PictureId + "&" : imageBaseUrl;
            }
            set { imageBaseUrl = value; }
        }
    }
}
