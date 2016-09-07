using UnityEngine;
using System.Collections;

public class PhysAnim : MonoBehaviour {
	
	public Rigidbody rBody;
	
	public Transform leftFeet, rightFeet;
	
	public LayerMask mask = ~0;
	
	public float walkSpeed = 1.5f;
	
	private Vector3 targetVelocity;
	
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
		
	}
	
	void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(transform.position, rightFeet.position);
		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, leftFeet  .position);
	}
	
}
