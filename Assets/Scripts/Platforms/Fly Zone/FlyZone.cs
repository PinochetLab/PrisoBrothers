using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyZone : MonoBehaviour {
	private void OnTriggerEnter2D(Collider2D collision) {
		Transform t = collision.transform;
		RedHero rh = t.GetComponent<RedHero>();
		BlueHero bh = t.GetComponent<BlueHero>();

		if ( rh ) {
			rh.SetFly(this);
		}
		else if ( bh ) {
			bh.SetFly(this);
		}
	}

	private void OnTriggerExit2D(Collider2D collision) {
		Transform t = collision.transform;
		RedHero rh = t.GetComponent<RedHero>();
		BlueHero bh = t.GetComponent<BlueHero>();

		if ( rh ) {
			rh.RemoveFly(this);
		}
		else if ( bh ) {
			bh.RemoveFly(this);
		}
	}
}