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
using System.Windows.Shapes;

namespace Candbox
{
    /// <summary>
    /// Логика взаимодействия для ModalWindowNicknameInput.xaml
    /// </summary>
    public partial class ModalWindowNicknameInput : Window
    {
        public ModalWindowNicknameInput()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public string Nickname
        {
            get { return TextboxNicknameInput.Text; }
        }
    }
}
