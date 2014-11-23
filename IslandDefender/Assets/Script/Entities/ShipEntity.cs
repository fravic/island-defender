using UnityEngine;
using System;
using System.Collections;

public class ShipEntity : GameEntity {

  public GameObject cameraTarget;
  public GameObject leftCannonfire;
  public GameObject rightCannonfire;

  public void Awake() {
  }

  public override void FixedUpdate() {
    base.FixedUpdate();

    cameraTarget.transform.position = this.rigidbody.position;
  }

  public void FireCannons() {
    leftCannonfire.particleSystem.Emit(4);
    rightCannonfire.particleSystem.Emit(4);
  }

}
