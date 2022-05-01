using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovementGizmos : MonoBehaviour
{
	[SerializeField]
	private float gizmoMultiplier;
	
	[SerializeField]
	private Rigidbody rb;

	private Vector3 lastVelocity;
	private Vector3 acceleration;
	private Planet planet;

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawLine(transform.position, transform.position + rb.velocity * gizmoMultiplier);
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(transform.position, transform.position + acceleration * gizmoMultiplier);
			
	}

	private void FixedUpdate()
	{
		acceleration = (rb.velocity - lastVelocity) / Time.fixedDeltaTime;
		lastVelocity = rb.velocity;
	}

	private void OnCollisionStay(Collision collision)
	{
		planet = collision.collider.GetComponent<Planet>();
	}
}
