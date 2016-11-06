using UnityEngine;
using System.Collections;

namespace UnityLisp {
	
	public class empty : lisp_object {
		
		public empty() {
			type = new object_type("empty");
		}
		
		public override string state_evalue() {
			return "nil";
		}
		
		public override string to_string() {
			return "-NULL-";
		}
		
		public override bool as_boolean() {
			return false;
		}
		
		public override lisp_object get_copy() {
			return this;
		}
	}
	
}
