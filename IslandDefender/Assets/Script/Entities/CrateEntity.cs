using UnityEngine;
using System;
using System.Collections;

public class CrateEntity : GameEntity {

  public GameObject explosionTemplate;

  private int _health = 10;

  public override void FixedUpdate() {
  }

  void OnParticleCollision(GameObject other) {
    ParticleSystem particleSystem;
    particleSystem = other.GetComponent<ParticleSystem>();

    int safeLength = particleSystem.safeCollisionEventSize;
    ParticleSystem.CollisionEvent[] collisionEvents = new ParticleSystem.CollisionEvent[safeLength];
        
    int numCollisionEvents = particleSystem.GetCollisionEvents(gameObject, collisionEvents);
    int i = 0;
    while (i < numCollisionEvents) {
      if (rigidbody) {
	Vector3 force = collisionEvents[i].velocity;
	gameObject.rigidbody.AddForce(force);
      }
      i++;
    }

    _health -= numCollisionEvents;
    if (_health <= 0) {
      Instantiate(explosionTemplate, rigidbody.position, Quaternion.identity);
      Destroy(gameObject);
    }
  }

}
