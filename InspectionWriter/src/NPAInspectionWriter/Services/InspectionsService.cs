using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using NPAInspectionWriter.Models;
using Newtonsoft.Json;
using NPAInspectionWriter.Helpers;
using NPAInspectionWriter.Extensions;
using System.Diagnostics;

namespace NPAInspectionWriter.Services
{
    public class InspectionsService : IInspectionsService
    {
        const string InspectionsControllerBase = "inspections";
        const string UploadControllerBase = "upload/inspectionImage";

        InspectionWriterClient _client { get; } = new InspectionWriterClient();
        AppRepository _app { get; } = AppRepository.Instance;
        IApplicationFileSystem _afs { get; }

        public InspectionsService(InspectionWriterClient client, AppRepository app, IApplicationFileSystem afs)
        {
            _client = client;
            _app = app;
            _afs = afs;
        }

        public InspectionsService()
        {
        }

        public async Task<string> GetAmsSettingAsync( string setting )
        {
            return await _client.GetAsync( $"{InspectionsControllerBase}/amsSetting/{setting}", failedResponse: string.Empty );
        }

        public async Task<string> GetAmsSettingAsync( string setting, Guid locationId )
        {
            return await _client.GetAsync( $"{InspectionsControllerBase}/amsSetting/{setting}/{locationId}", failedResponse: string.Empty );
        }

        public Task<Inspection> GetInspectionAsync( Guid inspectionId )
        {
            Debug.WriteLine( "GetInspectionAsync has not been implemented");
            throw new NotImplementedException();
        }

        public Task<IEnumerable<InspectionImage>> GetInspectionImagesFromInspectionAsync( LocalInspection inspection )
        {
            return GetInspectionImagesAsync( inspection );
        }

        public async Task<IEnumerable<InspectionImage>> GetInspectionImagesAsync(Guid inspectionId)
        {
            return await _client.GetAsync<IEnumerable<InspectionImage>>($"{InspectionsControllerBase}/getInspectionImagesWithURL/{inspectionId}");
        }

