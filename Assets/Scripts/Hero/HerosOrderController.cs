using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HerosOrderController : MonoBehaviour
{
	[SerializeField] private SortingGroup redGroup;
	[SerializeField] private SortingGroup blueGroup;

	public static HerosOrderController instance;

	private void Awake() {
		instance = this;
	}

	public void SetFirst(bool isRed) {
		redGroup.sortingOrder = isRed ? 2 : 1;
		blueGroup.sortingOrder = isRed ? 1 : 2;
	}
}
