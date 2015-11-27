public class SocketRoomConnect : JJSocket {
	public int room;

	public SocketRoomConnect() {
		type = JJSocketType.RoomConnect;
	}
}