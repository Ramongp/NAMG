using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quad : MonoBehaviour {


	Casilla start, end;
	public Casilla[,] mat = new Casilla[4,4];
	// Update is called once per frame
	void Start(){
		int cont=1;
	for(int i=0;i<=3;i++){
		for (int u =0;u<=3;u++)
		{
				
					cont+=1;
			}
		} 
	}
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray,out hit)){
			if (hit.collider.tag.Equals ("Quad")) {
				//Temporal
				if (hit.collider.name.Equals ("Refresh")) {
					for (int i = 0; i <= 3; i++) {
						for (int u = 0; u <= 3; u++) {
							mat [i, u].g.GetComponent<MeshRenderer> ().material.color = Color.white;
						}
					} 
				}

				else{
				if (Input.GetMouseButtonDown (0)) {
						bool found = false;
						int f = 0, c = 0;
						while (!found) {
							if (hit.collider.name.Equals (mat [f, c].g.name)) {
								start = mat [f, c];
								found = true;
							} else {
								c += 1;
								if (c == 4) {
									c = 0;
									f += 1;
								}
							}
						}
						GameObject.Find (hit.collider.name).GetComponent<MeshRenderer> ().material.color = Color.red;
				}


				else {
					if (Input.GetMouseButton (0) && !hit.collider.name.Equals(start.g.name)) {
						GameObject.Find (hit.collider.name).GetComponent<MeshRenderer> ().material.color = Color.green;


						bool found = false;
						int f = 0, c = 0;
						while (!found) 
						{
							if (hit.collider.name.Equals (mat[f,c].g.name) )
							{
								end = mat[f,c];
								found=true;
							}
							else{
								c+=1;
								if(c==4){
									c=0;f+=1;
								}
							}
						}
							for (int i = 0; i <= 3; i++) {
								for (int u = 0; u <= 3; u++) {
									mat [i, u].g.GetComponent<MeshRenderer> ().material.color = Color.white;
								}
							} 
						Rellenar (start, end);

					}

				
						if (Input.GetMouseButtonUp (0))
						GameObject.Find (hit.collider.name).GetComponent<MeshRenderer> ().material.color = Color.blue;
				}
			//Temporal 
				}
			}
		}
	}


	void Rellenar (Casilla start, Casilla end)
	{
		if (start.f == end.f || start.c == end.c) {
			end.g.GetComponent<MeshRenderer> ().material.color = Color.green;
			return;
		}
		if (start.f < end.f) {
			end.g.GetComponent<MeshRenderer> ().material.color = Color.green;
			Rellenar(start,mat[end.f-1,end.c]);
			if (start.c > end.c) {
				Rellenar (start, mat [end.f - 1, end.c + 1]);
				Rellenar (start, mat [end.f, end.c + 1]);
			} 
			else 
			{
				Rellenar (start, mat [end.f - 1, end.c - 1]);
				Rellenar (start, mat [end.f, end.c - 1]);
			}
		}
		else 
		{
			end.g.GetComponent<MeshRenderer> ().material.color = Color.green;
			Rellenar(start,mat[end.f+1,end.c]);
			if (start.c > end.c) {
				Rellenar (start, mat [end.f + 1, end.c + 1]);
				Rellenar (start, mat [end.f, end.c + 1]);
			} 
			else 
			{
				Rellenar (start, mat [end.f + 1, end.c - 1]);
				Rellenar (start, mat [end.f, end.c - 1]);
			}
		}
		return;
	}

}
	