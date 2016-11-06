using UnityEngine;
using System;
using System.Collections;

public enum MoveState {IDLE, WALK, }
public enum Side {LEFT = 0, RIGHT}

public class PhysAnim : MonoBehaviour {
	
	public Rigidbody rBody;
	
	public Transform leftFeet, rightFeet;
	
	public Feet left, right;
	
	public LayerMask mask = ~0;
	
	public float walkSpeed = 1.2f;
	
	private Vector3 targetVelocity;
	private MoveState moveState = MoveState.IDLE;
	private float leftTime = 1;
	private float rightTime = 1;
	
	private Side crtLegSide = Side.LEFT;
	private Feet standFeet;
	private Func<float> GetLegMoveTime = ()=>0;
	
	private bool isTargetStepPoint = false;
	// private bool isFrontLegOnGround = false;
	private RaycastHit targetStepPoint;
	
	private void SetState(MoveState newState) {
		if(moveState == newState)
			return;
		
		StopAllCoroutines();
		moveState = newState;
		if(moveState == MoveState.WALK)
			StartCoroutine(Walk());
		if(moveState == MoveState.IDLE)
			StartCoroutine(PlaceFeetAtGround());
	}
	
	private IEnumerator PlaceFeetAtGround() {
		if(rightTime < 1) {
			SetMoveLegSide(Side.RIGHT);
			// yield return StartCoroutine(MoveLeg(right,left, x=>rightTime = x, (rightTime > 0.5f? rightTime : 1-rightTime)));
			yield return StartCoroutine(MoveLeg(right,left, x=>rightTime = x, rightTime));
			
		}
		if(leftTime < 1) {
			SetMoveLegSide(Side.LEFT);
			// yield return StartCoroutine(MoveLeg(left,right, x=>leftTime = x, (leftTime>0.5f? leftTime : 1-leftTime)));
			yield return StartCoroutine(MoveLeg(left,right, x=>leftTime = x, leftTime));
			
		}
		
		Vector3 leftPos = left.groundHit.point;
		Vector3 rightPos = right.groundHit.point;
		
		while(true) {
			
			left.SetToPoint(leftPos);
			right.SetToPoint(rightPos);
			
			yield return null;
		}
		
	}
	
	private IEnumerator Walk() {
		while(true) {
			if(rightTime > 0) {
				SetMoveLegSide(Side.LEFT);
				yield return StartCoroutine(MoveLeg(left,right,x=>leftTime = x));
			}
			if(leftTime > 0) {
				SetMoveLegSide(Side.RIGHT);
				yield return StartCoroutine(MoveLeg(right,left,x=>rightTime = x));
			}
		}
	}
	
	private IEnumerator MoveLeg(Feet supportLeg, Feet moveLeg, Action<float> OutTime, float timing = 0) {
		float t = timing;
		OutTime(t);
		
		// Vector3 point = supportLeg.transform.position;
		Vector3 point = supportLeg.groundHit.point;
		float angle;
		
		while(t<1) {
			t += Time.deltaTime * 2;
			OutTime(t);
			
			supportLeg.SetToPoint(point);
			
			angle = supportLeg.GetAngle();
			
			moveLeg.SetLegAngle(angle, t, targetStepPoint.point);
			
			
			yield return null;
		}
		
	}
	
	private void SetMoveLegSide(Side side) {
		crtLegSide = side;
		if(crtLegSide == Side.LEFT) {
			GetLegMoveTime = ()=>leftTime;
			standFeet = left;
		}
		else {
			standFeet = right;
			GetLegMoveTime = ()=>rightTime;
		}
	}
	
	private float GetVerticalMove(float orgYSpeed) {
		if(standFeet != null && standFeet.isOnGround) {
			float targetHeight = GetTargetHeight();
			float diff = targetHeight - (transform.position.y - Mathf.Lerp(standFeet.transform.position.y, targetStepPoint.point.y,GetLegMoveTime()));
			
			float crtMoveSpeed = orgYSpeed + diff * 80 * Time.deltaTime;
			
			// return Mathf.Lerp(orgYSpeed, diff, 12*Time.deltaTime);
			
			return crtMoveSpeed * 0.85f;
		}
		else
			return orgYSpeed += -9.81f * Time.deltaTime;
	}
	
	private float minHeight = 0.3f, maxHeight = 1.2f;
	
	private float GetTargetHeight() {
		float btn = targetVelocity.z / 4f * (GetLegMoveTime() - 0.5f) * 2;
		float length = left.legLength * 0.975f;
		float height = Mathf.Sqrt(btn*btn + length * length);
		
		return Mathf.Clamp(height, minHeight, maxHeight);
	}
	
	
	// Use this for initialization
	void Start () {
		// Time.timeScale = 0.25f;
		targetVelocity = Vector3.zero;
		if(left.isOnGround)
			standFeet = left;
		else
			standFeet = right;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float m = Input.GetAxis("Horizontal");
		
		Vector3 targetBodyPoint = transform.position + targetVelocity * 0.5f * (1-GetLegMoveTime());
		Debug.DrawLine(transform.position, targetBodyPoint);
		float targetLegPointX = targetVelocity.z * 0.25f;
		
		isTargetStepPoint = Physics.Raycast(targetBodyPoint + Vector3.forward * targetLegPointX, -Vector3.up, out targetStepPoint, Mathf.Infinity, mask);
		if(isTargetStepPoint) {
			Debug.DrawLine(targetBodyPoint, targetStepPoint.point);
		}
		
		
		targetVelocity.z = Mathf.MoveTowards(targetVelocity.z, m * walkSpeed, 3*Time.deltaTime);
		targetVelocity.y = GetVerticalMove(targetVelocity.y);
		
		rBody.velocity = targetVelocity;
		
		
		// if(Mathf.Abs(targetVelocity.z) > 0)
		if(Mathf.Abs(m) > 0)
			SetState(MoveState.WALK);
		else
			SetState(MoveState.IDLE);
	}
	
	void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(transform.position, rightFeet.position);
		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, leftFeet.position);
	}
	
}
