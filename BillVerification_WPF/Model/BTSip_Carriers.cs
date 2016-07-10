using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BillVerification_WPF.Model
{
    class BTSip_Carriers : IEnumerator, IEnumerable
    {
        int position = -1;
        private BTSip_Carrier[] btsipList;

        public object Current { get { return btsipList[position]; } }
        public void Reset() { position = 0; }
        public IEnumerator GetEnumerator() { return (IEnumerator)this; }
        public bool MoveNext()
        {
            position++;
            return (position < btsipList.Length);
        }

    }
}
