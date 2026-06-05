using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Candbox
{
    public class FileProcessor
    {
        private readonly string _filePath;
        public FileProcessor()
        {
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "players.csv");
        }

        public void Write(string data)
        {
            using (FileStream fs = new FileStream(_filePath, FileMode.OpenOrCreate))
            {
                byte[] buffer = Encoding.Default.GetBytes(data);
                fs.WriteAsync(buffer, 0, buffer.Length);
            }
        }

        public async Task<string> Read()
        {
            using (FileStream fs = File.OpenRead(_filePath))
            {
                byte[] buffer = new byte[fs.Length];
                await fs.ReadAsync(buffer, 0, buffer.Length);
                string data = Encoding.Default.GetString(buffer);
                return data;
            }
        }

        public void Delete()
        {
            FileInfo file = new FileInfo(_filePath);
            if (file.Exists)
            {
                file.Delete();
            }
        }
    }
}
