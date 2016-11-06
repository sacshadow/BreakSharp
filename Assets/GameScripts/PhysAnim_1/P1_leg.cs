using UnityEngine;
using System.Collections;

public class P1_leg : MonoBehaviour {
	
	public P1_body body;
	public LayerMask mask = ~0;
	public Transform feetPoint;
	
	
	public float legAngle = 0;
	public float legLength = 1;
	
	public bool isOnGround = false;
	public RaycastHit groundHit;
	public Vector3 lastGroundPoint;
	
	
	
	
	public void InfoUpdate() {
		GroundCheck();
	}
	
	public void GroundCheck() {
		Vector3 dir = Quaternion.Euler(-legAngle,0,0) * Vector3.up * -1;
		isOnGround = Physics.Raycast(body.transform.position, dir, out groundHit, legLength, mask);
		if(isOnGround)
			lastGroundPoint = groundHit.point;
	}
	
}
