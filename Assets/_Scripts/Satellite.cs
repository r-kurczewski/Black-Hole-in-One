using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Satellite : MonoBehaviour
{
	[SerializeField]
	private Transform rotationCenter;

	[SerializeField][FormerlySerializedAs("orbitalSpeed")]
	private float orbitalSpeed;

	[SerializeField]
	[FormerlySerializedAs("clockwise")]
	private bool orbitClockwise;

	private Rigidbody rb;

	private float orbitAngle;

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
		orbitAngle = Vector2.SignedAngle(Vector3.right, -gravityVector);
	}

	private void FixedUpdate()
	{
		var gravityVector = rotationCenter.position - transform.position;
		var normalizedPosition = new Vector3(Mathf.Cos(orbitAngle * Mathf.PI / 180), Mathf.Sin(orbitAngle * Mathf.PI / 180));
		var newPosition = rotationCenter.position + gravityVector.magnitude * normalizedPosition;
		orbitAngle += orbitClockwise ? -orbitalSpeed : orbitalSpeed;
		rb.MovePosition(newPosition);
	}

	private void OnDrawGizmos()
	{
		var gravityVector = rotationCenter.position - transform.position;
		Gizmos.DrawLine(rotationCenter.position, rotationCenter.position - gravityVector);
		Gizmos.DrawLine(rotationCenter.position, rotationCenter.position + Vector3.right * gravityVector.magnitude);
	}

}
