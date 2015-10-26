using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum RoomSceneState {
	Default,
	Play,
	MyTurn,
	Move,
	Wait,
	ThrowWait,
}
public enum RoomTurn {
	Player1 = 1,
	Player2,
	Player3,
	Player4,
}

public class RoomScene : MonoBehaviour {

	public TcpClient Tcp;
	public Text txtPlayer1;
	public Text txtPlayer2;
	public Text txtMain;

	public GameObject Player1;
	public GameObject Player2;

	public int Player1Position = 0;
	public int Player1PositionNext = 0;
	public int Player2Position = 0;
	public int Player2PositionNext = 0;

	public RoomSceneState State = RoomSceneState.Default;
	public RoomTurn Turn;

	void Awake() {
//		Invoke("Move1", 1f);
//		Timers t = new Timer(); 
//		t.Elapsed += new ElapsedEventHandler(onTimer); 
//		t.Interval = 500; 
//		t.Start(); 
	}

	public void SetMyTurn() {
		State = RoomSceneState.MyTurn;
		txtMain.text = "Please, Input Space Bar...";
	}

	public void SetWait() {
		State = RoomSceneState.Wait;
		txtMain.text = "Please, Wait...";
	}

	void Update() {
		if(State == RoomSceneState.MyTurn) {
			if(Input.GetKeyDown(KeyCode.Space)) {
				Debug.Log("spacebar down");
				txtMain.text = "Dice...";
				State = RoomSceneState.ThrowWait;
				JJSocket sock = new JJSocket();
				sock.type = JJSocketType.RoomThrowDice;
				Tcp.SocketManager.tcpSocket.SendMessage(sock);
			}
		}
	}

	public void Move(SocketRoomThrowDice dice) {
		Debug.Log("t:" + dice.t + " d1:" + dice.d1 + " d2:" + dice.d2);
		if(dice.t == 1) {
			Turn = RoomTurn.Player1;
			Player1PositionNext = Player1Position + dice.d1 + dice.d2;
		} else if(dice.t == 2) {
			Turn = RoomTurn.Player2;
			Player2PositionNext = Player2Position + dice.d1 + dice.d2;
		}
		State = RoomSceneState.Move;
		InvokeRepeating("Move1", 0.5f, 0.35f);
	}

	void Move1() {
		int n = 0, next = 0;
		Transform ts = null;
		if(Turn == RoomTurn.Player1) {
			n = ++Player1Position;
			next = Player1PositionNext;
			ts = Player1.transform;
		} else if(Turn == RoomTurn.Player2) {
			n = ++Player2Position;
			next = Player2PositionNext;
			ts = Player2.transform;
		}
		SetPosition(n % 32, ts);

		if(n == next) {
			Debug.Log("MoveEnd");
			CancelInvoke("Move1");

			if((int)Turn == Main.Instance.mySlot) {
				JJSocket sock = new JJSocket();
				sock.type = JJSocketType.RoomPlayNext;
				Tcp.SocketManager.tcpSocket.SendMessage(sock);
			}
		}
	}

	void SetPosition(int n, Transform ts) {
		// x = 3.5, 3
		// 0 30, 0
		// 26.5
		// 5
		// 0, 8, 16, 24, 32=0

		float defaultX = 5f;
		float max = 25f;
		float small = 3f;
		float big = 3.5f;
		int lineCnt = 8;

		float x = 0, z = 0;
		if(n == 0) {
			x = max + defaultX;
			z = 0;
		} else if(n < lineCnt) {
			x = max + defaultX - small * (n - 1) - big;
		} else if(n <= lineCnt * 2) {
			x = defaultX;
		} else if(n < lineCnt * 3) {
			x = defaultX + small * ((n % lineCnt) - 1) + big;
		} else {
			x = max + defaultX;
		}

		if(n <= lineCnt) {
			z = 0;
		} else if(n < lineCnt * 2) {
			z = small * ((n % lineCnt) - 1) + big;
		} else if(n <= lineCnt * 3) {
			z = max;
		} else {
			z = max - small * ((n % lineCnt) - 1) - big;
		}

		Vector3 p = ts.localPosition;
		p.x = x;
		p.z = z;
		ts.localPosition = p;
	}
}