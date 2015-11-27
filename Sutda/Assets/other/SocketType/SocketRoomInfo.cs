public class SocketRoomInfo : JJSocket {
	public int room = 0;
	public string player0 = "";
	public string player1 = "";
	public string player2 = "";
	public string player3 = "";
	public string player4 = "";

	public SocketRoomInfo() {
		type = JJSocketType.RoomInfo;
	}
}