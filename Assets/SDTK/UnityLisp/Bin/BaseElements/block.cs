using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnityLisp {
	public class block : lisp_object {
		public List<lisp_object> element = new List<lisp_object>();
		
		public lisp_object this[int index] {
			get {
				return element[index];
			}
		}
		
		public int count {get { return element.Count; }}
		
		public block() {
			type = new object_type("block");
		}
		
		public block append(lisp_object new_element) {
			element.Add(new_element);
			return this;
		}
		
		public lisp_object last() {
			if(element.Count == 0)
				throw new System.Exception("block is empty");
			return element[element.Count-1];
		}
		
		public override string state_evalue() {
			return "("+String.Join(" ", element.Select(e=>e.evalue()).ToArray())+")";
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
