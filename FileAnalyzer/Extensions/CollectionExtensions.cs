// <copyright file="CollectionExtensions.cs" company="Supun De Silva">
// All rights reserved (C) Supun De Silva 2019
// </copyright>

namespace FileAnalyzer.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Collection Extensions
    /// </summary>
    public static class CollectionExtensions
    {
        public static string ToPrintableString<T>(this List<T> list, string itemPrefix = "", string itemSuffix = "")
        {
            StringBuilder builder = new StringBuilder();

            if (!string.IsNullOrEmpty(itemPrefix))
            {
                itemPrefix += " ";
            }

            if (!string.IsNullOrEmpty(itemSuffix))
            {
                itemSuffix = " " + itemSuffix;
            }

            foreach (T item in list)
            {
                builder.AppendLine($"{itemPrefix}{item.ToString()}{itemSuffix}");
            }

            return builder.ToString();
        }
    }
}
