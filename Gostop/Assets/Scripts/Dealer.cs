using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Jokbo {
	Jokbo_38,
	Jokbo_18,
	Jokbo_ddang,
}

public enum RaceType {
	Half,
	Double,
	Bbing,
	Check,
	Call,
	Die,
}

public class Dealer : MonoBehaviour {
	int RoomUserMax = 5;

	public PlayingRoom PlayingRoom;

	List<string> deck = new List<string>();
	int index = 0;

	public int leader = 0;
	public int round = 0;	// 첫 번째 0, 두 번째 1
	int money = 0;
	public int perMoney = 0;	// ㅍㅏㄴㄷㅗㄴ
	public bool isRace = true;
	public int raceCircle = 0;
	int raceStopSlot = 0;
	int roundMoney = 0;

	int[] raceMoneys = new int[5];
	PlayerPosition[] players;
	string[] playersCard = new string[10];

	int count = 0;

	void Awake() {
		for(int i = 0; i < 20; i++) {
			deck.Add(((i / 2) + 1) + "-" + (i % 2 + 1));
		}
		suffle();
		players = PlayingRoom.playersInfo;
	}

	public void suffle() {
		Debug.Log("suffle");
		for(int i = 0; i < deck.Count; i++) {
			int random = Random.Range(0, 20);
			string save = deck[i];
			deck[i] = deck[random];
			deck[random] = save;
		}

		for(int i = 0; i < deck.Count; i++) {
//			Debug.Log(i + " : " + deck[i]);
		}
	}

	public string pop() {
		return deck[index++];
	}                                                                                                                                                               

	public int jokbo() {
		return 0;
	}

	public void GiveCard(int r) {
		isRace = true;
		raceStopSlot = 0;
		raceCircle = 0;

		round = r;
		for(int i = 0; i < players.Length; i++) {
			string card = pop();
			playersCard[i * 2 + r] = card;

			if(i == PlayingRoom.mySlot) {
				if(round == 0) {
					PlayingRoom.playersInfo[i].SetCard1(card);
				} else {
					PlayingRoom.playersInfo[i].SetCard2(card);
				}
			} else {
				if(round == 0) {
					PlayingRoom.playersInfo[i].card1 = card;
					PlayingRoom.playersInfo[i].SetBack1();
				} else {
					PlayingRoom.playersInfo[i].card2 = card;
					PlayingRoom.playersInfo[i].SetBack2();
				}
			}
		}

		if(leader == PlayingRoom.mySlot) {
			PlayingRoom.Race();
		} else {
			AIPlayer ai = PlayingRoom.playersInfo[leader].GetComponent<AIPlayer>();
			ai.Race(round);
		}
	}

	public void Restart() {
		suffle();
		index = 0;

		leader = 0;
		round = 0;
		money = 0;
		isRace = true;
		raceStopSlot = 0;
		roundMoney = 0;
	}

	public void Race(RaceType race, int slot) {
		Debug.Log("round:" + round + " circle:" + raceCircle + " slot:" + slot + " race:" + race + " count:" + count);

		if(race == RaceType.Half) {
			RaceHalf(slot);
		} else if(race == RaceType.Call) {
			RaceCall(slot);
		} else if(race == RaceType.Check) {
			RaceCheck(slot);
		} else if(race == RaceType.Bbing) {
			RaceBbing(slot);
		} else if(race == RaceType.Double) {
			RaceDouble(slot);
		} else if(race == RaceType.Die) {
			RaceDie(slot);
		}

		if((round == 0 && isLastPlayer(slot) && isRace) || (race == RaceType.Call && isRace)) {
			Debug.Log("first call:" + slot);
			isRace = false;
			raceStopSlot = slot;
		}

		StartCoroutine("RaceNext", slot);
//		RaceNext(slot);
	}

	void RaceCheck(int slot) {
	}

	void RaceBbing(int slot) {
		MoneyRoom.Instance.Give(slot);
	}

	void RaceDouble(int slot) {
		// dda ddang
		MoneyRoom.Instance.Give(slot);
	}

	void RaceHalf(int slot) {
		MoneyRoom.Instance.Give(slot);
	}

	void RaceCall(int slot) {
	}

	void RaceDie(int slot) {
	}

	public bool isLastPlayer(int slot) {
		for(int i = 4; i >= 0; i--) {
			PlayerPosition p = players[i];
			if(p.slot == slot && !p.isDie) {
				return true;
			} else {
				return false;
			}
		}
		return false;
	}

	IEnumerator RaceNext(int slot) {
		yield return new WaitForSeconds(Random.Range(0.5f, 1f));
		slot++;
		if(slot >= PlayingRoom.playersInfo.Length) {
			slot = 0;
		}

		if(slot == leader) {
			raceCircle++;
		}

		if(round == 0 && raceCircle == 1 && isLastPlayer(slot)) {
			GiveCard(1);
		} else if(round == 1 && !isRace && raceStopSlot == slot) {
			Debug.Log("RaceNext CardOpen:" + slot);
			PlayingRoom.CardOpen();
		} else if(slot == PlayingRoom.mySlot) {
			Debug.Log("RaceNext Race:" + slot);
			PlayingRoom.Race();
		} else {
			Debug.Log("RaceNext:" + slot);
			AIPlayer ai = PlayingRoom.playersInfo[slot].GetComponent<AIPlayer>();
			ai.Race(round);
		}
	}
}