﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BillVerification_WPF.Model
{
    class Tata_Carriers : IEnumerator, IEnumerable
    {
        int position = -1;
        private Tata_Carrier[] tataList;

        public object Current { get { return tataList[position]; } }
        public IEnumerator GetEnumerator() { return (IEnumerator)this; }
        public void Reset() { position = 0; }
        public bool MoveNext()
        {
            position++;
            return (position < tataList.Length);
        }
    }
}
