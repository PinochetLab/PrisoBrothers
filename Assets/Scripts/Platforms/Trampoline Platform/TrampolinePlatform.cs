using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolinePlatform : MonoBehaviour
{
	[SerializeField] private Rigidbody2D myRB;
	[SerializeField] private float jumpSpeed;
	[SerializeField] private bool forRed = true;
	[SerializeField] private bool forBlue = true;
	[SerializeField] private TrampolineBlockGraphics graphics;
	private void OnTriggerEnter2D(Collider2D collision) {
		Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
		RedHero rh = collision.GetComponent<RedHero>();
		BlueHero bh = collision.GetComponent<BlueHero>();

		if ( !rb || rb.isKinematic) {
			return;
		}

		if (rh && forRed ) {
			rb.velocity = new Vector2(rb.velocity.x, jumpSpeed + myRB.velocity.y);
			if ( graphics ) {
				graphics.PlayAnimation(rb.position + Vector2.down * 0.5f);
			}
		}
		if ( bh && forBlue ) {
			rb.velocity = new Vector2(rb.velocity.x, jumpSpeed + myRB.velocity.y);
			if ( graphics ) {
				graphics.PlayAnimation(rb.position + Vector2.down * 0.5f);
			}
		}
	}
}
