using UnityEngine;
using System.Collections;
using System;

public abstract class GameEntity : MonoBehaviour {

  public GameObject MeshObject = null;

  private float _orientation;
  public float orientation {
    get {
      return _orientation;
    }
    set {
      while (value >= 360) value -= 360;
      while (value < 0) value += 360;
      float dO = _orientation - value;
      _orientation = value;
      MeshObject.transform.Rotate(new Vector3(0, 0, dO));
    }
  }

}
