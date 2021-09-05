using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace DBConverter.Forms
{
    public delegate void LoginDatas(string Address,int Port,String Pwd,String UName, DataBaseTypeEnum SelectedDBType);
    public enum DataBaseTypeEnum
    {
        MySql=1,
        SqlServer=2
    }
    public partial class LoginForm : Window
    {



        public Visibility PortTextVis
        {
            get { return (Visibility)GetValue(PortTextVisProperty); }
            set { SetValue(PortTextVisProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PortTextVis.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PortTextVisProperty =
            DependencyProperty.Register("PortTextVis", typeof(Visibility), typeof(LoginForm), new PropertyMetadata(Visibility.Visible));






        public event LoginDatas Logined;

        public static ObservableCollection<KeyValuePair<int, string>> DataBaseList = new ObservableCollection<KeyValuePair<int, string>>()
        {
        };

        static LoginForm()
        {
            //加载数据库列表集合
            foreach(var p in typeof(DataBaseTypeEnum).GetEnumNames())
            {
                int val = (int)Enum.Parse<DataBaseTypeEnum>(p);
                DataBaseList.Add(new(val, p+" 数据库"));
            }
        }
        public LoginForm()
        {
            InitializeComponent();
            this.TEXT_Address.Text = "localhost";
            this.TEXT_Port.Text = "3306";
            this.TEXT_Pwd.Text = "quxingbai";
            this.TEXT_UName.Text = "root";

            CB_DataBaseType.ItemsSource = DataBaseList;

        }
        private void BT_Login_Click(object sender, RoutedEventArgs e)
        {
            DataBaseTypeEnum sd = (DataBaseTypeEnum)Convert.ToInt32(CB_DataBaseType.SelectedValue);
            if (sd == DataBaseTypeEnum.SqlServer)
            {
                MessageBox.Show("尚未完成...");
                return;
            }

            Logined.Invoke(TEXT_Address.Text, int.Parse(TEXT_Port.Text), TEXT_Pwd.Text, TEXT_UName.Text,sd);
            Close();
        }

        private void BT_Quit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CB_DataBaseType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataBaseTypeEnum sd = (DataBaseTypeEnum)Convert.ToInt32(CB_DataBaseType.SelectedValue);
            bool hasPort = false;
            switch (sd)
            {
                case DataBaseTypeEnum.MySql:
                    hasPort = true;
                    ; break;
                case DataBaseTypeEnum.SqlServer:; break;
            }

            this.LB_PortTitle.Visibility = hasPort? Visibility.Visible: Visibility.Collapsed;
            this.TEXT_Port.Visibility = hasPort? Visibility.Visible: Visibility.Collapsed;

        }
    }
}
