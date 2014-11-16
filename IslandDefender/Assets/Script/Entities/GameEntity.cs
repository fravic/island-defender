using UnityEngine;
using System.Collections;
using System;

public abstract class GameEntity : MonoBehaviour {

  public GameObject meshObject = null;
  public float rotationSpeed;

  private float _targetOrientation = 0;
  public float targetOrientation {
    get {
      return _targetOrientation;
    }
    set {
      _targetOrientation = BoundAngle(value);
    }
  }

  private float _orientation;
  public float orientation {
    get {
      return _orientation;
    }
    set {
      value = BoundAngle(value);
      float dO = _orientation - value;
      meshObject.transform.Rotate(new Vector3(0, 0, dO));
      _orientation = value;
    }
  }

  public void Update() {
  }

  public void UpdateOrientation() {
    float rotSpeed = rotationSpeed * Time.deltaTime;

    bool ccw = _targetOrientation < _orientation;
    if (Math.Abs(_targetOrientation - _orientation) > 180) {
      ccw = !ccw;
    }

    if (Math.Abs(_targetOrientation - _orientation) < rotSpeed) {
      this.orientation = _targetOrientation;
    } else {
      this.orientation += (ccw ? -1 : 1) * rotSpeed;
    }
  }

  float BoundAngle(float value) {
    while (value >= 360) value -= 360;
    while (value < 0) value += 360;
    return value;
  }

}
