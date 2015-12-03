using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerPosition : MonoBehaviour {

	public int slot;
	public Text txtName;
	public Text txtMoney;
	public Text txtSlot;
	public int money;

	public string card1;
	public string card2;
	public Image imgCard1;
	public Image imgCard2;

	public bool isDie = false;

	public void SetSlot(int n) {
		slot = n;
		txtSlot.text = slot.ToString();
	}

	public void SetCard(string c1, string c2) {
		if(c1 == null || c2 == null) return;
		SetCard1(c1);
		SetCard2(c2);
	}

	public void SetBack1() {
		imgCard1.sprite = ImageContainer.Instance.GetBackImage();
		imgCard1.gameObject.SetActive(true);
	}

	public void SetBack2() {
		imgCard2.sprite = ImageContainer.Instance.GetBackImage();
		imgCard2.gameObject.SetActive(true);
	}

	public void SetCard1(string c) {
		string[] types = c.Split('-');
		int index = (int.Parse(types[0]) - 1) * 2 + (int.Parse(types[1]) - 1);
		card1 = c;
		imgCard1.sprite = ImageContainer.Instance.Images[index];
		imgCard1.gameObject.SetActive(true);
	}

	public void SetCard2(string c) {
		string[] types = c.Split('-');
		int index = (int.Parse(types[0]) - 1) * 2 + (int.Parse(types[1]) - 1);
		card2 = c;
		imgCard2.sprite = ImageContainer.Instance.Images[index];
		imgCard2.gameObject.SetActive(true);
	}

	public void Open() {
		SetCard1(card1);
		SetCard2(card2);
	}
}
