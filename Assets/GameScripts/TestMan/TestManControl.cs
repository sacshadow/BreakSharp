using UnityEngine;
using System.Collections;

public class TestManControl : MonoBehaviour {
	
	public TestManAnimation anim;
	
	
	public Vector2 moveInput;
	public bool isJet = false;
	public float rotateInput;
	public float damp = 2;
	
	private Vector3 myMoveInput;
	private Quaternion lookRotation;
	
	public void SetMovement(Vector2 moveInput) {
		this.myMoveInput = new Vector3(moveInput.x,0,moveInput.y);
	}
	
	public void SetRotate(float rotateInput, Quaternion lookRotation) {
		this.rotateInput = rotateInput;
		this.lookRotation = lookRotation;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		var convert = transform.InverseTransformDirection(myMoveInput);
		moveInput = new Vector2(convert.x,convert.z);
		
		if(moveInput.sqrMagnitude > 0.05f)
			transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, damp * Time.deltaTime);
		
	}
}
