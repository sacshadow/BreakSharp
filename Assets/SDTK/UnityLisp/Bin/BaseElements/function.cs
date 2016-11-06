using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;


namespace UnityLisp {
	public abstract class function : lisp_object {
		public function() {
			type = new object_type("function");
		}
	}
	
	public class lambda_function : function {
		public block parms;
		public lisp_object body;
		public environment env;
		public List<string> p;
		
		public lambda_function(block parms, lisp_object body, environment env) {
			this.parms = parms;
			this.body = body;
			this.env = env;
			this.p = Loop.SelectEach<lisp_object,string>(parms.element, param_name);
		}
		
		public override lisp_object eval(List<lisp_object> args) {
			return unity_lisp.evaluation(body, environment.new_local_env(p,args,env));
		}
		
		public string param_name(lisp_object param_name_object) {
			if(!unity_lisp.is_symbol(param_name_object))
				throw new Exception("param has illegal name " + param_name_object.evalue());
			return param_name_object.evalue();
		}
		
		public override string state_evalue() {
			return "(lambda " + parms.evalue() + " " + body.evalue() + ")";
		}
		
		public override string to_string() {
			return "[#LAMBDA," + parms.to_string() + "," + body.to_string() + "]";
		}
		
		public override lisp_object get_copy() {
			return new lambda_function(parms, body, env);
		}
	}
	
	public class builtin_function : function {
		
		public Func<List<lisp_object>, lisp_object> process;
		
		public builtin_function(Func<List<lisp_object>, lisp_object> process) {
			this.process = process;
		}
		
		public override lisp_object eval(List<lisp_object> args) {
			return process(args);
		}
		
		public override string state_evalue() {
			return "#BUILT-IN-Function";
		}
		
		public override lisp_object get_copy() {
			throw new System.Exception("not availble to builtin_function");
		}
	}
	
}
