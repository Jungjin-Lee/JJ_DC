using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

	public WaitingRoom WaitingRoom;
	public PlayingRoom PlayingRoom;
	public TcpSocketManager TcpSocketManager;

	public void OnClickJoinRoom() {
        TcpSocketManager.Instance.EventConnected = new TcpSocketManager.EventSocket(EventConnected);
        TcpSocketManager.Instance.EventConnectFailed = new TcpSocketManager.EventSocket(EventConnectFailed);
        TcpSocketManager.Connect("127.0.0.1", 8001); 
    }

    void EventConnected(JJSocket sock)
    {
        WaitingRoom.gameObject.SetActive(false);
        PlayingRoom.gameObject.SetActive(true);
        WaitingRoom.EventConnected(sock);
    }

     void EventConnectFailed(JJSocket sock)
    {
        Debug.Log("EventConnectFailed");
    }
}
