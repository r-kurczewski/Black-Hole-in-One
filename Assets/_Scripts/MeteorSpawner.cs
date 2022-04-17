using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MeteorSpawner : MonoBehaviour
{
	[SerializeField]
	private Meteor meteorPrefab;

	[SerializeField]
	private Vector3 endPosition;

	[SerializeField]
	private float interval;

	[SerializeField]
	private float startDelay;
	
	[SerializeField]
	private float meteorSpeed;

	private float time;
	private int meteorIndex = 0;

	private void Start()
	{
		time = startDelay;
	}

	private void FixedUpdate()
	{
		if (time > interval)
		{
			var meteor = Instantiate(meteorPrefab, transform.position, Quaternion.identity, transform);
			meteor.name = $"Meteor {meteorIndex}";
			var force = (endPosition - transform.position) * meteorSpeed;
;			meteor.GetComponent<Rigidbody>().AddForce(force);
			meteorIndex++;
			time = 0;
		}
		else time += Time.fixedDeltaTime;
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, 1f);
		Gizmos.DrawLine(transform.position, endPosition);
	}
}
