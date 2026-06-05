using System;
using System.Linq;
using System.Windows;

namespace Candbox
{
    /// <summary>
    /// Логика взаимодействия для LevelsWindow.xaml
    /// </summary>
    public partial class LevelsWindow : Window
    {
        DatabaseProcessor dbp = new DatabaseProcessor();
        Level level = null;
        User user = new User();
        int maxProgress = 8;
        public LevelsWindow(string _username, int _progress, int _language)
        {
            InitializeComponent();

            user.Name = _username;
            user.Progress = _progress;
            user.Language = _language;
            FillWithDefaultCode();
            InitializeLevel(user.Progress);
        }

        public void FillWithDefaultCode()
        {
            string code = "using System;\n\nnamespace DynamicNamespace\n{\n\tpublic class DynamicClass\n\t{\n\t\tpublic object Main()\n\t\t{\n\t\t\treturn 0;\n\t\t}\n\t}\n}";
            if (TextEditorCode.Text != code)
            {
                TextEditorCode.Text = code;
            }
        }

        private void InitializeLevel(int progress)
        {
            level = dbp.GetLevel(progress);

            TextBlockLevelName.Text = level.Name;
            TextBlockDescription.Text = level.Description;
            TextBlockUserName.Text = user.Name;
        }

        private void ButtonHint_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(level.Hint, "Подсказка", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ButtonLogOut_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(user);
            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();
            this.Close();
        }

        private void ButtonRun_Click(object sender, RoutedEventArgs e)
        {
            Compiler compiler = new Compiler();
            var response = compiler.Compile(TextEditorCode.Text);
            if (response != null)
            {
                bool success = false;
                if (response is int[] && level.Answer is int[])
                {
                    success = Compare((int[])response, (int[])level.Answer);
                } else if (response is int && level.Answer is int)
                {
                    success = Compare((int)response, int.Parse(level.Answer.ToString()));
                } else if (response is string[] && level.Answer is string[])
                {
                    success = Compare((string[])response, (string[])level.Answer);
                } else if (response is string &&  level.Answer is string)
                {
                    success = Compare((string)response, (string)level.Answer);
                }
                MessageBox.Show($"Результат компиляции: {response.ToString()}.");
                if (success)
                {
                    if (user.Progress < 8)
                    {
                        user.Progress += 1;
                        bool updated = dbp.Write(user.Name, user.Progress);
                        if (updated)
                        {
                            InitializeLevel(user.Progress);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Поздравляем! Ты прошёл все уровни, которые существуют на данный момент.");
                        MainWindow mainWindow = new MainWindow(user);
                        Application.Current.MainWindow = mainWindow;
                        mainWindow.Show();
                        this.Close();
                    }
                }
            }
        }

        private bool Compare(int[] a, int[] b)
        {
            if (a.OrderBy(x => x).SequenceEqual(b.OrderBy(x => x)))
            {
                return true;
            }
            return false;
        }

        private bool Compare(int a, int b)
        {
            if (a.Equals(b))
            {
                return true;
            }
            return false;
        }

        private bool Compare(string[] a, string[] b)
        {
            if (a.OrderBy(x => x).SequenceEqual(b.OrderBy(x => x)))
            {
                return true;
            }
            return false;
        }

        private bool Compare(string a, string b)
        {
            if (string.Equals(a, b, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }
    }
}
