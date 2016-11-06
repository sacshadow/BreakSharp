using UnityEngine;
using System.Collections;

public class Feet : MonoBehaviour {
	
	
	public Transform hip;
	public Transform feet;
	public LayerMask mask = ~0;
	
	public float legLength = 1f;
	
	public Vector3 targetPosition;
	public AnimationCurve feetLiftUp = AnimationCurve.Linear(0,0,1,0);
	
	public float legAngle = 0;
	
	public bool isOnGround = false;
	public RaycastHit groundHit;
	
	public Vector3 lastGroundPoint;
	
	// public Vector3 
	
	// private float orgFeetDis = 0;
	
	public float GetAngle() {
		Vector3 dir =  hip.position-feet.position;
		return Vector3.Angle(Vector3.up,dir) * Mathf.Sign(Vector3.Dot(Vector3.forward, dir));
	}
	
	public void SetLegAngle(float angle, float timeing, Vector3 targetPoint) {
		// legAngle = Mathf.LerpAngle(legAngle, angle, 120f*Time.deltaTime);
		
		// float feetDis = Mathf.Min(legLength * feetLiftUp.Evaluate(timeing), groundHit.distance); 
		
		// transform.localPosition = Quaternion.Euler(-legAngle,0,0) * Vector3.up * -feetDis;
		
		legAngle = angle;
		transform.position = Vector3.Lerp(lastGroundPoint, targetPoint, timeing) + Vector3.up * (1-feetLiftUp.Evaluate(timeing));
		
		
	}
	
	public void SetToPoint(Vector3 position) {
		transform.position = lastGroundPoint = position;
	}
	
	public void GroundCheck() {
		Vector3 dir = Quaternion.Euler(-legAngle,0,0) * Vector3.up * -1;
		isOnGround = Physics.Raycast(hip.position, dir, out groundHit, legLength, mask);
	}
	
	void Awake() {
		legAngle = GetAngle();
		GroundCheck();
		SetToPoint(transform.position);
	}
	
	void LateUpdate() {
		legAngle = -GetAngle();
		GroundCheck();
	}
	
}
