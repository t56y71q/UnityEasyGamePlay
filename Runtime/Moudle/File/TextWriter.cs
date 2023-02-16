using System;
using System.IO;
using System.Threading.Tasks;

namespace EasyGamePlay
{
    public class TextWriter : LocalFile
    {
        private StreamWriter stringWriter;

        public TextWriter(string path,bool append=false)
        {
            if (string.IsNullOrEmpty(path))
                return ;

            string dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir) || !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            stringWriter = new StreamWriter(path, append);
        }

        public override void CloseFile()
        {
            stringWriter.Close();
        }

        public void Flush()
        {
            stringWriter.Flush();
        }
       
        public void FlushAsync(Action complete)
        {
            Task task = Task.Run(stringWriter.FlushAsync);
            if (complete != null)
                task.GetAwaiter().OnCompleted(complete);
        }
       
        public void Write(char value)
        {
            stringWriter.Write(value);
        }
        
        public void Write(char[] buffer, int index, int count)
        {
            stringWriter.Write(buffer, index, count);
        }
      
        public void Write(string value)
        {
            stringWriter.Write(value);
        }
       
        public void Write(char[] buffer)
        {
            stringWriter.Write(buffer);
        }
      
        public void WriteAsync(char value, Action complete)
        {
            Task task = Task.Run(delegate() { return stringWriter.WriteAsync(value); });
            if (complete != null)
                task.GetAwaiter().OnCompleted(complete);
        }
       
        public void WriteAsync(string value, Action complete)
        {
            Task task = Task.Run(delegate () { return stringWriter.WriteAsync(value); });
            if (complete != null)
                task.GetAwaiter().OnCompleted(complete);
        }
       
        public void WriteAsync(char[] buffer, int index, int count, Action complete)
        {
            Task task = Task.Run(delegate () { return stringWriter.WriteAsync(buffer,index,count); });
            if (complete != null)
                task.GetAwaiter().OnCompleted(complete);
        }
      
        public void WriteLineAsync(Action complete)
        {
            Task task = Task.Run(stringWriter.WriteLineAsync);
            if (complete != null)
                task.GetAwaiter().OnCompleted(complete);
        }
        
        public void WriteLineAsync(char value, Action complete)
        {
            Task task = Task.Run(delegate () { return stringWriter.WriteLineAsync(value); });
            if (complete != null)
                task.GetAwaiter().OnCompleted(complete);
        }
       
        public void WriteLineAsync(string value, Action complete)
        {
            Task task = Task.Run(delegate () { return stringWriter.WriteLineAsync(value); });
            if (complete != null)
                task.GetAwaiter().OnCompleted(complete);
        }
       
        public void WriteLineAsync(char[] buffer, int index, int count, Action complete)
        {
            Task task = Task.Run(delegate () { return stringWriter.WriteLineAsync(buffer, index, count); });
            if (complete != null)
                task.GetAwaiter().OnCompleted(complete);
        }

        public override bool IsVaild()
        {
            return stringWriter != null;
        }
    }
}
