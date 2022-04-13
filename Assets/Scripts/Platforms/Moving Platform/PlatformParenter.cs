using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlatformParenter : MonoBehaviour, IDeactivated
{
	[SerializeField] private Transform parenter;
	[SerializeField] private Rigidbody2D rb;
	[SerializeField] private bool forRed;
	[SerializeField] private bool forBlue;
	[SerializeField] private UnityEvent onTrigger;
	[SerializeField] private UnityEvent onCollision;
	[SerializeField] private bool notParent;
	[SerializeField] private BandPlatform band;

	private List<Transform> childs = new List<Transform>();
	private List<Transform> parents = new List<Transform>();

	public bool IsHeroOn() {
		Transform rh = childs.Find(x => x.GetComponent<RedHero>());
		Transform bh = childs.Find(x => x.GetComponent<BlueHero>());
		return rh || bh;
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		Transform t = collision.transform;
		RedHero rh = t.GetComponent<RedHero>();
		BlueHero bh = t.GetComponent<BlueHero>();
		if (rh && forRed ) {
			onCollision.Invoke();
		}else if (bh && forBlue ) {
			onCollision.Invoke();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if ( notParent ) {
			return;
		}
		Transform t = collision.transform;
		RedHero rh = t.GetComponent<RedHero>();
		BlueHero bh = t.GetComponent<BlueHero>();

		if ( !childs.Contains(t) && rh && forRed) {
			childs.Add(t);
			parents.Add(t.parent);
			t.parent = parenter;
			rh.SetPlatform(rb);
			onTrigger.Invoke();
		}else if ( !childs.Contains(t) && bh && forBlue) {
			childs.Add(t);
			parents.Add(t.parent);
			t.parent = parenter;
			bh.SetPlatform(rb);
			onTrigger.Invoke();
		}
	}

	private void OnTriggerExit2D(Collider2D collision) {
		if ( notParent ) {
			return;
		}
		Transform t = collision.transform;
		RedHero rh = t.GetComponent<RedHero>();
		BlueHero bh = t.GetComponent<BlueHero>();

		if ( childs.Contains(t) && rh && forRed) {
			int index = childs.IndexOf(t);
			t.parent = parents[index];
			parents.RemoveAt(index);
			childs.Remove(t);
			rh.RemovePlatform(rb);
		}else if ( childs.Contains(t) && bh && forBlue) {
			int index = childs.IndexOf(t);
			t.parent = parents[index];
			parents.RemoveAt(index);
			childs.Remove(t);
			bh.RemovePlatform(rb);
		}
	}

	private void OnDeactivate() {
		foreach ( Transform t in childs ) {
			
			int index = childs.IndexOf(t);
			t.parent = parents[index];

			RedHero rh = t.GetComponent<RedHero>();
			BlueHero bh = t.GetComponent<BlueHero>();

			if ( rh ) {
				rh.RemovePlatform(rb);
				rh.enabled = true;
			}
			else if ( bh ) {
				bh.RemovePlatform(rb);
				bh.enabled = true;
			}
		}
		parents = new List<Transform>();
		childs = new List<Transform>();
	}

	private void FixedUpdate() {
		if ( band ) {
			foreach ( Transform child in childs ) {
				RedHero rh = child.GetComponent<RedHero>();
				BlueHero bh = child.GetComponent<BlueHero>();

				if (rh && forRed ) {
					band.SetVelocity(rh.transform.position);
				}else if (bh && forBlue ) {
					band.SetVelocity(bh.transform.position);
				}
			}
		}
	}

	public void Deactivate() {
		OnDeactivate();
	}
}
