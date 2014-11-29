using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerRotationInputBehavior : MonoBehaviour {
  public bool inputEnabled;
  public GameObject rudderImage;
  public float rudderRadialAcceleration = 50;

  private ShipEntity _entity;
  private TouchDistValidator _touchDistV;
  private AngleDiffTransformer _angleDiffT;
  private AngleQueueTransformer _angleQueueT;

  public void Awake() {
    _entity = gameObject.GetComponent<ShipEntity>();

    _touchDistV = new TouchDistValidator();
    _angleDiffT = new AngleDiffTransformer();
    _angleQueueT = new AngleQueueTransformer();
  }

  public void Update() {
    if (!inputEnabled) {
      return;
    }
    
    if (Input.touchCount > 0) {
      Touch touch = Input.GetTouch(0);
      if (touch.phase == TouchPhase.Began) {
	TouchBegan(touch);
      } else if (touch.phase == TouchPhase.Moved) {
	TouchMoved(touch);
      }
    }
  }

  private void TouchBegan(Touch touch) {
    _touchDistV.Reset();
    _angleDiffT.Reset();
    _angleQueueT.Reset();
  }

  private void TouchMoved(Touch touch) {
    if (_touchDistV.IsValid(touch)) {
      float angleDiff = _angleDiffT.Transform(touch);
      int torque = _angleQueueT.Transform(angleDiff);
      if (torque != 0) {
	_entity.RadialAccelerate(torque);

	Vector3 rudderTorque = new Vector3(0,0,-torque * rudderRadialAcceleration);
	rudderImage.rigidbody.AddTorque(rudderTorque, ForceMode.Acceleration);
      }
    }
  }


  /* TouchDistTransformer filters raw touches for significant distance */
  private class TouchDistValidator {
    private const int MIN_TOUCH_DELTA = 5;

    private Vector2 _lastRegTouchPos = Vector2.zero;
    private bool _lastRegTouchPosSet = false;

    public bool IsValid(Touch input) {
      if (_lastRegTouchPosSet) {
	Vector2 touchDelta = input.position - _lastRegTouchPos;
	if (touchDelta.magnitude > MIN_TOUCH_DELTA) {
	  SetLastRegTouchPos(input.position);
	  return true;
	}
      }
      SetLastRegTouchPos(input.position);
      return false;
    }

    public void Reset() {
      _lastRegTouchPosSet = false;
    }

    private void SetLastRegTouchPos(Vector2 val) {
      _lastRegTouchPos = val;
      _lastRegTouchPosSet = true;
    }
  }


  /* AngleDiffTransformer keeps track of touch angles and returns their
     differences */
  private class AngleDiffTransformer {
    private float _lastAngle = 0;
    private bool _lastAngleSet = false;

    public float Transform(Touch input) {
      float diff = 0;
      float angle = (float)Math.Atan2(input.deltaPosition.y,
				      input.deltaPosition.x)
	                     * 180 / (float)Math.PI;
      angle = Utils.BoundAngle(angle);

      if (angle != _lastAngle && _lastAngleSet) {
	if (angle > 270 && _lastAngle < 90) {
	  _lastAngle += 360;
	} else if (_lastAngle > 270 && angle < 90) {
	  angle += 360;
	}
	diff = -(angle - _lastAngle);
      }

      SetLastAngle(angle);
      return diff;
    }

    public void Reset() {
      _lastAngleSet = false;
    }

    private void SetLastAngle(float val) {
      _lastAngle = Utils.BoundAngle(val);
      _lastAngleSet = true;
    }
  }


  /* AngleQueueTransformer keeps a queue of angle differences until it gets
     big enough, then returns a direction to rotate, cw or ccw. */
  private class AngleQueueTransformer {
    private const int ANGLE_DIFFS_NEEDED = 5;
    private const float MIN_NECESSARY_SUM = 10;

    private Queue<float> _angleDiffs = new Queue<float>();

    public int Transform(float input) {
      if (input == 0) return 0;

      _angleDiffs.Enqueue(input);
      if (_angleDiffs.Count > ANGLE_DIFFS_NEEDED) {
	float sum = Utils.SumArray(_angleDiffs.ToArray(), 0.0f);
	_angleDiffs.Clear();

	if (sum > MIN_NECESSARY_SUM) {
	  return 1;
	} else if (sum < -MIN_NECESSARY_SUM) {
	  return -1;
	}
      }
      return 0;
    }

    public void Reset() {
      _angleDiffs.Clear();
    }
  }
}
