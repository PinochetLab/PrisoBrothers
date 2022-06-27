using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatSystem : MonoBehaviour
{
	[SerializeField] private float speed = 6;
	[SerializeField] private float spawnTime = 3f;
	[SerializeField] private GameObject ratPrefab;
	[SerializeField] private bool flipRat;
	[SerializeField] private bool noDelay;
	[SerializeField] private int ratsCount = 1;
	[SerializeField] private Transform end;

	private float time = 0;

	private void Start() {
		time = spawnTime;
		float distance = Vector3.Distance(end.position, transform.position);
		float _time = distance / speed / ratsCount;
		if ( noDelay ) {
			spawnTime = _time;
			time = _time;
		}
	}

	private void Update() {
		time += Time.deltaTime;
		if (time > spawnTime ) {
			time = 0;
			Spawn();
		}
	}

	private void Spawn() {
		GameObject rat = Instantiate(ratPrefab, transform);
		Rigidbody2D ratRB = rat.GetComponentInChildren<Rigidbody2D>();
		if ( flipRat ) {
			Vector3 s = rat.transform.localScale;
			s.y = -Mathf.Abs(s.y);
			rat.transform.localScale = s;
		}
		ratRB.velocity = transform.right * speed;
		float distance = Vector3.Distance(end.position, transform.position) + 1.5f;
		float time = distance / speed;
		Destroy(rat, time);
	}
}
