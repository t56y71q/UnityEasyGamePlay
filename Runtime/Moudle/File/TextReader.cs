using System;
using System.IO;
using System.Threading.Tasks;

namespace EasyGamePlay
{
    public class TextReader : LocalFile
    {
        private StreamReader textReader;

        public TextReader(string path)
        {
            if (System.IO.File.Exists(path))
            {
                textReader = new StreamReader(path);
            }
        }

        public override void CloseFile()
        {
            textReader.Close();
        }

        public override bool IsVaild()
        {
            return textReader != null;
        }

        public int Read(char[] buffer, int index, int count)
        {
            return textReader.Read(buffer, index, count);
        }
       
        public void ReadAsync(char[] buffer, int index, int count,Action<int> completed)
        {
            Task<int> task = Task.Run<int>(delegate () { return textReader.ReadAsync(buffer, index, count); });
            if(completed!=null)
            {
                task.GetAwaiter().OnCompleted(delegate(){
                    completed(task.GetAwaiter().GetResult());
                });
            }
        }

        public void ReadBlockAsync(char[] buffer, int index, int count, Action<int> completed)
        {
            Task<int> task= Task.Run<int>(delegate () { return textReader.ReadBlockAsync(buffer, index, count); });
            if (completed != null)
            {
                task.GetAwaiter().OnCompleted(delegate () {
                    completed(task.GetAwaiter().GetResult());
                });
            }
        }
        
        public string ReadLine()
        {
            return textReader.ReadLine();
        }

        public void  ReadLineAsync(Action<string> completed)
        {
            Task<string> task = Task.Run<string>( textReader.ReadLineAsync);
            if (completed != null)
            {
                task.GetAwaiter().OnCompleted(delegate () {
                    completed(task.GetAwaiter().GetResult());
                });
            }
        }
       
        public string ReadToEnd()
        {
            return textReader.ReadToEnd();
        }
        
        public void ReadToEndAsync(Action<string> completed)
        {
            Task<string> task = Task.Run<string>(delegate () { return textReader.ReadToEndAsync(); });
            if (completed != null)
            {
                task.GetAwaiter().OnCompleted(delegate () {
                    completed(task.GetAwaiter().GetResult());
                });
            }
        }
    }
}
