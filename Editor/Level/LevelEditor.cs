using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace EasyGamePlay.Editor
{
    public class LevelEditor
    {
        public string path { get; private set; }
        public event ColseLevel colseLevel;

        public void Open(string path)
        {
            this.path = path;
        }

        public void Create(string path)
        {
            TextWriter textWriter = new TextWriter(path);
            textWriter.Write(FrameWork.frameWork.serialize.Serialize(new LevelAsset(), SerializeType.json));
            textWriter.CloseFile();
            AssetDatabase.Refresh();
        }

        public void Save(LevelAsset levelAsset)
        {
            string data= FrameWork.frameWork.serialize.Serialize(levelAsset, SerializeType.json);
            TextWriter textWriter = new TextWriter(path);
            textWriter.Write(data);
            textWriter.CloseFile();
            AssetDatabase.Refresh();
        }

        public void Close()
        {
            path = null;
            colseLevel?.Invoke();
        }

        public LevelAsset LoadLevel()
        {
            TextReader textReader = new TextReader(path);
            string data = textReader.ReadToEnd();
            textReader.CloseFile();
            return FrameWork.frameWork.serialize.DeSerialize<LevelAsset>(data, SerializeType.json);
        }

        public string GetName()
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        public bool IsOpen()
        {
            return path != null;
        }


        public delegate void ColseLevel();

        [MenuItem("EasyGamePlay/Level/Close", false, 8)]
        private static void CloseLevel()
        {
            FrameWorkEditor.levelEditor.Close();
        }

        [MenuItem("EasyGamePlay/Level/Close", true, 8)]
        private static bool IsCloseVaildate()
        {
            return FrameWorkEditor.levelEditor.IsOpen();
        }
    }
}
