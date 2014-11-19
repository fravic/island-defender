using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Utils {
  public static float BoundAngle(float value) {
    while (value >= 360) value -= 360;
    while (value < 0) value += 360;
    return value;
  }

  public static Vector2 AvgVector2(IList<Vector2> vectors) {
    float x = 0f;
    float y = 0f;
    foreach (Vector2 vec in vectors) {
      x += vec.x;
      y += vec.y;
    }
    return new Vector2(x, y) / vectors.Count;
  }

  public static float SumArray(IList array, float init) {
    float sum = init;
    foreach (float i in array) {
      sum = sum + i;
    }
    return sum;
  }
}