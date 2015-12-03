using UnityEngine;
using System;
using System.Text;

public class GameConstants {
	// service false!!!
	static public bool IsDebugWindow		= true;
	static public bool IsDebugFPS			= false;
	static public bool IsSelectServer		= false;
	static public bool IsPlayDataLog		= false;

	// service true!!!
	static public bool IsRelease			= true;



	static bool _init = false;


	public static void Init() {
		if(_init) return;
		_init = true;
		SetPlatform();
	}

	public static void SetPlatform() {
	}


	public static string GetUDID() {
//		if(Application.platform == RuntimePlatform.IPhonePlayer && UDID != "") {
//			return UDID;
//		}
		return SystemInfo.deviceUniqueIdentifier;
	}
}