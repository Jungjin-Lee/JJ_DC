using UnityEngine;
using System.Collections;

public class Main {
	static Main _instance;

	public string id = "";
	public string pwd = "";
	public string name = "";

	public static Main Instance {
		get {
			if(_instance == null) {
				_instance = new Main();
			}
			return _instance;
		}
	}

	public void Init() {
	}
}