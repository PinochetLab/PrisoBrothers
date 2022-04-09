using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightlessZone : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision) {
		Transform t = collision.transform;
		RedHero rh = t.GetComponent<RedHero>();
		BlueHero bh = t.GetComponent<BlueHero>();

		if ( rh ) {
			rh.SetWeightless(this);
		}else if ( bh ) {
			bh.SetWeightless(this);
		}
	}

	private void OnTriggerExit2D(Collider2D collision) {
		Transform t = collision.transform;
		RedHero rh = t.GetComponent<RedHero>();
		BlueHero bh = t.GetComponent<BlueHero>();

		if ( rh ) {
			rh.RemoveWeightless(this);
		}
		else if ( bh ) {
			bh.RemoveWeightless(this);
		}
	}
}
