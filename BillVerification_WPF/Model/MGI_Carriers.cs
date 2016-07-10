using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BillVerification_WPF.Model
{
    class MGI_Carriers : IEnumerator, IEnumerable
    {
        int position = -1;
        private MGI_Carrier[] mgiList;

        public object Current { get { return mgiList[position]; } }
        public IEnumerator GetEnumerator() { return (IEnumerator)this; }
        public void Reset() { position = 0; }
        public bool MoveNext()
        {
            position++;
            return (position < mgiList.Length);
        }
    }
}
