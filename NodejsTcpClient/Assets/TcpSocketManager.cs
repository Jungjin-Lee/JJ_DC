using UnityEngine;
using System.Collections;
using LitJson;

public class TcpSocketManager : MonoBehaviour {

	public delegate void EventSocket(JJSocket sock);

	public EventSocket EventLoginSuccess;
	public EventSocket EventReceivedChat;

	public TcpSocket tcpSocket;

	void Awake() {
		tcpSocket = new TcpSocket();
	}

	void Update() {
		tcpSocket.ReceiveMessage();
		JJSocket sock = tcpSocket.Pop();
		ProcessSock(sock);
	}

	void ProcessSock(JJSocket sock) {
		if(sock == null) return;

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
		}
	}

	public void Connect() {
		tcpSocket.Connect();
	}
}