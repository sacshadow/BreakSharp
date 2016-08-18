using UnityEngine;
using System.Collections;

public class TestManAnimation : MonoBehaviour {
	
	public Animator animator;
	public TestManControl control;
	
	private Vector2 moveSpeed = Vector2.zero;
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
		moveSpeed = control.moveInput;
	
		if(control.isJet && moveSpeed.y > 0) {
			moveSpeed.y += 1;
			moveSpeed.x = control.rotateInput;
		}
			
		
		animator.SetFloat("xSpeed", moveSpeed.x);
		animator.SetFloat("ySpeed", moveSpeed.y);
	}
}
