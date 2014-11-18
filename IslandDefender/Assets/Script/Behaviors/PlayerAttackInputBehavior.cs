using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttackInputBehavior : MonoBehaviour {
  public bool inputEnabled;

  private ShipEntity _entity;
  private Dictionary<int,float> _touchDurations = new Dictionary<int,float>();

  public void Awake() {
    _entity = gameObject.GetComponent<ShipEntity>();
  }

  public void Update() {
    if (!inputEnabled) {
      return;
    }
    
    int i = 0;
    while (i < Input.touchCount) {
      Touch touch = Input.GetTouch(i++);

      if (touch.phase == TouchPhase.Began) {
	_touchDurations[touch.fingerId] = 0;

      } else if (touch.phase == TouchPhase.Ended) {
	if (_touchDurations[touch.fingerId] < .5) {
	  _entity.FireCannons();
	}

      } else if (touch.phase == TouchPhase.Moved && touch.deltaPosition.magnitude > 2) {
	_touchDurations[touch.fingerId] += .1f;

      } else {
	_touchDurations[touch.fingerId] += Time.deltaTime;

      }
    }
  }

}
