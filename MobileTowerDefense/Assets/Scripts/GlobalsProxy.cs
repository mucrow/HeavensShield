using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Mtd {
  public class GlobalsProxy<T> where T: class {
    T _proxied;

    public readonly UnityEvent<T> Register = new UnityEvent<T>();
    public readonly UnityEvent<T> Unregister = new UnityEvent<T>();

    public GlobalsProxy() {
      Register.AddListener(RegisterInternal);
      Unregister.AddListener(UnregisterInternal);
    }

    public GlobalsProxy(T proxied) {
      Register.AddListener(RegisterInternal);
      Unregister.AddListener(UnregisterInternal);
      RegisterInternal(proxied);
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

    void RegisterInternal(T proxied) {
      _proxied = proxied;
    }

    void UnregisterInternal(T proxied) {
      _proxied = null;
    }
  }
}
