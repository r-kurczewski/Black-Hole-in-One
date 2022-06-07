using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class GravitySource : MonoBehaviour
{
	[SerializeField][FormerlySerializedAs("gravity")]
	private float _gravity;

	private Rigidbody[] gravityTargets;

	public float Gravity => _gravity;

	private void Awake()
	{
		UpdateGravityTargets();
	}

	private void UpdateGravityTargets()
	{
		gravityTargets = GameObject.FindGameObjectsWithTag("GravityTarget")
			.Select(x=> x.GetComponent<Rigidbody>())
			.ToArray();
	}

	public void ApplyGravity(Rigidbody target)
	{
		var directionVector = (transform.position - target.transform.position);
		var distanceSquare = Mathf.Pow(directionVector.magnitude, 2);
		var gravity = directionVector.normalized * target.mass * Gravity / distanceSquare;
		if (gravity.magnitude > 0) target.AddForce(gravity);

		//var source = transform;
		//Debug.Log($"{source.name} pulls {target.name} with {gravity.magnitude * 10} force", source);
	}
	protected void FixedUpdate()
	{
		foreach (var target in gravityTargets)
		{
			ApplyGravity(target);
		}
	}
}
