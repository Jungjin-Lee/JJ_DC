public class SocketRoomInfo : JJSocket {
	public int room = 0;
	public string player1 = "";
	public string player2 = "";

	public SocketRoomInfo() {
		type = JJSocketType.RoomInfo;
	}
}