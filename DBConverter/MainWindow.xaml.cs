using DBConverter.Base.AbsDBEntrys;
using DBConverter.Base.Help;
using DBConverter.Base.Model;
using DBConverter.Controls;
using DBConverter.Controls.Pages;
using DBConverter.DBEntrys;
using DBConverter.Forms;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DBConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const String MySqlDefDBName = "mysql";
        public const String SqlServerDefDBName = "master";

        public ObservableCollection<DataBaseBasicModel> DataBaseList = new ObservableCollection<DataBaseBasicModel>();//显示数据库的源
        //public ObservableCollection<TableBasicModel> TableList = new ObservableCollection<TableBasicModel>();//显示所有表的源
        public ObservableCollection<dynamic> DataBaseInfoList = new ObservableCollection<dynamic>();//显示的当前选择数据库的内容列表
        public Dictionary<string, List<TableBasicModel>> TablesTemp = new Dictionary<string, List<TableBasicModel>>();
        public Dictionary<string, List<ProcedureBasicModel>> ProceduresTemp = new Dictionary<string, List<ProcedureBasicModel>>();
        public Dictionary<string, List<FunctionBasicModel>> FunctionsTemp = new Dictionary<string, List<FunctionBasicModel>>();
        public Dictionary<string, DataBaseBasicModel> DataBaseTemp = new Dictionary<string, DataBaseBasicModel>();
        //public DataBaseBasicModel DataBaseTemp { get; set; }
        private AbsDBEntry DBEntry = null;
        public enum DBInfoType
        {
            Table,
            Procedure,
            Function
        }
        private DataBaseTypeEnum DataBaseType { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            this.LIST_DBList.ItemsSource = DataBaseList;
            this.LIST_DBInfoList.ItemsSource = DataBaseInfoList;
            ShowLoginDb();
        }

        private void BT_HeadButtons_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as IconButton).Tag.ToString())
            {
                case "RELOGIN": ShowLoginDb(); break;
            }
        }
        //显示登陆界面
        public void ShowLoginDb()
        {
            var loginForm = new LoginForm();
            loginForm.Logined += LoginForm_Logined;
            loginForm.ShowDialog();
        }
        /// <summary>
        /// 登陆界面进行登录
        /// </summary>
        private void LoginForm_Logined(string Address, int Port, string Pwd, string UName, DataBaseTypeEnum SelectedDBType)
        {
            //默认数据库名称 第一次进时所用
            string defDBName = "";
            if (DBEntry != null)
            {
                DBEntry.Used -= DBEntry_Used;
                DBEntry.Dispose();
            }
            //mysql
            if (SelectedDBType == DataBaseTypeEnum.MySql)
            {
                defDBName = MySqlDefDBName;
                DBEntry = new MySqlEntry(Address, Port, UName, Pwd, defDBName);
            }
            //sql server
            else if (SelectedDBType == DataBaseTypeEnum.SqlServer)
            {
                defDBName = SqlServerDefDBName;
            }
            DBEntry.Used += DBEntry_Used;
            DBEntry.Use(defDBName);
            this.DataBaseType = SelectedDBType;
        }

        /// <summary>
        /// 切换数据库
        /// </summary>
        private void DBEntry_Used(object arg1, DataBaseBasicModel arg2)
        {
            DataBaseList.Clear();
            foreach (var item in DBEntry.QueryAllDataBase())
            {
                DataBaseList.Add(item);
            }
        }

        public void AddTextPage(String Title,String Text)
        {
            if (!CanAddPage(Title))
            {
                return;
            }
            this.TC_Pages.Items.Add(new TabItem()
            {
                Header = Title,
                Content = new TextBox()
                {
                    Text = Text,
                    Foreground = Brushes.White,
                    Background = Brushes.Transparent,
                    BorderThickness = new Thickness(0)
                }
            }) ;
            TC_Pages.SelectedIndex = TC_Pages.Items.Count - 1;
        }
        public void AddTablePage(TableBasicModel Table)
        {
            if (!CanAddPage(Table.Name))
            {
                return;
            }
            this.TC_Pages.Items.Add(new TabItem() { 
                Header=Table.Name,
                Content= new Page_TableInfo(Table)
            });
            TC_Pages.SelectedIndex = TC_Pages.Items.Count - 1;
        }
        public bool CanAddPage(String Title)
        {
            foreach(TabItem item in TC_Pages.Items)
            {
                if (item.Header.ToString() == Title)
                {
                    return false;
                }
            }
            return true;
        }

        private void LIST_DBList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (DataBaseBasicModel m in e.AddedItems)
            {
                //如果有缓存这个数据库的表就给它加进去
                if (TablesTemp.ContainsKey(m.Name))
                {
                    m.Tables = TablesTemp[m.Name];
                }
                else
                {
                    m.Tables = DBEntry.QueryAllTables(m.Name);
                    m.Procedures = DBEntry.QueryAllProcedures(m.Name);
                    m.Functions = DBEntry.QueryAllFunctions(m.Name);
                    this.TablesTemp.Add(m.Name, m.Tables);
                }
                m.Tables?.ForEach(t =>
                {
                    //TableList.Add(t);
                    DataBaseInfoList.Add(ToDBInfoItem(t.Name,m.Name, DBInfoType.Table));
                });
                m.Procedures.ForEach(p => AddDBInfoItem(p.Name, m.Name, DBInfoType.Procedure));
                m.Functions.ForEach(f => AddDBInfoItem(f.Name, m.Name, DBInfoType.Function));
            }

            foreach (DataBaseBasicModel m in e.RemovedItems)
            {
                //m.Tables?.ForEach(t =>
                //{
                //    TableList.Remove(t);
                //});
                //for(int f = 0; f < DataBaseInfoList.Count; f++)
                //{
                //    if (DataBaseInfoList[f].DataBase == m.Name)
                //    {
                //        DataBaseInfoList.Remove();
                //    }
                //}

                foreach(var i in DataBaseInfoList.Where(l => l.DataBase == m.Name).ToList())
                {
                    DataBaseInfoList.Remove(i);
                }

            }
        }
        public void LogError(String Text)
        {
            this.LIST_Log.Items.Add(new
            {
                Background="Red",
                Foreground="White",
                Text=Text
            });
        }
        public void LogMsg(String Text)
        {
            this.LIST_Log.Items.Add(new
            {
                Background = "",
                Foreground = "White",
                Text = Text
            });

        }
        /// <summary>
        /// 转换为在数据库详细列表中显示的项
        /// </summary>
        public dynamic ToDBInfoItem(String Name,String DBName,DBInfoType Type)
        {
            return new
            {
                Name=Name,
                Type=Type,
                DataBase=DBName,
            };
        }
        /// <summary>
        /// 添加一个数据库详细项
        /// </summary>
        public void AddDBInfoItem(String Name, String DBName, DBInfoType Type)
        {
            this.DataBaseInfoList.Add(ToDBInfoItem(Name, DBName, Type));
        }
        /// <summary>
        /// 根据当前数据库获取一个新数据库入口
        /// </summary>
        private AbsDBEntry GetNewDefDB()
        {
            switch (DataBaseType)
            {
                case DataBaseTypeEnum.MySql:
                    return new MySqlEntry(DBEntry.Server, DBEntry.Port, DBEntry.UserName, DBEntry.UserPassword, MySqlDefDBName);
                case DataBaseTypeEnum.SqlServer:; break;
            }
            throw new Exception("获取失败，未声明此类型");
        }
        private void MENUITEM_DBList_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as MenuItem).Tag.ToString())
            {
                case "TOSQLSERVER":
                    StringBuilder sqlb = new StringBuilder();
                    AbsDBEntry dbentry = GetNewDefDB();
                    foreach (DataBaseBasicModel db in LIST_DBList.SelectedItems)
                    {
                        dbentry.Use(db.Name);
                        sqlb.AppendLine(DBConvert.ToSQL_MySqlToSqlServer(dbentry.ToModel()));
                    }
                    dbentry.Dispose();
                    AddTextPage("SqlServer格式语句", sqlb.ToString());
                    sqlb.Clear();
                    ; break;
            }
        }

        private void LIST_DBInfoListItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //dynamic item = ((LIST_DBInfoList.SelectedItem as Control).DataContext);
            dynamic item = LIST_DBInfoList.SelectedItem as dynamic;
            switch (((DBInfoType)item.Type))
            {
                case DBInfoType.Table:
                    AddTablePage(DBEntry.QueryTableInfo(item.Name, new DataBaseBasicModel() { Name = item.DataBase }));
                    ;break;
                case DBInfoType.Function:
                    FunctionBasicModel m =DBEntry.QueryFunctionInfo(item.DataBase, item.Name);
                    AddTextPage(m.Name, m.Content);
                    ;break;
                case DBInfoType.Procedure:
                    ProcedureBasicModel mq =DBEntry.QueryProcedureInfo(item.DataBase, item.Name);
                    AddTextPage(mq.Name, mq.Content);
                    ; break;
            }
        }

    }
}
