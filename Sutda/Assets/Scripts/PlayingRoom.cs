using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayingRoom : MonoBehaviour {

	public int mySlot;
	public PlayerPosition myInfo;
	public PlayerPosition[] playersInfo;

	public Button btnStart;
	public Button btnExit;
	public Button btnRaceCall;
	public Button btnRaceDie;

	void Awake() {
		TcpSocketManager.Instance.EventLoginSuccess = new TcpSocketManager.EventSocket(EventLoginSuccess);
		TcpSocketManager.Instance.EventReceivedChat = new TcpSocketManager.EventSocket(EventReceivedChat);
		TcpSocketManager.Instance.EventRoomJoinSuccess = new TcpSocketManager.EventSocket(EventRoomJoinSuccess);
		TcpSocketManager.Instance.EventRoomConnectSuccess = new TcpSocketManager.EventSocket(EventRoomConnectSuccess);
		TcpSocketManager.Instance.EventRoomInfo = new TcpSocketManager.EventSocket(EventRoomInfo);
		TcpSocketManager.Instance.EventRoomGiveCard = new TcpSocketManager.EventSocket(EventRoomGiveCard);
		TcpSocketManager.Instance.EventRoomRaceNext = new TcpSocketManager.EventSocket(EventRoomRaceNext);
		TcpSocketManager.Instance.EventRoomGameResult = new TcpSocketManager.EventSocket(EventRoomGameResult);
	}
	
	void EventLoginSuccess(JJSocket sock) {
		SocketLoginSuccess login = sock as SocketLoginSuccess;
		Debug.Log("TcpClient - JJSocketType.LoginSuccess " + login.name);
		//		Main.Instance.name = login.name;
		//		LoginScene.SetActive(false);
		//		LobbyScene.SetActive(true);
		//		_user.Name = Main.Instance.name;
		
		SocketRoomConnect room = new SocketRoomConnect();
		TcpSocketManager.Instance.tcpSocket.SendMessage(room);
		//		SocketRoomJoin join = new SocketRoomJoin();
		//		join.index = 0;
		//		SocketManager.tcpSocket.SendMessage(join);
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
//		Main.Instance.room = room.room;
//		Main.Instance.mySlot = 1;
//		RoomScene.gameObject.SetActive(true);
//		Room3D.SetActive(true);
	}

	void EventRoomConnectSuccess(JJSocket sock) {
		SocketRoomConnectSuccess room = sock as SocketRoomConnectSuccess;
		Debug.Log("EventRoomConnectSuccess - room:" + room.room + " slot:" + room.slot);
//		Main.Instance.room = room.room;
//		Main.Instance.mySlot = room.slot;
		mySlot = room.slot;
	}

	void EventRoomInfo(JJSocket sock) {
		SocketRoomInfo room = sock as SocketRoomInfo;
		Debug.Log("EventRoomInfo - " + room.room);
//		playersInfo
//		Main.Instance.room = room.room;
//		Main.Instance.player1 = room.player1;
//		Main.Instance.player2 = room.player2;
//		RoomScene.txtPlayer1.text = Main.Instance.player1;
//		RoomScene.txtPlayer2.text = Main.Instance.player2;
//		
//		RoomScene.gameObject.SetActive(true);
//		Room3D.SetActive(true);
		if(mySlot == 0) {
			playersInfo[0].txtName.text = room.player1;
			playersInfo[1].txtName.text = room.player2;
			playersInfo[2].txtName.text = room.player3;
			playersInfo[3].txtName.text = room.player4;
		} else if(mySlot == 1) {
			playersInfo[0].txtName.text = room.player2;
			playersInfo[1].txtName.text = room.player3;
			playersInfo[2].txtName.text = room.player4;
			playersInfo[3].txtName.text = room.player0;
		} else if(mySlot == 2) {
			playersInfo[0].txtName.text = room.player3;
			playersInfo[1].txtName.text = room.player4;
			playersInfo[2].txtName.text = room.player0;
			playersInfo[3].txtName.text = room.player1;
		} else if(mySlot == 3) {
			playersInfo[0].txtName.text = room.player4;
			playersInfo[1].txtName.text = room.player0;
			playersInfo[2].txtName.text = room.player1;
			playersInfo[3].txtName.text = room.player2;
		} else if(mySlot == 4) {
			playersInfo[0].txtName.text = room.player0;
			playersInfo[1].txtName.text = room.player1;
			playersInfo[2].txtName.text = room.player2;
			playersInfo[3].txtName.text = room.player3;
		}
	}

	void EventRoomGiveCard(JJSocket sock) {
		SocketRoomGiveCard room = sock as SocketRoomGiveCard;
		string[] types = room.c.Split('-');
		int index = (int.Parse(types[0]) - 1) * 2 + (int.Parse(types[1]) - 1);
		Debug.Log("EventRoomGiveCard card:" + room.c + " index:" + index);
		if(room.n == 0) {
			myInfo.SetCard1(room.c);
//			myInfo.imgCard1.sprite = ImageContainer.Instance.Images[index];
//			myInfo.imgCard1.gameObject.SetActive(true);
		} else {
			myInfo.SetCard2(room.c);
//			myInfo.imgCard2.sprite = ImageContainer.Instance.Images[index];
//			myInfo.imgCard2.gameObject.SetActive(true);
			btnRaceCall.gameObject.SetActive(true);
			btnRaceDie.gameObject.SetActive(true);
		}
	}

	void EventRoomRaceNext(JJSocket sock) {
		SocketRoomRaceNext room = sock as SocketRoomRaceNext;
		Debug.Log("EventRoomRaceNext slot:" + room.n);
		if(room.n == mySlot) {
			btnRaceCall.gameObject.SetActive(true);
			btnRaceDie.gameObject.SetActive(true);
		} else {
			btnRaceCall.gameObject.SetActive(false);
			btnRaceDie.gameObject.SetActive(false);
		}
	}
	
	void EventRoomGameResult(JJSocket sock) {
		SocketRoomGameResult result = sock as SocketRoomGameResult;
		Debug.Log("EventRoomGameResult slot:" + result.playersCard);
		for(int i = 0; i < result.playersCard.Length; i++) {
			Debug.Log("slot:" + result.playersCard[i]);
		}

		int index = 0;
		if(mySlot == 0) {
			index = 2;
			playersInfo[0].SetCard(result.playersCard[2], result.playersCard[3]);
			playersInfo[1].SetCard(result.playersCard[4], result.playersCard[5]);
			playersInfo[2].SetCard(result.playersCard[6], result.playersCard[7]);
			playersInfo[3].SetCard(result.playersCard[8], result.playersCard[9]);
		} else if(mySlot == 1) {
			playersInfo[0].SetCard(result.playersCard[4], result.playersCard[5]);
			playersInfo[1].SetCard(result.playersCard[6], result.playersCard[7]);
			playersInfo[2].SetCard(result.playersCard[8], result.playersCard[9]);
			playersInfo[3].SetCard(result.playersCard[0], result.playersCard[1]);
		} else if(mySlot == 2) {
			playersInfo[0].SetCard(result.playersCard[6], result.playersCard[7]);
			playersInfo[1].SetCard(result.playersCard[8], result.playersCard[9]);
			playersInfo[2].SetCard(result.playersCard[0], result.playersCard[1]);
			playersInfo[3].SetCard(result.playersCard[2], result.playersCard[3]);
		} else if(mySlot == 3) {
			playersInfo[0].SetCard(result.playersCard[8], result.playersCard[9]);
			playersInfo[1].SetCard(result.playersCard[0], result.playersCard[1]);
			playersInfo[2].SetCard(result.playersCard[2], result.playersCard[3]);
			playersInfo[3].SetCard(result.playersCard[4], result.playersCard[5]);
		} else if(mySlot == 4) {
			playersInfo[0].SetCard(result.playersCard[0], result.playersCard[1]);
			playersInfo[1].SetCard(result.playersCard[2], result.playersCard[3]);
			playersInfo[2].SetCard(result.playersCard[4], result.playersCard[5]);
			playersInfo[3].SetCard(result.playersCard[6], result.playersCard[7]);
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
	}

	public void OnClickRaceCheck() {
	}

	public void OnClickRaceHalf() {
	}

	public void OnClickRaceCall() {
		JJSocket sock = new JJSocket();
		sock.type = JJSocketType.RoomRaceCall;
		TcpSocketManager.Instance.tcpSocket.SendMessage(sock);

		btnRaceCall.gameObject.SetActive(false);
		btnRaceDie.gameObject.SetActive(false);
	}

	public void OnClickRaceDie() {
		JJSocket sock = new JJSocket();
		sock.type = JJSocketType.RoomRaceDie;
		TcpSocketManager.Instance.tcpSocket.SendMessage(sock);

		btnRaceCall.gameObject.SetActive(false);
		btnRaceDie.gameObject.SetActive(false);
	}
}