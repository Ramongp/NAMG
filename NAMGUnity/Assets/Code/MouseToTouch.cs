using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseToTouch : MonoBehaviour {

	int numFilas = 9, Fila,Columna;
	Casilla start, end;
	public Casilla[,] mat;
	Vector3 originalPos,finalPos;
	public GameObject rect, result;
	public GameObject CasillaSize;
	RowsNColumns Borders;
	public static bool pintando, crafteando,correcto;
	Stack <GameObject> CasillasYaPint;
	Stack <Color> CasillasYaCol;
	Color Roj,Ama,Azu,Nar,Verd,Lila;

	// Use this for initialization
	void Start () {
		crafteando = false;
		//Resultado en esquina
		result =GameObject.Find("Resultado");
		Stopresult ();

		pintando = false;
		CasillasYaPint = new Stack<GameObject> ();
		CasillasYaCol = new Stack<Color>();
		//Colores

		Roj = new Color (1, 0, 0, 0.5f);Ama = new Color (1, 1, 0, 0.5f);Azu = new Color (0, 1, 1, 0.5f);Nar = new Color (1,0.5f,0,0.5f);Verd = new Color (0, 1, 0, 0.5f);
		Lila = new Color (1, 0, 1, 0.5f);

		Borders = this.gameObject.GetComponent<RowsNColumns> ();
		rect.GetComponent<SpriteRenderer> ().color =new Color (1,0,0, 0.5f);
		mat = new Casilla[numFilas+1,numFilas+1];
		int cont=1;
		//Colocar
		float Lado = CasillaSize.transform.localScale.x;
		Vector3 OriginalColocar = new Vector3( GameObject.Find ("Quad " + cont).transform.position.x,GameObject.Find ("Quad " + cont).transform.position.y,0);
		Vector3 Colocar = new Vector3( GameObject.Find ("Quad " + cont).transform.position.x,GameObject.Find ("Quad " + cont).transform.position.y,0);
		//Debug.Log("X "+Colocar.x.ToString()+" Y "+Colocar.y.ToString()+" Z "+Colocar.x.ToString());
		for(int i=0;i<=numFilas;i++){
			for (int u =0;u<=numFilas;u++)
			{
				GameObject g = GameObject.Find ("Quad " + cont);
				GameObject r = GameObject.Find ("R" + cont);
				mat [i, u] = new Casilla (g, i, u);
				cont+=1;
				g.transform.position=Colocar;
				r.transform.position = Colocar;
				Colocar= new Vector3 ((Colocar.x+Lado),Colocar.y,Colocar.z);
			}
			Colocar= new Vector3 (OriginalColocar.x,(Colocar.y-Lado),Colocar.z);
		} 
	}
	
	// Update is called once per frame
		void Update () {


		//mover resultado de multiplicación
		if (result.GetComponent<Text> ().text != " ") {
			result.transform.Translate(Vector3.up * Time.deltaTime);
		}
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
			if (crafteando) {
				if (!pintando) {

					//Ray ray = Camera.main.ScreenPointToRay (touchPosition);
					RaycastHit hit;
					if (Physics.Raycast (touchPosition, Vector3.forward, out hit)) {
						if (hit.collider.tag.Equals ("Quad")) {
							rect.SetActive (true);

							//ya estamos pintando
							pintando = true;
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
							//GameObject.Find (hit.collider.name).GetComponent<MeshRenderer> ().material.color = rect.GetComponent<SpriteRenderer> ().color;
							originalPos = hit.transform.position;
							rect.transform.position = originalPos;
							Borders.GetStart (hit.collider.transform.position, f, c);
						}
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
			//Ray ray2 = Camera.main.ScreenPointToRay (touchPosition);
			RaycastHit hit2;
			if (Physics.Raycast (touchPosition, Vector3.forward, out hit2)) {
				if (hit2.collider.tag.Equals ("Quad")) {
					if (rect.activeSelf.Equals (true)) {
						//	GameObject.Find (hit2.collider.name).GetComponent<MeshRenderer> ().material.color = rect.GetComponent<SpriteRenderer> ().color;
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
						//	GameObject.Find (hit2.collider.name).GetComponent<MeshRenderer> ().material.color = rect.GetComponent<SpriteRenderer> ().color;
						Rellenar (start, end);

						//Mostrar multiplicacion
						Fila = Borders.Fcont;
						Columna = Borders.Ccont;
						result.transform.position= new Vector3(touchPosition.x,touchPosition.y,0);
						result.GetComponent<Text> ().text = Fila.ToString () + " X " + Columna.ToString ();
						result.GetComponent<Text> ().color = Color.white;
						Invoke ("Changeresult", 1);
						Invoke ("Stopresult", 2);

						//Corroborar area
						Craft g = GameObject.Find ("CreadorRecetas").GetComponent<Craft> ();
						g.CorrectRect (Borders.Fcont * Borders.Ccont);

						if (correcto) {
							result.GetComponent<Text> ().color = Color.green;
							correcto = false;
						}

						Borders.Refresh ();
					}
				}

			}
			else
			{
				if(rect.activeSelf.Equals(true)){
				//Debug.Log ("Fuera");
				Borders.Refresh ();
				pintando = false;
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
			if (end.g.GetComponent<MeshRenderer> ().material.color.Equals (Color.white) || end.g.GetComponent<MeshRenderer> ().material.color.Equals (Color.white))
				end.g.GetComponent<MeshRenderer> ().material.color = colorRect;
			else {
				CasillasYaPint.Push (end.g);
				CasillasYaCol.Push (end.g.GetComponent<MeshRenderer> ().material.color );
				end.g.GetComponent<MeshRenderer> ().material.color = Colorear (end.g.GetComponent<MeshRenderer> ().material.color, rect.GetComponent<SpriteRenderer> ().color);
			}
			return;
		} else {
			if (start.f == end.f) {
				if (end.g.GetComponent<MeshRenderer> ().material.color.Equals (Color.white))
					end.g.GetComponent<MeshRenderer> ().material.color = colorRect;
				else {
					CasillasYaPint.Push (end.g);
					CasillasYaCol.Push (end.g.GetComponent<MeshRenderer> ().material.color );
					end.g.GetComponent<MeshRenderer> ().material.color = Colorear (end.g.GetComponent<MeshRenderer> ().material.color, rect.GetComponent<SpriteRenderer> ().color);
				}
				if (start.c > end.c) {
					Rellenar (start, mat [end.f, end.c + 1]);
				} else {
					Rellenar (start, mat [end.f, end.c - 1]);
				}
			} else {
				if (start.c == end.c) {
					if (end.g.GetComponent<MeshRenderer> ().material.color.Equals (Color.white))
						end.g.GetComponent<MeshRenderer> ().material.color = colorRect;
					else {
						CasillasYaPint.Push (end.g);
						CasillasYaCol.Push (end.g.GetComponent<MeshRenderer> ().material.color );
						end.g.GetComponent<MeshRenderer> ().material.color = Colorear (end.g.GetComponent<MeshRenderer> ().material.color, rect.GetComponent<SpriteRenderer> ().color);
					}
					if (start.f > end.f) {
						Rellenar (start, mat [end.f + 1, end.c]);
					} else {
						Rellenar (start, mat [end.f - 1, end.c]);
					}
				} else {
					if (start.f < end.f) {
						if (end.g.GetComponent<MeshRenderer> ().material.color.Equals (Color.white))
							end.g.GetComponent<MeshRenderer> ().material.color = colorRect;
						else {
							CasillasYaPint.Push (end.g);
							CasillasYaCol.Push (end.g.GetComponent<MeshRenderer> ().material.color );
							end.g.GetComponent<MeshRenderer> ().material.color = Colorear (end.g.GetComponent<MeshRenderer> ().material.color, rect.GetComponent<SpriteRenderer> ().color);
						}
						Rellenar (start, mat [end.f - 1, end.c]);
						if (start.c > end.c) {
							Rellenar (mat[end.f,start.c], mat [end.f, end.c + 1]);
						} else {
							Rellenar (mat[end.f,start.c], mat [end.f, end.c - 1]);
						}
					} else {
						if (end.g.GetComponent<MeshRenderer> ().material.color.Equals (Color.white))
							end.g.GetComponent<MeshRenderer> ().material.color = colorRect;
						else {
							CasillasYaPint.Push (end.g);
							CasillasYaCol.Push (end.g.GetComponent<MeshRenderer> ().material.color );
							end.g.GetComponent<MeshRenderer> ().material.color = Colorear (end.g.GetComponent<MeshRenderer> ().material.color, rect.GetComponent<SpriteRenderer> ().color);
						}
						Rellenar (start, mat [end.f + 1, end.c]);
						if (start.c > end.c) {
							Rellenar (mat[end.f,start.c], mat [end.f, end.c + 1]);
						} else {
							Rellenar (mat[end.f,start.c], mat [end.f, end.c - 1]);
						}
					}
					return;
				}
			}
		}
	}

	public void Refresh()
	{
		if (crafteando) {
			while (CasillasYaPint.Count > 0) {
				GameObject g = CasillasYaPint.Pop ();
				g.GetComponent<MeshRenderer> ().material.color = CasillasYaCol.Pop ();
			}

			for (int i = 0; i <= numFilas; i++) {
				for (int u = 0; u <= numFilas; u++) {
					if (mat [i, u].g.GetComponent<MeshRenderer> ().material.color.Equals (rect.GetComponent<SpriteRenderer> ().color))
						mat [i, u].g.GetComponent<MeshRenderer> ().material.color = Color.white;
				}
			} 
			pintando = false;
		}
	}

	public void RestartTable()
	{
		for (int i = 0; i <= numFilas; i++) {
			for (int u = 0; u <= numFilas; u++) {
					mat [i, u].g.GetComponent<MeshRenderer> ().material.color = Color.white;
			}
			CasillasYaPint.Clear ();
			CasillasYaPint.Clear ();
		} 
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


	Color Colorear ( Color C1,Color C2)
	{
		//Debug.Log (C1.ToString()+" "+ C2.ToString());
		if (C1.Equals(Roj)) {
			if (C2.Equals (Ama))
				return Nar;
			else
				return Lila;
		}

		if (C1.Equals(Ama)) {
			return Verd;
		}

		return Color.grey;
	}


	public int Corroborar(Color C){

		int cont = 0;
		Stack<GameObject> pila = CasillasYaPint;
		Stack<GameObject> save = new Stack<GameObject> ();
		while (pila.Count > 0) {
			GameObject color = pila.Pop ();
			if (color.GetComponent<MeshRenderer> ().material.color.Equals (C)) {
				//Debug.Log ("Son iguales");
				cont++;
			}
			save.Push (color);
		}
		while (save.Count > 0) {
			GameObject color2 = save.Pop ();
			CasillasYaPint.Push (color2);
		}
		return cont;
	}


	void Changeresult()
	{
		//Debug.Log (Fila.ToString () + " " +Columna.ToString ());
		result.GetComponent<Text> ().text = (Fila * Columna).ToString ();
	}

	void Stopresult()
	{
		result.GetComponent<Text> ().text = " ";
	}
}

