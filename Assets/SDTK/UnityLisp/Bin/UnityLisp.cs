using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnityLisp {
	
	public class token_collection {
		public List<string> token_list;
		
		public token_collection(string input) {
			token_list = input.Replace("(","( ").Replace("\n"," ").Replace(")"," )").Split(' ').ToList();
		}
		
		public string next() {
			while(token_list[0].Length == 0) {
				token_list.RemoveAt(0);
			}
				
			return token_list[0];
		}
		
		public string pop() {
			string s = token_list[0];
			token_list.RemoveAt(0);
			
			if(s.Length != 0)
				return s;
			
			return pop();
		}
	}
	
	public class unity_lisp {
		
		public static lisp_object load_environment(string input) {
			try {
				var result = evaluation(parse(input), environment.global);
				Debug.Log("global environment loaded");
				return result;
			}
			catch (System.Exception e) {
				throw e;
			}
		}
		
		public static token_collection tokenize(string input) {
			return new token_collection(input);
		}
		
		public static lisp_object parse(string program) {
			return read_from_tokens(tokenize(program));
		}
		
		public static lisp_object evaluation(lisp_object x, environment env) {
			// Debug.Log("evalue " + x.to_string() + " : " + env[x].to_string());
		
			if(is_symbol(x))
				return env[x];
			else if(!is_block(x))
				return x;
			
			block b = x as block;
			
			if(!is_symbol(b[0]) && !is_block(b[0]))
				throw new System.Exception("block " + b.evalue() + " not begin with symbol; " + b[0].evalue());
				// return x;
			
			if(is_symbol_match(b[0],"quote"))
				return b[1];
			
			if(is_symbol_match(b[0],"if")) {
				if(is_true(evaluation(b[1], env),env))
					return evaluation(b[2],env);
				else
					return evaluation(b[3],env);
			}
			
			if(is_symbol_match(b[0],"define")) {
				if(!is_symbol(b[1]))
					throw new System.Exception("can not define non symbol : " + b[1].evalue());
			
				env[b[1]] = evaluation(b[2],env);
				// Debug.Log("Define " + b[1].evalue() + " " + env[b[1]].evalue());
				return b[1];
			}
			
			if(is_symbol_match(b[0],"defun")) {
				if(!is_symbol(b[1]))
					throw new System.Exception("can not define non symbol : " + b[1].evalue());
				
				block parms = b[2] as block;
				block body = b[3] as block;
				
				env[b[1]] = new lambda_function(parms,body,env);
				Debug.Log("Defun " + b[1].evalue() + " " + env[b[1]].evalue());
				return b[1];
			}
			
			if(is_symbol_match(b[0],"let")) {
				return env.let(b[1],evaluation(b[2],env));
			}
			
			if(is_symbol_match(b[0],"lambda")) {
				var parms = b[1] as block;
				var body = b[2];
				return new lambda_function(parms,body,env);
			}
			
			if(is_symbol_match(b[0],"get"))
				return env[b[1]];
				// return evaluation(env[b[1]], env);
			
			if(is_symbol_match(b[0],"get-index")) {
				int index = (int)evaluation(b[1],env).as_number();
				block list = evaluation(b[2],env) as block;
				return evaluation(list[index],env);
			}
			
			if(is_symbol_match(b[0],"run")) {
				return evaluation(evaluation(b[1],env),env);
			}
			
			if(is_symbol_match(b[0],"sequence"))
				return lisp_sequence(b,env);
			
			if(is_symbol_match(b[0],"list")) {
				block rt = new list();
				Loop.Between(1,b.count).Do(index=>rt.append(evaluation(b[index],env)));
				return rt;
			}
			
			if(is_symbol_match(b[0], "push")) {
				block rt = evaluation(b.last(), env) as block;
				Loop.Between(1,b.count-1).Do(index=>rt.append(evaluation(b[index],env)));
				return rt;
			}
			
			if(is_symbol_match(b[0], "func-call")) {
				return evaluation(environment.cdr(b.element),env);
			}
			
			var lisp_object = evaluation(b[0],env);
			if(!is_function(lisp_object))
				throw new System.Exception("block " + b.evalue() + " is not a function");
			var args = Loop.Between(1,b.count).Select(index=>evaluation(b[index],env));
			// Debug.Log("process " + lisp_object.to_string());
			// Debug.Log("args " + args.Count);
			return lisp_object.eval(args);
		}
		
		public static lisp_object lisp_sequence(block b, environment env) {
			lisp_object rt = lisp_object.empty_value;
			Loop.Between(1, b.count).Do(x=> rt = evaluation(b[x],env));
			return rt;
		}
		
		public static bool is_true(lisp_object x, environment env) {
			// if(is_empty(x))
				// return false;
			// if(is_number(x))
				// return (x as number).value != 0;
			if(is_symbol(x))
				return env.is_exist(x.evalue());
			
			return x;
		}
		
		public static bool is_empty(lisp_object x) {
			return x.type.type == "empty";
		}
		
		public static bool is_symbol_match(lisp_object x, string key_word) {
			return x.evalue() == key_word;
		}
		
		public static bool is_symbol(lisp_object x) {
			return x.type.type == "symbol";
		}
		
		public static bool is_boolean(lisp_object x) {
			return x.type.type == "boolean";
		}
		
		public static bool is_number(lisp_object x) {
			return x.type.type == "number";
		}
		
		public static bool is_block(lisp_object x) {
			return x.type.type == "block";
		}
		
		public static bool is_function(lisp_object x) {
			return x.type.type == "function";
		}
		
		public static lisp_object read_from_tokens(token_collection tokens) {
			if(tokens.token_list.Count == 0)
				throw new System.Exception("unexpected EOF while reading");
			
			string token = tokens.pop();
			
			if(token[0] == '#') {
				tokens.token_list.Insert(0,token.Substring(1));
				return new block().append(new symbol("run")).append(read_from_tokens(tokens));
			}
			if(token[0] == '\'') {
				tokens.token_list.Insert(0,token.Substring(1));
				return new block().append(new symbol("quote")).append(read_from_tokens(tokens));
			}
			
			if(token == "(") {
				block temp = new block();
				
				while(tokens.next() != ")") {
					// Debug.Log(tokens.next());
					temp.append(read_from_tokens(tokens));
				}
				token = tokens.pop();
				
				return temp;
			}
			else if(token == ")")
				throw new System.Exception("unexpected symbol ')'");
			else
				return atom(token);
		}
		
		public static lisp_object atom(string token) {
			var token_parse_as_number = number.try_parse(token);
			if(token_parse_as_number.can_parse)
				return token_parse_as_number.result;
			var token_parse_as_boolean = boolean.try_parse(token);
			if(token_parse_as_boolean.can_parse)
				return token_parse_as_boolean.result;
			
			// if(token[0] == '#')
				// return new block().append(new symbol("run")).append(atom(token.Substring(1)));
			// if(token[0] == '\'')
				// return new block().append(new symbol("quote")).append(atom(token.Substring(1)));
				// return new block().append(new symbol("quote")).append(new symbol(token.Substring(1)));
			
			return new symbol(token);
		}
		
		public static T Pop<T>(List<T> list) {
			T rt = list[0];
			// Debug.Log("Pop " + rt.ToString());
			list.RemoveAt(0);
			return rt;
		}
		
	}
	
}
