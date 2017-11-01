using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для uscImageView.xaml
    /// </summary>
    public partial class uscImageView : UserControl
    {
        #region DependencyProperties
        public string CurrentImagePathToFile
        {
            get { return (string)GetValue(CurrentImagePathToFileProperty); }
            set { SetValue(CurrentImagePathToFileProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentImagePath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentImagePathToFileProperty =
            DependencyProperty.Register("CurrentImagePathToFile", typeof(string), typeof(uscImageView), new PropertyMetadata(null));

        public string NewImagePathToFile
        {
            get { return (string)GetValue(NewImagePathToFileProperty); }
            set { SetValue(NewImagePathToFileProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NewImagePath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NewImagePathToFileProperty =
            DependencyProperty.Register("NewImagePathToFile", typeof(string), typeof(uscImageView), new PropertyMetadata(null));

        public string NewImagePath
        {
            get { return (string)GetValue(NewImagePathProperty); }
            set { SetValue(NewImagePathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NewImagePath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NewImagePathProperty =
            DependencyProperty.Register("NewImagePath", typeof(string), typeof(uscImageView), new PropertyMetadata(null, (d, e) =>
            {
                if (d is uscImageView)
                {
                    if (e.NewValue != e.OldValue)
                    {
                        uscImageView imageView = d as uscImageView;
                        imageView.NewImagePathChanged(e.NewValue);
                    }
                }
            }));
        #endregion

        public uscImageView()
        {
            InitializeComponent();
        }        

        private void NewImagePathChanged(object newValue)
        {
            Storyboard fadeIn = this.Resources["FadeIn"] as Storyboard;
            Storyboard fadeOut = (this.Resources["FadeOut"] as Storyboard).Clone();

            CurrentImagePathToFile = NewImagePathToFile;
            NewImagePathToFile = newValue as string;

            fadeIn.Begin(imgNew);
            fadeOut.Begin(imgCurrent);

        }
    }
}
