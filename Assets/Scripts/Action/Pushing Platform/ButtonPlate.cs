using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonPlate : MonoBehaviour
{
	[SerializeField] private bool unReturnable;
	[SerializeField] private bool forRed = true;
	[SerializeField] private bool forBlue = true;
	[SerializeField] private UnityEvent onTapDown;
	[SerializeField] private UnityEvent onTapUp;

	private List<Transform> visitors = new List<Transform>();

	private void TryAddVisitor(Transform visitor) {
		if ( !visitors.Contains(visitor) ) {
			visitors.Add(visitor);
		}
	}

	private void TryRemoveVisitor(Transform visitor) {
		if ( visitors.Contains(visitor) ) {
			visitors.Remove(visitor);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		RedHero rh = collision.GetComponent<RedHero>();
		BlueHero bh = collision.GetComponent<BlueHero>();

		if (rh && forRed ) {
			if ( visitors.Count == 0 ) {
				onTapDown.Invoke();
			}
			TryAddVisitor(rh.transform);
		}else if (bh && forBlue ) {
			if ( visitors.Count == 0 ) {
				onTapDown.Invoke();
			}
			TryAddVisitor(bh.transform);
		}
	}

	private void OnTriggerExit2D(Collider2D collision) {
		if ( unReturnable ) {
			return;
		}

		RedHero rh = collision.GetComponent<RedHero>();
		BlueHero bh = collision.GetComponent<BlueHero>();

		if ( rh && forRed ) {
			TryRemoveVisitor(rh.transform);
			if ( visitors.Count == 0 ) {
				onTapUp.Invoke();
			}
		}
		else if ( bh && forBlue ) {
			TryRemoveVisitor(bh.transform);
			if ( visitors.Count == 0 ) {
				onTapUp.Invoke();
			}
		}
	}
}
