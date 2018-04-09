using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

	public void TwoPlayer(){
		SceneManager.LoadScene ("Scene3", LoadSceneMode.Single);
	}
	public void MultiPlayer(){
		SceneManager.LoadScene ("MainMenu", LoadSceneMode.Single);
	}
}
