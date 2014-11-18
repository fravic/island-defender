using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovementInputBehavior : MonoBehaviour {
  public bool inputEnabled;

  private ShipEntity _entity;

  private float _lastAngle;
  private Vector2 _lastRegTouchPos;
  private const int _minTouchDelta = 6;

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
	_lastRegTouchPos = touch.position;

      } else if (touch.phase == TouchPhase.Moved) {
	Vector2 touchDelta = touch.position - _lastRegTouchPos;
	if (touchDelta.magnitude > _minTouchDelta) {
	  _lastRegTouchPos = touch.position;

	  float angle = (float)Math.Atan2(touch.deltaPosition.y, touch.deltaPosition.x) * 180 / (float)Math.PI;
	  angle = Utils.BoundAngle(angle);

	  if (angle != _lastAngle) {
	    if (angle > 270 && _lastAngle < 90) {
	      _lastAngle += 360;
	    } else if (_lastAngle > 270 && angle < 90) {
	      angle += 360;
	    }

	    bool clockwise = (angle - _lastAngle > 0);
	    if (clockwise) {
	      _entity.targetOrientation += 3;
	    } else {
	      _entity.targetOrientation -= 3;
	    }
	    _lastAngle = angle;
	  }
	}

      } else if (touch.phase == TouchPhase.Ended) {
	_entity.targetOrientation = _entity.orientation;
      }

      _entity.UpdateOrientation();
      _entity.Accelerate();
    }
  }

}
