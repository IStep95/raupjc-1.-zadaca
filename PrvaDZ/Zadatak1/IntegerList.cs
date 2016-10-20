using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadatak1
{
    public class IntegerList : IIntegerList
    {
        private int[] _internalStorage;
        private int _currentIndex;

        public IntegerList()
        {
            _internalStorage = new int[4];
            _currentIndex = 0;
        }

        public IntegerList(int initialSize)
        {
            if (initialSize < 0)
            {
                throw new ArgumentOutOfRangeException("Initial size is not a positive number");
            }
            else
            {
                _internalStorage = new int[initialSize];
                _currentIndex = 0;
                              
            }
            
        }

       
        public void Add(int item)
        {
            int size = _internalStorage.Length;

            if (_currentIndex >= size)
            {
                int[] tmp = new int[size];
                tmp = _internalStorage;
                _internalStorage = new int[2 *size];

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


        public bool Remove(int item)
        {
            if (_currentIndex == 0) return false;
   
            for (int i = 0; i < _currentIndex; i++)
            {
                if (_internalStorage[i] == item)
                {
                    RemoveAt(i);
                }
            }
            return false;
        }
        

        public bool RemoveAt(int index)
        {
            if (_currentIndex == 0 || index < 0 || (index > _currentIndex -1)) return false;
            
            // Removing last element
            if (index == _currentIndex -1)
            {
                _internalStorage[index] = 0;
                _currentIndex -= 1;
                return true;
            }
            // Removing first and middle element
            int i = index;
            while (i < _currentIndex - 1)
            {
                _internalStorage[i] = _internalStorage[i + 1];
                i++;
            }
            _internalStorage[_currentIndex - 1] = 0;
            _currentIndex -= 1;
            return true;
        }


        public int GetElement(int index)
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


        public int IndexOf(int item)
        {
       
            for (int i = 0; i < _currentIndex; i++)
            {
                if (_internalStorage[i] == item)
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
                _internalStorage[i] = 0;
            }
            _currentIndex = 0;
        }


        public bool Contains(int item)
        {
           for (int i = 0; i < _currentIndex; i++)
           {
                if (_internalStorage[i] == (item))
                    return true;
           }
           return false;
        }
      
        
    }
}
