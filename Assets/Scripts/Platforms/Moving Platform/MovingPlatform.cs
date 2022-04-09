using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
	[SerializeField] private Rigidbody2D platformRB;
	[SerializeField] private List<Transform> wayPoints;
	[SerializeField] private bool looped = true;
	[SerializeField] private bool forward = true;
	[SerializeField] private int currentIndex = 0;
	[SerializeField] private float speed;
	[SerializeField] private float speedInPoint;
	[SerializeField] private GameObject blockPrefab;
	[SerializeField] private GameObject ribbonPrefab;

	private int nextIndex = 1;
	private float maxDistance = 0.1f;
	private Vector2 dir = Vector2.zero;

	private int NextIndex() {
		int index = currentIndex;
		int n = wayPoints.Count;
		int next = index;
		if ( forward ) {
			next = index + 1;
			if ( next >= n ) {
				if ( looped ) {
					next = 0;
				}
				else {
					next = index - 1;
					forward = false;
				}
			}
		}
		else {
			next = index - 1;
			if ( next < 0 ) {
				if ( looped ) {
					next = n - 1;
				}
				else {
					next = 1;
					forward = true;
				}
			}
		}
		return next;
	}

	private void Start() {
		nextIndex = NextIndex();
		SetDir();
		AdditionalGraphics();
	}

	private void AdditionalGraphics() {
		float spinSpeed = 200 * speed;
		for (int i = 0; i < wayPoints.Count; i++ ) {
			Vector3 position = wayPoints[i].position;
			GameObject block = Instantiate(blockPrefab, position, Quaternion.identity);
			block.transform.SetParent(transform);
			block.GetComponent<Spinner>().SetSpeed(spinSpeed);
		}
		GetComponentInChildren<Spinner>().SetSpeed(spinSpeed);

		int lastIndex = wayPoints.Count - 2;
		if ( looped ) {
			lastIndex = wayPoints.Count - 1;
		}

		for (int i = 0; i <= lastIndex; i++ ) {
			int j = (i + 1) % wayPoints.Count;
			Vector3 startPosition = wayPoints[i].position;
			Vector3 endPosition = wayPoints[j].position;
			Vector3 right = (endPosition - startPosition).normalized;
			float length = Vector3.Distance(startPosition, endPosition);
			Transform ribbon = Instantiate(ribbonPrefab, startPosition, Quaternion.identity).transform;
			ribbon.SetParent(transform);
			ribbon.right = right;
			SpriteRenderer sr = ribbon.GetComponentInChildren<SpriteRenderer>();
			Vector2 size = sr.size;
			size.x = length + 1;
			sr.size = size;
		}
	}

	private void ChangeIndex() {
		currentIndex = NextIndex();
		nextIndex = NextIndex();
	}

	private float GetRemainingDistance() {
		return Vector3.Distance(platformRB.position, wayPoints[nextIndex].position);
	}

	private float GetRatioWay() {
		return Vector3.Distance(platformRB.position, wayPoints[currentIndex].position) /
			Vector3.Distance(wayPoints[currentIndex].position, wayPoints[nextIndex].position);
	}

	private void SetVelocity() {
		platformRB.velocity = (wayPoints[nextIndex].position - wayPoints[currentIndex].position).normalized * speed;
	}

	private void SetDir() {
		dir = (wayPoints[nextIndex].position - wayPoints[currentIndex].position).normalized;
	}

	private Vector2 GetNextPosition() {
		return wayPoints[nextIndex].position;
	}

	private void SetVelocityRatio() {
		float ratio = GetRatioWay();
		float a = 4 * (1 - speedInPoint);
		platformRB.velocity = dir * speed * (-a * Mathf.Pow(ratio - 0.5f, 2) + 1);
	}

	private void FixedUpdate() {
		float distance = GetRemainingDistance();
		if ( distance < maxDistance ) {
			platformRB.MovePosition(GetNextPosition());
			ChangeIndex();
			SetDir();
		}

		SetVelocityRatio();
	}
}
