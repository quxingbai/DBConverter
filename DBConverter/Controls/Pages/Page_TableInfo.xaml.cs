using DBConverter.Base.Model;
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

namespace DBConverter.Controls.Pages
{
    /// <summary>
    /// Page_TableInfo.xaml 的交互逻辑
    /// </summary>
    public partial class Page_TableInfo : UserControl
    {


        public TableBasicModel Source
        {
            get { return (TableBasicModel)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(TableBasicModel), typeof(Page_TableInfo), new PropertyMetadata(null));


        public Page_TableInfo(TableBasicModel tb)
        {
            InitializeComponent();
            this.Source = tb;
            this.LIST_Columns.ItemsSource = Source.Columns;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
        }
    }
}
