using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpThroughPlatform : MonoBehaviour
{
	[SerializeField] private GameObject redPlatfrom;
	[SerializeField] private GameObject bluePlatfrom;
	[SerializeField] private Transform topPoint;
	[SerializeField] private Transform bottomPoint;

	private bool redActivated = false;
	private bool blueActivated = false;

	private void Update() {
		if ( RedHero.instance ) {
			float bottom = RedHero.instance.GetBottomY();
			float top = bottom + 1;
			float tP = topPoint.position.y;
			float bP = bottomPoint.position.y;

			if ( redActivated ) {
				if (top < bP ) {
					redActivated = false;
				}
			}
			else {
				if (bottom > tP ) {
					redActivated = true;
				}
			}
			redPlatfrom.SetActive(redActivated);
		}
		if ( BlueHero.instance ) {
			float bottom = BlueHero.instance.GetBottomY();
			float top = bottom + 1;
			float tP = topPoint.position.y;
			float bP = bottomPoint.position.y;

			if ( blueActivated ) {
				if ( top < bP ) {
					blueActivated = false;
				}
			}
			else {
				if ( bottom > tP ) {
					blueActivated = true;
				}
			}
			bluePlatfrom.SetActive(blueActivated);
		}
	}
}
