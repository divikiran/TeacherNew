using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace NPAInspectionWriter.DTO
{
    /// <summary>
    /// BaseDTO will contain the basic request and response properties so that
    /// all DTO subclasses can be used as requests and responses.
    /// </summary>
    public abstract class BaseDTO : INotifyPropertyChanged
    {

        // Request properties.
        public bool HasChanged { get { return ( HasChangedPropertyCollection.Count > 0 ); } }
        public struct Values { public object Orig; public object New; }
        public Dictionary<string, Values> HasChangedPropertyCollection = new Dictionary<string, Values>();
        public string AuthenticationToken { get; set; }
        public Dictionary<string, string> Parameters { get; set; }

        // Helper properties.
        public BaseDTO ParentDTO { get; set; }
        public string TableName { get; set; }
        public string PrimaryKey { get; set; }

        public virtual bool IsNewEntity { get; set; }
        public bool UseTransaction { get; set; }
        //If an objects needs to be linked to it's parent, set the SetChildParentLink property to true
        public bool SetChildParentLink { get; set; }

        // Response properties.
        public string ErrorMessageHeader { get; set; }
        public string ErrorMessage { get; set; }
        public PagingInfoDTO Paging { get; set; }

        // Common/Popular properties.
        public Guid? OnlineVendorID { get; set; }

        public BaseDTO()
        {
            AuthenticationToken = string.Empty;
            Parameters = null;

            ErrorMessageHeader = string.Empty;
            ErrorMessage = string.Empty;
        }

        // Impliment Interface
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged( string propertyName )
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if( handler != null ) handler( this, new PropertyChangedEventArgs( propertyName ) );
        }

        protected bool SetField<T>( ref T field, T value, Expression<Func<T>> selectorExpression )
        {
            if( EqualityComparer<T>.Default.Equals( field, value ) )
            {
                return false;
            }
            else
            {
                if( selectorExpression == null )
                    throw new ArgumentNullException( "selectorExpression" );
                MemberExpression body = selectorExpression.Body as MemberExpression;
                if( body == null )
                    throw new ArgumentException( "The body must be a member expression" );

                // log chanaged properties with dictionary
                if( !HasChangedPropertyCollection.ContainsKey( body.Member.Name ) )
                    HasChangedPropertyCollection.Add( body.Member.Name, new Values { Orig = field, New = value } );
                else
                {
                    Values prop = HasChangedPropertyCollection.FirstOrDefault( v => v.Key.Contains( body.Member.Name ) ).Value;
                    var orig = ( prop.Orig != null ) ? prop.Orig : field;
                    HasChangedPropertyCollection[ body.Member.Name ] = new Values { Orig = orig, New = value };
                }
                field = value;
                OnPropertyChanged( body.Member.Name );
            }

            return true;
        }

        //public void Save()
        //{
        //    using( DTOHelper helper = new DTOHelper() )
        //    {
        //        helper.Save( this );
        //    }
        //}

        //public static BaseDTO CreateOriginal( BaseDTO dto )
        //{
        //    BaseDTO ret;

        //    ret = ( BaseDTO )Activator.CreateInstance( dto.GetType() );

        //    Dictionary<string, BaseDTO.Values> list = dto.HasChangedPropertyCollection;
        //    Type t = ret.GetType();

        //    IEnumerable<PropertyInfo> props = t.GetRuntimeProperties();

        //    foreach( var prop in props )
        //    {
        //        if( list.ContainsKey( prop.Name ) )
        //            t.GetRuntimeProperty( prop.Name ).SetValue( ret, list[ prop.Name ].Orig, null );
        //        else
        //        {
        //            if( t.GetRuntimeProperty( prop.Name ).GetSetMethod != null )
        //                t.GetRuntimeProperty( prop.Name ).SetValue( ret, t.GetRuntimeProperty( prop.Name ).GetValue( dto, null ), null );
        //        }
        //    }

        //    return ret;
        //}

    }
}
