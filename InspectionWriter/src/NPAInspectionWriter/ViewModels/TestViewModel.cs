using System;
using System.Collections.Generic;

namespace NPAInspectionWriter.ViewModels
{
    public class TestViewModel : BaseViewModel
    {
        public List<string> Items
        {
            get;
            set;
        }
        public TestViewModel()
        {
            Items = new List<string>() { "A", "B", "C" };
            for (int i = 0; i < 20; i++)
            {
                Items.Add("https://upload.wikimedia.org/wikipedia/en/thumb/4/41/Flag_of_India.svg/1200px-Flag_of_India.svg.png");

            }
        }
    }
}
