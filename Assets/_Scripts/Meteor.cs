using System.Collections;
using UnityEngine;

public class Meteor : MonoBehaviour
{
	private const float DESTROY_TIME = 5f;

	private void Start()
	{
		Invoke("Destroy", DESTROY_TIME);
	}

	public void Destroy()
	{
		Destroy(gameObject);
	}
}