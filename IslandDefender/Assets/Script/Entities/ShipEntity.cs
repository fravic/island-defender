using UnityEngine;
using System;
using System.Collections;

public class ShipEntity : GameEntity {

  public GameObject cameraTarget;
  public GameObject leftCanonfire;
  public GameObject rightCanonfire;

  public void Awake() {
  }

  public override void FixedUpdate() {
    base.FixedUpdate();

    cameraTarget.transform.position = this.rigidbody.position;
  }

  public void FireCannons() {
    leftCanonfire.particleSystem.Emit(4);
    rightCanonfire.particleSystem.Emit(4);
  }

}
