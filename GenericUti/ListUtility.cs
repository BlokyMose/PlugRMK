using System;
using System.Collections.Generic;

namespace PlugRMK.GenericUti
{
    public static class ListUtility
    {
        /// <summary>
        /// Returns a member of the list by index; if index is not in range of the list's length, returns defaultValue
        /// </summary>
        public static T GetAt<T>(this IList<T> list, int index, T defaultValue = null) where T : class
        {
            if (index < 0)
                return defaultValue;
            else if (list.Count > index)
                return list[index];
            else
                return defaultValue;
        }

        /// <summary>
        /// Returns a member of the list by index; if index is not in range of the list's length, returns defaultValue
        /// </summary>
        public static T GetAt<T>(this IList<T> list, int index, int defaultIndex = 0) where T : class
        {
            if (index < 0)
                return list[defaultIndex];
            else if (list.Count > index)
                return list[index];
            else
                return list[defaultIndex];
        }

        /// <summary>
        /// Returns the last item in the list
        /// </summary>
        public static T GetLast<T>(this IList<T> list, int indexFromLast = 0) where T : class
        {
            return list[list.Count - (1+indexFromLast)];
        }

        /// <summary>
        /// Returns the last item in the list
        /// </summary>
        public static T GetLastStruct<T>(this IList<T> list, int indexFromLast = 0) where T : struct
        {
            return list[list.Count - (1 + indexFromLast)];
        }

        /// <summary>
        /// Returns the last item in the list
        /// </summary>
        public static int GetLast(this IList<int> list)
        {
            return list[list.Count - 1];
        }

        public static void AddIfHasnt<T>(this IList<T> list, T itemToAdd)
        {
            if(!list.Contains(itemToAdd)) list.Add(itemToAdd);
        }

        public static void AddIfHasnt<T>(this IList<T> list, IList<T> listToAdd)
        {
            foreach (var item in listToAdd)
                list.AddIfHasnt(item);
        }

        public static T GetRandom<T>(this IList<T> list) where T : class
        {
            if (list.Count == 0)
                return null;
            return list[new Random().Next(0, list.Count)];
        }

        public static T GetRandomStruct<T>(this IList<T> list) where T : struct
        {
            return list[new Random().Next(0, list.Count)];
        }

        public static void RemoveIfHas<T>(this IList<T> list, T removeValue)
        {
            if (list.Contains(removeValue)) list.Remove(removeValue);
        }

        public static void RemoveLast<T>(this IList<T> list)
        {
            list.RemoveAt(list.Count-1);
        }

        public static void RemoveNulls<T>(this IList<T> list)
        {
            for (int i = list.Count - 1; i >= 0; i--)
                if (list[i] == null || list[i].ToString() == "null")
                    list.RemoveAt(i);
        }

        /// <summary>return Count == 0</summary>
        public static bool IsEmpty<T>(this IList<T> list)
        {
            return list.Count == 0;
        }

        public static void Move<T>(this IList<T> list, T item, int toIndex)
        {
            if (!list.Contains(item)) return;
            list.Remove(item);
            list.Insert(toIndex, item);
        }

        public static void MoveToLast<T>(this IList<T> list, T item)
        {
            if (!list.Contains(item)) return;
            list.Remove(item);
            list.Add(item);
        }

        public static bool HasIndex<T>(this IList<T> list, int index) 
        {
            if (list.Count == 0) return false;
            else if (index < 0 || index >= list.Count) return false;
            return true;
        }

        public static bool Has<T>(this IList<T> list, T targetItem) where T : class
        {
            foreach (var item in list)
                if (item == targetItem) return true;
            return false;
        }
    }
}