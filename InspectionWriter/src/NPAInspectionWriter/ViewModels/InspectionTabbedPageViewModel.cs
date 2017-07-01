using System;
using System.Threading.Tasks;
using NPAInspectionWriter.Models;
using Xamarin.Forms;

namespace NPAInspectionWriter.ViewModels
{
    public class InspectionTabbedPageViewModel :CRWriterBase
    {
        LocalInspection _currentInspection;
        public LocalInspection CurrentInspection
		{
			get { return _currentInspection; }
			set 
            { 
                SetProperty(ref (_currentInspection), value); 
            }
		}

		public async Task<int> SaveImage(Picture picture)
		{
            var pics = await AppRepository.Instance.Table<Picture>().Where(x => x.InspectionId == CurrentInspection.InspectionId).OrderBy(x => x.Position).ToListAsync();
			picture.Position = pics.Count;
			AppRepository.Instance.Insert(picture);
			return pics.Count + 1;
		}

		public async Task<int> GetCurrentCount()
		{
            var pics = await AppRepository.Instance.Table<Picture>().Where(x => x.InspectionId == CurrentInspection.InspectionId).OrderBy(x => x.Position).ToListAsync();
			return pics.Count;
		}

        public override string Title => "Default";

        public override ImageSource Icon => "Default";

        public InspectionTabbedPageViewModel()
        {
        }
    }
}
