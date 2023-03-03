using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Mtd {
  public class MtdInput: MonoBehaviour {
    [SerializeField] MtdInputActionsAdapter _inputActionsAdapter;

    public readonly UnityEvent<bool> Click = new UnityEvent<bool>();
    public readonly UnityEvent<Vector2> Point = new UnityEvent<Vector2>();
    public readonly UnityEvent<float> Zoom = new UnityEvent<float>();

    void Start() {
      _inputActionsAdapter.StartInputActionsAdapter(this);
    }
  }
}
