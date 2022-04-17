using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
	public static LevelController instance;

	[SerializeField]
	private Camera levelCamera;
	
	[SerializeField]
	private Camera ballCamera;

	[SerializeField]
	private Ball _ball;

	public Ball Ball => _ball;

	public bool OutOfLevelBounds(Transform target) => Vector3.Distance(transform.position, target.position) > levelBounds;

	[SerializeField]
	private float levelBounds;

	private void Awake()
	{
		if (instance is null)
		{
			instance = this;
		}
		else
		{
			Destroy(this);
		}
	}

	private void Update()
	{
		InputUpdate();

		float distanceFromStart = Vector3.Distance(transform.position, Ball.transform.position);
		if (OutOfLevelBounds(Ball.transform))
		{
			Ball.RestoreLastPosition();
		}
	}

	private void InputUpdate()
	{
		if (Input.GetButtonDown("Camera"))
		{
			ballCamera.enabled = !ballCamera.enabled;
			levelCamera.enabled = !levelCamera.enabled;
		}
		else if (Input.GetButtonDown("Reset"))
		{
			RestartLevel();
		}
		else if (Input.GetButtonDown("SoftReset"))
		{
			Ball.RestoreLastPosition();
		}
	}

	public void RestartLevel()
	{
		Ball.RestoreStartPosition();
		//ClearLevel();
	}

	private void ClearLevel()
	{
		foreach(var meteor in FindObjectsOfType<Meteor>())
		{
			Destroy(meteor.gameObject);
		}
	}

	public void Lose()
	{
		Ball.RestoreLastPosition();
	}

	public void Win()
	{
		UnityEngine.Debug.Log("Win");
		ballCamera.enabled = false;
		levelCamera.enabled = true;
		_ball.gameObject.SetActive(false);
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, levelBounds);
	}
}
