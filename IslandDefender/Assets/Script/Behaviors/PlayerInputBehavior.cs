using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerInputBehavior : MonoBehaviour {
  public bool inputEnabled;

  private ShipEntity _entity;
  private Queue<Vector2> _pastTouchDeltas = new Queue<Vector2>();
  private Vector2 _lastRegTouchPos;

  private const int _minTouchDelta = 6;
  private const int _avgTouchCount = 5;

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

	  _pastTouchDeltas.Enqueue(touchDelta);
	  if (_pastTouchDeltas.Count > _avgTouchCount) {
	    _pastTouchDeltas.Dequeue();
	  }

	  // 135deg is based on camera angle, will fix later
	  Vector2 avg = Utils.AvgVector2(_pastTouchDeltas.ToArray());
	  _entity.targetOrientation = (float)(Math.Atan2(avg.y, avg.x) * 180 / Math.PI) - 135;
	}

      } else if (touch.phase == TouchPhase.Ended) {
	_entity.targetOrientation = _entity.orientation;
      }

      _entity.UpdateOrientation();
      _entity.Accelerate();
    }
  }

}
