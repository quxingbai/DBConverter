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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DBConverter.Controls
{
    public class TreeNodeItem : ListBoxItem
    {


        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(ImageSource), typeof(TreeNodeItem), new PropertyMetadata(null));



        public TreeNodeItem Children
        {
            get { return (TreeNodeItem)GetValue(ChildrenProperty); }
            set { SetValue(ChildrenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Children.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChildrenProperty =
            DependencyProperty.Register("Children", typeof(TreeNodeItem), typeof(TreeNodeItem), new PropertyMetadata(null));



        public int Level
        {
            get { return (int)GetValue(LevelProperty); }
            set { SetValue(LevelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Level.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LevelProperty =
            DependencyProperty.Register("Level", typeof(int), typeof(TreeNodeItem), new PropertyMetadata(0));




        public Thickness LevelMargin
        {
            get { return (Thickness)GetValue(LevelMarginProperty); }
            set { SetValue(LevelMarginProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LevelMargin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LevelMarginProperty =
            DependencyProperty.Register("LevelMargin", typeof(Thickness), typeof(TreeNodeItem), new PropertyMetadata(null));




        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsOpen.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(TreeNodeItem), new PropertyMetadata(false));



        static TreeNodeItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeNodeItem), new FrameworkPropertyMetadata(typeof(TreeNodeItem)));
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            
            this.LevelMargin = new Thickness(Level*5,1,0,0);
            base.OnPropertyChanged(e);
        }
    }
}
