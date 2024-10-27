using UnityEngine;

namespace Kapolas.Tools.Extensions
{
    public static class TransformExtensions 
    {
        public static T GetObjectWithTag<T>(this Transform _source, string _tag) where T : Component
        {
            T obj = default(T);
            T[] arr = _source.GetComponentsInChildren<T>(true);
            int length = arr.Length;
            for(int i = 0; i < length; i++)
            {
                if(arr[i].CompareTag(_tag))
                {
                    obj = arr[i];
                    break;
                }
            }
            return obj;
        }
        public static T[] GetObjecstWithoutTag<T>(this Transform _source, string _tag) where T : Component
        {
            System.Collections.Generic.List<T> finalArray = new System.Collections.Generic.List<T>();
            T[] arr = _source.GetComponentsInChildren<T>(true);
            int length = arr.Length;
            for (int i = 0; i < length; i++)
            {
                if (!arr[i].CompareTag(_tag))
                    finalArray.Add(arr[i]);
            }
            return finalArray.ToArray();
        }
    }
}