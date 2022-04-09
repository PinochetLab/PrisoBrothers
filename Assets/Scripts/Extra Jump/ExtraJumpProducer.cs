using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraJumpProducer : MonoBehaviour
{
	[SerializeField] private float updateTime = 1f;
	[SerializeField] SpriteRenderer spriteRenderer;
	private bool available = true;

	private void OnTriggerEnter2D(Collider2D collision) {
		if ( !available ) {
			return;
		}
		OnTrigger(collision);
	}

	private void OnTriggerStay2D(Collider2D collision) {
		if ( !available ) {
			return;
		}
		OnTrigger(collision);
	}

	private void OnTrigger(Collider2D collision) {
		RedHero rh = collision.GetComponent<RedHero>();
		BlueHero bh = collision.GetComponent<BlueHero>();

		if ( rh ) {
			rh.AddExtraJump();
			StartCoroutine(Disable());
		}else if ( bh ) {
			bh.AddExtraJump();
			StartCoroutine(Disable());
		}
	}

	private IEnumerator Disable() {
		available = false;
		spriteRenderer.enabled = false;
		yield return new WaitForSeconds(updateTime);
		spriteRenderer.enabled = true;
		available = true;
	}
}
