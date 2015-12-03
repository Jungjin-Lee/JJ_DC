using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum PlayingRoomState {
	None,
	Join,
	Play_MyTurn,
	Play_Other,
	End,
}

public class PlayingRoom : MonoBehaviour {

	public Dealer Dealer;
	public int mySlot;
	public PlayerPosition myInfo;
	public PlayerPosition[] playersInfo;

	public Button btnStart;
	public Button btnExit;
	public Button btnRaceHalf;
	public Button btnRaceBbing;
	public Button btnRaceCheck;
	public Button btnRaceCall;
	public Button btnRaceDie;

	public PlayingRoomState State = PlayingRoomState.None;

	void Awake() {
		for(int i = 0; i < playersInfo.Length; i++) {
			AIPlayer ai = playersInfo[i].GetComponent<AIPlayer>();
			if(ai) {
				ai.Dealer = Dealer;
			}
		}
	}

	void Start() {
		StartCoroutine("StartSetting");
	}

	IEnumerator StartSetting() {
		State = PlayingRoomState.Join;
		yield return new WaitForSeconds(0.1f);
		for(int i = 0; i < playersInfo.Length; i++) {
			playersInfo[i].SetSlot(i);
		}
		yield return new WaitForSeconds(1.1f);
		Dealer.GiveCard(0);
		yield return new WaitForSeconds(1.0f);
		State = PlayingRoomState.Play_MyTurn;
	}

	PlayerPosition GetPlayerAtSlot(int n) {
		for(int i = 0; i < playersInfo.Length; i++) {
			if(playersInfo[i].slot == n) {
				return playersInfo[i];
			}
		}
		return null;
	}

	int GetPlayerIndexAtSlot(int n) {
		for(int i = 0; i < playersInfo.Length; i++) {
			if(playersInfo[i].slot == n) {
				return i;
			}
		}
		return 0;
	}

	void EventRoomInfo(JJSocket sock) {
		SocketRoomInfo room = sock as SocketRoomInfo;
		Debug.Log("EventRoomInfo - " + room.room);
		GetPlayerAtSlot(0).txtName.text = room.player0;
		GetPlayerAtSlot(1).txtName.text = room.player1;
		GetPlayerAtSlot(2).txtName.text = room.player2;
		GetPlayerAtSlot(3).txtName.text = room.player3;
		GetPlayerAtSlot(4).txtName.text = room.player4;
	}

	void EventRoomGiveCard(JJSocket sock) {
		SocketRoomGiveCard room = sock as SocketRoomGiveCard;
		string[] types = room.c.Split('-');
		int index = (int.Parse(types[0]) - 1) * 2 + (int.Parse(types[1]) - 1);
		int leader = 0;
		Debug.Log("EventRoomGiveCard card:" + room.c + " index:" + index);

		if(mySlot == leader) {
			btnRaceBbing.gameObject.SetActive(true);
			btnRaceCheck.gameObject.SetActive(true);
			btnRaceHalf.gameObject.SetActive(true);
			btnRaceDie.gameObject.SetActive(true);
			btnRaceCall.gameObject.SetActive(false);
		} else {
			btnRaceBbing.gameObject.SetActive(false);
			btnRaceCheck.gameObject.SetActive(false);
			btnRaceHalf.gameObject.SetActive(false);
			btnRaceDie.gameObject.SetActive(false);
			btnRaceCall.gameObject.SetActive(false);
		}

		if(room.n == 0) {
			for(int i = 0; i < playersInfo.Length; i++) {
				if(playersInfo[i].slot == mySlot) {
					playersInfo[i].SetCard1(room.c);
				} else {
					playersInfo[i].SetBack1();
				}
			}
		} else {
			for(int i = 0; i < playersInfo.Length; i++) {
				if(playersInfo[i].slot == mySlot) {
					playersInfo[i].SetCard2(room.c);
				} else {
					playersInfo[i].SetBack2();
				}
			}
		}
	}

	public void Race() {
		State = PlayingRoomState.Play_MyTurn;
		btnRaceCheck.gameObject.SetActive(true);
		btnRaceBbing.gameObject.SetActive(true);
		btnRaceHalf.gameObject.SetActive(true);
		btnRaceDie.gameObject.SetActive(true);
		btnRaceCall.gameObject.SetActive(false);

		if(Dealer.round == 0 && Dealer.raceCircle == 1) {
			btnRaceCheck.gameObject.SetActive(false);
			btnRaceBbing.gameObject.SetActive(false);
			btnRaceHalf.gameObject.SetActive(false);
			btnRaceDie.gameObject.SetActive(true);
			btnRaceCall.gameObject.SetActive(true);
		} else if(Dealer.raceCircle > 0) {
			btnRaceCheck.gameObject.SetActive(false);
			btnRaceCall.gameObject.SetActive(true);
		}
	}

	public void CardOpen() {
		for(int i = 0; i < playersInfo.Length; i++) {
			playersInfo[i].Open();
		}
	}

	void EventRoomRaceHalf(JJSocket sock) {
		SocketRoomRaceHalf room = sock as SocketRoomRaceHalf;
		Debug.Log("EventRoomRaceHalf slot:" + room.n);
		int index = GetPlayerIndexAtSlot(room.n);
		MoneyRoom.Instance.Give(index);
	}

