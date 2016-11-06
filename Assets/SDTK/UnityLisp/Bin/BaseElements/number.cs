using UnityEngine;
using System.Collections;

namespace UnityLisp {

	public class number : lisp_object {
		public float value = 0;
		
		public number() {
			type = new object_type("number");
		}
		public number(float value) {
			type = new object_type("number");
			this.value = value;
		}
		
		public static number parse(string input) {
			float out_value = 0;
			if(float.TryParse(input, out out_value))
				return new number(out_value);
			throw new System.Exception("input can not convert to number");
		}
		
		public static number_parse_result try_parse(string input) {
			number_parse_result rt = new number_parse_result();
			float out_value = 0;
			rt.can_parse = float.TryParse(input, out out_value);
			if(rt.can_parse)
				rt.result = new number(out_value);
			return rt;
		}
		
		public override string state_evalue() {
			return this.value.ToString();
		}
		
		public override float as_number() {
			return value;
		}
		
		public override bool as_boolean() {
			return value != 0;
		}
		
		public override lisp_object get_copy() {
			return new number(value);
		}
	}
	
	public class number_parse_result {
		public bool can_parse;
		public number result;
		
		public number_parse_result() {
			this.can_parse = false;
		}
		
	}
	
}
