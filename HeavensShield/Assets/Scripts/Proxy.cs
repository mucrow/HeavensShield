using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Mtd {
  public class Proxy<T> where T: class {
    T _proxied;

    readonly UnityEvent<T> _register = new UnityEvent<T>();
    readonly UnityEvent<T> _unregister = new UnityEvent<T>();

    public bool IsRegistered => _proxied != null;

    public Proxy() { }

    public Proxy(T item) {
      _proxied = item;
    }

    public void Register(T item) {
      T temp = _proxied;
      _proxied = item;
      if (temp != null) {
        _unregister.Invoke(temp);
      }
      _register.Invoke(_proxied);
    }

    public void Unregister() {
      T temp = _proxied;
      _proxied = null;
      _unregister.Invoke(temp);
    }

    public void Unregister(T temp) {
      if (_proxied == temp) {
        _proxied = null;
        _unregister.Invoke(temp);
      }
    }

    public void AddRegisterListener(UnityAction<T> listener) {
      _register.AddListener(listener);
    }

    public void RemoveRegisterListener(UnityAction<T> listener) {
      _register.RemoveListener(listener);
    }

    public void AddUnregisterListener(UnityAction<T> listener) {
      _unregister.AddListener(listener);
    }

    public void RemoveUnregisterListener(UnityAction<T> listener) {
      _unregister.RemoveListener(listener);
    }

    public void With(UnityAction<T> callback) {
      if (_proxied != null) {
        callback(_proxied);
      }
      else {
        Debug.LogWarning("With() called on GlobalsProxy when object was null (use WithIfRegistered() if this is ok)");
      }
    }

    public void WithIfRegistered(UnityAction<T> callback) {
      if (_proxied != null) {
        callback(_proxied);
      }
    }

    public async Task WithAsync(Func<T, Task> callback) {
      if (_proxied != null) {
        await callback(_proxied);
      }
      else {
        Debug.LogWarning("WithAsync() called on GlobalsProxy when object was null (use WithIfRegisteredAsync() if this is ok)");
      }
    }

    public async Task WithIfRegisteredAsync(Func<T, Task> callback) {
      if (_proxied != null) {
        await callback(_proxied);
      }
    }

    public T GetNullable() {
      return _proxied;
    }
  }
}
