using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Text;
using UnityEngine.SceneManagement;

public class Client2 : NetworkManager {
	public static Client2 Instance { set; get; }
	private const int MAX_PLAYERS = 5;
	private int port = 6321;
	private int HostId;
	private int ConnectionId;

	public int ReliableChannel;
	public int UnReliableChannel;
	private int ourClientId;

	private bool isConnected = false;
	private bool isStarted = false;
	private bool loaded = false;
	private bool isFirst = true;

	private float ConnectionTime;

	private byte error;

	private string playerName;

	//private string IP;

	public Material mat1, mat2;

	public void Connect(){

//		string pName = GameObject.Find("InputField").GetComponent<InputField>().text;
//		if (pName == "") {
//			pName = "Potato";
//		}
//		string ip = GameObject.Find ("IP").GetComponent<InputField> ().text;
//		if (ip == "") {
//			Debug.Log ("Enter valid IP");
//			return;
//		}
		string ip = "192.168.43.1";

		NetworkTransport.Init ();
		ConnectionConfig cc = new ConnectionConfig ();

		ReliableChannel = cc.AddChannel (QosType.Reliable);
		UnReliableChannel = cc.AddChannel (QosType.Unreliable);

		HostTopology topology = new HostTopology (cc, MAX_PLAYERS);
		//NetworkServer.Reset ();
		HostId = NetworkTransport.AddHost (topology, 0, null);		//127.0.0.1
																						//192.168.43.1
		ConnectionId = NetworkTransport.Connect (HostId, ip, port, 0, out error);	//127.0.0.1
		ConnectionTime = Time.time;
		isConnected = true; 
	}
	private void Update(){
		if (!isConnected)
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
		case NetworkEventType.DataEvent:

			isConnected = true;


			string msg = Encoding.Unicode.GetString (recBuffer, 0, dataSize);
			Debug.Log ("Receiving: " + msg);
			string[] splitData = msg.Split ('|');

			switch(splitData[0]){
			case "AKNAME":
				OnAskName (splitData);
				break;
			case "CNN":
				Debug.Log (msg);
				break;
			case "DC":
				break;
			case "MOV":
				Transform t = GameObject.Find (splitData [1]).GetComponent<Transform> ();
				Debug.Log (t.name);
				float x = float.Parse (splitData [2]);
				float z = float.Parse (splitData [3]);
				t.position = new Vector3 (x, 0.5f, z);
				dummyAndroid.white_turn = false;
				dummyAndroid.black_turn = true;
				break;
			case "CUT":
				Transform tc = GameObject.Find (splitData [1]).GetComponent<Transform> ();
				Debug.Log (tc.name);
				float xc = float.Parse (splitData [2]);
				float zc = float.Parse (splitData [3]);
				tc.position = new Vector3 (xc, 0.5f, zc);
				GameObject destroyed_obj = GameObject.Find (splitData [4]);
				Destroy (destroyed_obj);
				dummyAndroid.white_turn = false;
				dummyAndroid.black_turn = true;
				break;
			case "PRO":
				GameObject promoted = GameObject.Find (splitData [1]);
				//Transform tp = GameObject.Find (splitData [1]).GetComponent<Transform> ();
				float xp = float.Parse (splitData [2]);
				float zp = float.Parse (splitData [3]);
				promoted.transform.position = new Vector3 (xp, 0.5f, zp);
				Renderer rend = promoted.GetComponent<Renderer> ();
				Material mat = promoted.GetComponent<Material> ();
				//rend.material = mat;
				promoted.transform.tag = splitData [5];
				if (promoted.transform.tag == "white_queen")
					rend.material = mat1;
				else if (promoted.transform.tag == "black_queen")
					rend.material = mat2;
				//Transform tp = GameObject.Find (splitData [6]).GetComponent<Transform>();
				//promoted.transform.parent = tp;
				dummyAndroid.white_turn = false;
				dummyAndroid.black_turn = true;
				break;
			case "OVR":
				dummyAndroid.Instance.GameOver (splitData[1]);
				break;
			case "QUI":
				dummyAndroid.Instance.OnUserQuit ("Black");
				break;
			default:
				Debug.Log("Invalid message:" +msg);
				break;
			}

			break;
		}

		if (isConnected && !loaded) {
			loaded = true;

			SceneManager.LoadScene ("Scene2");
				
			dummyAndroid.isBlack = true;
			dummyAndroid.isWhite = false;
			dummyAndroid.white_turn = false;
			dummyAndroid.black_turn = false;

			Debug.Log ("Connection established with " + connectionId);
			Debug.Log ("Client: "+NetworkManager.singleton.networkAddress);
		}

	}
	private void OnAskName(string[] data){
		ourClientId = int.Parse(data [1]);

		Send ("NAMEIS|" + playerName, ReliableChannel);

		for (int i = 2; i < data.Length - 1; i++) {
			string[] d = data [i].Split ('%');
		}
	}
	public void Send(string message, int channelId){
		Debug.Log ("Sending from:" + message);
		byte[] msg = Encoding.Unicode.GetBytes (message);
		NetworkTransport.Send (HostId, ConnectionId, channelId, msg, message.Length*sizeof(char), out error);
	}
}
