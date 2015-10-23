using UnityEngine;
using System.Collections;
using LitJson;

public class TcpSocketManager : MonoBehaviour {

	public delegate void EventSocket(JJSocket sock);

	public EventSocket EventLoginSuccess;
	public EventSocket EventReceivedChat;
	public EventSocket EventRoomJoinSuccess;
	public EventSocket EventRoomConnectSuccess;
	public EventSocket EventRoomInfo;
	public EventSocket EventRoomPlay;
	public EventSocket EventRoomPlayDice;
	public EventSocket EventRoomThrowDice;

	public TcpSocket tcpSocket;

	void Awake() {
		tcpSocket = new TcpSocket();
	}

	void Update() {
		tcpSocket.ReceiveMessage();
		JJSocket sock = tcpSocket.Pop();
		DebugWindow.Log(tcpSocket.PopSockString());
		ProcessSock(sock);
	}

	void ProcessSock(JJSocket sock) {
		if(sock == null) return;
//		SocketLoginSuccess login = sock as SocketLoginSuccess;

		switch(sock.type) {
		case JJSocketType.Connected:
			Debug.Log("ProcessSock - JJSocketType.Connected");
			SocketLogin login = new SocketLogin();
			login.id = Main.Instance.id;
			login.pwd = Main.Instance.pwd;
			tcpSocket.SendMessage(login);
			break;

		case JJSocketType.LoginSuccess:
			EventLoginSuccess(sock);
			break;

		case JJSocketType.ReceivedChat:
			EventReceivedChat(sock);
			break;

		case JJSocketType.RoomJoinSuccess:
			EventRoomJoinSuccess(sock);
			break;

		case JJSocketType.RoomConnectSuccess:
			EventRoomConnectSuccess(sock);
			break;
		case JJSocketType.RoomInfo:
			EventRoomInfo(sock);
			break;
		case JJSocketType.RoomPlay:
			break;
		case JJSocketType.RoomPlayDice:
			EventRoomPlayDice(sock);
			break;
		case JJSocketType.RoomThrowDice:
			EventRoomThrowDice(sock);
			break;
		}
	}

	public void Connect() {
		tcpSocket.Connect();
	}
}