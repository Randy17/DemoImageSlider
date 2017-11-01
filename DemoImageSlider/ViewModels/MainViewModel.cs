using DemoImageSlider.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoImageSlider.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _newIamgePath;

        public List<ImageItemViewModel> Images
        {
            get; set;
        }

        public string NewImagePath
        {
            get
            {
                return _newIamgePath;
            }
            set
            {
                _newIamgePath = value;
                RaisePropertyChanged("NewImagePath");
            }
        }

        public Command UpdateCommand
        {
            get
            {
                return new Command(Update);
            }
        }       

        public MainViewModel()
        {
            Images = new List<ImageItemViewModel>();
            Images.Add(new ImageItemViewModel("/Images/1.jpg"));
            Images.Add(new ImageItemViewModel("/Images/2.jpg"));
            Images.Add(new ImageItemViewModel("/Images/3.jpg"));
            Images.Add(new ImageItemViewModel("/Images/4.jpg"));
            Images.Add(new ImageItemViewModel("/Images/5.jpg"));
            Images.Add(new ImageItemViewModel("/Images/6.jpg"));
            Images.Add(new ImageItemViewModel("/Images/7.jpg"));
            Images.Add(new ImageItemViewModel("/Images/8.jpg"));

            NewImagePath = Images.First().PathToImageFile;

        }

        private void Update(object obj)
        {
            ImageItemViewModel vm = obj as ImageItemViewModel;
            NewImagePath = vm.PathToImageFile;
        }
    }
}
