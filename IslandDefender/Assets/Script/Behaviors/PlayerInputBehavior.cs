using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerInputBehavior : MonoBehaviour {
  public GameObject MainCamera;
  public float RotationSpeed;
  public bool InputEnabled;

  private ShipEntity _entity;

  public void Awake() {
    _entity = gameObject.GetComponent<ShipEntity>();
  }

  public void Update() {
    if (!InputEnabled) {
      return;
    }

    int i = 0;
    while (i < Input.touchCount) {
      if (Input.GetTouch(i).phase == TouchPhase.Moved) {
	Vector2 touchDelta = Input.GetTouch(i).deltaPosition;
	float targetAngle = (float)(Math.Atan2(touchDelta.y, touchDelta.x) * 180 / Math.PI) - 135;
	if (targetAngle < 0) {
	  targetAngle += 360;
	}

	float rotSpeed = RotationSpeed * Time.deltaTime;
	float angle = _entity.orientation;

	bool ccw = targetAngle < angle;
	if (Math.Abs(targetAngle - angle) > 180) {
	  ccw = !ccw;
	}

	if (Math.Abs(targetAngle - angle) < rotSpeed) {
	  angle = targetAngle;
	} else {
	  angle += (ccw ? -1 : 1) * rotSpeed;     
	}

	_entity.orientation = angle;
      }
      i++;
    }
  }

}
