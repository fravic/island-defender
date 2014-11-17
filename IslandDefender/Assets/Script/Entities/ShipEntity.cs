using UnityEngine;
using System;
using System.Collections;

public class ShipEntity : GameEntity {

  public GameObject cannonfireTemplate;

  private GameObject _leftCanonfire;
  private GameObject _rightCanonfire;

  public void Awake() {
    _leftCanonfire = (GameObject)Instantiate(cannonfireTemplate, transform.position + new Vector3(0, 5, 0), transform.rotation * new Quaternion(90, -12, -90, 1));
    _leftCanonfire.transform.parent = meshObject.transform;

    _rightCanonfire = (GameObject)Instantiate(cannonfireTemplate, transform.position + new Vector3(0, 5, 0), transform.rotation * new Quaternion(-90, -12, -90, 1));
    _rightCanonfire.transform.parent = meshObject.transform;
  }

  public void FireCannons() {
    _leftCanonfire.particleSystem.Emit(4);
    _rightCanonfire.particleSystem.Emit(4);
  }

}
