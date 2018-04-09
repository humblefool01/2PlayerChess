using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
	public static MainMenu Instance { set; get; }
	//public Button server, client;

	public void Server(){
		SceneManager.LoadScene ("Server");
	}
	public void Client(){
		SceneManager.LoadScene ("Client");
	}
}
