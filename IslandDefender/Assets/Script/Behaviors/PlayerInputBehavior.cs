using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerInputBehavior : MonoBehaviour {
  public bool inputEnabled;

  private ShipEntity _entity;

  private Vector2 _touchStartPos;

  public void Awake() {
    _entity = gameObject.GetComponent<ShipEntity>();
  }

  public void Update() {
    if (!inputEnabled) {
      return;
    }
    
    if (Input.touchCount > 0) {
      Touch touch = Input.GetTouch(0);
      if (touch.phase == TouchPhase.Began) {
	_touchStartPos = touch.position;
      } else if (touch.phase == TouchPhase.Moved) {
	Vector2 touchDelta = touch.position - _touchStartPos;
	// 135deg is based on camera angle, will fix later
	_entity.targetOrientation = (float)(Math.Atan2(touchDelta.y, touchDelta.x) * 180 / Math.PI) - 135;

	_entity.UpdateOrientation();
      }

      _entity.Accelerate();
    }
  }

}
