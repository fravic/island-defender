using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerAccelerationInputBehavior : MonoBehaviour {
  public bool inputEnabled;
  public float maxSpeed = 150;

  private ShipEntity _entity;
  private float _targetSpeed = 0;

  public void Awake() {
    _entity = gameObject.GetComponent<ShipEntity>();
  }

  public void Update() {
    if (!inputEnabled) {
      return;
    }

    if (_entity.rigidbody.velocity.magnitude < _targetSpeed) {
      _entity.Accelerate();
    }
  }

  public void setTargetVelocityPercentage(float target) {
    _targetSpeed = target * maxSpeed;
  }
}
