using UnityEngine;
using System;
using System.Collections;


public class P1_body : MonoBehaviour {
	
	public P1_leg left, right;
	
	public Vector3 targetMove;
	public Rigidbody rBody;
	public float walkSpeed = 1.2f;
	public float accWeight = 10;
	
	private float leftTime = 1;
	private float rightTime = 1;
	
	
	
	private void GoState(Func<IEnumerator> NewMoveState) {
		StopAllCoroutines();
		StartCoroutine(IGoState(NewMoveState));
	}
	private IEnumerator IGoState(Func<IEnumerator> NewMoveState) {
		yield return StartCoroutine(NewMoveState());
		StartCoroutine(IdleState());
	}
	
	private IEnumerator IdleState() {
		
		while(true) {
			yield return null;
		}
	}
	
	private IEnumerator WalkState() {
		
		while(true) {
			yield return null;
		}
	}
	
	void Start() {
		StartCoroutine(IdleState());
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		left.InfoUpdate();
		right.InfoUpdate();
		
		float h = Input.GetAxis("Horizontal");
		targetMove.z = h * walkSpeed;
		
		Vector3 acc = (targetMove - rBody.velocity) * accWeight - rBody.velocity * 0.3f;
		
		rBody.velocity += acc * Time.deltaTime;
	}
}
