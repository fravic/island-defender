using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Rigidbody))]

public abstract class GameEntity : MonoBehaviour {

  public GameObject meshObject = null;

  public float rotationSpeed = 90;     // Degrees per second
  public float maxSpeed = 7;
  public float acceleration = 5;
  public float driftDragFactor = 3;

  private float _targetOrientation = 0;
  public float targetOrientation {
    get {
      return _targetOrientation;
    }
    set {
      _targetOrientation = Utils.BoundAngle(value);
    }
  }

  private float _orientation;
  public float orientation {
    get {
      return _orientation;
    }
    set {
      value = Utils.BoundAngle(value);
      float dO = _orientation - value;
      meshObject.transform.Rotate(new Vector3(0, 0, dO));
      _orientation = value;
    }
  }

  private bool _shouldAccelerate;

  public void FixedUpdate() {
    UpdateAccelerationForce();
    LimitVelocity();
    UpdateWaterForce();
  }

  public void UpdateOrientation() {
    float rotSpeed = rotationSpeed * (rigidbody.velocity.magnitude / maxSpeed) * Time.deltaTime;

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

  public void Accelerate() {
    _shouldAccelerate = true;
  }

  void UpdateAccelerationForce() {
    if (_shouldAccelerate) {
      float angleRad = (_orientation + 90) * (float)Math.PI / 180;
      float dX = (float)Math.Cos(angleRad) * acceleration;
      float dZ = (float)Math.Sin(angleRad) * acceleration;
      Vector3 force = new Vector3(dX,0,dZ);
      rigidbody.AddForce(force);

      _shouldAccelerate = false;
    }
  }

  void LimitVelocity() {
    rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, maxSpeed);

    Debug.DrawLine(rigidbody.position, rigidbody.position + rigidbody.velocity, Color.green);
  }

  void UpdateWaterForce() {
    Vector3 rightDrift = Quaternion.Euler(0,-_orientation+90,0) * Vector3.forward;
    Vector3 undrift = Vector3.Project(-rigidbody.velocity * driftDragFactor, rightDrift);
    rigidbody.AddForce(undrift);

    Debug.DrawLine(rigidbody.position, rigidbody.position + undrift, Color.red);
  }
}
