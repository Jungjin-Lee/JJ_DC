using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayingRoom : MonoBehaviour {

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

	void Awake() {
		TcpSocketManager.Instance.EventLoginSuccess = new TcpSocketManager.EventSocket(EventLoginSuccess);
		TcpSocketManager.Instance.EventReceivedChat = new TcpSocketManager.EventSocket(EventReceivedChat);
		TcpSocketManager.Instance.EventRoomJoinSuccess = new TcpSocketManager.EventSocket(EventRoomJoinSuccess);
		TcpSocketManager.Instance.EventRoomConnectSuccess = new TcpSocketManager.EventSocket(EventRoomConnectSuccess);
		TcpSocketManager.Instance.EventRoomInfo = new TcpSocketManager.EventSocket(EventRoomInfo);
		TcpSocketManager.Instance.EventRoomGiveCard = new TcpSocketManager.EventSocket(EventRoomGiveCard);
		TcpSocketManager.Instance.EventRoomRaceHalf = new TcpSocketManager.EventSocket(EventRoomRaceHalf);
		TcpSocketManager.Instance.EventRoomRaceNext = new TcpSocketManager.EventSocket(EventRoomRaceNext);
		TcpSocketManager.Instance.EventRoomGameResult = new TcpSocketManager.EventSocket(EventRoomGameResult);
	}
	
	void EventLoginSuccess(JJSocket sock) {
		SocketLoginSuccess login = sock as SocketLoginSuccess;
		Debug.Log("TcpClient - JJSocketType.LoginSuccess " + login.name);
		
		SocketRoomConnect room = new SocketRoomConnect();
		TcpSocketManager.Instance.tcpSocket.SendMessage(room);
	}
	
	void EventReceivedChat(JJSocket sock) {
		SocketChat chat = sock as SocketChat;
//		Chat.Instance.ChatContainer2.text += chat.name +  " : " + chat.message + "\n";
		Debug.Log("EventReceivedChat - " + chat.name +  " : " + chat.message);
	}
	
	void EventRoomJoinSuccess(JJSocket sock) {
		SocketRoomJoin room = sock as SocketRoomJoin;
		Debug.Log("EventRoomJoinSuccess - " + room.room + " slot:0");
		btnStart.gameObject.SetActive(true);
		mySlot = 0;
		for(int i = 0; i < playersInfo.Length; i++) {
			playersInfo[i].SetSlot(i);
		}
	}

	void EventRoomConnectSuccess(JJSocket sock) {
		SocketRoomConnectSuccess room = sock as SocketRoomConnectSuccess;
		Debug.Log("EventRoomConnectSuccess - room:" + room.room + " slot:" + room.slot);
		mySlot = room.slot;
		int i = 0;
		int slot = mySlot;
		while(i < playersInfo.Length) {
			playersInfo[i].SetSlot(slot);
			slot++;
			i++;
			if(slot >= 5) {
				slot = 0;
			}
		}
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
		MoneyRoom.Instance.Give(0);

		btnRaceBbing.gameObject.SetActive(false);
		btnRaceCheck.gameObject.SetActive(false);
		btnRaceHalf.gameObject.SetActive(false);
		btnRaceCall.gameObject.SetActive(false);
		btnRaceDie.gameObject.SetActive(false);
	}

	public void OnClickRaceCheck() {
		JJSocket sock = new JJSocket();
		sock.type = JJSocketType.RoomRaceCheck;
		TcpSocketManager.Instance.tcpSocket.SendMessage(sock);

		btnRaceBbing.gameObject.SetActive(false);
		btnRaceCheck.gameObject.SetActive(false);
		btnRaceHalf.gameObject.SetActive(false);
		btnRaceCall.gameObject.SetActive(false);
		btnRaceDie.gameObject.SetActive(false);
	}

	public void OnClickRaceHalf() {
		JJSocket sock = new JJSocket();
		sock.type = JJSocketType.RoomRaceHalf;
		TcpSocketManager.Instance.tcpSocket.SendMessage(sock);
		MoneyRoom.Instance.Give(0);

		btnRaceBbing.gameObject.SetActive(false);
		btnRaceCheck.gameObject.SetActive(false);
		btnRaceHalf.gameObject.SetActive(false);
		btnRaceCall.gameObject.SetActive(false);
		btnRaceDie.gameObject.SetActive(false);
	}

	public void OnClickRaceCall() {
		JJSocket sock = new JJSocket();
		sock.type = JJSocketType.RoomRaceCall;
		TcpSocketManager.Instance.tcpSocket.SendMessage(sock);

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

		btnRaceBbing.gameObject.SetActive(false);
		btnRaceCheck.gameObject.SetActive(false);
		btnRaceHalf.gameObject.SetActive(false);
		btnRaceCall.gameObject.SetActive(false);
		btnRaceDie.gameObject.SetActive(false);
	}
}