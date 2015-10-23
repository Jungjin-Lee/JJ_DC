using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System;
using LitJson;
using UnityEngine.UI;

public class User {
	public string Name {get; set;}
	public string Message {get; set;}
}

public class TcpClient : MonoBehaviour {

	public GameObject LoginScene;
	public GameObject LobbyScene;
	public RoomScene RoomScene;
	public GameObject Room3D;

	public string IP = "127.0.0.1";
	public int PORT = 8000;

	public InputField txtName;
	public InputField txtMessage;
	public Text txtChatContainer;

	public TcpSocketManager SocketManager = null;
	User _user = new User();
	SocketChat sockChat = new SocketChat();

	byte[] _recvBuffer = new byte[1024];

	void Awake() {
		Main.Instance.Init();
	}

	void Start() {
		SocketManager.EventLoginSuccess = new TcpSocketManager.EventSocket(EventLoginSuccess);
		SocketManager.EventReceivedChat = new TcpSocketManager.EventSocket(EventReceivedChat);
		SocketManager.EventRoomJoinSuccess = new TcpSocketManager.EventSocket(EventRoomJoinSuccess);
		SocketManager.EventRoomConnectSuccess = new TcpSocketManager.EventSocket(EventRoomConnectSuccess);
		SocketManager.EventRoomInfo = new TcpSocketManager.EventSocket(EventRoomInfo);
		SocketManager.EventRoomPlay = new TcpSocketManager.EventSocket(EventRoomPlay);
		SocketManager.EventRoomPlayDice = new TcpSocketManager.EventSocket(EventRoomPlayDice);
		SocketManager.EventRoomThrowDice = new TcpSocketManager.EventSocket(EventRoomThrowDice);
	}

	void Update() {
		receiveMessage();
	}

	void EventLoginSuccess(JJSocket sock) {
		SocketLoginSuccess login = sock as SocketLoginSuccess;
		Debug.Log("TcpClient - JJSocketType.LoginSuccess " + login.name);
		Main.Instance.name = login.name;
		LoginScene.SetActive(false);
		LobbyScene.SetActive(true);
		_user.Name = Main.Instance.name;

		SocketRoomConnect room = new SocketRoomConnect();
		SocketManager.tcpSocket.SendMessage(room);
//		SocketRoomJoin join = new SocketRoomJoin();
//		join.index = 0;
//		SocketManager.tcpSocket.SendMessage(join);
	}

	void EventReceivedChat(JJSocket sock) {
		SocketChat chat = sock as SocketChat;
		Chat.Instance.ChatContainer2.text += chat.name +  " : " + chat.message + "\n";
		Debug.Log("EventReceivedChat - " + chat.name +  " : " + chat.message);
	}

	void EventRoomJoinSuccess(JJSocket sock) {
		SocketRoomJoin room = sock as SocketRoomJoin;
		Debug.Log("EventRoomJoinSuccess - " + room.room);
		Main.Instance.room = room.room;
		Main.Instance.mySlot = 1;
		RoomScene.gameObject.SetActive(true);
		Room3D.SetActive(true);
	}

	void EventRoomConnectSuccess(JJSocket sock) {
		SocketRoomConnectSuccess room = sock as SocketRoomConnectSuccess;
		Debug.Log("EventRoomConnectSuccess - " + room.room);
		Main.Instance.room = room.room;
		Main.Instance.mySlot = room.slot;
	}
	
	void EventRoomInfo(JJSocket sock) {
		SocketRoomInfo room = sock as SocketRoomInfo;
		Debug.Log("EventRoomInfo - " + room.room);
		Main.Instance.room = room.room;
		Main.Instance.player1 = room.player1;
		Main.Instance.player2 = room.player2;
		RoomScene.txtPlayer1.text = Main.Instance.player1;
		RoomScene.txtPlayer2.text = Main.Instance.player2;

		RoomScene.gameObject.SetActive(true);
		Room3D.SetActive(true);
	}

	void EventRoomPlay(JJSocket sock) {
	}

	void EventRoomPlayDice(JJSocket sock) {
		SocketRoomPlayDice room = sock as SocketRoomPlayDice;
		if(room.t == Main.Instance.mySlot) {
			Debug.Log("myTurn : ");
			RoomScene.SetMyTurn();
		} else {
			Debug.Log("myWait : ");
			RoomScene.SetWait();
		}
	}

