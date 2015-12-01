using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ImageContainer : MonoBehaviour {

	static ImageContainer instance;
	const int Back = 20;
	public Sprite[] Images;

	void Awake() {
		instance = this;
	}

	public static ImageContainer Instance {
		get {
			return instance;
		}
	}

	public Sprite GetBackImage() {
		return Images[Back];
	}
}
