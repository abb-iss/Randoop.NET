//*********************************************************
//
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Apache License, Version 2.0.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************



using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace Common
{
    public class CollectionOfCollections<T>
    {
        private Collection<Collection<T>> collections;

        // totalSize = sum(sizes)
        private int totalSize;

        public CollectionOfCollections()
        {
            this.collections = new Collection<Collection<T>>();
            this.totalSize = 0;
        }

        public void Add(Collection<T> c)
        {
            if (c == null) throw new ArgumentNullException();
            collections.Add(c);
            totalSize += c.Count;
        }

        public T Get(int i)
        {
            if (i < 0 || i >= totalSize) throw new ArgumentException("i");

            int accum = 0;
            for (int ci = 0; ci < collections.Count; ci++)
            {
                Collection<T> currColl = collections[ci];
                if (i < accum + currColl.Count)
                {
                    // Desired element is in currColl.
                    return currColl[i - accum];
                }
                accum += currColl.Count;
            }
            throw new RandoopBug();
        }

        public int Size()
        {
            return this.totalSize;
        }

    }
}
