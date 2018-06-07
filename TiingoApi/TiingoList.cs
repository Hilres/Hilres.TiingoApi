// <copyright file="TiingoList.cs" company="Hilres">
// Copyright (c) Hilres. All rights reserved.
// </copyright>

namespace Hilres.TiingoApi
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Tiingo list class.
    /// </summary>
    /// <typeparam name="T">Type of list.</typeparam>
    public class TiingoList<T> : TiingoResponse, IEnumerable<T>
        where T : class
    {
        /// <summary>
        /// Gets the number of elements in the collection.
        /// </summary>
        public int Count
        {
            get { return this.Items == null ? 0 : this.Items.Count; }
        }

        /// <summary>
        /// Gets the list of items.
        /// </summary>
        public IList<T> Items { get; internal set; }

        /// <summary>
        /// Indexer.
        /// </summary>
        /// <param name="index">Item number in list.</param>
        /// <returns>Item.</returns>
        public T this[int index]
        {
            get { return this.Items[index]; }
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}