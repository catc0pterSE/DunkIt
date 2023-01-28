using System;
using System.Collections.Generic;

namespace Modules.LiveData
{
    public class MutableLiveData<T>
    {
        private T _value;
        private List<Action<T>> _callbacks = new List<Action<T>>();

        public MutableLiveData() { }

        public MutableLiveData(T value)
        {
            _value = value;
        }

        public event Action<T> Changed;

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                Changed?.Invoke(_value);
            }
        }

        public static implicit operator LiveData<T>(MutableLiveData<T> mutableLiveData) =>
            mutableLiveData.ToLiveData();

        private LiveData<T> ToLiveData() =>
            new LiveData<T>(this);
    }
}