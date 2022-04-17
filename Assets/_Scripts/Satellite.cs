using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satellite : MonoBehaviour
{
	[SerializeField]
	private Transform rotationCenter;

	[SerializeField]
	private float speed;

	[SerializeField]
	private bool clockwise;

	private Rigidbody rb;

	private float angle;

	public Vector3 Velocity => rb.velocity;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void Start()
	{
		CalculateAngle();
	}

	private void CalculateAngle()
	{
		var gravityVector = rotationCenter.position - transform.position;
		angle = Vector2.SignedAngle(Vector3.right, -gravityVector);
	}

	private void FixedUpdate()
	{
		var gravityVector = rotationCenter.position - transform.position;
		var normalizedPosition = new Vector3(Mathf.Cos(angle * Mathf.PI / 180), Mathf.Sin(angle * Mathf.PI / 180));
		var newPosition = rotationCenter.position + gravityVector.magnitude * normalizedPosition;
		rb.MovePosition(newPosition);
		angle += (clockwise ? -speed : speed);
	}

	private void OnDrawGizmos()
	{
		var gravityVector = rotationCenter.position - transform.position;
		Gizmos.DrawLine(rotationCenter.position, rotationCenter.position - gravityVector);
		Gizmos.DrawLine(rotationCenter.position, rotationCenter.position + Vector3.right * gravityVector.magnitude);
	}

}
