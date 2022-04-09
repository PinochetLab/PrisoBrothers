using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHero : MonoBehaviour
{
	[SerializeField] private float moveSpeed;
	[SerializeField] private float jumpMaxSpeed;
	[SerializeField] private float jumpMinSpeed;
	[SerializeField] private float jumpPressedMaxTime;
	[SerializeField] private string moveAxis;
	[SerializeField] private string jumpAxis;
	[SerializeField] private string verticalAxis;
	[SerializeField] private float abilityTime;
	[SerializeField] private float abilitySpeed;


	[SerializeField] private Transform groundChecker1;
	[SerializeField] private Transform groundChecker2;
	[SerializeField] private LayerMask whatIsGround;
	[SerializeField] private LayerMask whatIsIce;

	[SerializeField] private Transform flipGraphics;
	[SerializeField] private TrailRenderer trailRenderer;
	[SerializeField] private float groundedTrailFadeTime;

	private float airTrailTime;

	private bool extraJump = false;

	private RigidbodyConstraints2D startConstraints;
	private float lastNonZeroXinput;
	private bool ability = false;
	private bool abilityAble = false;

	private Rigidbody2D rb;
	private Rigidbody2D platform;
	private float startGravityScale;
	private Vector2 inertia;
	private Vector2 startInertia;
	private float inertiaTime = 0;
	private const float inertiaTau = 0.1f;
	private bool isGrounded = true;
	private bool isIceGrounded = false;
	private float jumpPressedTime = 0;
	private bool jumpPressed = false;
	private bool fixToPlatform = false;

	private bool weightless = false;
	private WeightlessZone weightlessZone;

	[SerializeField] private float flyVerticalSpeed;
	private bool fly = false;
	private FlyZone flyZone;

	public static RedHero instance;

	private void Awake() {
		instance = this;
		rb = GetComponent<Rigidbody2D>();
		startConstraints = rb.constraints;
		startGravityScale = rb.gravityScale;
		airTrailTime = trailRenderer.time;
	}

	public void AddExtraJump() {
		extraJump = true;
	}

	public void SetFly(FlyZone flyZone) {
		this.flyZone = flyZone;
		fly = true;
	}

	public void RemoveFly(FlyZone flyZone) {
		if ( this.flyZone == flyZone ) {
			fly = false;
		}
	}

	public void SetWeightless(WeightlessZone weightlessZone) {
		this.weightlessZone = weightlessZone;
		weightless = true;
		rb.gravityScale = 0;
	}

	public void RemoveWeightless(WeightlessZone weightlessZone) {
		if ( this.weightlessZone == weightlessZone ) {
			weightless = false;
			rb.gravityScale = startGravityScale;
		}
	}

	private void StopAbility() {
		StopCoroutine(UseAbility());
		rb.constraints = startConstraints;
		ability = false;
	}

	private IEnumerator UseAbility() {
		/*ability = true;
		rb.constraints = startConstraints | RigidbodyConstraints2D.FreezePositionY;
		SetHorizontalVelocity(abilitySpeed * lastNonZeroXinput);*/
		yield return new WaitForSeconds(0);
		/*rb.constraints = startConstraints;
		ability = false;*/
	}

	private void WeightlessUpdate() {

	}

	private void FlyUpdate() {

	}
	private void Update() {

		if ( Input.GetButtonUp(jumpAxis) ) {
			jumpPressed = false;
			jumpPressedTime = 0;
		}

		if ( weightless ) {
			WeightlessUpdate();
			return;
		}

		if ( fly ) {
			FlyUpdate();
			return;
		}

		GroundCheck();
		if ( Input.GetButtonDown( jumpAxis ) && isGrounded ) {
			fixToPlatform = false;
			SetVerticalVelocity(jumpMinSpeed + inertia.y);
			jumpPressed = true;
			jumpPressedTime = 0;
		}
		else if ( Input.GetButtonDown(jumpAxis) && extraJump ) {
			SetVerticalVelocity(jumpMinSpeed + inertia.y);
			jumpPressed = true;
			jumpPressedTime = 0;
			extraJump = false;
		}
		else if ( Input.GetButtonDown(jumpAxis) ) {
			/*if ( !ability && abilityAble ) {
				abilityAble = false;
				StartCoroutine(UseAbility());
			}*/
		}
	}

	public void SetPlatform(Rigidbody2D platform) {
		inertia = platform.velocity;
		startInertia = inertia;
		this.platform = platform;
		fixToPlatform = true;
	}

	public void RemovePlatform(Rigidbody2D platform) {
		if ( this.platform == platform ) {
			inertiaTime = 0;
			inertia = platform.velocity;
			startInertia = inertia;
			this.platform = null;
			fixToPlatform = false;
		}
	}

	private Vector2 GetPlatformVelocity() {
		if ( platform ) {
			return platform.velocity;
		}
		else {
			return Vector2.zero;
		}
	}

	private void GroundCheck() {
		isGrounded = Physics2D.CircleCast(groundChecker1.position, 0.02f, Vector2.down, 0.02f, whatIsGround)
			|| Physics2D.CircleCast(groundChecker2.position, 0.02f, Vector2.down, 0.02f, whatIsGround);

		isIceGrounded = Physics2D.CircleCast(groundChecker1.position, 0.02f, Vector2.down, 0.02f, whatIsIce)
			|| Physics2D.CircleCast(groundChecker2.position, 0.02f, Vector2.down, 0.02f, whatIsIce);

		trailRenderer.time = (isGrounded && TrailLength() < 1) ? groundedTrailFadeTime : airTrailTime;

		if ( isGrounded ) {
			extraJump = true;
			abilityAble = true;
		}
	}

	public float GetBottomY() {
		return groundChecker1.position.y;
	}

	private float TrailLength() {
		float distance = 0;
		for ( int i = 0; i < trailRenderer.positionCount - 1; i++ ) {
			Vector3 p1 = trailRenderer.GetPosition(i);
			Vector3 p2 = trailRenderer.GetPosition(i + 1);
			distance += Vector3.Distance(p1, p2);
		}
		return distance;
	}

	private void WeightlessFixedUpdate() {
		float hor = Input.GetAxisRaw(moveAxis);
		float ver = Input.GetAxisRaw(verticalAxis);
		Vector2 direction = (Vector2.up * ver + Vector2.right * hor).normalized;
		if ( direction != Vector2.zero ) {
			rb.velocity = direction * moveSpeed;
		}
	}

	private void FlyFixedUpdate() {
		float horSpeed = Input.GetAxisRaw(moveAxis) * moveSpeed;
		float verSpeed = Input.GetButton(jumpAxis) ? flyVerticalSpeed : 0;
		SetHorizontalVelocity(horSpeed);
		if ( verSpeed != 0 ) {
			SetVerticalVelocity(verSpeed);
		}
	}
	private void FixedUpdate() {

		if ( weightless ) {
			WeightlessFixedUpdate();
			return;
		}

		if ( fly ) {
			FlyFixedUpdate();
			return;
		}

		if ( rb.velocity.magnitude != 0 ) {
			HerosOrderController.instance.SetFirst(true);
		}

		if ( jumpPressed ) {
			jumpPressedTime += Time.fixedDeltaTime;
			if ( jumpPressedTime < jumpPressedMaxTime ) {
				float t = jumpPressedTime / jumpPressedMaxTime;
				float speed = jumpMinSpeed * (1 - t) + jumpMaxSpeed * t;
				SetVerticalVelocity(speed + inertia.y);
			}
		}


		if ( !platform ) {
			inertiaTime += Time.fixedDeltaTime;
			inertia = startInertia * Mathf.Exp(-inertiaTime / inertiaTau);
		}
		else {
			inertia = platform.velocity;
			startInertia = inertia;
			if ( fixToPlatform ) {
				SetVerticalVelocity(inertia.y);
			}
		}

		float move = Input.GetAxisRaw(moveAxis);
		if ( move != 0 ) {
			lastNonZeroXinput = Mathf.Sign(move);
			Vector3 s = flipGraphics.localScale;
			s.x = lastNonZeroXinput * Mathf.Abs(s.x);
			flipGraphics.localScale = s;
		}
		if ( !ability ) {
			if ( !isIceGrounded ) {
				SetHorizontalVelocity(move * moveSpeed + inertia.x);
			}
			else {
				SetHorizontalVelocity(lastNonZeroXinput * moveSpeed + inertia.x);
			}
		}
	}

	private void SetVerticalVelocity(float vel) {
		Vector2 velocity = rb.velocity;
		velocity.y = vel;
		rb.velocity = velocity;
	}

	private void SetHorizontalVelocity(float vel) {
		Vector2 velocity = rb.velocity;
		velocity.x = vel;
		rb.velocity = velocity;
	}
}
