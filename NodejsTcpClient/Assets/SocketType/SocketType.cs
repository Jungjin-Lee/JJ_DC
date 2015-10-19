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
}

public class JJSocket {
	public int type;
}