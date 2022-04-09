using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingPlatform : MonoBehaviour
{
	[SerializeField] private float visibleTime;
	[SerializeField] private float invisibleTime;
	[SerializeField] private GameObject platform;
	[SerializeField] private GameObject poofEffectPrefab;
	[SerializeField] private GameObject partsObject;
	[SerializeField] private SpriteRenderer colorPart;
	[SerializeField] private float colorTime = 0.1f;
	[SerializeField] private bool bomb = false;
	[SerializeField] private float radius = 3;
	[SerializeField] private bool loop = false;

	private bool en = true;
	float alpha = 1;


	[ExecuteInEditMode]


	private void OnDrawGizmos() {
		if ( bomb ) {
			Gizmos.DrawWireSphere(transform.position, radius);
		}
	}

	private void Start() {
		if ( bomb ) {
			alpha = radius / GetComponentInChildren<CircleCollider2D>().radius;
			GetComponentInChildren<CircleCollider2D>().radius = radius;
		}
	}


	private void SetActive(GameObject go, bool en) {
		foreach ( var ide in go.GetComponentsInChildren<IDeactivated>() ) {
			if (!en) ide.Deactivate();
		}
		go.SetActive(en);
	}

	public void StartFade() {
		if ( !en ) {
			return;
		}
		if (poofEffectPrefab) {
			ParticleSystem ps = Instantiate(poofEffectPrefab, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
			ps.transform.localScale = ps.transform.localScale * alpha;
			//Destroy(ps.gameObject, ps.duration);
		}
		StartCoroutine(Fade());
		if ( colorPart ) {
			StartCoroutine(ColorCor());
		}
	}

	private IEnumerator ColorCor() {
		bool yellow = true;
		while ( true ) {
			yield return new WaitForSeconds(colorTime);
			yellow = !yellow;

			colorPart.color = yellow ? Color.yellow : Color.red;
		}
	}

	private IEnumerator Fade() {
		en = false;
		yield return new WaitForSeconds(visibleTime);

		if ( partsObject ) {
			partsObject.SetActive(true);
			var children = new List<GameObject>();
			foreach ( Transform child in partsObject.transform ) children.Add(child.gameObject);
			children.ForEach(child => child.transform.SetParent(null));
		}
		if ( !loop ) {
			foreach (IDeactivated id in gameObject.GetComponentsInChildren<IDeactivated>(true) ) {
				id.Deactivate();
			}
			Destroy(gameObject);
		}

		SetActive(platform, false);
		yield return new WaitForSeconds(invisibleTime);

		//Destroy(gameObject);
		SetActive(platform, true);
		en = true;
	}
}
