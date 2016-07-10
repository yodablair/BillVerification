using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BillVerification_WPF.Model
{
    class IBasis_Carriers : IEnumerator, IEnumerable
    {
        int position = -1;
        private IBasis_Carrier[] ibasisList;

        public object Current { get { return ibasisList[position]; } }
        public IEnumerator GetEnumerator() { return (IEnumerator)this; }
        public void Reset() { position = 0; }
        public bool MoveNext()
        {
            position++;
            return (position < ibasisList.Length);
        }
    }
}
