using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ImageContainer : MonoBehaviour {

	static ImageContainer instance;
	public Sprite[] Images;

	void Awake() {
		instance = this;
	}

	public static ImageContainer Instance {
		get {
			return instance;
		}
	}
}
