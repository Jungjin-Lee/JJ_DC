using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaitingRoom : MonoBehaviour {

	public Text txtName;

	void Awake() {
	}

	void Start() {
		TcpSocketManager.Instance.EventConnected = new TcpSocketManager.EventSocket(EventConnected);
	}

	void EventConnected(JJSocket sock) {
		SocketLogin login = new SocketLogin();
		login.id = txtName.text;
		login.pwd = "2";
		TcpSocketManager.Instance.tcpSocket.SendMessage(login);
	}
}
