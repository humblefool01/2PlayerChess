using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class dummyAndroid : MonoBehaviour {

	public static dummyAndroid Instance { set; get; }
	private int d = 0;
	private GameObject obj, obj2, temp;
	private Material material;
	private Renderer rend, rend2, rend3;
	private string s;
	private char[] c;
	private int i, j, k, chance;
	private bool two_step_possible, selected, restart, white_right_rook_moved,white_left_rook_moved, white_king_moved,black_right_rook_moved, black_left_rook_moved, black_king_moved, promotion, gameOver;
	GameObject front1, front2, front_left, front_right, destination, board_copy, selected_obj;
	public Material black, white;
	public GameObject Board, white_left_rook, white_right_rook, black_left_rook, black_right_rook, white_pieces, black_pieces, white_queen, black_queen;
	private GameObject[] board_state, array, knight_positions, king_positions;
	public GameObject[] final_board;
	private Color[] board_color;
	private GameObject destroyed_object;
	public Text t;
	public Material[] mats;
	public static bool isWhite, isBlack, white_turn, black_turn;
	public GameObject button;
	public Client2 client;
	public ServerScript server;

	void Start(){

		Instance = this;
		Client2 client = FindObjectOfType<Client2> ();
		ServerScript server = FindObjectOfType<ServerScript> ();
		//isWhite = client.isHost;

		rend = GetComponent<Renderer>();
		i = 0;
		j = 0;
		k = 0;
		promotion = false;
		restart = false;
		//		white_turn = true;
		//		black_turn = false;
		selected = false;
		board_copy = Board;
		board_state = new GameObject[64];
		array = new GameObject[64];
		knight_positions = new GameObject[8];
		king_positions = new GameObject[11];
		board_color = new Color[64];
		destroyed_object = new GameObject ();
		white_right_rook_moved = false;
		white_left_rook_moved = false;
		white_king_moved = false;
		black_right_rook_moved = false;
		black_left_rook_moved = false;
		black_king_moved = false;
		button.SetActive (false);
		gameOver = false;
		//		isWhite = false;
		//		isBlack = false;
	}

	public void Restart() {
		SceneManager.LoadScene ("Scene2", LoadSceneMode.Single);

	}

	void Update(){

		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
			Debug.Log (isWhite+" "+isBlack);
			obj = MouseRaycast (Input.GetTouch(0).position);
			s = obj.transform.parent.name;
			if (selected) {
				Dehilighter ();
			}
			//********WHITE'S TURN********//

			if ((white_turn && isWhite)) {

				if (obj.transform.parent.name != "Board" && obj.transform.parent.parent.name != "black") {
					selected_obj = obj;
					//Debug.Log (selected_obj.name);

					selected = true;
				}

				switch (obj.transform.tag) {

				case "white_pawns":
					front1 = RayCast (new Vector3 (obj.transform.position.x + 1, obj.transform.position.y + 7.0f, obj.transform.position.z));
					if (front1.transform.parent.name == "Board") {
						rend = front1.GetComponent<Renderer> ();
						board_state [i] = front1;
						board_color [i] = rend.material.color;
						i++;
						rend.material.color = Color.cyan;

						if (obj.transform.position.x == -2) {
							front2 = RayCast (new Vector3 (obj.transform.position.x + 2, obj.transform.position.y + 5.0f, obj.transform.position.z));
							if (front2.transform.parent.name == "Board") {
								rend = front2.GetComponent<Renderer> ();
								board_state [i] = front2;
								board_color [i] = rend.material.color;
								i++;
								rend.material.color = Color.cyan;

							}
						}
					}
					front_left = RayCast (new Vector3 (obj.transform.position.x + 1, obj.transform.position.y + 5.0f, obj.transform.position.z + 1));
					if (front_left != null && front_left.transform.parent.name != "Board" && (front_left.transform.parent.parent.name == "black")) {
						front_left = RayCast (new Vector3 (front_left.transform.position.x, front_left.transform.position.y, front_left.transform.position.z));
						rend = front_left.GetComponent<Renderer> ();
						board_state [i] = front_left;
						board_color [i] = rend.material.color;
						i++;
						rend.material.color = Color.cyan;

					}
					front_right = RayCast (new Vector3 (obj.transform.position.x + 1, obj.transform.position.y + 5.0f, obj.transform.position.z - 1));
					if (front_right != null && front_right.transform.parent.name != "Board" && (front_right.transform.parent.parent.name == "black")) {
						front_right = RayCast (new Vector3 (front_right.transform.position.x, front_right.transform.position.y, front_right.transform.position.z));
						rend = front_right.GetComponent<Renderer> ();
						board_state [i] = front_right;
						board_color [i] = rend.material.color;
						i++;
						rend.material.color = Color.cyan;

					}

					break;

				case "white_bishop":
					/*1*/		

					for (k = 0; k < 64; k++) {
						if (array [k] != null) {
							array [k] = null;
						}
					}

					float bx, bz;
					k = 0;
					bx = obj.transform.position.x;
					bz = obj.transform.position.z;
					array [k] = RayCast (new Vector3 (bx + 1, 7.0f, bz + 1));
					bx++;
					bz++;
					while (array [k] != null && array [k].transform.parent.tag == "Board") {
						rend = array [k].GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
						bx++;
						bz++;
						array [k] = RayCast (new Vector3 (bx, 7.0f, bz));
					}
					if (array [k] == null) {
					}

					else if (array [k].transform.parent.parent.name == "black") {
						temp = array [k];
						temp = RayCast (new Vector3 (array [k].transform.position.x, array [k].transform.position.y, array [k].transform.position.z));
						rend = temp.GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
					} else {
					}

					/*2*/
					bx = obj.transform.position.x;
					bz = obj.transform.position.z;
					array [k] = RayCast (new Vector3 (bx + 1, 7.0f, bz - 1));
					bx++;
					bz--;
					while (array [k] != null && array [k].transform.parent.tag == "Board") {
						rend = array [k].GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
						bx++;
						bz--;
						array [k] = RayCast (new Vector3 (bx, 7.0f, bz));
					}
					if (array [k] == null) {
					}

					else if (array [k].transform.parent.parent.name == "black") {
						temp = array [k];
						temp = RayCast (new Vector3 (array [k].transform.position.x, array [k].transform.position.y, array [k].transform.position.z));
						rend = temp.GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
					} else {
					}
					/*3*/
					bx = obj.transform.position.x;
					bz = obj.transform.position.z;
					array [k] = RayCast (new Vector3 (bx - 1, 7.0f, bz + 1));
					bx--;
					bz++;
					while (array [k] != null && array [k].transform.parent.tag == "Board") {
						rend = array [k].GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
						bx--;
						bz++;
						array [k] = RayCast (new Vector3 (bx, 7.0f, bz));
					}
					if (array [k] == null) {
					}

					else if (array [k].transform.parent.parent.name == "black") {
						temp = array [k];
						temp = RayCast (new Vector3 (array [k].transform.position.x, array [k].transform.position.y, array [k].transform.position.z));
						rend = temp.GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
					} else {
					}
					/*4*/
					bx = obj.transform.position.x;
					bz = obj.transform.position.z;
					array [k] = RayCast (new Vector3 (bx - 1, 7.0f, bz - 1));
					bx--;
					bz--;
					while (array [k] != null && array [k].transform.parent.tag == "Board") {
						rend = array [k].GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
						bx--;
						bz--;
						array [k] = RayCast (new Vector3 (bx, 7.0f, bz));
					}
					if (array [k] == null) {
					}

					else if (array [k].transform.parent.parent.name == "black") {
						temp = array [k];
						temp = RayCast (new Vector3 (array [k].transform.position.x, array [k].transform.position.y, array [k].transform.position.z));
						rend = temp.GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
					} else {
					}

					break;

				case "white_rook":

					for (k = 0; k < 64; k++) {
						if (array [k] != null) {
							array [k] = null;
						}
					}

					float rx, rz;

					/*1*/  						//Right check
					k = 0;
					rx = obj.transform.position.x;
					rz = obj.transform.position.z;
					array [k] = RayCast (new Vector3 (rx, 7.0f, rz - 1));
					rz--;
					while (array [k] != null && array [k].transform.parent.tag == "Board") {
						rend = array [k].GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
						rz--;
						array [k] = RayCast (new Vector3 (rx, 7.0f, rz));
					}
					if (array [k] == null) {
					}

					else if (array [k].transform.parent.parent.name == "black") {
						temp = array [k];
						temp = RayCast (new Vector3 (array [k].transform.position.x, array [k].transform.position.y, array [k].transform.position.z));
						rend = temp.GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
					} else {
					}

					/*2*/						//Left check
					rx = obj.transform.position.x;
					rz = obj.transform.position.z;
					array [k] = RayCast (new Vector3 (rx, 7.0f, rz + 1));
					rz++;
					while (array [k] != null && array [k].transform.parent.tag == "Board") {
						rend = array [k].GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
						rz++;
						array [k] = RayCast (new Vector3 (rx, 7.0f, rz));

					}
					if (array [k] == null) {
					}

					else if (array [k].transform.parent.parent.name == "black") {
						temp = array [k];
						temp = RayCast (new Vector3 (array [k].transform.position.x, array [k].transform.position.y, array [k].transform.position.z));
						rend = temp.GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
					} else {
					}
					/*3*/				//Up check
					rx = obj.transform.position.x;
					rz = obj.transform.position.z;
					array [k] = RayCast (new Vector3 (rx + 1, 7.0f, rz));
					rx++;
					while (array [k] != null && array [k].transform.parent.tag == "Board") {
						rend = array [k].GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
						rx++;
						array [k] = RayCast (new Vector3 (rx, 7.0f, rz));

					}
					if (array [k] == null) {
					}

					else if (array [k].transform.parent.parent.name == "black") {
						temp = array [k];
						temp = RayCast (new Vector3 (array [k].transform.position.x, array [k].transform.position.y, array [k].transform.position.z));
						rend = temp.GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
					}
					/*4*/				//Down check
					rx = obj.transform.position.x;
					rz = obj.transform.position.z;
					array [k] = RayCast (new Vector3 (rx - 1, 7.0f, rz));
					rx--;
					while (array [k] != null && array [k].transform.parent.tag == "Board") {
						rend = array [k].GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
						rx--;
						array [k] = RayCast (new Vector3 (rx, 7.0f, rz));

					}
					if (array [k] == null) {
					}

					else if (array [k].transform.parent.parent.name == "black") {
						temp = array [k];
						temp = RayCast (new Vector3 (array [k].transform.position.x, array [k].transform.position.y, array [k].transform.position.z));
						rend = temp.GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
					}


					/*for (i = 0; i < 64; i++) {
						Debug.Log (array [i]);
					}*/


					break;

				case "white_queen":

					for (k = 0; k < 64; k++) {
						if (array [k] != null) {
							array [k] = null;
						}
					}

					float qx, qz;
					/*1*/		k = 0;
					qx = obj.transform.position.x;
					qz = obj.transform.position.z;
					array [k] = RayCast (new Vector3 (qx + 1, 7.0f, qz + 1));
					qx++;
					qz++;
					while (array [k] != null && array [k].transform.parent.tag == "Board") {
						rend = array [k].GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
						qx++;
						qz++;
						array [k] = RayCast (new Vector3 (qx, 7.0f, qz));
					}
					if (array [k] == null) {
					}

					else if (array [k].transform.parent.parent.name == "black") {
						temp = array [k];
						temp = RayCast (new Vector3 (array [k].transform.position.x, array [k].transform.position.y, array [k].transform.position.z));
						rend = temp.GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
					} else {
					}

					qx = obj.transform.position.x;
					qz = obj.transform.position.z;
					array [k] = RayCast (new Vector3 (qx, 7.0f, qz - 1));
					qz--;
					while (array [k] != null && array [k].transform.parent.tag == "Board") {
						rend = array [k].GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
						qz--;
						array [k] = RayCast (new Vector3 (qx, 7.0f,qz));
					}
					if (array [k] == null) {
					}

					else if (array [k].transform.parent.parent.name == "black") {
						temp = array [k];
						temp = RayCast (new Vector3 (array [k].transform.position.x, array [k].transform.position.y, array [k].transform.position.z));
						rend = temp.GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
					} else {
					}



					/*2*/
					qx = obj.transform.position.x;
					qz = obj.transform.position.z;
					array [k] = RayCast (new Vector3 (qx + 1, 7.0f, qz - 1));
					qx++;
					qz--;
					while (array [k] != null && array [k].transform.parent.tag == "Board") {
						rend = array [k].GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
						qx++;
						qz--;
						array [k] = RayCast (new Vector3 (qx, 7.0f, qz));
					}
					if (array [k] == null) {
					}

					else if (array [k].transform.parent.parent.name == "black") {
						temp = array [k];
						temp = RayCast (new Vector3 (array [k].transform.position.x, array [k].transform.position.y, array [k].transform.position.z));
						rend = temp.GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
					} else {
					}

					qx = obj.transform.position.x;
					qz = obj.transform.position.z;
					array [k] = RayCast (new Vector3 (qx, 7.0f, qz + 1));
					qz++;
					while (array [k] != null && array [k].transform.parent.tag == "Board") {
						rend = array [k].GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
						qz++;
						array [k] = RayCast (new Vector3 (qx, 7.0f,qz));
					}
					if (array [k] == null) {
					}

					else if (array [k].transform.parent.parent.name == "black") {
						temp = array [k];
						temp = RayCast (new Vector3 (array [k].transform.position.x, array [k].transform.position.y, array [k].transform.position.z));
						rend = temp.GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
					} else {
					}

					/*3*/
					qx = obj.transform.position.x;
					qz = obj.transform.position.z;
					array [k] = RayCast (new Vector3 (qx - 1, 7.0f, qz + 1));
					qx--;
					qz++;
					while (array [k] != null && array [k].transform.parent.tag == "Board") {
						rend = array [k].GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
						qx--;
						qz++;
						array [k] = RayCast (new Vector3 (qx, 7.0f, qz));
					}
					if (array [k] == null) {
					}

					else if (array [k].transform.parent.parent.name == "black") {
						temp = array [k];
						temp = RayCast (new Vector3 (array [k].transform.position.x, array [k].transform.position.y, array [k].transform.position.z));
						rend = temp.GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
					} else {
					}

					qx = obj.transform.position.x;
					qz = obj.transform.position.z;
					array [k] = RayCast (new Vector3 (qx+1, 7.0f, qz));
					qx++;
					while (array [k] != null && array [k].transform.parent.tag == "Board") {
						rend = array [k].GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
						qx++;
						array [k] = RayCast (new Vector3 (qx, 7.0f,qz));
					}
					if (array [k] == null) {
					}

					else if (array [k].transform.parent.parent.name == "black") {
						temp = array [k];
						temp = RayCast (new Vector3 (array [k].transform.position.x, array [k].transform.position.y, array [k].transform.position.z));
						rend = temp.GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
					} else {
					}

					/*4*/
					qx = obj.transform.position.x;
					qz = obj.transform.position.z;
					array [k] = RayCast (new Vector3 (qx - 1, 7.0f, qz - 1));
					qx--;
					qz--;
					while (array [k] != null && array [k].transform.parent.tag == "Board") {
						rend = array [k].GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
						qx--;
						qz--;
						array [k] = RayCast (new Vector3 (qx, 7.0f, qz));
					}
					if (array [k] == null) {
					}

					else if (array [k].transform.parent.parent.name == "black") {
						temp = array [k];
						temp = RayCast (new Vector3 (array [k].transform.position.x, array [k].transform.position.y, array [k].transform.position.z));
						rend = temp.GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
					} else {
					}

					qx = obj.transform.position.x;
					qz = obj.transform.position.z;
					array [k] = RayCast (new Vector3 (qx-1, 7.0f, qz));
					qx--;
					while (array [k] != null && array [k].transform.parent.tag == "Board") {
						rend = array [k].GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
						qx--;
						array [k] = RayCast (new Vector3 (qx, 7.0f,qz));
					}
					if (array [k] == null) {
					}

					else if (array [k].transform.parent.parent.name == "black") {
						temp = array [k];
						temp = RayCast (new Vector3 (array [k].transform.position.x, array [k].transform.position.y, array [k].transform.position.z));
						rend = temp.GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
					} else {
					}

					break;

				case "white_knight":
					float kx, kz;
					int n;
					kx = obj.transform.position.x;
					kz = obj.transform.position.z;

					knight_positions [0] = RayCast (new Vector3 (kx + 1, 7.0f, kz - 2));
					knight_positions [1] = RayCast (new Vector3 (kx + 1, 7.0f, kz + 2));
					knight_positions [2] = RayCast (new Vector3 (kx + 2, 7.0f, kz - 1));
					knight_positions [3] = RayCast (new Vector3 (kx + 2, 7.0f, kz + 1));
					knight_positions [4] = RayCast (new Vector3 (kx - 1, 7.0f, kz - 2));
					knight_positions [5] = RayCast (new Vector3 (kx - 2, 7.0f, kz - 1));
					knight_positions [6] = RayCast (new Vector3 (kx - 2, 7.0f, kz + 1));
					knight_positions [7] = RayCast (new Vector3 (kx - 1, 7.0f, kz + 2));
					for (n = 0; n < 8; n++) {
						if (knight_positions [n] == null) {
						} else if (knight_positions [n].transform.parent.name == "Board") {
							rend = knight_positions [n].GetComponent<Renderer> ();
							rend.material.color = Color.cyan;
						} else if (knight_positions [n].transform.parent.parent.name == "black") {
							temp = knight_positions [n];
							temp = RayCast (new Vector3 (knight_positions [n].transform.position.x, knight_positions [n].transform.position.y, knight_positions [n].transform.position.z));
							rend = temp.GetComponent<Renderer> ();
							rend.material.color = Color.cyan;
							;
						}
					}

					break;

				case "white_king":
					float king_x, king_z;
					king_x = obj.transform.position.x;
					king_z = obj.transform.position.z;
					int p = 0;
					king_positions [0] = RayCast (new Vector3 (king_x, 7.0f, king_z + 1));
					king_positions [1] = RayCast (new Vector3 (king_x, 7.0f, king_z - 1));
					king_positions [2] = RayCast (new Vector3 (king_x + 1, 7.0f, king_z));
					king_positions [3] = RayCast (new Vector3 (king_x - 1, 7.0f, king_z));
					king_positions [4] = RayCast (new Vector3 (king_x + 1, 7.0f, king_z + 1));
					king_positions [5] = RayCast (new Vector3 (king_x + 1, 7.0f, king_z - 1));
					king_positions [6] = RayCast (new Vector3 (king_x - 1, 7.0f, king_z + 1));
					king_positions [7] = RayCast (new Vector3 (king_x - 1, 7.0f, king_z - 1));
					for (p = 0; p < 8; p++) {
						if (king_positions [p] == null) {
						} else if (king_positions [p].transform.parent.name == "Board") {
							rend = king_positions [p].GetComponent<Renderer> ();
							rend.material.color = Color.cyan;
						} else if (king_positions [p].transform.parent.parent.name == "black") {
							temp = king_positions [p];
							temp = RayCast (new Vector3 (king_positions [p].transform.position.x, king_positions [p].transform.position.y, king_positions [p].transform.position.z));
							rend = temp.GetComponent<Renderer> ();
							rend.material.color = Color.cyan;

						}
					}

					if (!white_king_moved && (!white_right_rook_moved || !white_left_rook_moved)) {

						if (!white_left_rook_moved) {
							king_positions [8] = RayCast (new Vector3 (king_x, 7.0f, king_z + 1));
							if (king_positions [8].transform.parent.name == "Board") {
								king_positions [8] = RayCast (new Vector3 (king_x, 7.0f, king_z + 2));
								if (king_positions [8].transform.parent.name == "Board") {
									king_positions [9] = RayCast (new Vector3 (king_x, 7.0f, king_z + 3));
									if (king_positions [9].transform.parent.name == "Board") {
										rend = king_positions [8].GetComponent<Renderer> ();   //king_positions[8] stores left castle position
										rend.material.color = Color.cyan;
									}
								}
							}
						}
						if (!white_right_rook_moved) {
							king_positions [9] = RayCast (new Vector3 (king_x, 7.0f, king_z - 1));
							if (king_positions [9].transform.parent.name == "Board") {
								king_positions [9] = RayCast (new Vector3 (king_x, 7.0f, king_z - 2));
								if (king_positions [9].transform.parent.name == "Board") {
									rend = king_positions [9].GetComponent<Renderer> ();    //king_positions[9] stores right castle position
									rend.material.color = Color.cyan;	
								}
							}
						}
					}
					break;

				case "Board": // For Movement

					if (selected) {
						if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
							destination = MouseRaycast (Input.GetTouch(0).position);

							switch (selected_obj.transform.parent.tag) { // switch case to check which piece, inorder to decide its movement

							case "white_pawns":
								if (destination.transform.position.z == selected_obj.transform.position.z) {
									if (selected_obj.transform.position.x == -2.0f) {
										if (destination.transform.position.x == selected_obj.transform.position.x + 1 || destination.transform.position.x == selected_obj.transform.position.x + 2) {
											selected_obj.transform.position = new Vector3 (destination.transform.position.x, 0.5f, destination.transform.position.z);
											//AudioSource audio = GetComponent<AudioSource> ();
											//audio.PlayOneShot(movement_clip);

											SendMoves(selected_obj);

											white_turn = false;
											black_turn = true;
										}
									} else {
										if (destination.transform.position.x == selected_obj.transform.position.x + 1) {
											selected_obj.transform.position = new Vector3 (destination.transform.position.x, 0.5f, destination.transform.position.z);
											//AudioSource audio = GetComponent<AudioSource> ();
											//audio.PlayOneShot(movement_clip);
											if (selected_obj.transform.position.x == 4) {
												Promotion (selected_obj);
											}

											//string msg = "CMOV|";
											//msg += (String)selected_obj.name + "|";
											//msg += selected_obj.transform.position.x.ToString() + "|";
											//msg += selected_obj.transform.position.z.ToString();
											//Debug.Log (client.name);
											//client.Send (msg);
											//string msg = "moved";
											//client.Send(msg);
											//Server.Instance.BroadcastMessage(msg);
											else
												SendMoves(selected_obj);

										
											white_turn = false;
											black_turn = true;
										}
									}
								}
								break;

							case "white_bishop":
								int x = 0;
								int t = (int)((destination.transform.position.z - selected_obj.transform.position.z) / (destination.transform.position.x - selected_obj.transform.position.x));
								if (t == -1 || t == 1) {
									if (destination.transform.parent.name == "Board") {
										for (x = 0; x < 64; x++) {
											if (array [x] == destination) {
												selected_obj.transform.position = new Vector3 (destination.transform.position.x, 0.5f, destination.transform.position.z);
												//AudioSource audio = GetComponent<AudioSource> ();
												//audio.PlayOneShot(movement_clip);

												SendMoves(selected_obj);

												white_turn = false;
												black_turn = true;
											}
										}
									}
								}


								break;

							case "white_rook":
								if (destination.transform.position.x == selected_obj.transform.position.x || destination.transform.position.z == selected_obj.transform.position.z) {
									if (destination.transform.parent.name == "Board") {
										for (x = 0; x < 64; x++) {
											if (array [x] == destination) {
												selected_obj.transform.position = new Vector3 (destination.transform.position.x, 0.5f, destination.transform.position.z);
											//	AudioSource audio = GetComponent<AudioSource> ();
											//	audio.PlayOneShot(movement_clip);

												SendMoves(selected_obj);

												white_turn = false;
												black_turn = true;
												if (selected_obj.transform.name == "white_rook_right" && !white_right_rook_moved) {
													white_right_rook_moved = true;
												}
												if (selected_obj.transform.name == "white_rook_left" && !white_left_rook_moved) {
													white_left_rook_moved = true;
												}
											}
										}
									}
								}
								break;
							case "white_queen":
								int y = 0;
								int q = (int)((destination.transform.position.z - selected_obj.transform.position.z) / (destination.transform.position.x - selected_obj.transform.position.x));
								if (q == -1 || q == 1) {
									if (destination.transform.parent.name == "Board") {
										for (y = 0; y < 64; y++) {
											if (array [y] == destination) {
												selected_obj.transform.position = new Vector3 (destination.transform.position.x, 0.5f, destination.transform.position.z);
											//	AudioSource audio = GetComponent<AudioSource> ();
											//	audio.PlayOneShot(movement_clip);

												SendMoves(selected_obj);

												white_turn = false;
												black_turn = true;
											}
										}
									}

								} else if (destination.transform.position.x == selected_obj.transform.position.x || destination.transform.position.z == selected_obj.transform.position.z) {
									if (destination.transform.parent.name == "Board") {
										for (y = 0; y < 64; y++) {
											if (array [y] == destination) {
												selected_obj.transform.position = new Vector3 (destination.transform.position.x, 0.5f, destination.transform.position.z);
												//AudioSource audio = GetComponent<AudioSource> ();
											//	audio.PlayOneShot(movement_clip);

												SendMoves(selected_obj);

												white_turn = false;
												black_turn = true;
											}
										}
									}
								}
								break;

							case "white_knight":
								int w = 0;
								for (w = 0; w < 8; w++) {
									if (destination == knight_positions [w]) {
										selected_obj.transform.position = new Vector3 (destination.transform.position.x, 0.5f, destination.transform.position.z);
										//AudioSource audio = GetComponent<AudioSource> ();
										//audio.PlayOneShot(movement_clip);

										SendMoves(selected_obj);

										white_turn = false;
										black_turn = true;
									}
									knight_positions [w] = null;
								}

								break;
							case "white_king":
								int r;
								for (r = 0; r < 8; r++) {
									if (king_positions [r] == null) {
									} else if (destination == king_positions [r]) {
										selected_obj.transform.position = new Vector3 (destination.transform.position.x, 0.5f, destination.transform.position.z);
										//AudioSource audio = GetComponent<AudioSource> ();
										//audio.PlayOneShot(movement_clip);

										SendMoves(selected_obj);

										white_turn = false;
										black_turn = true;
										if (!white_king_moved) {
											white_king_moved = true;
										}
									}
									king_positions [r] = null;
								}                                                      //Castling movement
								if (destination == king_positions [8] && !white_king_moved) {
									selected_obj.transform.position = new Vector3 (destination.transform.position.x, 0.5f, destination.transform.position.z);
									//AudioSource audio = GetComponent<AudioSource> ();
									//audio.PlayOneShot(movement_clip);
									white_turn = false;
									black_turn = true;
									white_left_rook.transform.position = new Vector3 (-3, 0.5f, 1);

									SendMoves (selected_obj, white_left_rook);

									white_king_moved = true;
									white_left_rook_moved = true;
								}
								if (destination == king_positions [9] && !white_king_moved) {
									selected_obj.transform.position = new Vector3 (destination.transform.position.x, 0.5f, destination.transform.position.z);
									//AudioSource audio = GetComponent<AudioSource> ();
									//audio.PlayOneShot(movement_clip);
									white_turn = false;
									black_turn = true;
									white_right_rook.transform.position = new Vector3 (-3, 0.5f, -1);

									SendMoves(selected_obj, white_right_rook);

									white_king_moved = true;
									white_right_rook_moved = true;
								}

								break;
							}
						}
					}

					break;

				default: // Cutting
					switch (selected_obj.transform.parent.tag) {
					case "white_pawns":
						if ((obj.transform.position.x == selected_obj.transform.position.x + 1 && obj.transform.position.z == selected_obj.transform.position.z + 1) || (obj.transform.position.x == selected_obj.transform.position.x + 1) && (obj.transform.position.z == selected_obj.transform.position.z - 1)) {
							selected_obj.transform.position = new Vector3 (obj.transform.position.x, obj.transform.position.y, obj.transform.position.z);
							destroyed_object = obj;

							GameObject destroyed_obj = obj;
							SendCutMoves (selected_obj, destroyed_obj);

							Destroy (obj);
							//AudioSource audio = GetComponent<AudioSource> ();
							//audio.PlayOneShot(cutting_clip);
							if (destroyed_object.name == "black_king") {
								GameOver ("White");
							}
							if (selected_obj.transform.position.x == 4) {
								Promotion (selected_obj);
							}
							white_turn = false;
							black_turn = true;
						}
						break;
					case "white_bishop":

						int t = (int)((obj.transform.position.z - selected_obj.transform.position.z) / (obj.transform.position.x - selected_obj.transform.position.x));
						if (t == -1 || t == 1) {
							if (obj.transform.parent.parent.tag == "black") {
								for (d = 0; d < 64; d++) {
									if (array [d] != null) {
										if (array [d] == obj) {
											selected_obj.transform.position = new Vector3 (obj.transform.position.x, 0.5f, obj.transform.position.z);
											destroyed_object = obj;

											GameObject destroyed_obj = obj;
											SendCutMoves (selected_obj, destroyed_obj);

											Destroy (obj);
											//AudioSource audio = GetComponent<AudioSource> ();
											//audio.PlayOneShot (cutting_clip);
											if (destroyed_object.name == "black_king") {
												GameOver ("White");
											}
											white_turn = false;
											black_turn = true;
										} else {
										}
									}
								}
							}
						}
						break;
					case "white_rook":
						if (obj.transform.position.x == selected_obj.transform.position.x || obj.transform.position.z == selected_obj.transform.position.z) {
							if (obj.transform.parent.parent.tag == "black") {

								for(d = 0;d<64;d++){
									if (array [d] != null) {
										if (array [d] == obj) {
											selected_obj.transform.position = new Vector3 (obj.transform.position.x, 0.5f, obj.transform.position.z);
											destroyed_object = obj;

											GameObject destroyed_obj = obj;
											SendCutMoves (selected_obj, destroyed_obj);

											Destroy (obj);
										//	AudioSource audio = GetComponent<AudioSource> ();
											//audio.PlayOneShot (cutting_clip);
											if (destroyed_object.name == "black_king") {
												GameOver ("White");
											}
											white_turn = false;
											black_turn = true;
										} else {
										}
									}
								}
							}
						}
						break;
					case "white_queen":
						int q = (int)((obj.transform.position.z - selected_obj.transform.position.z) / (obj.transform.position.x - selected_obj.transform.position.x));
						if ((obj.transform.position.x == selected_obj.transform.position.x || obj.transform.position.z == selected_obj.transform.position.z) || (q == -1 || q == 1)) {
							if (obj.transform.parent.parent.name == "black") {
								for (d = 0; d < 64; d++) {
									if (array [d] != null){
										if (array [d] == obj) {
											selected_obj.transform.position = new Vector3 (obj.transform.position.x, 0.5f, obj.transform.position.z);
											destroyed_object = obj;

											GameObject destroyed_obj = obj;
											SendCutMoves (selected_obj, destroyed_obj);

											Destroy (obj);
											//AudioSource audio = GetComponent<AudioSource> ();
											//audio.PlayOneShot (cutting_clip);
											if (destroyed_object.name == "black_king") {
												white_turn = false;
												black_turn = true;
												GameOver ("White");
											}
											white_turn = false;
											black_turn = true;
										}
									}
								}			

							}
						}
						break;

					case "white_knight":
						int w = 0;
						if (obj.transform.parent.parent.name == "black") {
							for (w = 0; w < 8; w++) {
								if (knight_positions [w] != null) {
									if (obj.transform.position.x == knight_positions [w].transform.position.x && obj.transform.position.z == knight_positions [w].transform.position.z) {
										selected_obj.transform.position = new Vector3 (obj.transform.position.x, 0.5f, obj.transform.position.z);
										destroyed_object = obj;

										GameObject destroyed_obj = obj;
										SendCutMoves (selected_obj, destroyed_obj);

										Destroy (obj);
										//AudioSource audio = GetComponent<AudioSource> ();
										//audio.PlayOneShot(cutting_clip);
										if (destroyed_object.name == "black_king") {
											GameOver ("White");
										}
										white_turn = false;
										black_turn = true;
									}
								}
							}
							for (w = 0; w < 8; w++) {
								knight_positions [w] = null;
							}
						}
						break;
					case "white_king":
						int r = 0;
						if (obj.transform.parent.parent.name == "black") {
							for (r = 0; r < 8; r++) {
								if (king_positions [r] != null) {
									for ( d = 0; d < 8; d++) {
										if (king_positions [d] == obj) {
											if (obj.transform.position.x == king_positions [r].transform.position.x && obj.transform.position.z == king_positions [r].transform.position.z) {
												selected_obj.transform.position = new Vector3 (obj.transform.position.x, 0.5f, obj.transform.position.z);
												destroyed_object = obj;

												GameObject destroyed_obj = obj;
												SendCutMoves (selected_obj, destroyed_obj);

												Destroy (obj);
											//	AudioSource audio = GetComponent<AudioSource> ();
											//	audio.PlayOneShot (cutting_clip);
												if (destroyed_object.name == "black_king") {
													GameOver ("White");
												}
												white_turn = false;
												black_turn = true;
												if (!white_king_moved) {
													white_king_moved = true;
												}
											}
										}
									}
								}
							}
							for (w = 0; w < 8; w++) {
								knight_positions [w] = null;
							}
						}
						break;

					}

					break;

				}
				i = 0;
				/*for (j = 0; j < 64; j++) {
					array [j] = null;
				}*/
			} 


			//********BLACK'S TURN********//



			else if ((black_turn && isBlack)) {

				if (obj.transform.parent.name != "Board" && obj.transform.parent.parent.name != "white") {
					selected_obj = obj;
					selected = true;
				}

				switch (obj.transform.tag) {

				case "black_pawns":
					front1 = RayCast (new Vector3 (obj.transform.position.x - 1, obj.transform.position.y + 7.0f, obj.transform.position.z));
					if (front1.transform.parent.name == "Board") {
						rend = front1.GetComponent<Renderer> ();
						board_state [i] = front1;
						board_color [i] = rend.material.color;
						i++;
						rend.material.color = Color.cyan;

						if (obj.transform.position.x == 3) {
							front2 = RayCast (new Vector3 (obj.transform.position.x - 2, obj.transform.position.y + 5.0f, obj.transform.position.z));
							if (front2.transform.parent.tag == "Board") {
								rend = front2.GetComponent<Renderer> ();
								board_state [i] = front2;
								board_color [i] = rend.material.color;
								i++;
								rend.material.color = Color.cyan;

							}
						}
					}
					front_left = RayCast (new Vector3 (obj.transform.position.x - 1, obj.transform.position.y + 5.0f, obj.transform.position.z - 1));
					if (front_left != null && front_left.transform.parent.name != "Board" && (front_left.transform.parent.parent.name == "white")) {
						front_left = RayCast (new Vector3 (front_left.transform.position.x, front_left.transform.position.y, front_left.transform.position.z));
						rend = front_left.GetComponent<Renderer> ();
						board_state [i] = front_left;
						board_color [i] = rend.material.color;
						i++;
						rend.material.color = Color.cyan;
					}
					front_right = RayCast (new Vector3 (obj.transform.position.x - 1, obj.transform.position.y + 5.0f, obj.transform.position.z + 1));
					if (front_right != null && front_right.transform.parent.name != "Board" && (front_right.transform.parent.parent.name == "white")) {
						front_right = RayCast (new Vector3 (front_right.transform.position.x, front_right.transform.position.y, front_right.transform.position.z));
						rend = front_right.GetComponent<Renderer> ();
						board_state [i] = front_right;
						board_color [i] = rend.material.color;
						i++;
						rend.material.color = Color.cyan;
					}

					break;


				case "black_bishop":

					/*1*/	
					for (k = 0; k < 64; k++) {
						if (array [k] != null) {
							array [k] = null;
						}
					}

					float bx, bz;
					k = 0;
					bx = obj.transform.position.x;
					bz = obj.transform.position.z;
					array [k] = RayCast (new Vector3 (bx + 1, 7.0f, bz + 1));
					bx++;
					bz++;
					while (array [k] != null && array [k].transform.parent.tag == "Board") {
						rend = array [k].GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
						bx++;
						bz++;
						array [k] = RayCast (new Vector3 (bx, 7.0f, bz));
					}
					if (array [k] == null) {
					}

					else if (array [k].transform.parent.parent.name == "white" ) {
						temp = array [k];
						temp = RayCast (new Vector3 (array [k].transform.position.x, array [k].transform.position.y, array [k].transform.position.z));
						rend = temp.GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
					} else {
					}

					/*2*/
					bx = obj.transform.position.x;
					bz = obj.transform.position.z;
					array [k] = RayCast (new Vector3 (bx + 1, 7.0f, bz - 1));
					bx++;
					bz--;
					while (array [k] != null && array [k].transform.parent.tag == "Board") {
						rend = array [k].GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
						bx++;
						bz--;
						array [k] = RayCast (new Vector3 (bx, 7.0f, bz));
					}
					if (array [k] == null) {
					}

					else if (array [k].transform.parent.parent.name == "white") {
						temp = array [k];
						temp = RayCast (new Vector3 (array [k].transform.position.x, array [k].transform.position.y, array [k].transform.position.z));
						rend = temp.GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
					} else {
					}
					/*3*/
					bx = obj.transform.position.x;
					bz = obj.transform.position.z;
					array [k] = RayCast (new Vector3 (bx - 1, 7.0f, bz + 1));
					bx--;
					bz++;
					while (array [k] != null && array [k].transform.parent.tag == "Board") {
						rend = array [k].GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
						bx--;
						bz++;
						array [k] = RayCast (new Vector3 (bx, 7.0f, bz));
					}
					if (array [k] == null) {
					}

					else if (array [k].transform.parent.parent.name == "white") {
						temp = array [k];
						temp = RayCast (new Vector3 (array [k].transform.position.x, array [k].transform.position.y, array [k].transform.position.z));
						rend = temp.GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
					} else {
					}
					/*4*/
					bx = obj.transform.position.x;
					bz = obj.transform.position.z;
					array [k] = RayCast (new Vector3 (bx - 1, 7.0f, bz - 1));
					bx--;
					bz--;
					while (array [k] != null && array [k].transform.parent.tag == "Board") {
						rend = array [k].GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
						bx--;
						bz--;
						array [k] = RayCast (new Vector3 (bx, 7.0f, bz));
					}
					if (array [k] == null) {
					}

					else if (array [k].transform.parent.parent.name == "white") {
						temp = array [k];
						temp = RayCast (new Vector3 (array [k].transform.position.x, array [k].transform.position.y, array [k].transform.position.z));
						rend = temp.GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
					} else {
					}

					break;


				case "black_rook":


					for (k = 0; k < 64; k++) {
						if (array [k] != null) {
							array [k] = null;
						}
					}

					float rx, rz;

					/*1*/  						//Right check
					k = 0;
					rx = obj.transform.position.x;
					rz = obj.transform.position.z;
					array [k] = RayCast (new Vector3 (rx, 7.0f, rz - 1));
					rz--;
					while (array [k] != null && array [k].transform.parent.tag == "Board") {
						rend = array [k].GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
						rz--;
						array [k] = RayCast (new Vector3 (rx, 7.0f, rz));
					}
					if (array [k] == null) {
					}

					else if (array [k].transform.parent.parent.name == "white") {
						temp = array [k];
						temp = RayCast (new Vector3 (array [k].transform.position.x, array [k].transform.position.y, array [k].transform.position.z));
						rend = temp.GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
					} else {
					}

					/*2*/						//Left check
					rx = obj.transform.position.x;
					rz = obj.transform.position.z;
					array [k] = RayCast (new Vector3 (rx, 7.0f, rz + 1));
					rz++;
					while (array [k] != null && array [k].transform.parent.tag == "Board") {
						rend = array [k].GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
						rz++;
						array [k] = RayCast (new Vector3 (rx, 7.0f, rz));

					}
					if (array [k] == null) {
					}

					else if (array [k].transform.parent.parent.name == "white") {
						temp = array [k];
						temp = RayCast (new Vector3 (array [k].transform.position.x, array [k].transform.position.y, array [k].transform.position.z));
						rend = temp.GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
					} else {
					}
					/*3*/				//Up check
					rx = obj.transform.position.x;
					rz = obj.transform.position.z;
					array [k] = RayCast (new Vector3 (rx + 1, 7.0f, rz));
					rx++;
					while (array [k] != null && array [k].transform.parent.tag == "Board") {
						rend = array [k].GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
						rx++;
						array [k] = RayCast (new Vector3 (rx, 7.0f, rz));

					}
					if (array [k] == null) {
					}

					else if (array [k].transform.parent.parent.name == "white") {
						temp = array [k];
						temp = RayCast (new Vector3 (array [k].transform.position.x, array [k].transform.position.y, array [k].transform.position.z));
						rend = temp.GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
					}
					/*4*/				//Down check
					rx = obj.transform.position.x;
					rz = obj.transform.position.z;
					array [k] = RayCast (new Vector3 (rx - 1, 7.0f, rz));
					rx--;
					while (array [k] != null && array [k].transform.parent.tag == "Board") {
						rend = array [k].GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
						rx--;
						array [k] = RayCast (new Vector3 (rx, 7.0f, rz));

					}
					if (array [k] == null) {
					}

					else if (array [k].transform.parent.parent.name == "white") {
						temp = array [k];
						temp = RayCast (new Vector3 (array [k].transform.position.x, array [k].transform.position.y, array [k].transform.position.z));
						rend = temp.GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
					}



					break;

				case "black_queen":

					for (k = 0; k < 64; k++) {
						if (array [k] != null) {
							array [k] = null;
						}
					}

					float qx, qz;
					/*1*/		k = 0;
					qx = obj.transform.position.x;
					qz = obj.transform.position.z;
					array [k] = RayCast (new Vector3 (qx + 1, 7.0f, qz + 1));
					qx++;
					qz++;
					while (array [k] != null && array [k].transform.parent.tag == "Board") {
						rend = array [k].GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
						qx++;
						qz++;
						array [k] = RayCast (new Vector3 (qx, 7.0f, qz));
					}
					if (array [k] == null) {
					}

					else if (array [k].transform.parent.parent.name == "white") {
						temp = array [k];
						temp = RayCast (new Vector3 (array [k].transform.position.x, array [k].transform.position.y, array [k].transform.position.z));
						rend = temp.GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
					} else {
					}

					qx = obj.transform.position.x;
					qz = obj.transform.position.z;
					array [k] = RayCast (new Vector3 (qx, 7.0f, qz - 1));
					qz--;
					while (array [k] != null && array [k].transform.parent.tag == "Board") {
						rend = array [k].GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
						qz--;
						array [k] = RayCast (new Vector3 (qx, 7.0f,qz));
					}
					if (array [k] == null) {
					}

					else if (array [k].transform.parent.parent.name == "white") {
						temp = array [k];
						temp = RayCast (new Vector3 (array [k].transform.position.x, array [k].transform.position.y, array [k].transform.position.z));
						rend = temp.GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
					} else {
					}



					/*2*/
					qx = obj.transform.position.x;
					qz = obj.transform.position.z;
					array [k] = RayCast (new Vector3 (qx + 1, 7.0f, qz - 1));
					qx++;
					qz--;
					while (array [k] != null && array [k].transform.parent.tag == "Board") {
						rend = array [k].GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
						qx++;
						qz--;
						array [k] = RayCast (new Vector3 (qx, 7.0f, qz));
					}
					if (array [k] == null) {
					}

					else if (array [k].transform.parent.parent.name == "white") {
						temp = array [k];
						temp = RayCast (new Vector3 (array [k].transform.position.x, array [k].transform.position.y, array [k].transform.position.z));
						rend = temp.GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
					} else {
					}

					qx = obj.transform.position.x;
					qz = obj.transform.position.z;
					array [k] = RayCast (new Vector3 (qx, 7.0f, qz + 1));
					qz++;
					while (array [k] != null && array [k].transform.parent.tag == "Board") {
						rend = array [k].GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
						qz++;
						array [k] = RayCast (new Vector3 (qx, 7.0f,qz));
					}
					if (array [k] == null) {
					}

					else if (array [k].transform.parent.parent.name == "white") {
						temp = array [k];
						temp = RayCast (new Vector3 (array [k].transform.position.x, array [k].transform.position.y, array [k].transform.position.z));
						rend = temp.GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
					} else {
					}

					/*3*/
					qx = obj.transform.position.x;
					qz = obj.transform.position.z;
					array [k] = RayCast (new Vector3 (qx - 1, 7.0f, qz + 1));
					qx--;
					qz++;
					while (array [k] != null && array [k].transform.parent.tag == "Board") {
						rend = array [k].GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
						qx--;
						qz++;
						array [k] = RayCast (new Vector3 (qx, 7.0f, qz));
					}
					if (array [k] == null) {
					}

					else if (array [k].transform.parent.parent.name == "white") {
						temp = array [k];
						temp = RayCast (new Vector3 (array [k].transform.position.x, array [k].transform.position.y, array [k].transform.position.z));
						rend = temp.GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
					} else {
					}

					qx = obj.transform.position.x;
					qz = obj.transform.position.z;
					array [k] = RayCast (new Vector3 (qx+1, 7.0f, qz));
					qx++;
					while (array [k] != null && array [k].transform.parent.tag == "Board") {
						rend = array [k].GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
						qx++;
						array [k] = RayCast (new Vector3 (qx, 7.0f,qz));
					}
					if (array [k] == null) {
					}

					else if (array [k].transform.parent.parent.name == "white") {
						temp = array [k];
						temp = RayCast (new Vector3 (array [k].transform.position.x, array [k].transform.position.y, array [k].transform.position.z));
						rend = temp.GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
					} else {
					}

					/*4*/
					qx = obj.transform.position.x;
					qz = obj.transform.position.z;
					array [k] = RayCast (new Vector3 (qx - 1, 7.0f, qz - 1));
					qx--;
					qz--;
					while (array [k] != null && array [k].transform.parent.tag == "Board") {
						rend = array [k].GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
						qx--;
						qz--;
						array [k] = RayCast (new Vector3 (qx, 7.0f, qz));
					}
					if (array [k] == null) {
					}

					else if (array [k].transform.parent.parent.name == "white") {
						temp = array [k];
						temp = RayCast (new Vector3 (array [k].transform.position.x, array [k].transform.position.y, array [k].transform.position.z));
						rend = temp.GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
					} else {
					}

					qx = obj.transform.position.x;
					qz = obj.transform.position.z;
					array [k] = RayCast (new Vector3 (qx-1, 7.0f, qz));
					qx--;
					while (array [k] != null && array [k].transform.parent.tag == "Board") {
						rend = array [k].GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
						qx--;
						array [k] = RayCast (new Vector3 (qx, 7.0f,qz));
					}
					if (array [k] == null) {
					}

					else if (array [k].transform.parent.parent.name == "white") {
						temp = array [k];
						temp = RayCast (new Vector3 (array [k].transform.position.x, array [k].transform.position.y, array [k].transform.position.z));
						rend = temp.GetComponent<Renderer> ();
						rend.material.color = Color.cyan;
						k++;
					} else {
					}
					break;

				case "black_knight":
					float kx, kz;
					int n;
					kx = obj.transform.position.x;
					kz = obj.transform.position.z;

					knight_positions [0] = RayCast (new Vector3 (kx + 1, 7.0f, kz - 2));
					knight_positions [1] = RayCast (new Vector3 (kx + 1, 7.0f, kz + 2));
					knight_positions [2] = RayCast (new Vector3 (kx + 2, 7.0f, kz - 1));
					knight_positions [3] = RayCast (new Vector3 (kx + 2, 7.0f, kz + 1));
					knight_positions [4] = RayCast (new Vector3 (kx - 1, 7.0f, kz - 2));
					knight_positions [5] = RayCast (new Vector3 (kx - 2, 7.0f, kz - 1));
					knight_positions [6] = RayCast (new Vector3 (kx - 2, 7.0f, kz + 1));
					knight_positions [7] = RayCast (new Vector3 (kx - 1, 7.0f, kz + 2));
					for (n = 0; n < 8; n++) {
						if (knight_positions [n] == null) {
						} else if (knight_positions [n].transform.parent.name == "Board") {
							rend = knight_positions [n].GetComponent<Renderer> ();
							rend.material.color = Color.cyan;
						} else if (knight_positions [n].transform.parent.parent.name == "white") {
							knight_positions [n] = RayCast (new Vector3 (knight_positions [n].transform.position.x, knight_positions [n].transform.position.y, knight_positions [n].transform.position.z));
							rend = knight_positions [n].GetComponent<Renderer> ();
							rend.material.color = Color.cyan;
							;
						}
					}

					break;

				case "black_king":
					float king_x, king_z;
					king_x = obj.transform.position.x;
					king_z = obj.transform.position.z;
					int p = 0;
					king_positions [0] = RayCast (new Vector3 (king_x, 7.0f, king_z + 1));
					king_positions [1] = RayCast (new Vector3 (king_x, 7.0f, king_z - 1));
					king_positions [2] = RayCast (new Vector3 (king_x + 1, 7.0f, king_z));
					king_positions [3] = RayCast (new Vector3 (king_x - 1, 7.0f, king_z));
					king_positions [4] = RayCast (new Vector3 (king_x + 1, 7.0f, king_z + 1));
					king_positions [5] = RayCast (new Vector3 (king_x + 1, 7.0f, king_z - 1));
					king_positions [6] = RayCast (new Vector3 (king_x - 1, 7.0f, king_z + 1));
					king_positions [7] = RayCast (new Vector3 (king_x - 1, 7.0f, king_z - 1));
					for (p = 0; p < 8; p++) {
						if (king_positions [p] == null) {
						} else if (king_positions [p].transform.parent.name == "Board") {
							rend = king_positions [p].GetComponent<Renderer> ();
							rend.material.color = Color.cyan;
						} else if (king_positions [p].transform.parent.parent.name == "white") {
							temp = king_positions [p];
							temp = RayCast (new Vector3 (king_positions [p].transform.position.x, king_positions [p].transform.position.y, king_positions [p].transform.position.z));
							rend = temp.GetComponent<Renderer> ();
							rend.material.color = Color.cyan;

						}
					}

					if (!black_king_moved && (!black_right_rook_moved || !black_left_rook_moved)) {

						if (!black_right_rook_moved) {
							king_positions [8] = RayCast (new Vector3 (king_x, 7.0f, king_z + 1));
							if (king_positions [8].transform.parent.name == "Board") {
								king_positions [8] = RayCast (new Vector3 (king_x, 7.0f, king_z + 2));
								if (king_positions [8].transform.parent.name == "Board") {
									king_positions [9] = RayCast (new Vector3 (king_x, 7.0f, king_z + 3));
									if (king_positions [9].transform.parent.name == "Board") {
										rend = king_positions [8].GetComponent<Renderer> ();   //king_positions[8] stores left castle position
										rend.material.color = Color.cyan;
									}
								}
							}
						}
						if (!black_left_rook_moved) {
							king_positions [9] = RayCast (new Vector3 (king_x, 7.0f, king_z - 1));
							if (king_positions [9].transform.parent.name == "Board") {
								king_positions [9] = RayCast (new Vector3 (king_x, 7.0f, king_z - 2));
								if (king_positions [9].transform.parent.name == "Board") {
									rend = king_positions [9].GetComponent<Renderer> ();    //king_positions[9] stores right castle position
									rend.material.color = Color.cyan;	
								}
							}
						}
					}
					break;

				case "Board": // For Movement

					if (selected) {
						if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
							destination = MouseRaycast (Input.GetTouch(0).position);
							switch (selected_obj.transform.parent.tag) { // switch case to check which piece, inorder to decide its movement

							case "black_pawns":
								if (destination.transform.position.z == selected_obj.transform.position.z) {
									if (selected_obj.transform.position.x == 3.0f) {
										if (destination.transform.position.x == selected_obj.transform.position.x - 1 || destination.transform.position.x == selected_obj.transform.position.x - 2) {
											selected_obj.transform.position = new Vector3 (destination.transform.position.x, 0.5f, destination.transform.position.z);
											//AudioSource audio = GetComponent<AudioSource> ();
											//audio.PlayOneShot(movement_clip);

											SendMoves (selected_obj);

											white_turn = true;
											black_turn = false;
										}
									} else {
										if (destination.transform.position.x == selected_obj.transform.position.x - 1) {
											selected_obj.transform.position = new Vector3 (destination.transform.position.x, 0.5f, destination.transform.position.z);
											//AudioSource audio = GetComponent<AudioSource> ();
											//audio.PlayOneShot(movement_clip);
											if (selected_obj.transform.position.x == -3) {
												Promotion (selected_obj);
											}
											else
												SendMoves (selected_obj);

											white_turn = true;
											black_turn = false;
										}
									}
								}
								break;

							case "black_bishop":
								int x = 0;
								int t = (int)((destination.transform.position.z - selected_obj.transform.position.z) / (destination.transform.position.x - selected_obj.transform.position.x));
								if (t == -1 || t == 1) {
									if (destination.transform.parent.name == "Board") {
										for (x = 0; x < 64; x++) {
											if (array [x] == destination) {
												selected_obj.transform.position = new Vector3 (destination.transform.position.x, 0.5f, destination.transform.position.z);
											//	AudioSource audio = GetComponent<AudioSource> ();
											//	audio.PlayOneShot(movement_clip);

												SendMoves (selected_obj);

												white_turn = true;
												black_turn = false;
											}
										}
									}
								}


								break;

							case "black_rook":
								if (destination.transform.position.x == selected_obj.transform.position.x || destination.transform.position.z == selected_obj.transform.position.z) {
									if (destination.transform.parent.name == "Board") {
										for (x = 0; x < 64; x++) {
											if (array [x] == destination) {
												selected_obj.transform.position = new Vector3 (destination.transform.position.x, 0.5f, destination.transform.position.z);
											//	AudioSource audio = GetComponent<AudioSource> ();
											//	audio.PlayOneShot(movement_clip);

												SendMoves (selected_obj);

												white_turn = true;
												black_turn = false;
												if (selected_obj.transform.name == "black_rook_right" && !black_right_rook_moved) {
													black_right_rook_moved = true;
												}
												if (selected_obj.transform.name == "black_rook_left" && !black_left_rook_moved) {
													black_left_rook_moved = true;
												}
											}
										}
									}
								}
								break;
							case "black_queen":
								int y = 0;
								int q = (int)((destination.transform.position.z - selected_obj.transform.position.z) / (destination.transform.position.x - selected_obj.transform.position.x));
								if (q == -1 || q == 1) {
									if (destination.transform.parent.name == "Board") {
										for (y = 0; y < 64; y++) {
											if (array [y] == destination) {
												selected_obj.transform.position = new Vector3 (destination.transform.position.x, 0.5f, destination.transform.position.z);
												//AudioSource audio = GetComponent<AudioSource> ();
												//audio.PlayOneShot(movement_clip);

												SendMoves (selected_obj);

												white_turn = true;
												black_turn = false;
											}
										}
									}

								} else if (destination.transform.position.x == selected_obj.transform.position.x || destination.transform.position.z == selected_obj.transform.position.z) {
									if (destination.transform.parent.name == "Board") {
										for (y = 0; y < 64; y++) {
											if (array [y] == destination) {
												selected_obj.transform.position = new Vector3 (destination.transform.position.x, 0.5f, destination.transform.position.z);
												//AudioSource audio = GetComponent<AudioSource> ();
												//audio.PlayOneShot(movement_clip);

												SendMoves (selected_obj);

												white_turn = true;
												black_turn = false;
											}
										}
									}
								}
								break;

							case "black_knight":
								int w = 0;
								for (w = 0; w < 8; w++) {
									if (destination == knight_positions [w]) {
										selected_obj.transform.position = new Vector3 (destination.transform.position.x, 0.5f, destination.transform.position.z);
										//AudioSource audio = GetComponent<AudioSource> ();
										//audio.PlayOneShot(movement_clip);

										SendMoves (selected_obj);

										white_turn = true;
										black_turn = false;
									}
									knight_positions [w] = null;
								}

								break;
							case "black_king":
								int r;
								for (r = 0; r < 8; r++) {
									if (king_positions [r] == null) {
									} else if (destination == king_positions [r]) {
										selected_obj.transform.position = new Vector3 (destination.transform.position.x, 0.5f, destination.transform.position.z);
										//AudioSource audio = GetComponent<AudioSource> ();
										//audio.PlayOneShot(movement_clip);

										SendMoves (selected_obj);

										white_turn = true;
										black_turn = false;
										if (!black_king_moved) {
											black_king_moved = true;
										}
									}
									king_positions [r] = null;
								}                                                      //Castling movement
								if (destination == king_positions [8] && !black_king_moved) {
									selected_obj.transform.position = new Vector3 (destination.transform.position.x, 0.5f, destination.transform.position.z);
									//AudioSource audio = GetComponent<AudioSource> ();
									//audio.PlayOneShot(movement_clip);
									white_turn = true;
									black_turn = false;
									black_left_rook.transform.position = new Vector3 (4, 0.5f, 1);

									SendMoves (selected_obj, black_left_rook);

									black_king_moved = true;
									black_left_rook_moved = true;
								}
								if (destination == king_positions [9] && !black_king_moved) {
									selected_obj.transform.position = new Vector3 (destination.transform.position.x, 0.5f, destination.transform.position.z);
								//	AudioSource audio = GetComponent<AudioSource> ();
									//audio.PlayOneShot(movement_clip);
									white_turn = true;
									black_turn = false;
									black_right_rook.transform.position = new Vector3 (4, 0.5f, -1);

									SendMoves (selected_obj, black_right_rook);

									black_king_moved = true;
									black_right_rook_moved = true;
								}

								break;
							}
						}
					}

					break;



				default: // Cutting
					switch (selected_obj.transform.parent.tag) {
					case "black_pawns":
						if ((obj.transform.position.x == selected_obj.transform.position.x - 1 && obj.transform.position.z == selected_obj.transform.position.z - 1) || (obj.transform.position.x == selected_obj.transform.position.x - 1) && (obj.transform.position.z == selected_obj.transform.position.z + 1)) {
							selected_obj.transform.position = new Vector3 (obj.transform.position.x, obj.transform.position.y, obj.transform.position.z);
							destroyed_object = obj;

							GameObject destroyed_obj = obj;
							SendCutMoves (selected_obj, destroyed_obj);

							Destroy (obj);
							//AudioSource audio = GetComponent<AudioSource> ();
							//audio.PlayOneShot (cutting_clip);
							if (destroyed_object.name == "white_king") {
								GameOver ("Black");
							}
							if (selected_obj.transform.position.x == -3) {									
								Promotion (selected_obj);
							}
							white_turn = true;
							black_turn = false;

						}




						break;
					case "black_bishop":
						int t = (int)((obj.transform.position.z - selected_obj.transform.position.z) / (obj.transform.position.x - selected_obj.transform.position.x));
						if (t == -1 || t == 1) {
							if (obj.transform.parent.parent.tag == "white") {
								for (d = 0; d < 64; d++) {
									if (array [d] != null) {
										if (array [d] == obj) {
											selected_obj.transform.position = new Vector3 (obj.transform.position.x, 0.5f, obj.transform.position.z);
											destroyed_object = obj;

											GameObject destroyed_obj = obj;
											SendCutMoves (selected_obj, destroyed_obj);

											Destroy (obj);
											//AudioSource audio = GetComponent<AudioSource> ();
											//audio.PlayOneShot (cutting_clip);
											if (destroyed_object.name == "white_king") {
												GameOver ("Black");
											}
											white_turn = true;
											black_turn = false;
										} else {
										}
									}
								}
							}
						}
						break;
					case "black_rook":
						if (obj.transform.position.x == selected_obj.transform.position.x || obj.transform.position.z == selected_obj.transform.position.z) {
							if (obj.transform.parent.parent.tag == "white") {

								for (d = 0; d < 64; d++) {
									if (array [d] != null) {
										if (array [d] == obj) {
											selected_obj.transform.position = new Vector3 (obj.transform.position.x, 0.5f, obj.transform.position.z);
											destroyed_object = obj;

											GameObject destroyed_obj = obj;
											SendCutMoves (selected_obj, destroyed_obj);

											Destroy (obj);
											//AudioSource audio = GetComponent<AudioSource> ();
											//audio.PlayOneShot (cutting_clip);
											if (destroyed_object.name == "white_king") {
												GameOver ("Black");
											}
											white_turn = true;
											black_turn = false;
										} else {
										}
									}
								}
							}
						}
						break;
					case "black_queen":
						int q = (int)((obj.transform.position.z - selected_obj.transform.position.z) / (obj.transform.position.x - selected_obj.transform.position.x));
						if ((obj.transform.position.x == selected_obj.transform.position.x || obj.transform.position.z == selected_obj.transform.position.z) || (q == -1 || q == 1)) {
							if (obj.transform.parent.parent.name == "white") {
								for (d = 0; d < 64; d++) {
									if (array [d] != null) {
										if (array [d] == obj) {
											selected_obj.transform.position = new Vector3 (obj.transform.position.x, 0.5f, obj.transform.position.z);
											destroyed_object = obj;

											GameObject destroyed_obj = obj;
											SendCutMoves (selected_obj, destroyed_obj);

											Destroy (obj);
										//	AudioSource audio = GetComponent<AudioSource> ();
											//audio.PlayOneShot (cutting_clip);
											if (destroyed_object.name == "white_king") {
												white_turn = false;
												black_turn = true;
												GameOver ("Black");
											}
											white_turn = true;
											black_turn = false;
										}
									}
								}			

							}
						}
						break;

					case "black_knight":
						int w = 0;
						if (obj.transform.parent.parent.name == "white") {
							for (w = 0; w < 8; w++) {
								if (knight_positions [w] != null) {
									if (obj.transform.position.x == knight_positions [w].transform.position.x && obj.transform.position.z == knight_positions [w].transform.position.z) {
										selected_obj.transform.position = new Vector3 (obj.transform.position.x, 0.5f, obj.transform.position.z);
										destroyed_object = obj;

										GameObject destroyed_obj = obj;
										SendCutMoves (selected_obj, destroyed_obj);

										Destroy (obj);
										//AudioSource audio = GetComponent<AudioSource> ();
										//audio.PlayOneShot(cutting_clip);
										if (destroyed_object.name == "white_king") {
											GameOver ("Black");
										}
										white_turn = true;
										black_turn = false;
									}
								}
							}
							for (w = 0; w < 8; w++) {
								knight_positions [w] = null;
							}
						}
						break;
					case "black_king":
						int r = 0;
						if (obj.transform.parent.parent.name == "white") {
							for (r = 0; r < 8; r++) {
								if (king_positions [r] != null) {
									for (d = 0; d < 8; d++) {
										if (king_positions [d] == obj) {
											if (obj.transform.position.x == king_positions [r].transform.position.x && obj.transform.position.z == king_positions [r].transform.position.z) {
												selected_obj.transform.position = new Vector3 (obj.transform.position.x, 0.5f, obj.transform.position.z);
												destroyed_object = obj;

												GameObject destroyed_obj = obj;
												SendCutMoves (selected_obj, destroyed_obj);

												Destroy (obj);
											//	AudioSource audio = GetComponent<AudioSource> ();
											//	audio.PlayOneShot (cutting_clip);
												if (destroyed_object.name == "white_king") {
													GameOver ("Black");
												}
												white_turn = true;
												black_turn = false;
												if (!black_king_moved) {
													black_king_moved = true;
												}
											}
										}
									}
								}
							}
							for (w = 0; w < 8; w++) {
								knight_positions [w] = null;
							}
						}
						break;

					}

					break;

				}
				i = 0;
				for (j = 0; j > 64; j++) {
					array [j] = null;
				}
			}

		}
	}


	GameObject RayCast (Vector3 pos){
		RaycastHit hit2;

		if (Physics.Raycast (pos, -Vector3.up, out hit2, 10.0f)) {
			obj2 = hit2.transform.gameObject;
			//Debug.Log (obj2);
			return(obj2);
		} else {
			return null;
		}
	}
	GameObject MouseRaycast (Vector3 pos){
		GameObject temp;
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (pos);
		if (Physics.Raycast (ray, out hit, 100f)) {
			temp = hit.transform.gameObject;

			return(temp);
		} else {
			return null;
		}
	}

	public void GameOver(string winner){
		t.text = winner + "  Wins!";
		if (winner == "White") {
			Destroy (black_pieces);
		} else if (winner == "Black") {
			Destroy (white_pieces);
		}
		button.SetActive (true);
		if (!gameOver)
			SendGameOver (winner);
		gameOver = true;

		//restart = true;

	}

	void Promotion(GameObject obj){
		promotion = true;
		Debug.Log ("promotion");
		if (promotion) {
			if (obj.transform.parent.parent.tag == "white") {
				Debug.Log ("before queen");
				rend = obj.GetComponent<Renderer> ();
				rend.material = mats[0];
				Debug.Log ("after queen");
				Debug.Log (mats[0]);
				obj.transform.tag = "white_queen";
				obj.transform.parent = white_queen.transform;

				SendPromotion (obj, obj.transform.parent, mats[0], obj.transform.tag);
			}
			else if (obj.transform.parent.parent.tag == "black") {
				Debug.Log ("before queen");
				rend = obj.GetComponent<Renderer> ();
				rend.material = mats[1];
				Debug.Log ("after queen");
				Debug.Log (mats[1]);
				obj.transform.tag = "black_queen";
				obj.transform.parent = black_queen.transform;

				SendPromotion (obj, obj.transform.parent, mats[1], obj.transform.tag);
			}
		}
		promotion = false;
	}

	void Dehilighter(){
		int p;
		for (p = 0; p < 64; p++) {

			if (final_board [p] != null) {

				rend = final_board [p].GetComponent<Renderer> ();
				if (p % 2 == 0) {
					rend.material = black;
				} else if (p % 2 == 1) {
					rend.material = white;
				}
			}
		}

	}
	public void OnQuitButtonPressed(){
		string msg = "QUI|";
		if (server != null)
			server.Send (msg, 1, 1);
		else
			Debug.Log ("Server error!");
		Application.Quit ();
	}
	public void OnUserQuit(string winner){
		t.text = "Opponent has left.\n You win!";
		if (winner == "White") {
			Destroy (black_pieces);
		} else if (winner == "Black") {
			Destroy (white_pieces);
		}
	}
	public void SendMoves(GameObject selected_obj){
		string msg = "MOV|";
		msg += (String)selected_obj.name + "|";
		msg += selected_obj.transform.position.x.ToString() + "|";
		msg += selected_obj.transform.position.z.ToString();
		if (server != null)
			server.Send (msg, 1, 1);
		//client.Send(msg, 1);
		else
			Debug.Log ("Server error!");
		//server.BroadcastMessage (msg);
	}	
	public void SendMoves(GameObject selected_obj, GameObject rook){
		string msg = "MOV|";
		msg += (String)selected_obj.name + "|";
		msg += selected_obj.transform.position.x.ToString() + "|";
		msg += selected_obj.transform.position.z.ToString();
		if (server != null)
			server.Send (msg, 1, 1);
		//client.Send(msg, 1);
		else
			Debug.Log ("Server error!");
		string msg2 = "MOV|";
		msg2 += (String)rook.name + "|";
		msg2 += rook.transform.position.x.ToString () + "|";
		msg2 += rook.transform.position.z.ToString () + "|";
		if (server != null)
			server.Send (msg2, 1, 1);
		//client.Send(msg, 1);
		else
			Debug.Log ("Server error!");
		//server.BroadcastMessage (msg);
	}		
	public void SendCutMoves(GameObject selected_obj, GameObject destroyed_obj){
		string msg = "CUT|";
		msg += (String)selected_obj.name + "|";
		msg += selected_obj.transform.position.x.ToString() + "|";
		msg += selected_obj.transform.position.z.ToString() + "|"; 
		msg += (String)destroyed_obj.name;
		if (server != null)
			server.Send (msg, 1, 1);
		//client.Send(msg, 1);
		else
			Debug.Log ("Server error!");
	}
	public void SendPromotion(GameObject obj, Transform t, Material material, String tag){
		string msg = "PRO|";
		msg += (String)selected_obj.name + "|";
		msg += selected_obj.transform.position.x.ToString() + "|";
		msg += selected_obj.transform.position.z.ToString () + "|";
		msg += material + "|";
		msg += tag + "|";
		msg += t;
		if (server != null)
			server.Send (msg, 1, 1);
		//client.Send(msg, 1);
		else
			Debug.Log ("Server error!");
	}
	public void SendGameOver(String winner){
		string msg = "OVR|";
		msg += winner;
		if (server != null)
			server.Send (msg, 1, 1);
		//client.Send(msg, 1);
		else
			Debug.Log ("Server error!");

	}
}




