using DemoImageSlider.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoImageSlider.ViewModels
{
    public class ImageItemViewModel : ViewModelBase
    {
        public string PathToImageFile
        {
            get;set;
        }

        public ImageItemViewModel(string pathToImageFile)
        {
            PathToImageFile = pathToImageFile;
        }
    }
}
