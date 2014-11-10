using UnityEngine;
using System.Collections;
using System;

public abstract class GameEntity : MonoBehaviour {

  public GameObject MeshObject = null;

  private float _orientation;
  public float Orientation {
    get {
      return _orientation;
    }
    set {
      float dO = _orientation - value;
      _orientation = value;
      MeshObject.transform.Rotate(new Vector3(0, 0, dO));
    }
  }

}
