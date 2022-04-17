using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
	private const float NOT_MOVING_TRESHOLD = 0.05f;

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
	
	

	private void Awake()
	{
		lastPos = startPos.Copy;
	}

	private void OnMouseDown()
	{
		clicked = true;
	}

	private void OnMouseUp()
	{
		lineRenderer.enabled = false;

		rb.AddForce(hitVector * forceMultiplier);
		hitVector = Vector3.zero;
		clicked = false;
	}

	private void OnCollisionStay(Collision collision)
	{
		activeCollision = collision.collider;
	}

	private void OnCollisionExit(Collision collision)
	{
		activeCollision = null;
	}

	private void Update()
	{
		Planet planet = activeCollision?.GetComponent<Planet>();
		bool onSafePlanet = !planet?.DestroysBall ?? false;
		float? distanceToPlanet = (planet?.transform.position - transform.position)?.magnitude;
		bool notMoving = lastDistanceToPlanet - distanceToPlanet < NOT_MOVING_TRESHOLD;

		stand = onSafePlanet && notMoving;

		// save new checkpoint
		if (stand)
		{
			lastPos.planet = planet;
			lastPos.relativePos = planet.transform.InverseTransformPoint(transform.position);
		}

		// draw hit line
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

			lineRenderer.SetPosition(0, transform.position);
			lineRenderer.SetPosition(1, transform.position + hitVector);
		}
		lastDistanceToPlanet = distanceToPlanet;

		RenderBallIndicator();
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
		gameObject.SetActive(true);
		rb.MovePosition(startPos.Position);
		lastPos = startPos.Copy;
		rb.velocity = Vector3.zero;
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

		public Vector3 Position => planet.transform.TransformPoint(relativePos);
	}
}
