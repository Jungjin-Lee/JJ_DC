using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

	public WaitingRoom WaitingRoom;
	public PlayingRoom PlayingRoom;
	public TcpSocketManager TcpSocketManager;

	public void OnClickJoinRoom() {
		TcpSocketManager.Connect("127.0.0.1", 8001);
		WaitingRoom.gameObject.SetActive(false);
		PlayingRoom.gameObject.SetActive(true);
	}
}
