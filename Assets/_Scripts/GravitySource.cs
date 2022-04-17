using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GravitySource : MonoBehaviour
{
	[SerializeField][FormerlySerializedAs("gravity")]
	private float _gravity;

	public float Gravity => _gravity;

	public void ApplyGravity(Rigidbody target)
	{
		var directionVector = (transform.position - target.transform.position);
		var distanceSquare = Mathf.Pow(directionVector.magnitude, 2);
		var gravity = directionVector.normalized * target.mass * this._gravity / distanceSquare;
		if (gravity.magnitude > 0) target.AddForce(gravity);

		//var source = transform;
		//Debug.Log($"{source.name} pulls {target.name} with {gravity.magnitude * 10} force", source);
	}
	private void FixedUpdate()
	{
		foreach (var target in GameObject.FindGameObjectsWithTag("GravityTarget"))
		{
			ApplyGravity(target.GetComponent<Rigidbody>());
		}
	}
}
