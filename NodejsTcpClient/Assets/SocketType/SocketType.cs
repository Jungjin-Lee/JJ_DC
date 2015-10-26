//
//public enum ESocketType {
//	Connected,
//	Chat
//}

public abstract class JJSocketType {
	
	public const int Connect = 1;
	public const int Connected = 2;
	public const int Login = 5;
	public const int LoginSuccess = 6;
	public const int SendChat = 10;
	public const int ReceivedChat = 11;

	public const int RoomJoin = 20;
	public const int RoomJoinSuccess = 21;
	public const int RoomJoinFailed = 22;

	public const int RoomConnect = 30;
	public const int RoomConnectSuccess = 31;
	public const int RoomConnectFailed = 32;

	public const int RoomInfo = 40;

	public const int RoomPlay = 50;
	public const int RoomPlayDice = 51;
	public const int RoomThrowDice = 52;
	public const int RoomPlayNext = 53;
}

public class JJSocket {
	public int type;
}