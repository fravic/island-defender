using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovementInputBehavior : MonoBehaviour {
  public bool inputEnabled;

  private ShipEntity _entity;

  private float _lastAngle;
  private Queue<float> _angleDiffs;
  private Vector2 _lastRegTouchPos;

  private const int _minTouchDelta = 5;

  public void Awake() {
    _entity = gameObject.GetComponent<ShipEntity>();
    _angleDiffs = new Queue<float>();
  }

  public void Update() {
    if (!inputEnabled) {
      return;
    }
    
    if (Input.touchCount > 0) {
      Touch touch = Input.GetTouch(0);

      if (touch.phase == TouchPhase.Began) {
	_lastRegTouchPos = touch.position;
	_lastAngle = 1000; // 'Unset' flag: refactor this
	_angleDiffs.Clear();

      } else if (touch.phase == TouchPhase.Moved) {
	Vector2 touchDelta = touch.position - _lastRegTouchPos;
	if (touchDelta.magnitude > _minTouchDelta) {
	  _lastRegTouchPos = touch.position;
	  float angle = (float)Math.Atan2(touch.deltaPosition.y, touch.deltaPosition.x) * 180 / (float)Math.PI;
	  angle = Utils.BoundAngle(angle);

	  if (angle != _lastAngle && _lastAngle != 1000) {
	    if (angle > 270 && _lastAngle < 90) {
	      _lastAngle += 360;
	    } else if (_lastAngle > 270 && angle < 90) {
	      angle += 360;
	    }
	    _angleDiffs.Enqueue(angle - _lastAngle);
	  }
	  _lastAngle = Utils.BoundAngle(angle);
	}

	if (_angleDiffs.Count > 5) {
	  float sum = Utils.SumArray(_angleDiffs.ToArray(), 0.0f);
	  if (sum > 20) {
	    _entity.targetOrientation += 20;
	  } else if (sum < -20) {
	    _entity.targetOrientation -= 20;
	  }
	  _angleDiffs.Clear();
	}

      } else if (touch.phase == TouchPhase.Ended) {
	_entity.targetOrientation = _entity.orientation;

      }

      _entity.UpdateOrientation();
      _entity.Accelerate();
    }
  }

}
