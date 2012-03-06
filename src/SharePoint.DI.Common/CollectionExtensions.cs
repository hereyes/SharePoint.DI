// Type: Castle.Core.Internal.CollectionExtensions
// Assembly: Castle.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// Assembly location: C:\Dev\Git\Castle\Windsor\lib\NET40\Castle.Core.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace SharePoint.DI.Common
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class CollectionExtensions
    {
        public static TResult[] ConvertAll<T, TResult>(this T[] items, Converter<T, TResult> transformation)
        {
            return Array.ConvertAll<T, TResult>(items, transformation);
        }

        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            if (items == null)
                return;
            foreach (T obj in items)
                action(obj);
        }

        public static T Find<T>(this T[] items, Predicate<T> predicate)
        {
            return Array.Find<T>(items, predicate);
        }

        public static T[] FindAll<T>(this T[] items, Predicate<T> predicate)
        {
            return Array.FindAll<T>(items, predicate);
        }

        /// <summary>
        /// Checks whether or not collection is null or empty. Assumes colleciton can be safely enumerated multiple times.
        /// 
        /// </summary>
        /// <param name="this"/>
        /// <returns/>
        public static bool IsNullOrEmpty(this IEnumerable @this)
        {
            if (@this != null)
                return !@this.GetEnumerator().MoveNext();
            else
                return true;
        }
    }
}