        public Task<IEnumerable<InspectionItem>> GetInspectionItemsAsync( Inspection inspection )
        {
            Debug.WriteLine( "GetInspectionItemsAsync has not been implemented");
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Inspection>> GetInspectionsAsync( InspectionSearchRequest searchRequest )
        {
            Debug.WriteLine( "GetInspectionsAsync has not been implemented");
            throw new NotImplementedException();
        }

        public Task<InspectionType> GetInspectionTypeAsync( Inspection inspection )
        {
            Debug.WriteLine( "GetInspectionTypeAsync has not been implemented");
            throw new NotImplementedException();
        }

        public async Task<InspectionMaster> GetMasterAsync( Guid masterId )
        {
            return await _client.GetAsync<InspectionMaster>( $"{InspectionsControllerBase}/masters/{masterId}", failedResponse: null );
        }

        public async Task<InspectionMaster> GetMasterWithPreviousValueAsync( Guid vehicleId, Guid masterId, int numberOfDays )
        {
            if( vehicleId == default( Guid ) ) throw new ArgumentException( $"The 'vehicleId' specified is invalid: {vehicleId}" );
            if( masterId == default( Guid ) ) throw new ArgumentException( $"This 'masterId' specified is invalid: {masterId}" );

            return await _client.GetAsync<InspectionMaster>( $"{InspectionsControllerBase}/masterWithPreviousVal/{vehicleId}/{masterId}/{numberOfDays}" );
        }

        public async Task<InspectionMaster> GetMasterForInspectionWithSelectedValueAsync( Guid inspectionId )
        {
            if( inspectionId == default( Guid ) ) throw new ArgumentException( $"This 'inspectionId' specified is invalid: {inspectionId}" );

            return await _client.GetAsync<InspectionMaster>( $"{InspectionsControllerBase}/inspectionWithSelectedVal/{inspectionId}" );
        }

        public async Task<InspectionTypesAndMasters> GetMastersAndTypesAsync( Inspection inspection )
        {
            var response = await _client.GetStringAsync( $"{InspectionsControllerBase}/{inspection.InspectionId}/typesAndMasters" );
            System.Diagnostics.Debug.WriteLineIf( _client.LastResponse == null, response );

            var im = JsonConvert.DeserializeObject<InspectionTypesAndMasters>( response );
            return im;
        }

        public async Task<InspectionTypesAndMasters> GetMasterAndTypeAsync(int inspectionTypeId, Guid masterId)
        {
            var response = await _client.GetStringAsync($"{InspectionsControllerBase}/{inspectionTypeId}/{masterId}/typeAndMaster");
            var im = JsonConvert.DeserializeObject<InspectionTypesAndMasters>(response);
            return im;
        }

        public async Task<IEnumerable<InspectionMaster>> GetMastersAsync( Inspection inspection )
        {
            return await _client.GetAsync<IEnumerable<InspectionMaster>>( $"{InspectionsControllerBase}/{inspection.InspectionId}/masters", failedResponse: new List<InspectionMaster>() );
        }

        //public async Task<IEnumerable<InspectionImage>> GetInspectionPicturesAsync(Guid inspectionId)
        //{
        //    return await _db.Table<Picture>>(inspectionId);
        //}

        public async Task<System.Net.Http.HttpResponseMessage> SaveNewInspectionAsync( Inspection inspection )
        {
            // prepare the Inspection object
            inspection.MasterDisplayName = null;

            for(int i = inspection.InspectionItems.Count - 1; i >= 0; i--)
            {
                if(inspection.InspectionItems[i].OptionId == Guid.Empty)
                {
                    inspection.InspectionItems.RemoveAt(i);
                }
                else if (inspection.InspectionItems[i].ItemComments == "")
                {
                    inspection.InspectionItems[i].ItemComments = null;
                }
            }

            // upload the inspection
            var body = JsonConvert.SerializeObject(inspection);

            NPAInspectionWriter.Logging.StaticLogger.InfoLogger(body);

            HttpContent content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _client.PostAsync($"{InspectionsControllerBase}", content);

            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //TODO upload status event?
                //_eventAggregator.GetEvent<InspectionUploadStatusEvent>().Publish( new InspectionUploadStatus( true ) );

                // upload the images
                var imageDir = _afs.ApplicationRoot;
                var images = await _app.GetPicturesById(inspection.InspectionId);
                var imagesAwaitingUpload = images.Where(i => !i.UploadSuccess);
                int ipos = 0;
                bool success = true;

                foreach (var picture in imagesAwaitingUpload)
                {
                    using (MultipartFormDataContent formData = new MultipartFormDataContent())
                    {
                        string filename = picture.Id.TryToString() + ".jpeg";
                        formData.Add(new StringContent(inspection.InspectionId.TryToString()), "InspectionId");

                        StreamContent streamContent = new StreamContent(_afs.GetFileStream(picture.LocalPath));

                        var header = new ContentDispositionHeaderValue("form-data");
                        header.Name = "image";
                        header.FileName = filename;
                        header.Parameters.Add(new NameValueHeaderValue("pictureId", picture.Id.TryToString()));
                        header.Parameters.Add(new NameValueHeaderValue("displayOrder", picture.Position.TryToString()));
                        header.Parameters.Add(new NameValueHeaderValue("width", "1600"));
                        header.Parameters.Add(new NameValueHeaderValue("height", "1200"));

                        streamContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                        streamContent.Headers.ContentDisposition = header;

                        formData.Add(streamContent, "file", filename);

                        response = await _client.PostAsync($"{UploadControllerBase}", formData);
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            //TODO Upload status event?
                            //_eventAggregator.GetEvent<InspectionUploadStatusEvent>().Publish( new InspectionUploadStatus( true, imagesAwaitingUpload.Count(), ipos ) );
                            // update the count, increment the progress bar and set the image "uploaded" value
                            Xamarin.Forms.MessagingCenter.Send<InspectionsService, double>(this, "UploadProgress", ((double)imagesAwaitingUpload.Count() / (double)ipos));
                            picture.UploadSuccess = true;
                            _app.Update(picture);
                        }
                        else
                        {
                            success = false;
                        }
                    }

                    if (success)
                    {
                        // update the inspection to complete
                        LocalInspection li = inspection;

                        li.IsLocalInspection = false;
                        _app.Update(li);
                    }
                    else
                    {
                        LocalInspection li = inspection;
                        li.FailedUpload = true;
                        _app.Update(li);
                    }
                }
            }


            //form.Add(new ByteArrayContent(imagebytearraystring, 0, imagebytearraystring.Count()), "profile_pic", "hello1.jpg");
            //HttpResponseMessage response = await httpClient.PostAsync("PostUrl", form);

            return response;

        }

        public async Task<IEnumerable<string>> ImageUploadSuccessAsync( Inspection inspection, IEnumerable<InspectionImage> images )
        {
            // TODO: Check what on earth the API is really looking for.
            return await _client.GetAsync<IEnumerable<string>>( $"{InspectionsControllerBase}/uploadSuccess/{inspection.InspectionId}/{images.Count()}", new List<string>() );
        }

        public void GetPictureData(ref List<Picture> pics)
        {
            for (int i = 0; i < pics.Count; i++)
            {
                try
                {
                    // _afs.ApplicationRoot + "/" + pics[i].LocalPath
                    pics[i].ImageData = _afs.GetFileData( pics[i].LocalPath );
                }
                catch (Exception ex)
                {
                    NPAInspectionWriter.Logging.StaticLogger.InfoLogger(ex.Message);
                }
            }
        }
    }
}
