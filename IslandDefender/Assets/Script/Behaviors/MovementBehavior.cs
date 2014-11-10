using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MovementBehavior : MonoBehaviour {

  public float Speed;

  private ShipEntity _entity;

  public void Awake() {
    _entity = gameObject.GetComponent<ShipEntity>();
  }

  public void Update() {
    float angleRad = (_entity.Orientation + 90) * (float)Math.PI / 180;
    float frameSpeed = Speed * Time.deltaTime;
    float dX = frameSpeed * (float)Math.Cos(angleRad);
    float dZ = frameSpeed * (float)Math.Sin(angleRad);
    gameObject.transform.Translate(dX, 0, dZ);
  }

}
