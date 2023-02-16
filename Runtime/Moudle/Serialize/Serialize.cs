using System;
using System.Collections.Generic;

namespace EasyGamePlay
{
    public class ESerialize
    {
        private SortedList<int, SerializeConverter> serializeConverters;

        public ESerialize()
        {
            serializeConverters = new SortedList<int, SerializeConverter>((int)SerializeType.count);

            serializeConverters.Add((int)SerializeType.json, new JsonSerializeConverter());
        }

        public string Serialize(object serializeAble, SerializeType serializeType)
        {
            if(serializeConverters.TryGetValue((int)serializeType,out SerializeConverter serializeConverter))
                return serializeConverter.Serialize(serializeAble);
            return string.Empty;
        }

        public T DeSerialize<T>(string data, SerializeType serializeType)
        {
            if (serializeConverters.TryGetValue((int)serializeType, out SerializeConverter serializeConverter))
                return serializeConverter.DeSerialize<T>(data);
            return default;
        }

        public object DeSerialize(string data, Type type, SerializeType serializeType)
        {
            if (serializeConverters.TryGetValue((int)serializeType, out SerializeConverter serializeConverter))
                return serializeConverter.DeSerialize(data, type);
            return default;
        }

        public void OverWrite(string data, object @object, SerializeType serializeType)
        {
            if (serializeConverters.TryGetValue((int)serializeType, out SerializeConverter serializeConverter))
                 serializeConverter.OverWrite(data, @object);
        }

    }
}
