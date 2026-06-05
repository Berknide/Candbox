using System;
using System.IO;
using System.Windows;

namespace Candbox
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DatabaseProcessor dbp = new DatabaseProcessor();
        User user = null;
        private int lang = 1;
        int progress = 1;
        public MainWindow()
        {
            InitializeComponent();
            AddToDatabase();
        }

        public MainWindow(User _user)
        {
            InitializeComponent();

            user = _user;
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonSettings_Click(object sender, RoutedEventArgs e)
        {
            bool logined = false;
            if (user == null)
            {
                logined = OpenModalNicknameInput();
            }
            else
            {
                logined = true;
            }
            if (!logined)
            {
                MessageBox.Show("Вы зашли как гость.");
                user = new User
                {
                    Name = "Гость",
                    Progress = 1,
                    Language = 1,
                };
            }
            SettingsWindow settings = new SettingsWindow(user);
            Application.Current.MainWindow = settings;
            settings.Show();
            this.Close();
        }

        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            if (OpenModalNicknameInput())
            {
                ShowLevels(user.Name, progress, lang);
            }
        }

        private bool OpenModalNicknameInput()
        {
            ModalWindowNicknameInput nicknameInput = new ModalWindowNicknameInput();
            if (nicknameInput.ShowDialog() == true)
            {
                if (nicknameInput.Nickname == String.Empty || nicknameInput.Nickname == null)
                {
                    MessageBox.Show("Введите своё имя!");
                }
                else
                {
                    user = dbp.Read(nicknameInput.Nickname);

                    if (user != null)
                    {
                        progress = user.Progress;
                        lang = user.Language;

                        MessageBox.Show($"Вы зашли под именем {user.Name}");
                        return true;
                    }
                    else
                    {
                        bool result = dbp.WriteNew(nicknameInput.Nickname, lang);
                        if (result)
                        {
                            MessageBox.Show($"Вы зарегистрированы под именем {nicknameInput.Nickname}");
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("Не удалось авторизировать пользователя.", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Warning);
                            FileProcessor fp = new FileProcessor();
                            fp.Write(nicknameInput.Nickname + ",1");
                        }
                    }
                }
            }
            return false;
        }

        private async void AddToDatabase()
        {
            string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "players.csv");
            FileInfo fi = new FileInfo(filePath);
            if (fi.Exists)
            {
                FileProcessor fp = new FileProcessor();
                string data = await fp.Read();
                string[] strings = data.Split(new char[] { ',' });
                string name = strings[0];
                if (!string.IsNullOrEmpty(name))
                {
                    dbp.WriteNew(name, lang);
                }

                fp.Delete();
            }
        }

        private void ShowLevels(string name, int progress, int language)
        {
            LevelsWindow levels = new LevelsWindow(name, progress, language);
            Application.Current.MainWindow = levels;
            levels.Show();
            this.Close();
        }
    }
}