	void EventRoomRaceNext(JJSocket sock) {
		SocketRoomRaceNext room = sock as SocketRoomRaceNext;
		Debug.Log("EventRoomRaceNext slot:" + room.n);

		if(room.type2 == JJSocketType.RoomRaceBbing) {
		} else if(room.type2 == JJSocketType.RoomRaceCheck) {
		} else if(room.type2 == JJSocketType.RoomRaceHalf) {
		} else if(room.type2 == JJSocketType.RoomRaceDie) {
		} else if(room.type2 == JJSocketType.RoomRaceCall) {
		}

		if(room.n == mySlot) {
			btnRaceBbing.gameObject.SetActive(true);
			btnRaceCheck.gameObject.SetActive(false);
			btnRaceHalf.gameObject.SetActive(true);
			btnRaceDie.gameObject.SetActive(true);
			btnRaceCall.gameObject.SetActive(true);
		} else {
			btnRaceBbing.gameObject.SetActive(false);
			btnRaceCheck.gameObject.SetActive(false);
			btnRaceHalf.gameObject.SetActive(false);
			btnRaceDie.gameObject.SetActive(false);
			btnRaceCall.gameObject.SetActive(false);
		}

		if(room.n2 == mySlot) {
			MoneyRoom.Instance.Give(0);
		} else {
			int index = GetPlayerIndexAtSlot(room.n2);
			MoneyRoom.Instance.Give(index);
		}
	}
	
	void EventRoomGameResult(JJSocket sock) {
		SocketRoomGameResult result = sock as SocketRoomGameResult;
		Debug.Log("EventRoomGameResult slot:" + result.playersCard);
		for(int i = 0; i < result.playersCard.Length; i++) {
			Debug.Log("slot:" + result.playersCard[i]);
		}

		for(int i = 0; i < playersInfo.Length; i++) {
			GetPlayerAtSlot(i).SetCard(result.playersCard[i * 2], result.playersCard[i * 2 + 1]);
		}

		btnRaceCall.gameObject.SetActive(false);
		btnRaceDie.gameObject.SetActive(false);
	}

	public void OnClickStart() {
		SocketRoomPlay play = new SocketRoomPlay();
		TcpSocketManager.Instance.tcpSocket.SendMessage(play);
		btnStart.gameObject.SetActive(false);
		btnRaceCall.gameObject.SetActive(true);
		btnRaceDie.gameObject.SetActive(true);
	}

	public void OnClickExit() {
		JJSocket sock = new JJSocket();
		sock.type = JJSocketType.RoomSelfExit;
		TcpSocketManager.Instance.tcpSocket.SendMessage(sock);
	}

	public void OnClickRaceBBing() {
//		MoneyRoom.Instance.Give(0);
		Dealer.Race(RaceType.Bbing, mySlot);

		btnRaceBbing.gameObject.SetActive(false);
		btnRaceCheck.gameObject.SetActive(false);
		btnRaceHalf.gameObject.SetActive(false);
		btnRaceCall.gameObject.SetActive(false);
		btnRaceDie.gameObject.SetActive(false);
	}

	public void OnClickRaceCheck() {
//		JJSocket sock = new JJSocket();
//		sock.type = JJSocketType.RoomRaceCheck;
//		TcpSocketManager.Instance.tcpSocket.SendMessage(sock);
		Dealer.Race(RaceType.Check, mySlot);

		btnRaceBbing.gameObject.SetActive(false);
		btnRaceCheck.gameObject.SetActive(false);
		btnRaceHalf.gameObject.SetActive(false);
		btnRaceCall.gameObject.SetActive(false);
		btnRaceDie.gameObject.SetActive(false);
	}

	public void OnClickRaceHalf() {
//		JJSocket sock = new JJSocket();
//		sock.type = JJSocketType.RoomRaceHalf;
//		TcpSocketManager.Instance.tcpSocket.SendMessage(sock);
//		MoneyRoom.Instance.Give(0);

		Dealer.Race(RaceType.Half, mySlot);
		State = PlayingRoomState.Play_Other;

		btnRaceBbing.gameObject.SetActive(false);
		btnRaceCheck.gameObject.SetActive(false);
		btnRaceHalf.gameObject.SetActive(false);
		btnRaceCall.gameObject.SetActive(false);
		btnRaceDie.gameObject.SetActive(false);
	}

	public void OnClickRaceCall() {
//		JJSocket sock = new JJSocket();
//		sock.type = JJSocketType.RoomRaceCall;
//		TcpSocketManager.Instance.tcpSocket.SendMessage(sock);

//		Dealer.RaceCall(mySlot);
		Dealer.Race(RaceType.Call, mySlot);
		State = PlayingRoomState.Play_Other;

		btnRaceBbing.gameObject.SetActive(false);
		btnRaceCheck.gameObject.SetActive(false);
		btnRaceHalf.gameObject.SetActive(false);
		btnRaceCall.gameObject.SetActive(false);
		btnRaceDie.gameObject.SetActive(false);
	}

	public void OnClickRaceDie() {
		JJSocket sock = new JJSocket();
		sock.type = JJSocketType.RoomRaceDie;
		TcpSocketManager.Instance.tcpSocket.SendMessage(sock);
		Dealer.Race(RaceType.Die, mySlot);

		btnRaceBbing.gameObject.SetActive(false);
		btnRaceCheck.gameObject.SetActive(false);
		btnRaceHalf.gameObject.SetActive(false);
		btnRaceCall.gameObject.SetActive(false);
		btnRaceDie.gameObject.SetActive(false);
	}
}