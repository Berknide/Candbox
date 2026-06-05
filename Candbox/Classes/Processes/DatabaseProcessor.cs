using System;
using System.Data.SQLite;
using System.IO;
using System.Linq;
namespace Candbox
{
    public class DatabaseProcessor
    {
        private readonly string _connectionString;
        private readonly string _databasePath;

        public DatabaseProcessor()
        {
            _databasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PlayersDatabase.db");
            _connectionString = $"Data Source={_databasePath}";

            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            if (!File.Exists(_databasePath))
            {
                CreateDatabase();
            }
        }

        private bool CreateDatabase()
        {
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    var SpecialCommand = connection.CreateCommand();
                    SpecialCommand.CommandText = @"PRAGMA foreign_keys = ON";
                    SpecialCommand.ExecuteNonQuery();

                    var CreateTable = connection.CreateCommand();

                    CreateTable.CommandText = @"
                    CREATE TABLE IF NOT EXISTS LEVELS (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    NAME TEXT UNIQUE NOT NULL,
                    DESCRIPTION TEXT NOT NULL,
                    ANSWER JSONB NOT NULL,
                    HINT TEXT NOT NULL
                    )";
                    CreateTable.ExecuteNonQuery();

                    CreateTable.CommandText = @"
                    CREATE TABLE IF NOT EXISTS LANGUAGES (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    LANG TEXT UNIQUE NOT NULL
                    )";
                    CreateTable.ExecuteNonQuery();

                    CreateTable.CommandText = @"
                    CREATE TABLE IF NOT EXISTS USERS (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    NAME TEXT UNIQUE NOT NULL,
                    PASSWORD TEXT,
                    PROGRESS INTEGER,
                    LANGUAGE INTEGER,
                    FOREIGN KEY (PROGRESS) REFERENCES LEVELS(ID),
                    FOREIGN KEY (LANGUAGE) REFERENCES LANGUAGES(ID)
                    )";
                    CreateTable.ExecuteNonQuery();

                    var InsertTable = connection.CreateCommand();
                    InsertTable.CommandText = @"
                    INSERT INTO LEVELS (NAME, DESCRIPTION, ANSWER, HINT)
                    VALUES
                    ('Сумматор', 'Посчитать сумму чисел 5 и 10', '{""value"": 15, ""type"": ""number""}', 'Серьёзно? Используй оператор ""+"" и запиши новое значение в переменную.'),
                    ('Калькулятор', 'Посчитать сумму, разность, произведение и частное чисел 20 и 10. Ответ вернуть массивом типа int[].', '{""value"": [30,10,200,2], ""type"": ""array""}', 'Чтобы положить ответ создай массив int[] arr = {""a"", ""b"", ""c"", ""d""}.'),
                    ('Семь раз отмерь - один раз отрежь', 'Обрезать строку "" hello world   "".', '{""value"": ""hello world"", ""type"": ""string""}', 'Для удаления пробелов слева и справа у строк используется метод класса string.Trim().'),
                    ('Предложение', 'Сложить строки ""An"", ""apple"", ""a"", ""day"", ""keeps"", ""a"", ""doctor"", ""away"", добавив пробелы между ними.', '{""value"": ""An apple a day keeps a doctor away"", ""type"": ""string""}', 'Задачу можно решить двумя способами: \n1. Сложи строки так же, как и числа, добавив пробел;\n2. Используй метод класса string.Concat(string, string)'),
                    ('Уроки арифметики', 'Определить количество чётных и нечётных чисел в массиве [2, 17, 10, 31, 30]. Ответ вернуть массивом типа int[], где первое число - чётное, второе - нечётное.', '{""value"": [3,2], ""type"": ""array""}', 'Для решения этой задачи нужно знать простой бинарный оператор - % (деление с остатком): при таком делении на 2 у чётных чисел остаток равен 0.\nЧтобы пройтись по массиву, используй цикл со счётчиком for.\nЧтобы создать массив, нужно написать след. выражение: int[] arr = new int[n], где n - количество эл-тов, либо int[] arr = { int a, int b, int c ...}.'),
                    ('Считалочка', 'Посчитать длину массива [4, 6, 9, 1, 13, 56, 2, 1, 3324]', '{""value"": 9, ""type"": ""number""}', 'Самый простой способ, конечно же, вручную посчитать, но это неинтересно :). Проще всего на C# использовать обыкновенный цикл while и переменную количества.'),
                    ('Считалочка 2', 'Посчитать количество символов в строке ""Съешь же ещё этих мягких французских булок да выпей чаю"".', '{""value"": 55, ""type"": ""number""}', 'Многие языки программирования позволяют представить строку как массив символов (char[]): такой метод позволит работать со строкой как с обычным массивом. Если решил предыдущий уровень, этот не составит тоже труда :).'),
                    ('Конвертер массивов', 'Вернуть в другой массив количество *, сколько есть в каждом элементе данного массива (например, [2, 1] = [""**"", ""*""]). Массив для задания: [4, 3, 5, 1].', '{""value"": [""****"",""***"",""*****"",""*""], ""type"": ""array""}', 'Цикл for тебе в помощь :).')
                    ";
                    InsertTable.ExecuteNonQuery();

                    InsertTable.CommandText = @"
                    INSERT INTO LANGUAGES (LANG)
                    VALUES
                    ('ENG'),
                    ('RUS'),
                    ('GER'),
                    ('ESP'),
                    ('JAP'),
                    ('CHN')
                    ";
                    InsertTable.ExecuteNonQuery();

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Write(string nickname, int progress)
        {
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    string insert = @"
                    UPDATE USERS
                    SET PROGRESS = @progress
                    WHERE NAME = @name
                    ";

                    using (var command = new SQLiteCommand(insert, connection))
                    {
                        command.Parameters.AddWithValue("@name", nickname);
                        command.Parameters.AddWithValue("@progress", progress);

                        command.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool WriteNew(string nickname, int language)
        {
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    string update = @"
                    INSERT INTO USERS (NAME, PROGRESS, LANGUAGE)
                    VALUES (@name, 1, @language);
                    ";

                    using (var command = new SQLiteCommand(update, connection))
                    {
                        command.Parameters.AddWithValue("@name", nickname);
                        command.Parameters.AddWithValue("@language", language);

                        command.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public User Read(string nickname)
        {
            User user = null;

            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    var read = "SELECT NAME, PROGRESS, LANGUAGE FROM USERS WHERE NAME = @name";
                    using (var command = new SQLiteCommand(read, connection))
                    {
                        command.Parameters.AddWithValue("@name", nickname);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int a = 1;
                                user = new User
                                {
                                    Name = reader.GetValue(0).ToString(),
                                    Progress = int.TryParse(reader.GetValue(1).ToString(), out a) ? int.Parse(reader.GetValue(1).ToString()) : 1,
                                    Language = int.TryParse(reader.GetValue(2).ToString(), out a) ? int.Parse(reader.GetValue(2).ToString()) : 1
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception) { }

            return user;
        }

        public Level GetLevel(int progress)
        {
            Level level = null;
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    var read = "SELECT NAME, DESCRIPTION, HINT FROM LEVELS WHERE ID = @id";
                    using (var command = new SQLiteCommand(read, connection))
                    {
                        command.Parameters.AddWithValue("@id", progress);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                level = new Level
                                {
                                    Name = reader.GetValue(0).ToString(),
                                    Description = reader.GetValue(1).ToString(),
                                    Hint = reader.GetValue(2).ToString(),
                                };
                            }
                        }
                    }

                    read = "SELECT ANSWER -> 'value' FROM LEVELS WHERE ID = @id";
                    using (var command = new SQLiteCommand(read, connection))
                    {
                        command.Parameters.AddWithValue("@id", progress);
                        var answer = command.ExecuteScalar() as string;

                        if (answer != null)
                        {
                            answer = answer.Trim();
                            answer = answer.Replace('[', ',');
                            answer = answer.Replace(']', ',');
                            answer = answer.Trim(new char[] { ',' });
                            answer = answer.Trim(new char[] { '\"' });
                            string[] str = answer.Split(new char[] { ',' });

                            if (str.Length > 1)
                            {
                                if (int.TryParse(str[0], out var a))
                                {
                                    int[] num = str.Select(int.Parse).ToArray();
                                    level.Answer = num;
                                }
                                else
                                {
                                    for (int i = 0; i < str.Length; i++)
                                    {
                                        str[i] = str[i].Trim(new char[] { '\"' });
                                    }
                                    level.Answer = str;
                                }
                            }
                            else
                            {
                                if (int.TryParse(answer, out var a))
                                {
                                    level.Answer = int.Parse(answer);
                                }
                                else
                                {
                                    level.Answer = answer;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception) { }

            return level;
        }
    }
}
