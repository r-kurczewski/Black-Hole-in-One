using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelController : MonoBehaviour
{
	public static LevelController instance;

	[SerializeField]
	private Camera levelCamera;

	[SerializeField]
	private Camera ballCamera;

	[SerializeField]
	private float levelBounds;

	[SerializeField]
	private bool _paused;

	private Ball _ball;

	private LevelPausedView pausedView;

	private LevelCompletedView completedView;

	private BallInfoView ballInfoView;

	private int hitCount;

	public Ball Ball { get => _ball; set => _ball = value; }
	public bool GamePaused => _paused;

	public bool OutOfLevelBounds(Transform target) => Vector3.Distance(transform.position, target.position) > levelBounds;

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

		pausedView = FindObjectOfType<LevelPausedView>(true);
		completedView = FindObjectOfType<LevelCompletedView>(true);
		ballInfoView = FindObjectOfType<BallInfoView>(true);
	}
	private void Start()
	{
		DisableGamePause();
	}

	private void OnDestroy()
	{
		if (instance == this) instance = null;
	}

	private void Update()
	{
		InputUpdate();

		if (OutOfLevelBounds(Ball.transform))
		{
			Ball.RestoreLastPosition();
		}
	}

	public void SetHitPower(float hitPower)
	{
		ballInfoView.SetPower(hitPower);
	}

	public void IncreaseHitCounter()
	{
		hitCount++;
		ballInfoView.SetHitCounter(hitCount);
	}

	private void InputUpdate()
	{
		if (Input.GetButtonDown("Menu"))
		{
			ChangePauseState();
		}

		if (GamePaused) return;

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

	public void ChangePauseState()
	{
		_paused = !_paused;
		UpdateGamePauseState();
	}

	public void EnableGamePause()
	{
		_paused = true;
		UpdateGamePauseState();
	}

	public void DisableGamePause()
	{
		_paused = false;
		UpdateGamePauseState();
	}

	private void UpdateGamePauseState()
	{
		Time.timeScale = _paused ? 0 : 1;
		pausedView.gameObject.SetActive(_paused);
	}

	public void RestartLevel()
	{
		Ball.RestoreStartPosition();
		hitCount = 0;
		ballInfoView.SetHitCounter(hitCount);
	}

	public void Lose()
	{
		Ball.RestoreLastPosition();
	}

	public void Win()
	{
		completedView.gameObject.SetActive(true);
		completedView.SetHits(hitCount);
		ballCamera.enabled = false;
		levelCamera.enabled = true;
		Ball.gameObject.SetActive(false);
	}

	private void OnDrawGizmos()
	{
		Handles.DrawWireDisc(transform.position, Vector3.forward, levelBounds);
	}
}
