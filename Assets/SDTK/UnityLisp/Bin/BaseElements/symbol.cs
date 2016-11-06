using UnityEngine;
using System.Collections;

namespace UnityLisp {
	public class symbol : lisp_object {
		public string name {get; private set; }
		
		public symbol(string name) {
			type = new object_type("symbol");
			this.name = name;
		}
		
		public override string state_evalue() {
			return this.name;
		}
		
		public override string to_string() {
			return "'" + evalue() + "'";
		}
		
		public override lisp_object get_copy() {
			return new symbol(name);
		}
	}
}
