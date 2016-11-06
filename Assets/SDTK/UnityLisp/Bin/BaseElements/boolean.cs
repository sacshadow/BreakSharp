using UnityEngine;
using System.Collections;

namespace UnityLisp {

	public class boolean : lisp_object {
		
		public bool value;
		
		public boolean() {
			type = new object_type("boolean");
		}
		public boolean(bool value) {
			type = new object_type("boolean");
			this.value = value;
		}
		
		public static boolean parse(string input) {
			bool out_value = false;
			if(bool.TryParse(input, out out_value))
				return new boolean(out_value);	
			throw new System.Exception("input can not convert to boolean");
		}
		
		public static boolean_parse_result try_parse(string input) {
			boolean_parse_result rt = new boolean_parse_result();
			bool out_value = false;
			rt.can_parse = bool.TryParse(input, out out_value);
			if(rt.can_parse)
				rt.result = new boolean(out_value);
			return rt;
		}
		
		public override string state_evalue() {
			return this.value.ToString();
		}
		
		public override bool as_boolean() {
			return value;
		}
		
		public override lisp_object get_copy() {
			return new boolean(value);
		}
		
	}
	
	public class boolean_parse_result {
		public bool can_parse;
		public boolean result;
		
		public boolean_parse_result() {
			this.can_parse = false;
		}
		
	}
	
}
