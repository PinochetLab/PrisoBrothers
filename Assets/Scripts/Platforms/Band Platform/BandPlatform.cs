using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BandPlatform : MonoBehaviour
{
	[SerializeField] private Transform mainTransform;
	[SerializeField] private float speed;
	[SerializeField] private bool clockwise;
	[SerializeField] private Rigidbody2D rb;
	[SerializeField] private LineRenderer[] lineRenderers;

	public void SetVelocity(Vector3 position) {
		Vector2 localPoint = mainTransform.worldToLocalMatrix.MultiplyVector(position - mainTransform.position);
		Vector2 dir = Vector2.up;
		float angle = Mathf.Atan2(localPoint.y, localPoint.x);
		angle = Mathf.Rad2Deg * angle;

		if (angle > 180 ) {
			angle -= 180;
		}
		if (angle < -180 ) {
			angle += 180;
		}
		if ( (angle >= -180 && angle <= -135) || (angle >= 135 && angle <= 180) ) {
			dir = Vector2.up;
		} else if ( angle >= -135 && angle <= -45 ) {
			dir = Vector2.right;
		} else if ( angle >= -45 && angle <= 45 ) {
			dir = Vector2.down;
		} else if ( angle >= 45 && angle <= 135 ) {
			dir = Vector2.left;
		}
		dir *= -(clockwise ? 1 : -1);
		Vector2 velocity = mainTransform.localToWorldMatrix.MultiplyVector(dir).normalized * speed;
		rb.velocity = velocity;
	}
	private void Start() {
		foreach (LineRenderer lineRenderer in lineRenderers ) {
			Material material = lineRenderer.material;
			material.SetFloat("_Speed", speed * 16f / 6f / 2.75f);
			lineRenderer.material = material;
		}
	}
}
