using UnityEngine;
using System.Collections;

public class Feet : MonoBehaviour {
	
	
	public Transform hip;
	public Transform feet;
	public LayerMask mask = ~0;
	
	public float legLength = 1f;
	
	public bool isGround = false;
	public bool isHited = false;
	public RaycastHit hit;
	
	public Vector3 targetPosition;
	
	
	
	
	
	void LateUpdate() {
		Vector3 checkPoint = isGround ? groundPoint : feet.position;
	
		isHited = Physics.Raycast(hip.position, checkPoint - hip.position, out hit, 1.5f, mask);
		
		if(isHited)
			Debug.DrawLine(hit.position, hit.position + hit.normal);
		
		// if(isHited && hit.distance < legLength)
			// targetPosition = hit.position;
		
	}
	
}
