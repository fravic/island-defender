using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MovementBehavior : MonoBehaviour {

  private ShipEntity _entity;

  public void Awake() {
    _entity = gameObject.GetComponent<ShipEntity>();
  }

  public void Update() {
  }

}
