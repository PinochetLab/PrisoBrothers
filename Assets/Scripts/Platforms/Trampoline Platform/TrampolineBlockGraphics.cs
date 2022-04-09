using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineBlockGraphics : MonoBehaviour
{
	[SerializeField] private float animationSpeed = 1f;
	[SerializeField] private string triggerName = "Jump";
	[SerializeField] private Transform blocksParent;
	[SerializeField] private GameObject dustEffectPrefab;

	private List<Transform> blockPoints = new List<Transform>();
	private List<Animator> blockAnimators = new List<Animator>();

	private void Awake() {
		for (int i = 0; i < blocksParent.childCount; i++ ) {
			Transform blockPoint = blocksParent.GetChild(i);
			Animator blockAnimator = blockPoint.GetComponent<Animator>();
			blockAnimator.speed = animationSpeed;
			blockPoints.Add(blockPoint);
			blockAnimators.Add(blockAnimator);
		}
	}

	public void PlayAnimation(Vector3 position) {
		int i = GetClosestIndex(position);
		blockAnimators[i].SetTrigger(triggerName);
		if ( dustEffectPrefab ) {
			Instantiate(dustEffectPrefab, position, Quaternion.Euler(Vector3.forward * 45));
		}
	}

	private int GetClosestIndex(Vector3 position) {
		int index = 0;
		float minDistance = float.PositiveInfinity;
		for (int i = 0; i < blockPoints.Count; i++ ) {
			float distance = Vector3.Distance(position, blockPoints[i].position);
			print(i + "   " + distance);
			if (distance < minDistance ) {
				minDistance = distance;
				index = i;
			}
		}
		print("index " + index);
		return index;
	}
}
