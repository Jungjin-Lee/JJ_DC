using UnityEngine;
using System.Collections;

public class Main {
	static Main _instance;

	public string id = "";
	public string pwd = "";
	public string name = "";


	public int room = -1;
	public int mySlot = -1;

	public string player1 = "";
	public string player2 = "";

	public static Main Instance {
		get {
			if(_instance == null) {
				_instance = new Main();
			}
			return _instance;
		}
	}

	public void Init() {
		room = -1;
		mySlot = -1;
	}
}