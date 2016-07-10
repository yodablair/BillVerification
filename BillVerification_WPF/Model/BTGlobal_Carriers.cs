using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BillVerification_WPF.Model
{
    class BTGlobal_Carriers : IEnumerator, IEnumerable
    {
        private BTGlobal_Carrier[] btglobalList;
        int position = -1;

        public object Current { get { return btglobalList[position]; } }
        public void Reset() { position = 0; }
        public IEnumerator GetEnumerator() { return (IEnumerator)this; }
        public bool MoveNext()
        {
            position++;
            return (position < btglobalList.Length);
        }

    }
}
