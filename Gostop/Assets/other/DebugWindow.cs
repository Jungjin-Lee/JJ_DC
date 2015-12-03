using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DebugWindow : MonoBehaviour {

	public static DebugWindow instance;
	bool isShow = false;
	public Text txt;

	void Awake() {
		instance = this;
		DontDestroyOnLoad (this);
	}

	private static void Init() {
		Make ();
	}

	private static void Make() {
//		if (instance == null) {
//			GameObject go = new GameObject("--DebugWindow");
//			instance = go.AddComponent<DebugWindow>();
//		}
	}

	void Update () {
		if(!GameConstants.IsDebugWindow) return;

		if (Input.touchCount == 4) {
			if(Input.GetTouch(3).phase == TouchPhase.Began) {
				isShow = !isShow;
			}
		} else if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.F10)) {
			isShow = !isShow;
		} else if(Input.GetKey(KeyCode.LeftControl)) {
			if(Input.GetMouseButtonDown(0)) {
				isShow = !isShow;
			}
		}
	}

	
	public static void Log(string str) {
		if(!GameConstants.IsDebugWindow) return;
		if(instance == null ) return;
		if(str == null) return;

		Make ();
		instance.txt.text += "\n" + str;
	}
}