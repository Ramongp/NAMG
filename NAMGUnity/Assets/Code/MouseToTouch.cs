using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseToTouch : MonoBehaviour {

	int numFilas = 9;
	Casilla start, end;
	public Casilla[,] mat;
	// Use this for initialization
	void Start () {
		mat = new Casilla[numFilas+1,numFilas+1];
		int cont=1;
		for(int i=0;i<=numFilas;i++){
			for (int u =0;u<=numFilas;u++)
			{
				mat [i, u] = new Casilla (GameObject.Find ("Quad " + cont), i, u,GameObject.Find("F"+i).GetComponent<Text>());
				cont+=1;
			}
		} 
	}
	
	// Update is called once per frame
		void Update () {

					// Handle native touch events
					foreach (Touch touch in Input.touches) {
						HandleTouch (touch.fingerId, Camera.main.ScreenToWorldPoint (touch.position), touch.phase);
					}

					// Simulate touch events from mouse events
					if (Input.touchCount == 0) {
						if (Input.GetMouseButtonDown (0)) {
							HandleTouch (10, Camera.main.ScreenToWorldPoint (Input.mousePosition), TouchPhase.Began);
						}
						if (Input.GetMouseButton (0)) {
							HandleTouch (10, Camera.main.ScreenToWorldPoint (Input.mousePosition), TouchPhase.Moved);
						}
						if (Input.GetMouseButtonUp (0)) {
							HandleTouch (10, Camera.main.ScreenToWorldPoint (Input.mousePosition), TouchPhase.Ended);
						}
					}
				}

		private void HandleTouch(int touchFingerId, Vector3 touchPosition, TouchPhase touchPhase) {
			switch (touchPhase) {
		case TouchPhase.Began:

			Ray ray = Camera.main.ScreenPointToRay (touchPosition);
				RaycastHit hit;
			if (Physics.Raycast (touchPosition,Vector3.forward, out hit)) {
					if (hit.collider.tag.Equals ("Quad")) {
						//Temporal
						if (hit.collider.name.Equals ("Refresh")) {
						for (int i = 0; i <= numFilas; i++) {
							for (int u = 0; u <= numFilas; u++) {
									mat [i, u].g.GetComponent<MeshRenderer> ().material.color = Color.white;
								}
							} 
						} else {
							bool found = false;
							int f = 0, c = 0;
							while (!found) {
								if (hit.collider.name.Equals (mat [f, c].g.name)) {
									start = mat [f, c];
									found = true;
								} else {
									c += 1;
								if (c == numFilas+1) {
										c = 0;
										f += 1;
									}
								}
							}
							GameObject.Find (hit.collider.name).GetComponent<MeshRenderer> ().material.color = Color.red;
						}
					}
				}
					break;
		case TouchPhase.Moved:

			Ray ray2 = Camera.main.ScreenPointToRay (touchPosition);
			RaycastHit hit2;
			if (Physics.Raycast (touchPosition, Vector3.forward, out hit2)) {
				if (hit2.collider.tag.Equals ("Quad")) {

					bool found2 = false;
					int f2 = 0, c2 = 0;
					while (!found2) {
						if (hit2.collider.name.Equals (mat [f2, c2].g.name)) {
							end = mat [f2, c2];
							found2 = true;
						} else {
							c2 += 1;
							if (c2 == numFilas+1) {
								c2 = 0;
								f2 += 1;
							}
						}
					}
					for (int i = 0; i <= numFilas; i++) {
						for (int u = 0; u <= numFilas; u++) {
							mat [i, u].g.GetComponent<MeshRenderer> ().material.color = Color.white;
						}
					} 
					Rellenar (start, end);
				}
			}
				break;
			case TouchPhase.Ended:
				// TODO
				break;
			}
		}

		void Rellenar (Casilla start, Casilla end)
	{
		if (start.f == end.f && start.c == end.c) {
			end.g.GetComponent<MeshRenderer> ().material.color = Color.green;
			return;
		} else {
			if (start.f == end.f) {
				end.g.GetComponent<MeshRenderer> ().material.color = Color.green;
				if (start.c > end.c) {
					Rellenar (start, mat [end.f, end.c + 1]);
				} else {
					Rellenar (start, mat [end.f, end.c - 1]);
				}
			} else {
				if (start.c == end.c) {
					end.g.GetComponent<MeshRenderer> ().material.color = Color.green;
					if (start.f > end.f) {
						Rellenar (start, mat [end.f + 1, end.c]);
					} else {
						Rellenar (start, mat [end.f - 1, end.c]);
					}
				} else {
					if (start.f < end.f) {
						end.g.GetComponent<MeshRenderer> ().material.color = Color.green;
						Rellenar (start, mat [end.f - 1, end.c]);
						if (start.c > end.c) {
							Rellenar (start, mat [end.f - 1, end.c + 1]);
							Rellenar (start, mat [end.f, end.c + 1]);
						} else {
							Rellenar (start, mat [end.f - 1, end.c - 1]);
							Rellenar (start, mat [end.f, end.c - 1]);
						}
					} else {
						end.g.GetComponent<MeshRenderer> ().material.color = Color.green;
						Rellenar (start, mat [end.f + 1, end.c]);
						if (start.c > end.c) {
							Rellenar (start, mat [end.f + 1, end.c + 1]);
							Rellenar (start, mat [end.f, end.c + 1]);
						} else {
							Rellenar (start, mat [end.f + 1, end.c - 1]);
							Rellenar (start, mat [end.f, end.c - 1]);
						}
					}
					return;
				}
			}
		}
	}
}

