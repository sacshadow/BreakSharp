using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnityLisp {
	
	public class environment {
		public static environment global {
			get {
				if(global_environment == null)
					global_environment = standard_env();
				return global_environment;
			}
		}
		public static environment global_environment;
		
		public environment outer = null;
		public Dictionary<string, lisp_object> env;
		
		public bool is_dirty = false;
		
		public lisp_object this [lisp_object key] {
			get {
				if(env.ContainsKey(key.evalue()))
					return env[key.evalue()];
				if(outer != null)
					return outer[key];
				
				return key;
			}
			set {
				set_new_element(key.evalue(), value);
			}
		}
		
		public static environment standard_env() {
			environment def = new environment();
			def.env = new Dictionary<string, lisp_object>();
			
			def.env.Add("+", builtin_math_func((x,y)=>x+y));
			def.env.Add("-", builtin_math_func((x,y)=>x-y));
			def.env.Add("*", builtin_math_func((x,y)=>x*y));
			def.env.Add("/", builtin_math_func((x,y)=>x/y));
			
			def.env.Add(">", builtin_compare((x,y)=>x>y));
			def.env.Add("<", builtin_compare((x,y)=>x<y));
			def.env.Add(">=", builtin_compare((x,y)=>x>=y));
			def.env.Add("<=", builtin_compare((x,y)=>x<=y));
			def.env.Add("==", builtin_compare((x,y)=>x==y));
			def.env.Add("!=", builtin_compare((x,y)=>x!=y));
			
			def.env.Add("true", lisp_object.true_value);
			def.env.Add("false", lisp_object.empty_value);
			
			def.env.Add("nil", lisp_object.empty_value);
			def.env.Add("empty", lisp_object.empty_value);
			
			def.env.Add("pi", new number(Mathf.PI));
			
			def.env.Add("car", new builtin_function(x=>x[0]));
			def.env.Add("cdr", new builtin_function(cdr));
			// def.env.Add("cons", new builtin_function(cons));
			
			def.env.Add("print", print());
			
			environment std = new environment();
			std.env = new Dictionary<string, lisp_object>();
			std.outer = def;
			// std.outer.env.Add("source-code", new builtin_function(x=>std.source_code()));
			
			return std;
		}
		
		public static environment new_local_env(List<string> parms, List<lisp_object> args, environment outer = null) {
			if(parms.Count > args.Count)
				throw new System.Exception(String.Format(
					"args missing : params({0}), args({1})"
					, String.Join(", ",parms.ToArray())
					, String.Join(" ",args.Select(x=>x.evalue()).ToArray())));
			
			environment temp = new_local_env(outer);
			
			Loop.Count(parms.Count).Do(x=>temp.append(parms[x],args[x]));
			
			// Debug.Log(temp.source_code().evalue());
			
			return temp;
		}
		
		public static environment new_local_env(environment outer = null) {
			environment temp = new environment();
			temp.outer = outer;
			temp.env = new Dictionary<string, lisp_object>();
			
			return temp;
		}
		
		public static lisp_object builtin_compare(Func<float,float,bool> builtin_operator) {
			// Func<lisp_object,float> num = x=> (x as number).value;
			
			Func<List<lisp_object>, lisp_object> process = float_array => {
				float first = float_array[0];
				for(int i = 1; i<float_array.Count; i++) {
					if(!builtin_operator(first, float_array[i]))
						return lisp_object.empty_value;
				}
				return lisp_object.true_value;
			};
			
			return new builtin_function(process);
		}
		
		public static lisp_object builtin_math_func(Func<float,float,float> builtin_operator) {
		
			Func<List<lisp_object>, lisp_object> process = float_array => {
				float rt = float_array[0];
				Loop.Between(1,float_array.Count).Do(x=> rt = builtin_operator(rt, float_array[x]));
				return new number(rt);
			};
		
			return new builtin_function(process);
		}
		
		public static lisp_object print() {
			return new builtin_function(
				x=> {
					Debug.Log(String.Join(", ", x.Select(e=>e.evalue()).ToArray()));
					return lisp_object.true_value;
				}
			);
		}
		
		public static lisp_object cdr(List<lisp_object> list) {
			block rt = new block();
			Loop.Between(1,list.Count).Do(x=>rt.append(list[x]));
			return rt;
		}
		
		public static block func_block(string func_name) {
			block rt = new block();
			rt.append(new symbol(func_name));
			return rt;
		}
		
		// public static lisp_object cons(List<lisp_object> list) {
			// block rt = new block();
			// Loop.Count();
		// }
		
		public void append(string key, lisp_object value) {
			env.Add(key, value);
			// Debug.Log("add " + key + " : " + value.to_string());
		}
		
		public lisp_object let(lisp_object key, lisp_object value) {
			return let_key_value(key.evalue(), value);
		}
		
		public lisp_object let_key_value(string key, lisp_object value) {
			if(env.ContainsKey(key)) {
				env[key] = value;
				return value;
			}
			if(outer != null)
				return outer.let_key_value(key,value);
			return lisp_object.empty_value;
		}
		
		public lisp_object set_new_element(string key, lisp_object value) {
			if(!env.ContainsKey(key))
				append(key, value);
			else {
				env[key] = value;
				// Debug.Log("add " + key + " : " + value.to_string());
			}
				
			is_dirty = true;
			return value;
		}
		
		public bool is_exist(string key) {
			if(env.ContainsKey(key))
				return true;
			
			if(outer == null)
				return false;
			return outer.is_exist(key);
		}
		
		public lisp_object source_code() {
			block code_block = func_block("sequence");
			
			foreach(var kvp in env) {
				code_block.append(func_block("define").append(new symbol(kvp.Key)).append(kvp.Value));
			}
			
			code_block.append(lisp_object.true_value);
			
			return code_block;
		}
		
		
		
		
	}
	
}