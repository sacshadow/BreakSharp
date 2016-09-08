using UnityEngine;
using System.Collections;

public class Feet : MonoBehaviour {
	
	
	public Transform hip;
	public Transform feet;
	public LayerMask mask = ~0;
	
	public float legLength = 1f;
	
	public Vector3 targetPosition;
	
	public float legAngle = 0;
	
	
	public void SetLegAngle(float angle) {
		legAngle = angle;
		
		transform.localPosition = Quaternion.Euler(angle,0,0) * Vector3.up * legLength;
	}
	
	
	void LateUpdate() {
		
	}
	
}
