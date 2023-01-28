using System;
using System.Collections.Generic;
using Utility.Extensions;

namespace Modules.LiveData
{
    public class LiveData<T> : IDisposable
    {
        private MutableLiveData<T> _mutableLiveData;
        private List<Action<T>> _callbacks = new List<Action<T>>();

        public LiveData(MutableLiveData<T> mutableLiveData)
        {
            _mutableLiveData = mutableLiveData;
            _mutableLiveData.Changed += OnChanged;
        }

        ~LiveData() => 
            Dispose();

        public T Value => _mutableLiveData.Value;

        private void OnChanged(T value)
        {
            foreach (Action<T> callback in _callbacks)
            {
                callback.Invoke(value);
            }
        }
        
        public void Dispose()
        {
            _callbacks = null;
            _mutableLiveData.Changed -= OnChanged;
            GC.SuppressFinalize(this);
        }

        public void Observe(Action<T> callback)
        {
            if (_callbacks.Contains(callback))
                return;

            _callbacks.Add(callback);

            callback(_mutableLiveData.Value);
        }
    }
}