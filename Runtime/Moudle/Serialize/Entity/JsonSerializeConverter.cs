using System;
using UnityEngine;

namespace EasyGamePlay
{
    class JsonSerializeConverter : SerializeConverter
    {
        public override T DeSerialize<T>(string data)
        {
            return JsonUtility.FromJson<T>(data);
        }

        public override object DeSerialize(string data, Type type)
        {
            return JsonUtility.FromJson(data,type);
        }

        public override void OverWrite(string data, object @object)
        {
            JsonUtility.FromJsonOverwrite(data, @object);
        }

        public override string Serialize(object serializeAble)
        {
            return JsonUtility.ToJson(serializeAble);
        }
    }
}
