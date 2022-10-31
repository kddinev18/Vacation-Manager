using MahApps.Metro.IconPacks;
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

namespace Vacation_Manager.View.Code_behind.UserControls
{
    /// <summary>
    /// Interaction logic for TextBoxControl.xaml
    /// </summary>
    public partial class TextBoxControl : UserControl
    {
        public TextBoxControl()
        {
            InitializeComponent();
        }

        public PackIconMaterialKind TextBoxIcon
        {
            get { return (PackIconMaterialKind)GetValue(TextBoxIconProperty); }
            set { SetValue(TextBoxIconProperty, value); }
        }

        public static readonly DependencyProperty TextBoxIconProperty = DependencyProperty.Register("TextBoxIcon", typeof(PackIconMaterialKind), typeof(TextBoxControl));

        public string TextBoxHintText
        {
            get { return (string)GetValue(TextBoxHintTextProperty); }
            set { SetValue(TextBoxHintTextProperty, value); }
        }

        public static readonly DependencyProperty TextBoxHintTextProperty = DependencyProperty.Register("TextBoxHintText", typeof(string), typeof(TextBoxControl));
    }
}
