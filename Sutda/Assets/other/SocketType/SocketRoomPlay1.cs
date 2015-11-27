public class SocketRoomGameResult : JJSocket {

	public string[] playersCard;

	public SocketRoomGameResult() {
		type = JJSocketType.RoomGameResult;
	}
}