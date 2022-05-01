using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : GravitySource
{
	[SerializeField]
	private bool _destroysBall;

	[SerializeField]
	private bool _ballCheckpoint;

	[SerializeField]
	private float rotationSpeed = 0.05f;

	[SerializeField]
	private bool clockwise;

	private Rigidbody rb;

	public bool DestroysBall => _destroysBall;

	public bool BallCheckpoint => _ballCheckpoint;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
	}


	private void OnCollisionEnter(Collision collision)
	{
		bool ballCollision = collision.gameObject == LevelController.instance.Ball.gameObject;
		if (DestroysBall && ballCollision)
		{
			LevelController.instance.Lose();
		}
	}

	private new void FixedUpdate()
	{
		base.FixedUpdate();
		rb.MoveRotation(Quaternion.Euler(0, Time.fixedDeltaTime * rotationSpeed, 0) * rb.rotation);
	}
}
