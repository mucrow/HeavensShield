using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Mtd.UI {
  public class SelectionCircle: MonoBehaviour {
    [SerializeField] GameObject _rotate;
    [SerializeField] GameObject _showHide;
    [SerializeField] GameObject _squish;

    [SerializeField] Vector3 _defaultScale = new Vector3(1.5f, 0.7f, 1f);

    [SerializeField] bool _rotateClockwise = true;
    [SerializeField] float _rotationsPerSecond = 1.5f;

    void Start() {
      StopPreviewingRange();
    }

    void Update() {
      var currentRotation = _rotate.transform.localRotation.eulerAngles.z;
      var rotationDirection = _rotateClockwise ? -1f : 1f;
      var rotationDelta = Time.deltaTime * 360f * _rotationsPerSecond * rotationDirection;
      var newRotation = (currentRotation + rotationDelta) % 360f;
      _rotate.transform.localRotation = Quaternion.Euler(0f, 0f, newRotation);
    }

    public void Show() {
      _showHide.SetActive(true);
    }

    public void Hide() {
      _showHide.SetActive(false);
    }

    public void SetWorldPosition(Vector3 pos) {
      gameObject.transform.position = pos;
    }

    public void PreviewRange(float range) {
      _squish.transform.localScale = new Vector3(1f, 1f, 1f);
      SetParticlesExtension(range);
    }

    public void StopPreviewingRange() {
      _squish.transform.localScale = _defaultScale;
      SetParticlesExtension(0.5f);
    }

    void SetParticlesExtension(float length) {
      var particles = GetComponentsInChildren<SelectionCircleParticle>();
      foreach (var particle in particles) {
        particle.SetExtension(length);
      }
    }
  }
}
