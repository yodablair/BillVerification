using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BillVerification_WPF.Model
{
    class IDT_Carriers : IEnumerator, IEnumerable
    {
        int position = -1;
        private IDT_Carrier[] idtList;

        public void Reset() { position = 0; }
        public object Current { get { return (IEnumerator)this; } }
        public IEnumerator GetEnumerator() { return (IEnumerator)this; }
        public bool MoveNext()
        {
            position++;
            return (position < idtList.Length);
        }
    }
}
