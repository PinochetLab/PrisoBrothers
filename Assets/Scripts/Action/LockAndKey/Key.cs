using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
	[SerializeField] private LockSystem lockSystem;
	[SerializeField] private bool forRed;
	[SerializeField] private bool forBlue;

	private void OnTriggerEnter2D(Collider2D other) {
		RedHero rh = other.GetComponent<RedHero>();
		BlueHero bh = other.GetComponent<BlueHero>();
		if (rh && forRed ) {
			lockSystem.OpenLocks();
		}else if (bh && forBlue ) {
			lockSystem.OpenLocks();
		}
	}
}
