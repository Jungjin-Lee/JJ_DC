public class SocketLogin : JJSocket {
	public string id;
	public string pwd;

	public SocketLogin() {
		type = JJSocketType.Login;
	}
}