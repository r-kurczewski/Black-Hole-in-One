using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		var ball = LevelController.instance.Ball;
		if (other.gameObject == ball.gameObject)
		{
			LevelController.instance.Win();
		}
		else Destroy(other.gameObject);
	}
}
