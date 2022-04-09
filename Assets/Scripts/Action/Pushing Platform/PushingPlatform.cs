using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushingPlatform : MonoBehaviour
{
	[SerializeField] private Transform pushingPlatform;
	[SerializeField] private SpriteRenderer spriteRenderer;
	[SerializeField] private BoxCollider2D boxCollider2D;
	[SerializeField] private float openSpeed;
	[SerializeField] private float closeSpeed;
	[SerializeField] private bool closed;

	private float startScaleX;
	private float startBoxSize;
	private bool open = false;
	private float currentRatio = 1;
	private Vector3 startPosition;

	private void Awake() {
		startScaleX = spriteRenderer.size.x;
		startBoxSize = boxCollider2D.size.x;
		startPosition = pushingPlatform.localPosition;
	}
	private void SetSpriteScale(float sizeX) {
		Vector2 size = spriteRenderer.size;
		size.x = sizeX;
		spriteRenderer.size = size;
	}

	private void SetBoxSize(float sizeX) {
		Vector2 size = boxCollider2D.size;
		size.x = sizeX;
		boxCollider2D.size = size;
	}

	private void SetRatio(float r) {
		float scaleX = startScaleX * r;
		float boxSize = startBoxSize * r;
		SetSpriteScale(scaleX);
		SetBoxSize(boxSize);
		pushingPlatform.localPosition = startPosition * r;
	}

	public void Open() {
		open = true;
	}

	public void Close() {
		open = false;
	}

	private void Update() {
		float speed = open ? openSpeed : -closeSpeed;
		currentRatio += speed * Time.deltaTime;
		currentRatio = Mathf.Clamp(currentRatio, 0, 1);
		SetRatio(currentRatio);
	}
}
