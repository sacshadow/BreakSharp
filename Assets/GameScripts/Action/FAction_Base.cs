using UnityEngine;
using System.Collections;

public abstract class FAction_Base {
	
	public TestManControl control;
	
	public FAction_Base(TestManControl control) {
		this.control = control;
	}
	
	public abstract void BeginAction();
	
	public abstract void EndAction();
	
	public abstract IEnumerator Process();
}
