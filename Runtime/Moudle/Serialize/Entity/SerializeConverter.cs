using System;

namespace EasyGamePlay
{
    public abstract class SerializeConverter
    {
        public abstract string Serialize(object serializeAble);
        public abstract T DeSerialize<T>(string data);
        public abstract object DeSerialize(string data,Type type);
        public abstract void OverWrite(string data, object @object);
    }
}
