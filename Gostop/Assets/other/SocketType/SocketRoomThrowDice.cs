public class SocketRoomThrowDice : JJSocket {
	public int t;	// turn
	public int d1;	// dice1
	public int d2;	// dice2

	public SocketRoomThrowDice() {
		type = JJSocketType.RoomThrowDice;
	}
}