using UnityEngine;
using System.Collections;
using LitJson;

public class TcpSocketManager : MonoBehaviour {

	static TcpSocketManager instance;
	public delegate void EventSocket(JJSocket sock);

	public EventSocket EventConnected;
    public EventSocket EventConnectFailed;
    public EventSocket EventLoginSuccess;
	public EventSocket EventReceivedChat;
	public EventSocket EventRoomJoinSuccess;
	public EventSocket EventRoomConnectSuccess;
	public EventSocket EventRoomInfo;
	public EventSocket EventRoomPlay;
	public EventSocket EventRoomPlayDice;
	public EventSocket EventRoomThrowDice;
	public EventSocket EventRoomGiveCard;
	public EventSocket EventRoomRaceHalf;
	public EventSocket EventRoomRaceNext;
	public EventSocket EventRoomGameResult;

	public TcpSocket tcpSocket;

	public static TcpSocketManager Instance {
		get {
			return instance;
		}
	}

	void Awake() {
		instance = this;
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
			EventConnected(sock);
			break;
       case JJSocketType.ConnectFailed:
            EventConnectFailed(sock);
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

		case JJSocketType.RoomGiveCard:
			EventRoomGiveCard(sock);
			break;
		case JJSocketType.RoomRaceHalf:
			EventRoomRaceHalf(sock);
			break;
		case JJSocketType.RoomRaceNext:
			EventRoomRaceNext(sock);
			break;
		case JJSocketType.RoomGameResult:
			EventRoomGameResult(sock);
			break;
		}
	}

	public void Connect(string ip, int port) {
		tcpSocket.Connect(ip, port);
	}
}