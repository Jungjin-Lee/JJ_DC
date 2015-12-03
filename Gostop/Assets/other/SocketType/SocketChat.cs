public class SocketChat : JJSocket {
	public string name;
	public string message;

	public SocketChat() {
		type = JJSocketType.SendChat;
	}
}