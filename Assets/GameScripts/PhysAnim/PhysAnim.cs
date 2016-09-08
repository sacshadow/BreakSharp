using UnityEngine;
using System.Collections;

public enum MoveState {IDLE, WALK, }

public class PhysAnim : MonoBehaviour {
	
	public Rigidbody rBody;
	
	public Transform leftFeet, rightFeet;
	
	public Feet left, right;
	
	public LayerMask mask = ~0;
	
	public float walkSpeed = 1.2f;
	
	private Vector3 targetVelocity;
	private MoveState moveState = MoveState.IDLE;
	
	
	private void SetState(MoveState newState) {
		if(moveState == newState)
			return;
		
		StopAllCoroutines();
		moveState = newState;
		if(moveState == MoveState.WALK)
			StartCoroutine(Walk());
	}
	
	private IEnumerator Walk() {
	
		float aleft, aright, move;
		
		aleft = GetStartAngle(left.legAngle, right.legAngle);
		aright = GetStartAngle(right.legAngle, left.legAngle);
		
		while(true) {
			move += Time.deltaTime * walkSpeed;
			
			yield return null;
		}
	}
	
	private float GetStartAngle(float a, float b) {
		if(a > b)
			return a;
		else return a + 60;
	}
	
	
	// Use this for initialization
	void Start () {
		targetVelocity = Vector3.zero;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float m = Input.GetAxis("Horizontal");
		
		
		targetVelocity.z = Mathf.MoveTowards(targetVelocity.z, m * walkSpeed, 3*Time.deltaTime);
		// targetVelocity
		
		rBody.velocity = targetVelocity;
		
		if(targetVelocity.z > 0)
			SetState(MoveState.WALK);
		else
			SetState(MoveState.IDLE);
	}
	
	void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(transform.position, rightFeet.position);
		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, leftFeet  .position);
	}
	
}
