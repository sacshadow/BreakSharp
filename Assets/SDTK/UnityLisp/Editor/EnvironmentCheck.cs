using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityLisp {

	public class EnvironmentCheck : EditorWindow {
		public static EnvironmentCheck ec;
		
		public List<string> define;
		public Action<string> get_define;
		public Vector2 scroll;
		
		public static void Display(environment env, Action<string> get_define) {
			if(ec == null)
				ec = EditorWindow.GetWindow(typeof(EnvironmentCheck)) as EnvironmentCheck;
			
			ec.Set(env, get_define);
		}
		
		
		public void Set(environment env, Action<string> get_define) {
			define = new List<string>();
			
			foreach(var kvp in env.env) {
				define.Add(environment.func_block("define").append(new symbol(kvp.Key)).append(kvp.Value).evalue());
			}
			
			this.get_define = get_define;
		}
		
		
		void OnGUI() {
			scroll = GUILayout.BeginScrollView(scroll);
			for(int i =0; i< define.Count; i++) {
				if(GUILayout.Button(define[i]) && get_define != null)
					get_define(define[i]);
			}
			
			GUILayout.EndScrollView();
		}
		
		void Update() {
			Repaint();
		}
		
	}

}
