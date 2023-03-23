using System;
using UnityEngine;
using UnityEngine.Events;

namespace Mtd {
  public class GlobalsProxy<T> where T: class {
    T _proxied;

    public UnityEvent<T> Register = new UnityEvent<T>();
    public UnityEvent<T> Unregister = new UnityEvent<T>();

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
        Debug.LogWarning("With() called on GlobalsProxy when object was null (use WithIfRegistered())");
      }
    }

    public void WithIfRegistered(UnityAction<T> callback) {
      if (_proxied != null) {
        callback(_proxied);
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
