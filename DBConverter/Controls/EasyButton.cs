using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DBConverter.Controls
{
    public class EasyButton : ButtonBase
    {
        static EasyButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EasyButton), new FrameworkPropertyMetadata(typeof(EasyButton)));
        }
    }
}
