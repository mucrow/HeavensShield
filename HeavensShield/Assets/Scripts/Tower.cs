using UnityEngine;

namespace Mtd {
  public class Tower: MonoBehaviour {
    [SerializeField] int _health = 100;
    public int Health => _health;

    enum TowerDirection { DoorFacesNorth, DoorFacesSouth, DoorFacesEast, DoorFacesWest }
    [SerializeField] TowerDirection _direction;

    [SerializeField] SpriteRenderer _mainSpriteRenderer;
    [SerializeField] SpriteRenderer _backFloorEdgeSpriteRenderer;
    [SerializeField] Sprite _doorFacesNorthSprite;
    [SerializeField] Sprite _doorFacesSouthSprite;
    [SerializeField] Sprite _doorFacesEastSprite;
    [SerializeField] Sprite _doorFacesWestSprite;

    void Start() {
      Globals.UI.HUD.UpdateTowerHealth(_health);
    }

    public void ReceiveDamage(int amount) {
      _health -= amount;
      Globals.UI.HUD.UpdateTowerHealth(_health);
      if (_health <= 0) {
        Globals.ScenarioManager.With(scenarioManager => {
          scenarioManager.NotifyTowerDestroyed();
        });
      }
    }

    public void OnValidate() {
      if (_direction == TowerDirection.DoorFacesNorth) {
        _backFloorEdgeSpriteRenderer.enabled = true;
        _mainSpriteRenderer.sprite = _doorFacesNorthSprite;
      }
      else if (_direction == TowerDirection.DoorFacesSouth) {
        _backFloorEdgeSpriteRenderer.enabled = false;
        _mainSpriteRenderer.sprite = _doorFacesSouthSprite;
      }
      else if (_direction == TowerDirection.DoorFacesEast) {
        _backFloorEdgeSpriteRenderer.enabled = false;
        _mainSpriteRenderer.sprite = _doorFacesEastSprite;
      }
      else if (_direction == TowerDirection.DoorFacesWest) {
        _backFloorEdgeSpriteRenderer.enabled = false;
        _mainSpriteRenderer.sprite = _doorFacesWestSprite;
      }
    }
  }
}
