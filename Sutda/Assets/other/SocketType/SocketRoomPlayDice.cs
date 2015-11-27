public class SocketRoomPlayDice : JJSocket {
	public int t;	// turn

	public SocketRoomPlayDice() {
		type = JJSocketType.RoomPlayDice;
	}
}