using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadatak3
{
    public class GenericListEnumerator<T> : IEnumerator<T>
    {
        private GenericList<T> _genericList;
        private int index = 0;

        public GenericListEnumerator(GenericList<T> genericList)
        {
            this._genericList = genericList;
        }

        public T Current
        {
            get
            {
                index++;
                return _genericList.GetElement(index - 1);
            }
        }

        object IEnumerator.Current
        {
            get
            {
                index++;
                return _genericList.GetElement(index - 1);
            }
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (index < _genericList.Count)
            {
                return true;
            }
            return false;
        }

        public void Reset()
        {
            index = 0;
        }
    }
}
