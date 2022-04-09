using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPart : MonoBehaviour
{
	private Rigidbody2D rb;
	private SpriteRenderer sr;
	[SerializeField] private float minStartFallingSpeed;
	[SerializeField] private float maxStartFallingSpeed;
	[SerializeField] private float maxDistance = 1f;

	private float distance = 0;
	float dt = 0.6f;

	private void Awake() {
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
	}

	private void Start() {
		float speed = Random.Range(minStartFallingSpeed, maxStartFallingSpeed);
		float x = Random.Range(-1f, 1f) * 1f;
		dt = Random.Range(0.5f, 0.9f);
		rb.velocity = speed * Vector2.down + Vector2.right * x;
		rb.angularVelocity = 100 * Random.Range(-1f, 1f);
	}

	private float GetAlpha(float t) {
		if (t < dt ) {
			return 1f;
		}
		else {
			return -(1 / (1f - dt)) * t + (1 / (1f - dt));
		}
		return 1f / (1f + Mathf.Exp(t - 1));
	}

	private void Update() {
		distance += rb.velocity.magnitude * Time.deltaTime;
		float t = distance / maxDistance;
		Color color = sr.color;
		color.a = GetAlpha(t);
		sr.color = color;
		if (distance > maxDistance ) {
			Destroy(gameObject);
		}
	}
}