	void EventRoomThrowDice(JJSocket sock) {
		SocketRoomThrowDice dice = sock as SocketRoomThrowDice;
		RoomScene.Move(dice);
	}

	public void Connect() {
		_user.Name = txtName.text;
		Main.Instance.id = _user.Name;
		if(String.IsNullOrEmpty(_user.Name)) {
			return;
		}

		SocketManager.Connect();
//		if(_socket.Connected) {
//			Debug.Log("connected");
//			LoginScene.SetActive(false);
//			LobbyScene.SetActive(true);
//			txtChatContainer.text += "1";
//		} else {
//			Debug.Log("fail to connect");
//		}
	}

	public void SendChat() {
		_user.Message = txtMessage.text;
		sockChat.name = Main.Instance.name;
		sockChat.message = txtMessage.text;
		txtMessage.text = "";
		if(String.IsNullOrEmpty(sockChat.message)) {
			Debug.Log("TcpClient - SendMessage is null");
			return;
		}

//		string jsonUserString = JsonMapper.ToJson(_user);
//		Debug.Log("Send : " + jsonUserString);
		SocketManager.tcpSocket.SendMessage(sockChat);
	}

	void SendComplete(IAsyncResult ar) {
		try {
			if(SocketManager == null) {
				return;
			}
			int len = 0;
//			int len = _socket.EndSend(ar);
			if(len == 1) {
				Debug.Log("Send success");
			}
		} catch(Exception e) {
			Debug.LogError("Send Exception : " + e.Message);
			Shutdown();
		}
	}

	void receiveMessage() {
//		try {
//			if(_socket == null) {
//				return;
//			}
//			_socket.BeginReceive(_recvBuffer, 0, _recvBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveComplete), null);
//		} catch(Exception e) {
//			Debug.LogError("BeginException : " + e.Message);
//			Shutdown();
//		}
	}

	void ReceiveComplete(IAsyncResult ar) {
		try {
			if(SocketManager == null) {
				Debug.Log("socket is null");
				return;
			}

			int len = 0;
//			int len = _socket.EndReceive(ar);
			if(len == 0) {
				Shutdown();
			} else {
				byte[] cuttingBuffer = new byte[len];
//				for(int i = 0; i < len; i++) {
//					cuttingBuffer[i] = _recvBuffer[i];
//				}
				System.Buffer.BlockCopy(_recvBuffer, 0, cuttingBuffer, 0, len);

				string jsonString = System.Text.Encoding.UTF8.GetString(cuttingBuffer);
				JsonData json = JsonMapper.ToObject(jsonString);
				Debug.Log("Receive : " + json.ToString());
				Debug.Log("name : " + json["Name"].ToString() + " message : " + json["Message"].ToString());
//				txtChatContainer.text += "\n" + json["Name"].ToString() + " : " + json["Message"].ToString();
//				txtChatContainer.text += json["Name"].ToString();"\n" + json["Name"].ToString() + " : " + json["Message"].ToString()
				string str = "\n" + json["Name"].ToString() + " : " + json["Message"].ToString();
//				StartCoroutine(WaitAndChat(str));
				Debug.Log(1);
				Chat.Instance.ChatContainer2.text += "2";
				Debug.Log(2);
//				Chat.Instance.ChatContainer1.text += "1";
				Debug.Log(3);
				System.Array.Clear(_recvBuffer, 0, _recvBuffer.Length);
			}
		} catch(Exception e) {
			Debug.LogError("Receive Exception : " + e.Message);
			Shutdown();
		}
	}

	IEnumerator WaitAndChat(string str)
	{
		yield return new WaitForSeconds(3.0f);
		txtChatContainer.text += str;
	}
	
	void Shutdown() {
		if(SocketManager != null) {
			try {
				Debug.Log("shutdown");
//				_socket.Shutdown(SocketShutdown.Both);
				SocketManager = null;
				LoginScene.SetActive(true);
				LobbyScene.SetActive(false);
			} catch(Exception e) {
				Debug.LogError("Sutdown Exception : " + e.Message);
				SocketManager = null;
			}
		}
	}
}