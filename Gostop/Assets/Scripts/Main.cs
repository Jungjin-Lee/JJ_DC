using UnityEngine;
using System.Collections;

public enum GameType {
	None,
	Game_2Card,
	Game_3Card,
}

public class Main : MonoBehaviour {
	public WaitingRoom WaitingRoom;
	public PlayingRoom PlayingRoom;
	public TcpSocketManager TcpSocketManager;
	public Dealer Delaer;
	public GameType GameType = GameType.None;

	public void OnClickJoinRoom() {
		TcpSocketManager.Connect("127.0.0.1", 8001);
		WaitingRoom.gameObject.SetActive(false);
		PlayingRoom.gameObject.SetActive(true);
	}

	public void OnClick2Card() {
		GameType = GameType.Game_2Card;
		Delaer.perMoney = 1000;
		WaitingRoom.gameObject.SetActive(false);
		PlayingRoom.gameObject.SetActive(true);
	}

	public void OnClick3Card() {
		GameType = GameType.Game_3Card;
		Delaer.perMoney = 1000;
		WaitingRoom.gameObject.SetActive(false);
		PlayingRoom.gameObject.SetActive(true);
	}
}