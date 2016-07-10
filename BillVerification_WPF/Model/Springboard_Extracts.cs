using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BillVerification_WPF.Model
{
    class Springboard_Extracts : IEnumerator, IEnumerable
    {
        int position = -1;
        private Sprinboard_Extract[] springboardList;

        public object Current { get { return springboardList[position]; } }
        public void Reset() { position = 0; }
        public IEnumerator GetEnumerator() { return (IEnumerator)this; }
        public bool MoveNext()
        {
            position++;
            return (position < springboardList.Length);
        }

    }
}
