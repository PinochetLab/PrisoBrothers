using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
	[SerializeField] private PlatformParenter platformParenter;
	[SerializeField] private Rigidbody2D rb;
	[SerializeField] private float fallSpeed;
	[SerializeField] private float riseSpeed;

	private float maxY = 0;

	private void Start() {
		maxY = rb.position.y;
	}

	private bool IsUpper() {
		return rb.position.y >= maxY;
	}

	private void SetY(float y) {
		rb.MovePosition(new Vector2(rb.position.x, y));
	}

	private void FixedUpdate() {
		bool isHeroOn = platformParenter.IsHeroOn();
		Vector2 velocity = Vector2.down * (isHeroOn ? fallSpeed : -riseSpeed);
		if ( IsUpper() ) {
			if ( velocity.y > 0 ) {
				SetY(maxY);
				rb.velocity = Vector2.zero;
			}
			else {
				rb.velocity = velocity;
			}
		}
		else {
			rb.velocity = velocity;
		}
	}
}
