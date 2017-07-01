using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SQLite.Net.Attributes;

namespace NPAInspectionWriter.Models
{
    public class Picture : INotifyPropertyChanged
    {
        private Guid _id;
        [PrimaryKey]
        public Guid Id
        {
            get { return _id; }
            set { SetProperty( ref _id, value ); }
        }

        public Guid VehicleId { get; set; }

        public Guid InspectionId { get; set; }

        public int Position { get; set; }

        private string _baseUrl;
        public string BaseUrl
        {
            get { return _baseUrl; }
            set { SetProperty( ref _baseUrl, value ); }
        }

        private string _localPath;
        public string LocalPath
        {
            get { return _localPath; }
            set { SetProperty( ref _localPath, value ); }
        }

        private bool _localImage;
        public bool LocalImage
        {
            get { return _localImage; }
            set { SetProperty( ref _localImage, value ); }
        }

        public bool IsLandscape { get; set; }

        [Ignore]
        public bool UploadSuccess { get; set; } = false;

        private byte[] _imageData;
        [Ignore]
        public byte[] ImageData
        {
            get { return _imageData; }
            set { SetProperty( ref _imageData, value ); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private bool SetProperty<T>( ref T storage, T value, [CallerMemberName] string propertyName = null )
        {
            if( object.Equals( storage, value ) ) return false;

            storage = value;
            this.OnPropertyChanged( propertyName );

            return true;
        }

        private void OnPropertyChanged( [CallerMemberName]string propertyName = null )
        {
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
        }
    }
}
