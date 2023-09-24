using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Jenga
{
    [Serializable]
    public class DataWrapper<T>
    {
        public T[] dataReceived;
    }
    
    public static class JsonService 
    {
        public static T[] FromJson<T>(string jsonString)
        {
            // Use JsonUtility to parse each JSON object separately and create an array of objects
            string jsonArray = "{\"dataReceived\":" + jsonString + "}";
            DataWrapper<T> wrapper = JsonUtility.FromJson<DataWrapper<T>>(jsonArray);
            return wrapper.dataReceived;
        }
        
        public static string ToJson<T>(T[] array)
        {
            var wrapper = new DataWrapper<T> {dataReceived = array};
            return JsonUtility.ToJson(wrapper, true);
        }
    }
}