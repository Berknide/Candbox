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
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        User user = null;
        public SettingsWindow(User _user)
        {
            InitializeComponent();

            user = _user;
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(user);
            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();
            this.Close();
        }

        private void ButtonBack_MouseMove(object sender, MouseEventArgs e)
        {
            ButtonBack.Background = new SolidColorBrush(Color.FromArgb(60, 156, 149, 237));
        }

        private void ButtonBack_MouseLeave(object sender, MouseEventArgs e)
        {
            ButtonBack.Background = new SolidColorBrush(Color.FromArgb(60, 194, 223, 245));
        }

        private void ButtonTheme_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonTheme_MouseMove(object sender, MouseEventArgs e)
        {
            ButtonTheme.Background = new SolidColorBrush(Color.FromArgb(60, 156, 149, 237));
        }

        private void ButtonTheme_MouseLeave(object sender, MouseEventArgs e)
        {
            ButtonTheme.Background = new SolidColorBrush(Color.FromArgb(60, 194, 223, 245));
        }

        private void ButtonFont_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonFont_MouseMove(object sender, MouseEventArgs e)
        {
            ButtonFont.Background = new SolidColorBrush(Color.FromArgb(60, 156, 149, 237));
        }

        private void ButtonFont_MouseLeave(object sender, MouseEventArgs e)
        {
            ButtonFont.Background = new SolidColorBrush(Color.FromArgb(60, 194, 223, 245));
        }

        private void ButtonLanguage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonLanguage_MouseMove(object sender, MouseEventArgs e)
        {
            ButtonLanguage.Background = new SolidColorBrush(Color.FromArgb(60, 156, 149, 237));
        }

        private void ButtonLanguage_MouseLeave(object sender, MouseEventArgs e)
        {
            ButtonLanguage.Background = new SolidColorBrush(Color.FromArgb(60, 194, 223, 245));
        }

        private void ButtonReset_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите сбросить прогресс?", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                user.Progress = 1;
                DatabaseProcessor dbp = new DatabaseProcessor();
                bool success = dbp.Write(user.Name, user.Progress);
                if (success)
                {
                    MessageBox.Show("Ваш прогресс был сброшен.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void ButtonReset_MouseMove(object sender, MouseEventArgs e)
        {
            ButtonReset.Background = new SolidColorBrush(Color.FromArgb(60, 156, 149, 237));
        }

        private void ButtonReset_MouseLeave(object sender, MouseEventArgs e)
        {
            ButtonReset.Background = new SolidColorBrush(Color.FromArgb(60, 194, 223, 245));
        }

        private void ButtonLogout_Click(object sender, RoutedEventArgs e)
        {
            user = null;
            MessageBox.Show("Вы вышли из системы.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ButtonLogout_MouseMove(object sender, MouseEventArgs e)
        {
            ButtonLogout.Background = new SolidColorBrush(Color.FromArgb(60, 156, 149, 237));
        }

        private void ButtonLogout_MouseLeave(object sender, MouseEventArgs e)
        {
            ButtonLogout.Background = new SolidColorBrush(Color.FromArgb(60, 194, 223, 245));
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ButtonExit_MouseMove(object sender, MouseEventArgs e)
        {
            ButtonExit.Background = new SolidColorBrush(Color.FromArgb(60, 156, 149, 237));
        }

        private void ButtonExit_MouseLeave(object sender, MouseEventArgs e)
        {
            ButtonExit.Background = new SolidColorBrush(Color.FromArgb(60, 194, 223, 245));
        }
    }
}
