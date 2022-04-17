using System.Collections;
using UnityEngine;

public class Meteor : MonoBehaviour
{
	private void Start()
	{
		InvokeRepeating("CheckOutOfBounds", 3, 3);
	}

	public void CheckOutOfBounds()
	{
		if (LevelController.instance.OutOfLevelBounds(transform))
		{
			Destroy(gameObject);
		}
	}
}