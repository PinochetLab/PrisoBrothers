using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
	[SerializeField] private float speed = 100;

	public void SetSpeed(float speed) {
		this.speed = speed;
	}

	private void Update() {
		transform.Rotate(-Vector3.forward * speed * Time.deltaTime);
	}
}
