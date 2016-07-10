using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace BillVerification_WPF.Model
{
    class BICS_List : IEnumerator, IEnumerable
    {
        private BICS_Carrier[] bicsList;
        int position = -1;

        public object Current { get { return bicsList[position]; } }
        public IEnumerator GetEnumerator() { return (IEnumerator)this; }
        public void Reset() { position = 0; }
        public bool MoveNext()
        {
            position++;
            return (position < bicsList.Length);
        }

    }
}
