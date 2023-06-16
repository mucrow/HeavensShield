using System.Collections;
using System.Collections.Generic;
using Mtd.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Mtd {
  [RequireComponent(typeof(MtdInputActionsAdapter))]
  public class MtdInput: MonoBehaviour {
    [SerializeField] MtdInputActionsAdapter _inputActionsAdapter;

    public readonly UnityEvent<Vector2> DragStart = new UnityEvent<Vector2>();
    public readonly UnityEvent<Vector2> Drag = new UnityEvent<Vector2>();
    public readonly UnityEvent<Vector2> DragEnd = new UnityEvent<Vector2>();
    public readonly UnityEvent<ScreenAndWorldPoint> Tap = new UnityEvent<ScreenAndWorldPoint>();
    public readonly UnityEvent<float> Zoom = new UnityEvent<float>();

    void Start() {
      _inputActionsAdapter.StartInputActionsAdapter(this);
    }
  }
}
