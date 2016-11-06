using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityLisp {
	public class object_type {
		public readonly string type;
		public object_type (string type) {
			this.type = type;
		}
	}
	
	public abstract class lisp_object {
		public readonly static lisp_object true_value = new boolean(true);
		public readonly static lisp_object empty_value = new empty();
		
		public object_type type;
		public bool is_quote = false;
		
		public abstract string state_evalue();
		
		public virtual string evalue() {
			// return is_quote ? "(quote " + state_evalue + ")" : state_evalue;
			return state_evalue();
		}
		
		public virtual string to_string() {
			return evalue();
		}
		
		public virtual lisp_object eval(List<lisp_object> args) {
			var b0 = new block();
			var b1 = new block();
			b0.append(this);
			b0.append(b1);
			args.ForEach(a => b1.append(a));
			
			return b0;
		}
		
		public virtual lisp_object set_quoted() {
			var copy = get_copy();
			copy.is_quote = true;
			return copy;
		}
		
		public abstract lisp_object get_copy();
		
		public virtual float as_number() {
			throw new Exception("object is not a number : " + evalue());
		}
		
		public virtual bool as_boolean() {
			throw new Exception("object is not a boolean : " + evalue());
		}
		
		public static implicit operator float(lisp_object o) {
			return o.as_number();
		}
		
		public static implicit operator bool(lisp_object o) {
			return o.as_boolean();
		}
	}
}
