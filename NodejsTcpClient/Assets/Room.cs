using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum RoomState {
	Default,
	Player1_Dice,
	Player1_Move,
	Player2_Dice,
	Player2_Move,
}

public class Player {
	int p;
}

public class Room : MonoBehaviour {

	// x start:30 unit:3.5
	// p1 start:29.5
	// p2 start:30.5

	int playerCnt = 2;
	int nowFocusPlayer = 0;

	RoomState State = RoomState.Default;

	public GameObject player1;
	public GameObject player2;
	public Text CenterText;

	public int myIndex = 0;
}