// <copyright file="ConcurrentList.cs" company="Supun De Silva">
// All rights reserved (C) Supun De Silva 2019
// </copyright>

namespace FileAnalyzer.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// ConcurrentList
    /// </summary>
    /// <typeparam name="T">template type</typeparam>
    public class ConcurrentList<T>
    {
        /// <summary>
        /// The data
        /// </summary>
        private readonly List<T> data;

        /// <summary>
        /// The synchronize object
        /// </summary>
        private readonly object syncObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrentList{T}"/> class.
        /// </summary>
        public ConcurrentList()
        {
            this.data = new List<T>();
            this.syncObject = new object();
        }

        public void AddRange(List<T> inputData)
        {
            try
            {
                Monitor.Enter(this.syncObject);
                this.data.AddRange(inputData);
                Monitor.Exit(this.syncObject);
            }
            catch (Exception ex)
            {
                Monitor.Exit(this.syncObject);
                throw ex;
            }
        }
    }
}
