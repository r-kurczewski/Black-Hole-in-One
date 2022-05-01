using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
	private const float NOT_MOVING_TRESHOLD = 0.05f;
	private const float APPLY_FORCE_THRESHOLD = 0.5f;
	[SerializeField]
	private LineRenderer lineRenderer;

	[SerializeField]
	private GameObject indicator;

	[SerializeField]
	public Rigidbody rb;

	[SerializeField]
	private float maxHitLineLength;

	[SerializeField]
	private float forceMultiplier;

	[SerializeField]
	private Vector3 hitVector;

	private bool clicked;

	[SerializeField]
	private bool stand;

	private Collider activeCollision;

	[SerializeField]
	private BallPosition startPos;

	[SerializeField]
	private BallPosition lastPos;

	private float? lastDistanceToPlanet;
	private float? distanceToPlanet;

	private bool GamePaused => LevelController.instance.GamePaused;

	private void Start()
	{
		lastPos = startPos.Copy;
		LevelController.instance.Ball = this;
	}

	private void OnMouseDown()
	{
		if (GamePaused) return;

		clicked = true;
	}

	private void OnMouseUp()
	{
		if (GamePaused) return;

		lineRenderer.enabled = false;

		if (hitVector.magnitude > APPLY_FORCE_THRESHOLD)
		{
			rb.AddForce(hitVector * forceMultiplier);
			LevelController.instance.IncreaseHitCounter();
		}
		ResetHit();
		clicked = false;
	}

	private void ResetHit()
	{
		hitVector = Vector3.zero;
		LevelController.instance.SetHitPower(0);
	}

	private void OnCollisionStay(Collision collision)
	{
		activeCollision = collision.collider;
	}

	private void OnCollisionExit(Collision collision)
	{
		activeCollision = null;
	}

	private void FixedUpdate()
	{
		rb.MovePosition(rb.position * Vector2.one); // pull ball to cord z=0
	}

	private void Update()
	{
		if (GamePaused)
		{
			if (clicked)
			{
				lineRenderer.enabled = false;
				ResetHit();
				clicked = false;
			}
			return;
		}

		UpdateBallSpawnPoint();
		UpdateHitVector();
		UpdateHitLine();
		RenderBallIndicator();
		lastDistanceToPlanet = distanceToPlanet;
	}

	private void UpdateBallSpawnPoint()
	{
		Planet planet = activeCollision?.GetComponent<Planet>();
		bool isCheckpoint = planet?.BallCheckpoint ?? false;
		distanceToPlanet = (planet?.transform.position - transform.position)?.magnitude;
		bool notMoving = lastDistanceToPlanet - distanceToPlanet < NOT_MOVING_TRESHOLD;

		stand = isCheckpoint && notMoving;

		// save new checkpoint
		if (stand)
		{
			lastPos.planet = planet;
			lastPos.relativePos = transform.position - planet.transform.position;
			lastPos.relativePos.z = 0; // resets influence of planet rotation 
		}
	}

	private void UpdateHitVector()
	{
		if (clicked && stand)
		{
			lineRenderer.enabled = true;
			var mousePos = Input.mousePosition;
			mousePos.z = -Camera.main.transform.position.z;
			mousePos = Camera.main.ScreenToWorldPoint(mousePos);
			mousePos.z = 0;

			hitVector = mousePos - transform.position;
			if (hitVector.magnitude > maxHitLineLength)
			{
				hitVector = hitVector.normalized * maxHitLineLength;
			}
			float hitPower = hitVector.magnitude > APPLY_FORCE_THRESHOLD ? hitVector.magnitude : 0;
			LevelController.instance.SetHitPower(hitPower);
		}
	}

	private void UpdateHitLine()
	{
		lineRenderer.SetPosition(0, transform.position);
		lineRenderer.SetPosition(1, transform.position + hitVector);
	}

	public void RestoreLastPosition()
	{
		rb.MovePosition(lastPos.Position);
		rb.velocity = lastPos.planet.GetComponent<Satellite>()?.Velocity ?? Vector3.zero;
	}

	public void RenderBallIndicator()
	{
		var zDistance = -Camera.main.transform.position.z;
		var bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, zDistance));
		var topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, zDistance));
		var position = transform.position;
		if (position.x < bottomLeft.x || position.y < bottomLeft.y
			|| position.x > topRight.x || position.y > topRight.y)
		{
			Vector3 indicatorPos = transform.position;
			indicatorPos.x = Mathf.Clamp(indicatorPos.x, bottomLeft.x, topRight.x);
			indicatorPos.y = Mathf.Clamp(indicatorPos.y, bottomLeft.y, topRight.y);
			indicator.transform.position = indicatorPos;
			indicator.SetActive(true);
		}
		else
		{
			indicator.SetActive(false);
		}
	}

	public void RestoreStartPosition()
	{
		rb.velocity = Vector3.zero;
		gameObject.SetActive(true);
		rb.MovePosition(startPos.Position);
		lastPos = startPos.Copy;
	}

	[Serializable]
	private class BallPosition
	{
		public Planet planet;
		public Vector3 relativePos;

		public BallPosition() { }

		public BallPosition(Planet planet, Vector3 relativePos)
		{
			this.planet = planet;
			this.relativePos = relativePos;
		}

		public BallPosition Copy => new BallPosition(planet, relativePos);

		public Vector3 Position => planet.transform.position + relativePos;
	}
}
