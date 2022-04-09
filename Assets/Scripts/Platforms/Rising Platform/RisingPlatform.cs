using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingPlatform : MonoBehaviour {
	[SerializeField] private PlatformParenter platformParenter;
	[SerializeField] private Rigidbody2D rb;
	[SerializeField] private float riseSpeed;
	[SerializeField] private float fallSpeed;

	private float minY = 0;

	private void Start() {
		minY = rb.position.y;
	}

	private bool IsUnder() {
		return rb.position.y <= minY;
	}

	private void SetY(float y) {
		rb.MovePosition(new Vector2(rb.position.x, y));
	}

	private void FixedUpdate() {
		bool isHeroOn = platformParenter.IsHeroOn();
		Vector2 velocity = Vector2.up * (isHeroOn ? riseSpeed : -fallSpeed);
		if ( IsUnder() ) {
			if ( velocity.y < 0 ) {
				SetY(minY);
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
