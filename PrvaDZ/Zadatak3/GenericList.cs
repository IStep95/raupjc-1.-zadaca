using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadatak3
{
    public class GenericList<X> : IGenericList<X>
    {
        private X[] _internalStorage;
        private int _currentIndex;


        public GenericList()
        {
            _internalStorage = new X[4];
            _currentIndex = 0;
        }

        public GenericList(int initialSize)
        {
            if (initialSize < 0)
            {
                throw new ArgumentOutOfRangeException("Initial size is not a positive number");
            }
            else
            {
                _internalStorage = new X[initialSize];
                _currentIndex = 0;

            }
        }


        public void Add(X item)
        {
            int size = _internalStorage.Length;

            if (_currentIndex >= size)
            {
                X[] tmp = new X[size];
                tmp = _internalStorage;
                _internalStorage = new X[2 * size];

                for (int i = 0; i < size; i++)
                {
                    _internalStorage[i] = tmp[i];
                }

                _currentIndex = size;
                _internalStorage[_currentIndex] = item;
                _currentIndex++;

            }
            else
            {
                _internalStorage[_currentIndex] = item;
                _currentIndex++;
            }
        }


        public bool Remove(X item)
        {
            if (_currentIndex == 0) return false;

            for (int i = 0; i < _currentIndex; i++)
            {
                if (_internalStorage[i].Equals(item))
                {
                    RemoveAt(i);
                }
            }
            return false;
        }


        public bool RemoveAt(int index)
        {
            if (_currentIndex == 0 || index < 0 || (index > _currentIndex - 1)) return false;

            // Removing last element
            if (index == (_currentIndex - 1))
            {
                _internalStorage[index] = default(X);
                _currentIndex -= 1;
                return true;
            }
            // Removing first and middle element
            int i = index;
            while (i < (_currentIndex - 1))
            {
                _internalStorage[i] = _internalStorage[i + 1];
                i++;
            }
            _internalStorage[_currentIndex - 1] = default(X);
            _currentIndex -= 1;
            return true;
        }


        public X GetElement(int index)
        {
            if ((index >= 0) && index <= (_currentIndex - 1))
            {
                return _internalStorage[index];
            }
            else
            {
                throw new IndexOutOfRangeException("Index is out of range.");
            }
        }


        public int IndexOf(X item)
        {
            for (int i = 0; i < _currentIndex; i++)
            {
                if (_internalStorage[i].Equals(item))
                {
                    return i;
                }
            }
            return -1;
        }


        public int Count
        {
            get { return _currentIndex; }
        }


        public void Clear()
        {
            for (int i = 0; i < _currentIndex; i++)
            {
                _internalStorage[i] = default(X);
            }
            _currentIndex = 0;
        }


        public bool Contains(X item)
        {
            for (int i = 0; i < _currentIndex; i++)
            {
                if (_internalStorage[i].Equals(item))
                    return true;
            }
            return false;
        }

        public IEnumerator<X> GetEnumerator()
        {
            return new GenericListEnumerator<X>(this);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
