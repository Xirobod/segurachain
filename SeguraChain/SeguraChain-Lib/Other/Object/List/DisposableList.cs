﻿using System;
using System.Collections.Generic;
using System.Linq;
using SeguraChain_Lib.Blockchain.Block.Object.Structure;

namespace SeguraChain_Lib.Other.Object.List
{
    public class DisposableList<V> : IDisposable
    {
        public DisposableList(bool enableSort = false, int capacity = 0, IList<V> listCopy = null)
        {
            bool fromCopy = false;
            if (listCopy != null)
            {
                if (listCopy.Count > 0)
                {
                    GetList = new List<V>(listCopy);
                    if (enableSort)
                    {
                        var typeOfData = typeof(V);
                        if (typeof(ClassBlockObject) != typeOfData && typeof(byte) != typeOfData && typeof(byte[]) != typeOfData && typeof(string[]) != typeOfData)
                        {
                            Sort();
                        }
                    }
                    fromCopy = true;
                }
            }
            if(!fromCopy)
            {
                GetList = capacity > 0 ? new List<V>(capacity) : new List<V>();
            }

            if (GetList == null)
            {
                GetList = new List<V>();
            }
        }

        #region Dispose functions

        public bool Disposed;

        ~DisposableList()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                Clear();
            }
            Disposed = true;
        }

        #endregion

        public int Count => GetList.Count;

        public void Add(V data)
        {
            GetList.Add(data);
        }

        public bool Contains(V data)
        {
            return GetList.Contains(data);
        }

        public bool Remove(V data)
        {
            try
            {
                return GetList.Remove(data);
            }
            catch
            {
                return false;
            }
        }

        public V ElementAt(int index)
        {
            return GetList.ElementAt(index);
        }

        public V this[int i]
        {
            get => GetList[i];
            set => GetList[i] = value;
        }

        public void Clear()
        {
            GetList.Clear();
            GetList.TrimExcess();
        }

        public ICollection<V> GetAll => GetList;

        public List<V> GetList { get; }

        public void Sort()
        {
            GetList.Sort();
        }
    }
}
