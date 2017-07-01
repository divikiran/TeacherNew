using System;
using System.Collections.Generic;
using NPAInspectionWriter.Models;
using Xamarin.Forms;

namespace NPAInspectionWriter.Views.Layouts
{
    public class BaseLocalInspectionItemRow : ViewCell
    {
        public static readonly BindableProperty InspectionIdProperty =
           BindableProperty.Create(nameof(InspectionId),
                                   typeof(Guid),
                                   typeof(DefaultLocalInspectionItemRow),
                                   Guid.Empty);
        
		public static readonly BindableProperty VehicleIdProperty =
		   BindableProperty.Create(nameof(VehicleId),
								   typeof(Guid),
								   typeof(DefaultLocalInspectionItemRow),
								   defaultValue: Guid.Empty);

		public static readonly BindableProperty InspectionTypeProperty =
			BindableProperty.Create(nameof(InspectionType),
									typeof(string),
									typeof(DefaultLocalInspectionItemRow),
									defaultValue: "Unknown Inspection Type");

		public static readonly BindableProperty InspectionDateProperty =
			BindableProperty.Create(nameof(InspectionDate),
									typeof(DateTime),
									typeof(DefaultLocalInspectionItemRow),
									defaultValue: DateTime.Now.ToLocalTime());

		public static readonly BindableProperty ScoreProperty =
			BindableProperty.Create(nameof(Score),
									typeof(int?),
									typeof(DefaultLocalInspectionItemRow),
									defaultValue: null);

		public static readonly BindableProperty PhotoCountProperty =
			BindableProperty.Create(nameof(PhotoCount),
									typeof(int?),
									typeof(DefaultLocalInspectionItemRow),
									defaultValue: null);

		public static readonly BindableProperty StockNumberProperty =
			BindableProperty.Create(nameof(StockNumber),
									typeof(string),
									typeof(DefaultLocalInspectionItemRow),
									defaultValue: string.Empty);

		public static readonly BindableProperty NewVehicleModelIdProperty =
			BindableProperty.Create(nameof(NewVehicleModelId),
									typeof(string),
									typeof(DefaultLocalInspectionItemRow),
									defaultValue: string.Empty);

		public static readonly BindableProperty IncompleteProperty =
			BindableProperty.Create(nameof(Incomplete),
									typeof(bool),
									typeof(DefaultLocalInspectionItemRow),
									defaultValue: true);

		public static readonly BindableProperty PicturesProperty =
			BindableProperty.Create(nameof(Pictures),
									typeof(List<Picture>),
									typeof(DefaultLocalInspectionItemRow),
									defaultValue: null);

		public static readonly BindableProperty YearMakeProperty =
			BindableProperty.Create(nameof(YearMake),
									typeof(string),
									typeof(DefaultLocalInspectionItemRow),
									defaultValue: string.Empty);

		public static readonly BindableProperty ModelProperty =
			BindableProperty.Create(nameof(Model),
									typeof(string),
									typeof(DefaultLocalInspectionItemRow),
									defaultValue: string.Empty);

		public Guid VehicleId
		{
			get { return (Guid)GetValue(VehicleIdProperty); }
			set { SetValue(VehicleIdProperty, value); }
		}

		public Guid InspectionId
		{
            get { return (Guid)GetValue(InspectionIdProperty); }
            set { SetValue(InspectionIdProperty, value); }
		}

		public string InspectionType
		{
			get { return (string)GetValue(InspectionTypeProperty); }
			set { SetValue(InspectionTypeProperty, value); }
		}

		public DateTime InspectionDate
		{
			get { return (DateTime)GetValue(InspectionDateProperty); }
			set { SetValue(InspectionDateProperty, value); }
		}

		public int? Score
		{
			get { return (int?)GetValue(ScoreProperty); }
			set { SetValue(ScoreProperty, value); }
		}

		public List<Picture> Pictures
		{
			get { return (List<Picture>)GetValue(PicturesProperty); }
			set
			{
				SetValue(PicturesProperty, value);
				SetValue(PhotoCountProperty, Pictures?.Count ?? 0);
			}
		}

		public int? PhotoCount
		{
			get { return (int?)GetValue(PhotoCountProperty); }
			set { SetValue(PhotoCountProperty, value); }
		}

		public string StockNumber
		{
			get { return (string)GetValue(StockNumberProperty); }
			set { SetValue(StockNumberProperty, value); }
		}

		public string NewVehicleModelId
		{
			get { return (string)GetValue(NewVehicleModelIdProperty); }
			set { SetValue(NewVehicleModelIdProperty, value); }
		}

		public bool Incomplete
		{
			get { return (bool)GetValue(IncompleteProperty); }
			set { SetValue(IncompleteProperty, value); }
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

		}

        public object CommandParameter { get; set; }

		public string YearMake
		{
			get { return (string)GetValue(YearMakeProperty); }
			set { SetValue(YearMakeProperty, value); }
		}

		public string Model
		{
			get { return (string)GetValue(ModelProperty); }
			set { SetValue(ModelProperty, value); }
		}
    }
}

