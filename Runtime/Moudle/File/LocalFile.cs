using System;
using System.IO;

namespace EasyGamePlay
{
    public abstract class LocalFile
    {
        public abstract void CloseFile();
        public abstract bool IsVaild();

        public static void MoveFile(string sourcefile,string destfile)
        {
            File.Move(sourcefile, destfile);
        }

        public static void Copy(string sourcefile, string destfile,bool overWrite)
        {
            File.Copy(sourcefile, destfile, overWrite);
        }
    }
}
