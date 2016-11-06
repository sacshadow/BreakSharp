using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnityLisp {

	public class list : block {
	
		public list() {
			type = new object_type("block");
		}
		
		public override string state_evalue() {
			return "(list "+String.Join(" ", element.Select(e=>e.evalue()).ToArray())+")";
		}
		
		public override string to_string() {
			return "[" + String.Join(", ", element.Select(e=>e.to_string()).ToArray()) + "]";
		}
		
		public override lisp_object get_copy() {
			var rt = new block();
			rt.element = element;
			return rt;
		}
	}
}
