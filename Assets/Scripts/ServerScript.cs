using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using UnityEngine.SceneManagement;

public class ServerClient{
	public int connectionId;
	public string playerName;
}
public class Client{
	public int clientConnectionId;
	public string clientPlayerName;
}
public class ServerScript : NetworkManager{
	public static ServerScript Instance { set; get; }
	private const int MAX_PLAYERS = 5;
	private int port = 6321;
	private int HostId, ConnectionId, x;

	public int ReliableChannel;
	public int UnReliableChannel;

	private bool isStarted = false, isConnected = false, loaded = false;

	private byte error;

	private List<ServerClient> clients = new List<ServerClient>();

	public Material mat1, mat2;

	private void Start(){
		NetworkTransport.Init ();
		ConnectionConfig cc = new ConnectionConfig ();

		ReliableChannel = cc.AddChannel (QosType.Reliable);
		UnReliableChannel = cc.AddChannel (QosType.Unreliable);

		HostTopology topology = new HostTopology (cc, MAX_PLAYERS);
		//NetworkServer.Reset ();
		HostId = NetworkTransport.AddHost (topology, port, null);
		//Debug.Log (HostId);
		//ConnectionId = NetworkTransport.Connect (HostId, "127.0.0.1", port, 0, out error);
		isStarted = true;
	}

	private void Update(){
		if (!isStarted)
			return;
		int recHostId;
		int connectionId;
		int channelId;
		byte[] recBuffer = new byte[1024];
		int bufferSize = 1024;
		int dataSize;
		byte error;
		NetworkEventType recData = NetworkTransport.Receive (out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);

		switch (recData) {
		case NetworkEventType.Nothing:
			Debug.Log (recData);
			break;
		case NetworkEventType.ConnectEvent:
			
			Debug.Log ("Player " + connectionId + " has connected");
			isConnected = true;
			Connection (connectionId);
			break;
		case NetworkEventType.DataEvent:
			string msg = Encoding.Unicode.GetString (recBuffer, 0, dataSize);
			Debug.Log ("Receiving from:" + connectionId + " : " + msg);

			string[] splitData = msg.Split ('|');

//			if (connectionId == 1)
//				Send (msg, ReliableChannel, 2);
//			else if (connectionId == 2)
//				Send (msg, ReliableChannel, 1);

			switch(splitData[0]){
			case "NAMEIS":
				OnNameIs (connectionId, splitData [1]);
				break;
			case "MOV":
				Transform tm = GameObject.Find (splitData [1]).GetComponent<Transform> ();

				float xm = float.Parse (splitData [2]);
				float zm = float.Parse (splitData [3]);
				tm.position = new Vector3 (xm, 0.5f, zm);
				dummyAndroid.white_turn = true;
				dummyAndroid.black_turn = false;
				break;
			case "CUT":
				Transform tc = GameObject.Find (splitData [1]).GetComponent<Transform> ();
				Debug.Log (tc.name);
				float xc = float.Parse (splitData [2]);
				float zc = float.Parse (splitData [3]);
				tc.position = new Vector3 (xc, 0.5f, zc);
				GameObject destroyed_obj = GameObject.Find (splitData [4]);
				Destroy (destroyed_obj);
				dummyAndroid.white_turn = true;
				dummyAndroid.black_turn = false;
				break;
			case "PRO":
				GameObject promoted = GameObject.Find (splitData [1]);
				//Transform tp = GameObject.Find (splitData [1]).GetComponent<Transform> ();
				float xp = float.Parse (splitData [2]);
				float zp = float.Parse (splitData [3]);
				promoted.transform.position = new Vector3 (xp, 0.5f, zp);
				Renderer rend = promoted.GetComponent<Renderer> ();
				Material mat = promoted.GetComponent<Material> ();
				promoted.transform.tag = splitData [5];
				if (promoted.transform.tag == "white_queen")
					rend.material = mat1;
				else if (promoted.transform.tag == "black_queen")
					rend.material = mat2;
				dummyAndroid.white_turn = true;
				dummyAndroid.black_turn = false;

				//Transform tp = GameObject.Find (splitData [6]).GetComponent<Transform> ();
				//promoted.transform.parent = tp;
				break;
			case "OVR":
				dummyAndroid.Instance.GameOver (splitData[1]);
				break;
			case "QUI":
				dummyAndroid.Instance.OnUserQuit ("White");
				break;
			default:
				//Debug.Log("Invalid message:" +msg);
				break;
			}

			break;
		case NetworkEventType.DisconnectEvent:
			Debug.Log ("Player " + connectionId + " has disconnected");
			break;
		}

		if (isConnected && !loaded) {
			//Debug.Log (isStarted);
			loaded = true;

			SceneManager.LoadScene ("Scene2");
			dummyAndroid.isWhite = true;
			dummyAndroid.isBlack = false;
			dummyAndroid.white_turn = true;
			dummyAndroid.black_turn = false;
			Debug.Log ("Connection established with " + connectionId);
			Debug.Log ("Server: "+NetworkManager.singleton.networkAddress);
		}

	}
	public void Connection(int connId){
		ServerClient c = new ServerClient ();
		c.connectionId = connId;
		c.playerName = "TEMP";
		clients.Add (c);
		string msg = "ASKNAME|" + c.connectionId + "|";
		foreach (ServerClient sc in clients) {
			msg += sc.playerName + "%" + sc.connectionId + "|";
		}
		msg = msg.Trim ('|');
		Send (msg, ReliableChannel, connId);
	}
	public void Send(string message, int channelId, int connId){		//Function called through dummyAndroid script
		List<ServerClient> c = new List<ServerClient> ();
		c.Add (clients.Find(x => x.connectionId == connId));
		Send (message, channelId, c);
	}
	public void Send(string message, int channelId, List<ServerClient> c){
//		Debug.Log (c.Count);
		Debug.Log ("Sending " + message);
		byte[] msg = Encoding.Unicode.GetBytes (message);
		foreach (ServerClient sc in c) {
			NetworkTransport.Send (HostId, 1, channelId, msg, message.Length*sizeof(char), out error);	// sc.connectionId
		}
	}
	private void OnNameIs(int connId, string playerName){
		clients.Find (x => x.connectionId == connId).playerName = playerName;
		Send ("CNN|" + playerName + "|" + connId, ReliableChannel, clients);
	}
}