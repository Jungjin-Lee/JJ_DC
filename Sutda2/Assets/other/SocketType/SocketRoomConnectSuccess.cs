public class SocketRoomConnectSuccess : JJSocket {
	public int room = -1;
	public int slot = -1;

	public SocketRoomConnectSuccess() {
		type = JJSocketType.RoomConnectSuccess;
	}
}