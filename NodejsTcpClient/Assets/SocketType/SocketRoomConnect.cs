public class SocketRoomConnect : JJSocket {
	public int index;

	public SocketRoomConnect() {
		type = JJSocketType.RoomJoin;
	}
}