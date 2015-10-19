using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Chat : MonoBehaviour {

	static Chat _instance;
	public Text ChatContainer1;
	public Text ChatContainer2;

	void Awake() {
		_instance = this;
		ChatContainer2 = GetComponent<Text>();
	}

	public static Chat Instance {
		get {
			if(_instance != null) {
				return _instance;
			}

			return null;
		}
	}
}