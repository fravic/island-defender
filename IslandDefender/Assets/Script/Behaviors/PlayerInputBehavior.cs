using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerInputBehavior : MonoBehaviour {
  public GameObject MainCamera;
  public bool InputEnabled = true;

  private ShipEntity _entity;

  public void Awake() {
    _entity = gameObject.GetComponent<ShipEntity>();
  }

  public void Update() {
    if (!InputEnabled) {
      return;
    }

    if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved) {
      Vector2 touchDelta = Input.GetTouch(0).deltaPosition;
      _entity.Orientation -= touchDelta.y;
    }
  }

}
