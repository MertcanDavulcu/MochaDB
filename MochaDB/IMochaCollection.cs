﻿using System;
using System.Collections.Generic;

namespace MochaDB {
    /// <summary>
    /// Collection interface for MochaDB.
    /// </summary>
    /// <typeparam name="T">Item type of collector.</typeparam>
    public interface IMochaCollection<T>:IEnumerable<T> {
        #region Events

        event EventHandler<EventArgs> Changed;

        #endregion

        #region Methods

        void Clear();
        void Add(T item);
        void AddRange(IEnumerable<T> items);
        void Remove(T item);
        void RemoveAt(int index);
        int IndexOf(T item);
        bool Contains(T item);
        int MaxIndex();
        bool IsEmptyCollection();
        T ElementAt(int index);
        T[] ToArray();
        List<T> ToList();
        T GetFirst();
        T GetLast();

        #endregion

        #region Properties

        int Count { get; }
        T this[int index] { get; }

        #endregion
    }
}
