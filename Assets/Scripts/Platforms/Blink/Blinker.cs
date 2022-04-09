using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinker : MonoBehaviour
{
	[SerializeField] private List<GameObject> gameObjects;
	[SerializeField] private float switchTime;
	[SerializeField] private int currentIndex = 0;

	private void CloseAll() {
		for ( int i = 0; i < gameObjects.Count; i++ ) {
			SetActive(gameObjects[i], false);
		}
	}

	private void SetActive(GameObject go, bool en) {
		foreach (var ide in go.GetComponentsInChildren<IDeactivated>() ) {
			if (!en) ide.Deactivate();
		}
		go.SetActive(en);
	}

	private void SwitchTo(int i) {
		CloseAll();
		SetActive(gameObjects[i], true);
	}

	private void ChangeIndex() {
		currentIndex = (currentIndex + 1) % gameObjects.Count;
	}

	private void Start() {
		StartCoroutine(Do());
	}

	IEnumerator Do() {
		SwitchTo(currentIndex);
		yield return new WaitForSeconds(switchTime);
		ChangeIndex();
		StartCoroutine(Do());
	}
}
