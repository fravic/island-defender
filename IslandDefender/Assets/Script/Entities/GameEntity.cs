using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]

public abstract class GameEntity : MonoBehaviour {

  public GameObject meshObject = null;

  public float orientation {
    get {
      return -(rigidbody.rotation.eulerAngles.y - 90);
    }
  }

  public virtual void Awake() {
    GameObject water = GameObject.Find("Water");
    Physics.IgnoreCollision(collider, water.collider);
  }

  public abstract void FixedUpdate();

}
