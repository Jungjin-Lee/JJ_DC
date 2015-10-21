public class SocketRoomJoin : JJSocket {
	public int room;

	public SocketRoomJoin() {
		type = JJSocketType.RoomJoin;
	}
}