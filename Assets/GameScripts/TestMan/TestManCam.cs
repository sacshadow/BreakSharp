using UnityEngine;
using System.Collections;

public class TestManCam : MonoBehaviour {
	
	public Transform followTarget;
	public Transform axis;
	public float xRotateSpeed = 180;
	public float yRotateSpeed = 90;
	
	// Use this for initialization
	void Start () {
		transform.parent = null;
	}
	
	// Update is called once per frame
	void Update () {
		
		Vector2 look = Vector2.zero;
		look.x = Input.GetAxis("Mouse X");
		look.y = Input.GetAxis("Mouse Y");
		
		transform.Rotate(Vector3.up * xRotateSpeed * look.x * Time.deltaTime);
		axis.Rotate(Vector3.right * yRotateSpeed * -look.y * Time.deltaTime);
		
	}
	
	void LateUpdate() {
		transform.position = followTarget.position;
	}
}
