using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerPosition : MonoBehaviour {

	public Text txtName;
	public Text txtMoney;
	public int money;

	public Image imgCard1;
	public Image imgCard2;

	public void SetCard(string c1, string c2) {
		if(c1 == null || c2 == null) return;
		SetCard1(c1);
		SetCard2(c2);
	}

	public void SetCard1(string c) {
		string[] types = c.Split('-');
		int index = (int.Parse(types[0]) - 1) * 2 + (int.Parse(types[1]) - 1);
		imgCard1.sprite = ImageContainer.Instance.Images[index];
		imgCard1.gameObject.SetActive(true);
	}

	public void SetCard2(string c) {
		string[] types = c.Split('-');
		int index = (int.Parse(types[0]) - 1) * 2 + (int.Parse(types[1]) - 1);
		imgCard2.sprite = ImageContainer.Instance.Images[index];
		imgCard2.gameObject.SetActive(true);
	}
}
