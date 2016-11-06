using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using UnityLisp;
using SDTK;

public class UnityLispConsole : EditorWindow {
	public static string autoPath = Application.dataPath.Replace("Assets","") + "/unity_lisp.ulisp";
	
	public string input = "";
	public string output = ">> Ready To Input\n\n";
	public Vector2 scroll;
	
	public environment consoleEnvironment;
	
	[MenuItem("UnityLisp/Console &u")]
	public static void OpenConsole() {
		var console = EditorWindow.GetWindow(typeof(UnityLispConsole)) as UnityLispConsole;
		console.consoleEnvironment = environment.global;
	}
	
	void OnEnable() {
		output = ">> Ready To Input\n\n";
		
		ReloadEnv();
	}
	
	void OnGUI() {
		GUILayout.BeginHorizontal();
		if(GUILayout.Button("Clean"))
			output = ">> Ready To Input\n\n";
		
		if(GUILayout.Button("Load", GUILayout.Width(60)))
			ReloadEnv();
		if(GUILayout.Button("Environment", GUILayout.Width(100)))	
			UpdateEnv();
		GUILayout.EndHorizontal();
		
		scroll = GUILayout.BeginScrollView(scroll);
			GUILayout.Label(output);
		GUILayout.EndScrollView();
		
		input = GUILayout.TextArea(input, GUILayout.Height(80));
		
		if(Event.current!=null && Event.current.isKey)
			KeyCommand(Event.current);
		
	}
	
	private void ReloadEnv() {
		if(DataRW.IsDataExists(autoPath))
			unity_lisp.load_environment(DataRW.LoadFile(autoPath));
	}
	
	private void UpdateEnv() {
		EnvironmentCheck.Display(environment.global,SetInput);
		Focus();
	}
	
	private void SetInput(string input) {
		this.input = input;
		Repaint();
	}
	
	private void KeyCommand(Event e) {
		if(e.keyCode == KeyCode.Return)
			CheckInput();
			
		if(e.keyCode == KeyCode.Tab)
			MatchBracket();
	}
	
	private void MatchBracket() {
		int fb = input.Replace("(","").Length;
		int rb = input.Replace(")","").Length;
		string attach = "";
		
		Loop.Count(rb - fb).Do(x=>attach+=")");
		input += attach;
		
		Repaint();
	}
	
	private void CheckInput() {
		int fb = input.Replace("(","").Length;
		int rb = input.Replace(")","").Length;
		
		if(fb == rb)
			Program();
	}
	
	private void Program() {
		string[] inputCmd = input.Split('\n');
		Loop.ForEach(inputCmd, AddEachLine);
		
		var lispProcess = unity_lisp.parse(input);
		
		input = "";
		
		output += "\n>>" + lispProcess.to_string() + "\n";
		
		try {
			var result = unity_lisp.evaluation(lispProcess, consoleEnvironment);
			output += ">>" + result.evalue()+"\n\n";
			
			if(environment.global.is_dirty) {
				environment.global.is_dirty = false;
				UpdateEnv();
				DataRW.CreateFile(environment.global.source_code().evalue(), autoPath);
			}
		}
		catch (System.Exception e) {
			scroll = Vector2.up * 2880f;
			Repaint();
			
			throw e;
		}
		
		scroll = Vector2.up * 2880f;
		Repaint();
	}
	
	private void AddEachLine(string line) {
		if(line.Length == 0)
			return;
		output += "<<" + line + "\n";
	}
	
}
