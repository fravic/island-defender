using UnityEngine;
using System;
using System.Collections;

public class ShipEntity : GameEntity {

  public GameObject cameraTarget;
  public GameObject leftCannonfire;
  public GameObject rightCannonfire;

  public float maxSpeed = 100;
  public float acceleration = 500;
  public float radialAcceleration = 20;
  public float driftDrag = 20;

  private bool _shouldAccelerate = false;
  private int _shouldApplyTorqueDir = 0;

  public override void FixedUpdate() {
    UpdateAcceleration();
    UpdateTorque();
    LimitVelocity();
    UpdateWaterForce();

    cameraTarget.transform.position = this.rigidbody.position;
  }

  public void FireCannons() {
    leftCannonfire.particleSystem.Emit(4);
    rightCannonfire.particleSystem.Emit(4);
  }

  public void Accelerate() {
    _shouldAccelerate = true;
  }

  // torque = 1 is cw, -1 is ccw, 0 is no torque
  public void RadialAccelerate(int torqueDir) {
    _shouldApplyTorqueDir = torqueDir;
  }

  void UpdateAcceleration() {
    if (_shouldAccelerate) {
      float orientationRad = this.orientation * (float)Math.PI / 180;
      float dX = (float)Math.Cos(orientationRad) * acceleration;
      float dZ = (float)Math.Sin(orientationRad) * acceleration;
      Vector3 force = new Vector3(dX,0,dZ);
      rigidbody.AddForce(force, ForceMode.Acceleration);

      _shouldAccelerate = false;
    }
  }

  void UpdateTorque() {
    if (_shouldApplyTorqueDir != 0) {
      Vector3 torque = Vector3.up * _shouldApplyTorqueDir * radialAcceleration;
      rigidbody.AddTorque(torque, ForceMode.Acceleration);

      _shouldApplyTorqueDir = 0;
    }
  }

  void LimitVelocity() {
    rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, maxSpeed);
  }

  void UpdateWaterForce() {
    Vector3 rightDrift = Quaternion.Euler(0,-this.orientation,0) * Vector3.forward;
    Vector3 undrift = Vector3.Project(-rigidbody.velocity * driftDrag, rightDrift);
    rigidbody.AddForce(undrift, ForceMode.Acceleration);
  }

}
