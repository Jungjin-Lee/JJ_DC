using UnityEngine;
using System.Collections;

public class AIPlayer : MonoBehaviour {
	public Dealer Dealer;
	PlayerPosition Player;
	int round = 0;

	void Awake() {
		Player = GetComponent<PlayerPosition>();
	}

	public void Race(int r) {
		round = r;
		if(isAbleRace()) {
			if(isAbleHalf()) {
				Debug.Log("AIPlayer Race:" + RaceType.Half + " slot:" + Player.slot);
				Dealer.Race(RaceType.Half, Player.slot);
			}
		} else {
			Dealer.Race(RaceType.Call, Player.slot);
		}
	}

	bool isAbleRace() {
		if(round == 0 && Dealer.raceCircle > 0) {
			return false;
		} else if(round == 1 && !Dealer.isRace) {
			return false;
		}
		return true;
	}

	bool isAbleHalf() {
		return true;
	}
}