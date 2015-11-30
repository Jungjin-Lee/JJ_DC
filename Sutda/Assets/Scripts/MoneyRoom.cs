using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoneyRoom : MonoBehaviour {
	static MoneyRoom instance;
	public Transform hide;
	GameObject pfCoin;
	List<GameObject> HideContainer = new List<GameObject>();
	List<GameObject> ShowContainer = new List<GameObject>();
	Vector3 defaultScale = new Vector3(1f, 1f, 1f);

	void Awake() {
		instance = this;
		pfCoin = (GameObject)Resources.Load ("Prefabs/pfCoin") as GameObject;
		Init();
	}

	public static MoneyRoom Instance {
		get {
			return instance;
		}
	}

	GameObject Make() {
		return (GameObject)GameObject.Instantiate(pfCoin, Vector3.zero, Quaternion.identity);
	}

	void Init() {
		for(int i = 0; i < 300; i++) {
			GameObject go = Make();
			go.SetActive(false);
			go.transform.SetParent(hide);
			HideContainer.Add(go);
		}
	}

	GameObject Pop() {
		GameObject go;
		if(HideContainer.Count > 0) {
			go = HideContainer[0];
			HideContainer.RemoveAt(0);
			ShowContainer.Add(go);
		} else {
			go = Make();
			ShowContainer.Add(go);
		}
		go.SetActive(true);
		go.transform.SetParent(transform);
		go.transform.localScale = defaultScale;
		go.transform.localRotation = Quaternion.identity;
		return go;
	}

	void Push(GameObject go) {
		ShowContainer.Remove(go);
		go.SetActive(false);
		go.transform.SetParent(hide);
		HideContainer.Add(go);
	}

	void Update2() {
		if(Input.GetMouseButtonDown(0)) {
			for(int i = 0; i < 5; i++) {
				GameObject go = Pop();
				Vector3 p = Vector3.zero;
				p.Set(0f, -155.67f, 0f);
				go.transform.localPosition = p;

				LeanTween.moveLocal(go, new Vector3(Random.Range(-70, 70), Random.Range(-70, 70), 0), 0.25f);
				LeanTween.rotateLocal(go, new Vector3(0f, 0f, Random.Range(90f, 360f)), 0.25f);
			}
		}
	}

	public void Clear() {
		while(ShowContainer.Count > 0) {
			Push(ShowContainer[0]);
		}
	}

	public void Give(int slot) {
		for(int i = 0; i < 5; i++) {
			GameObject go = Pop();
			Vector3 p = Vector3.zero;
			if(slot == 0) {
				p.Set(0f, -155.67f, 0f);
			} else if(slot == 1) {
				p.Set(-300f, -155.67f, 0f);
			} else if(slot == 2) {
				p.Set(-300f, 0f, 0f);
			} else if(slot == 3) {
				p.Set(-300f, 150f, 0f);
			} else if(slot == 4) {
				p.Set(300f, 150f, 0f);
			}
			go.transform.localPosition = p;
			
			LeanTween.moveLocal(go, new Vector3(Random.Range(-70, 70), Random.Range(-70, 70), 0), 0.25f);
			LeanTween.rotateLocal(go, new Vector3(0f, 0f, Random.Range(90f, 360f)), 0.25f);
		}
	}
}
