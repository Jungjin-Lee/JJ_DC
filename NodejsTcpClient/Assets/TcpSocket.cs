using UnityEngine;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System;
using LitJson;

public class TcpSocket {

	public string IP = "127.0.0.1";
	public int PORT = 8000;

	List<JJSocket> SockList = new List<JJSocket>();
	List<string> sockStringList = new List<string>();

	Socket _socket = null;
	User _user = new User();

	byte[] _recvBuffer = new byte[1024];

	public void Connect() {
//		if(String.IsNullOrEmpty(_user.Name)) {
//			return;
//		}
		_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		_socket.Connect(IP, PORT);
		if(_socket.Connected) {
			Debug.Log("connected");
		} else {
			Debug.Log("fail to connect");
		}
	}

	public void SendMessage(string str) {
//		_user.Message = txtMessage.text;
//		if(String.IsNullOrEmpty(_user.Message)) {
//			return;
//		}
//
//		string jsonUserString = JsonMapper.ToJson(_user);
//		Debug.Log("Send : " + jsonUserString);

		try {
			byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);
			_socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, new AsyncCallback(SendComplete), null);
//			_user.Message = "";
//			txtMessage.text = "";
		} catch(Exception e) {
//			Debug.LogError("send fail exception : " + e.Message);
			Shutdown();
		}
	}
	
	public void SendMessage(JJSocket sock) {
		try {
			string json = JsonMapper.ToJson(sock);
			byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
			_socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, new AsyncCallback(SendComplete), null);
		} catch(Exception e) {
			Debug.LogError("send fail exception : " + e.Message);
			DebugWindow.Log("send fail exception : " + e.Message);
			Shutdown();
		}
	}

	void SendComplete(IAsyncResult ar) {
		try {
			if(_socket == null) {
				return;
			}
			int len = _socket.EndSend(ar);
			if(len == 1) {
//				Debug.Log("Send success");
			}
		} catch(Exception e) {
			Debug.LogError("Send Exception : " + e.Message);
			DebugWindow.Log("Send Exception : " + e.Message);
			Shutdown();
		}
	}

	public void ReceiveMessage() {
		try {
			if(_socket == null) {
				return;
			}
			_socket.BeginReceive(_recvBuffer, 0, _recvBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveComplete), null);
		} catch(Exception e) {
			Debug.LogError("BeginException : " + e.Message);
			DebugWindow.Log("BeginException : " + e.Message);
			Shutdown();
		}
	}

	void ReceiveComplete(IAsyncResult ar) {
		try {
			if(_socket == null) {
//				Debug.Log("socket is null");
				return;
			}

			int len = _socket.EndReceive(ar);
			if(len == 0) {
				Shutdown();
			} else {
				byte[] cuttingBuffer = new byte[len];
//				for(int i = 0; i < len; i++) {
//					cuttingBuffer[i] = _recvBuffer[i];
//				}
				System.Buffer.BlockCopy(_recvBuffer, 0, cuttingBuffer, 0, len);

				string jsonString = System.Text.Encoding.UTF8.GetString(cuttingBuffer);
				Debug.Log("recv : " + jsonString);
				sockStringList.Add("recv : " + jsonString);
				JsonData json = JsonMapper.ToObject(jsonString);


				Debug.Log("type : " + json["type"]);
				int type = int.Parse(json["type"].ToString());
				AddQueue(type, jsonString);
				System.Array.Clear(_recvBuffer, 0, _recvBuffer.Length);
			}
		} catch(Exception e) {
			Debug.LogError("Receive Exception : " + e.Message);
			DebugWindow.Log("Receive Exception : " + e.Message);
			Shutdown();
		}
	}

	void AddQueue(int type, string json) {
		JJSocket sock = null;

		switch(type) {
		case JJSocketType.Connected:
			sock = new JJSocket();
			break;
		case JJSocketType.LoginSuccess:
			sock = JsonMapper.ToObject<SocketLoginSuccess>(json);
			break;
		case JJSocketType.ReceivedChat:
			sock = JsonMapper.ToObject<SocketChat>(json);
			break;
		case JJSocketType.RoomJoinSuccess:
			sock = JsonMapper.ToObject<SocketRoomJoin>(json);
			break;
		case JJSocketType.RoomConnectSuccess:
			sock = JsonMapper.ToObject<SocketRoomConnectSuccess>(json);
			break;
		case JJSocketType.RoomInfo:
			sock = JsonMapper.ToObject<SocketRoomInfo>(json);
			break;
		default:
			Debug.LogError("TcpSocket - AddQueue - Set Type Please");
			DebugWindow.Log("TcpSocket - AddQueue - Set Type Please");
			break;
		}
		sock.type = type;
		SockList.Add(sock);
	}
	
	void Shutdown() {
		if(_socket != null) {
			try {
				Debug.Log("shutdown");
				DebugWindow.Log("shutdown");
				_socket.Shutdown(SocketShutdown.Both);
				_socket = null;
//				LoginScene.SetActive(true);
//				LobbyScene.SetActive(false);
			} catch(Exception e) {
				Debug.LogError("Sutdown Exception : " + e.Message);
				DebugWindow.Log("Sutdown Exception : " + e.Message);
				_socket = null;
			}
		}
	}

	public JJSocket Pop() {
		JJSocket sock = null;
		if(SockList.Count > 0) {
			sock = SockList[0];
			SockList.RemoveAt(0);
		}
		return sock;
	}

	public string PopSockString() {
		string str = null;
		if(sockStringList.Count > 0) {
			str = sockStringList[0];
			sockStringList.RemoveAt(0);
		}
		return str;
	}
}