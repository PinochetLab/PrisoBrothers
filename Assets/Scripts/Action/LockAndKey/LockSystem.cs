using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockSystem : MonoBehaviour
{
	[SerializeField] private float lockTime;

	private List<GameObject> lockObjects = new List<GameObject>();

	private void Awake() {
		foreach (Transform child in transform ) {
			lockObjects.Add(child.gameObject);
		}
	}

	public void OpenLocks() {
		StartCoroutine(OpenLocksCor());
	}

	private IEnumerator OpenLocksCor() {
		for (int i = 0; i < lockObjects.Count; i++ ) {
			yield return new WaitForSeconds(lockTime);
			lockObjects[i].SetActive(false);
		}
	}

	public void CloseLocks() {
		for (int i = 0; i < lockObjects.Count; i++ ) {
			lockObjects[i].SetActive(true);
		}
	}
}
