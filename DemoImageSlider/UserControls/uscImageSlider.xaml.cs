using DemoImageSlider.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DemoImageSlider.UserControls
{
    /// <summary>
    /// Логика взаимодействия для uscImageSlider.xaml
    /// </summary>
    public partial class uscImageSlider : UserControl, INotifyPropertyChanged
    {
        #region Fields
        private const double MARGIN = 4;
        private int _sliderItemsCount = 5;
        private List<ImageItemViewModel> Queue = new List<ImageItemViewModel>();
        private double _itemWidth = 0;
        private double _actualHeight = 0;
        #endregion

        #region Properties
        public ObservableCollection<uscImageSliderItem> SliderItems
        {
            get;set;
        }
        #endregion

        #region DependencyProperties
        public int VisibleItemsCount
        {
            get { return (int)GetValue(VisibleItemsCountProperty); }
            set { SetValue(VisibleItemsCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VisibleItemsCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VisibleItemsCountProperty =
            DependencyProperty.Register("VisibleItemsCount", typeof(int), typeof(uscImageSlider), new PropertyMetadata(3, VisibleItemsCountChanged));

        public IList ImagesList
        {
            get { return (IList)GetValue(ImagesListProperty); }
            set { SetValue(ImagesListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImagesList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImagesListProperty =
            DependencyProperty.Register("ImagesList", typeof(IList), typeof(uscImageSlider), new PropertyMetadata(new List<ImageItemViewModel>(), (d, e) =>
            {
                if (d is uscImageSlider)
                {
                    uscImageSlider slider = d as uscImageSlider;
                    slider.ImagesListChanged(e.NewValue as List<ImageItemViewModel>);
                }
            }));



        public ICommand ItemClickCommand
        {
            get { return (ICommand)GetValue(ItemClickCommandProperty); }
            set { SetValue(ItemClickCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemClickCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemClickCommandProperty =
            DependencyProperty.Register("ItemClickCommand", typeof(ICommand), typeof(uscImageSlider), new PropertyMetadata(null));



        public object ItemClickCommandParameter
        {
            get { return (object)GetValue(ItemClickCommandParameterProperty); }
            set { SetValue(ItemClickCommandParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemClickCommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemClickCommandParameterProperty =
            DependencyProperty.Register("ItemClickCommandParameter", typeof(object), typeof(uscImageSlider), new PropertyMetadata(null));




        #endregion

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public uscImageSlider()
        {            
            InitializeComponent();
            ImagesList = new List<Image>();
            SliderItems = new ObservableCollection<uscImageSliderItem>();
            PropertyChanged(this, new PropertyChangedEventArgs("SliderItems"));
        }

        #region Methods
        private void RecalculateItemsPositions()
        {
            for (int i = 0; i < _sliderItemsCount; i++)
            {
                RecalculateItemPositionByIndex(i);
            }
        }

        private void RecalculateItemPositionByIndex(int index)
        {
            SliderItems[index].Width = _itemWidth;
            double x = CalculateItemLeftPositionForIndex(index);
            Canvas.SetLeft(SliderItems[index], x);            
        }

        private double CalculateItemLeftPositionForIndex(int index)
        {
            return MARGIN * index + ((index - 1) * _itemWidth);
        }
        #endregion

        #region EventHandlers
        private static void VisibleItemsCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is uscImageSlider)
            {
                uscImageSlider slider = d as uscImageSlider;
                slider._sliderItemsCount = (int)(e.NewValue) + 2;
            }
        }

        private void ImagesListChanged(List<ImageItemViewModel> e)
        {
            var imagesList = e;
            if (imagesList == null || imagesList.Count == 0)
                return;
            SliderItems.Add(new uscImageSliderItem() { DataContext = imagesList.Last() });
            for (int i = 0; i < _sliderItemsCount - 1; i++)
            {
                SliderItems.Add(new uscImageSliderItem() { DataContext = imagesList[i] });
            }            

            Queue = new List<ImageItemViewModel>();
            for (int i = _sliderItemsCount - 1; i < imagesList.Count - 1; i++)
            {
                Queue.Add(imagesList[i]);
            }

            var width = itemsControl.ActualWidth;
            if (width == 0)
                return;

            _itemWidth = ((width - MARGIN) / VisibleItemsCount) - MARGIN;

            RecalculateItemsPositions();
        }

        private void usc_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _actualHeight = itemsControl.ActualHeight;

            var width = itemsControl.ActualWidth;
            if (width == 0 || SliderItems == null || SliderItems.Count == 0)
                return;           

            _itemWidth = ((width - MARGIN) / VisibleItemsCount) - MARGIN;
            RecalculateItemsPositions();

            PropertyChanged(this, new PropertyChangedEventArgs("SliderItems"));
        }

        private void btnRight_Click(object sender, RoutedEventArgs e)
        {
            var temp = SliderItems[0];
            SliderItems.RemoveAt(0);
            Queue.Add(temp.DataContext as ImageItemViewModel);
            SliderItems.Add(new uscImageSliderItem() { DataContext = Queue.First() });
            Queue.RemoveAt(0);
            RecalculateItemPositionByIndex(_sliderItemsCount - 1);

            for (int i = 0; i < _sliderItemsCount - 1; i++)
            {
                DoubleAnimation animation = new DoubleAnimation()
                {
                    From = CalculateItemLeftPositionForIndex(i + 1),
                    To = CalculateItemLeftPositionForIndex(i),
                    Duration = new Duration(TimeSpan.FromMilliseconds(500))
                };

                SliderItems[i].BeginAnimation(Canvas.LeftProperty, animation);
            }

        }

        private void btnLeft_Click(object sender, RoutedEventArgs e)
        {
            var temp = SliderItems[_sliderItemsCount - 1];
            SliderItems.RemoveAt(_sliderItemsCount - 1);
            Queue.Insert(0, temp.DataContext as ImageItemViewModel);
            SliderItems.Insert(0, new uscImageSliderItem() { DataContext = Queue.Last() });
            Queue.RemoveAt(Queue.Count - 1);
            RecalculateItemPositionByIndex(0);

            for (int i = 1; i < _sliderItemsCount; i++)
            {
                DoubleAnimation animation = new DoubleAnimation()
                {
                    From = CalculateItemLeftPositionForIndex(i - 1),
                    To = CalculateItemLeftPositionForIndex(i),
                    Duration = new Duration(TimeSpan.FromMilliseconds(500))
                };

                SliderItems[i].BeginAnimation(Canvas.LeftProperty, animation);
            }
        }
        #endregion
    }
}
