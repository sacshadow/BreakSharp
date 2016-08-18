using UnityEngine;
using System.Collections;

public class TestManInput : MonoBehaviour {
	
	public TestManControl control;
	public TestManCam cam;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 move = Vector2.zero;
		move.x = Input.GetAxis("Horizontal");
		move.y = Input.GetAxis("Vertical");
		move.Normalize();
		
		Vector3 convert = new Vector3(move.x,0,move.y);
		convert = cam.transform.TransformDirection(convert);
		
		control.SetMovement(new Vector2(convert.x,convert.z));
		
		control.isJet = Input.GetKey(KeyCode.LeftShift);
		
		control.SetRotate(GetAngle(),Quaternion.LookRotation(cam.transform.forward));
	}
	
	private float GetAngle() {
		return Vector3.Angle(cam.transform.forward, transform.forward)
			* Mathf.Sign(Vector3.Dot(cam.transform.forward, transform.right))
			/ 30;
	}
}
