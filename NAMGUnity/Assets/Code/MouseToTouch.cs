using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseToTouch : MonoBehaviour {

	int numFilas = 9;
	Casilla start, end;
	public Casilla[,] mat;
	Vector3 originalPos,finalPos;
	public GameObject rect;
	public GameObject CasillaSize;
	RowsNColumns Borders;
	public static bool pintando;
	// Use this for initialization
	void Start () {
		pintando = false;
		Borders = this.gameObject.GetComponent<RowsNColumns> ();
		rect.GetComponent<SpriteRenderer> ().color =new Color (1,0,0, 0.5f);
		mat = new Casilla[numFilas+1,numFilas+1];
		int cont=1;
		for(int i=0;i<=numFilas;i++){
			for (int u =0;u<=numFilas;u++)
			{
				mat [i, u] = new Casilla (GameObject.Find ("Quad " + cont), i, u);
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
			if (!pintando) {

				Ray ray = Camera.main.ScreenPointToRay (touchPosition);
				RaycastHit hit;
				if (Physics.Raycast (touchPosition, Vector3.forward, out hit)) {
					if (hit.collider.tag.Equals ("Quad")) {
						rect.SetActive (true);

						//ya estamos pintando
						pintando=true;
						//buscar start
						bool found = false;
						int f = 0, c = 0;
						while (!found) {
							if (hit.collider.name.Equals (mat [f, c].g.name)) {
								start = mat [f, c];
								found = true;
							} else {
								c += 1;
								if (c == numFilas + 1) {
									c = 0;
									f += 1;
								}
							}
						}
						GameObject.Find (hit.collider.name).GetComponent<MeshRenderer> ().material.color = rect.GetComponent<SpriteRenderer> ().color;
						originalPos = hit.transform.position;
						rect.transform.position = originalPos;
						Borders.GetStart (hit.collider.transform.position, f, c);
					}
				}
			}
			break;
		case TouchPhase.Moved:
			Borders.Moving (touchPosition);
			if (originalPos.x < touchPosition.x) { //x++ y++
				if (originalPos.y < touchPosition.y) {
					rect.transform.localScale = new Vector3 ((touchPosition.x + CasillaSize.transform.localScale.x / 2 - originalPos.x), (touchPosition.y + CasillaSize.transform.localScale.y / 2 - originalPos.y), 1);
					rect.transform.position = new Vector3 (originalPos.x + rect.transform.localScale.x / 2 - CasillaSize.transform.localScale.x / 2, originalPos.y + rect.transform.localScale.y / 2 - CasillaSize.transform.localScale.y / 2, rect.transform.position.z);
				} else { // x++ y--
					rect.transform.localScale = new Vector3 ((touchPosition.x + CasillaSize.transform.localScale.x / 2 - originalPos.x), (touchPosition.y - CasillaSize.transform.localScale.y / 2 - originalPos.y), 1);
					rect.transform.position = new Vector3 (originalPos.x + rect.transform.localScale.x / 2 - CasillaSize.transform.localScale.x / 2, originalPos.y + rect.transform.localScale.y / 2 + CasillaSize.transform.localScale.y / 2, rect.transform.position.z);
				}
			} else {
				if (originalPos.y < touchPosition.y) { // x-- y++
					rect.transform.localScale = new Vector3 ((touchPosition.x - CasillaSize.transform.localScale.x / 2 - originalPos.x), (touchPosition.y + CasillaSize.transform.localScale.y / 2 - originalPos.y), 1);
					rect.transform.position = new Vector3 (originalPos.x + rect.transform.localScale.x / 2 + CasillaSize.transform.localScale.x / 2, originalPos.y + rect.transform.localScale.y / 2 - CasillaSize.transform.localScale.y / 2, rect.transform.position.z);
				} else { 	//x-- y--
					rect.transform.localScale = new Vector3 ((touchPosition.x - CasillaSize.transform.localScale.x / 2 - originalPos.x), (touchPosition.y - CasillaSize.transform.localScale.y / 2 - originalPos.y), 1);
					rect.transform.position = new Vector3 (originalPos.x + rect.transform.localScale.x / 2 + CasillaSize.transform.localScale.x / 2, originalPos.y + rect.transform.localScale.y / 2 + CasillaSize.transform.localScale.y / 2, rect.transform.position.z);
				}
			}
			if ((touchPosition.x<originalPos.x+CasillaSize.transform.localScale.x/2)&&(touchPosition.x>originalPos.x-CasillaSize.transform.localScale.x/2)){
				rect.transform.localScale = new Vector3 (CasillaSize.transform.localScale.x, rect.transform.localScale.y, 1);
				rect.transform.position = new Vector3 (originalPos.x, rect.transform.position.y, rect.transform.position.z);
			}
			if ((touchPosition.y<originalPos.y+CasillaSize.transform.localScale.y/2)&&(touchPosition.y>originalPos.y-CasillaSize.transform.localScale.y/2)) {
				rect.transform.localScale = new Vector3 (rect.transform.localScale.x, CasillaSize.transform.localScale.y, 1);
				rect.transform.position = new Vector3 (rect.transform.position.x, originalPos.y, rect.transform.position.z);
			}

			break;
			/*Ray ray2 = Camera.main.ScreenPointToRay (touchPosition);
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
				break;*/


		case TouchPhase.Ended:
			Ray ray2 = Camera.main.ScreenPointToRay (touchPosition);
			RaycastHit hit2;
			if (Physics.Raycast (touchPosition, Vector3.forward, out hit2)) {
				if (hit2.collider.tag.Equals ("Quad")) {
					if (rect.activeSelf.Equals (true)) {
						GameObject.Find (hit2.collider.name).GetComponent<MeshRenderer> ().material.color = rect.GetComponent<SpriteRenderer> ().color;
						bool found = false;
						int f = 0, c = 0;
						while (!found) {
							if (hit2.collider.name.Equals (mat [f, c].g.name)) {
								end = mat [f, c];
								found = true;
							} else {
								c += 1;
								if (c == numFilas + 1) {
									c = 0;
									f += 1;
								}
							}
						}
						GameObject.Find (hit2.collider.name).GetComponent<MeshRenderer> ().material.color = rect.GetComponent<SpriteRenderer> ().color;
						Rellenar (start, end);
						//Corroborar area
						Craft g = GameObject.Find ("CreadorRecetas").GetComponent<Craft>();
						g.CorrectRect (Borders.Fcont * Borders.Ccont);


						Borders.Refresh ();
					}
				}

			}
			rect.transform.localScale = new Vector3 (CasillaSize.transform.localScale.x, CasillaSize.transform.localScale.y, rect.transform.localScale.z);
			rect.SetActive (false);


			break;
			}
		}

		void Rellenar (Casilla start, Casilla end)
	{
		Color colorRect = rect.GetComponent<SpriteRenderer> ().color;
		if (start.f == end.f && start.c == end.c) {
			end.g.GetComponent<MeshRenderer> ().material.color = colorRect;
			return;
		} else {
			if (start.f == end.f) {
				end.g.GetComponent<MeshRenderer> ().material.color = colorRect;
				if (start.c > end.c) {
					Rellenar (start, mat [end.f, end.c + 1]);
				} else {
					Rellenar (start, mat [end.f, end.c - 1]);
				}
			} else {
				if (start.c == end.c) {
					end.g.GetComponent<MeshRenderer> ().material.color = colorRect;
					if (start.f > end.f) {
						Rellenar (start, mat [end.f + 1, end.c]);
					} else {
						Rellenar (start, mat [end.f - 1, end.c]);
					}
				} else {
					if (start.f < end.f) {
						end.g.GetComponent<MeshRenderer> ().material.color =colorRect;
						Rellenar (start, mat [end.f - 1, end.c]);
						if (start.c > end.c) {
							Rellenar (start, mat [end.f - 1, end.c + 1]);
							Rellenar (start, mat [end.f, end.c + 1]);
						} else {
							Rellenar (start, mat [end.f - 1, end.c - 1]);
							Rellenar (start, mat [end.f, end.c - 1]);
						}
					} else {
						end.g.GetComponent<MeshRenderer> ().material.color = colorRect;
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

	public void Refresh()
	{
		for (int i = 0; i <= numFilas; i++) {
			for (int u = 0; u <= numFilas; u++) {
				if(mat [i, u].g.GetComponent<MeshRenderer> ().material.color.Equals(rect.GetComponent<SpriteRenderer>().color))
					mat [i, u].g.GetComponent<MeshRenderer> ().material.color = Color.white;
			}
		} 
		pintando = false;
	}



	public void Yellow()
	{
		rect.GetComponent<SpriteRenderer> ().color = new Color (1,1,0, 0.5f);
	}

	public void Red()
	{
		rect.GetComponent<SpriteRenderer> ().color = new Color (1,0,0, 0.5f);
	}

	public void Blue()
	{
		rect.GetComponent<SpriteRenderer> ().color = new Color (0,0,1, 0.5f);
	}
}

