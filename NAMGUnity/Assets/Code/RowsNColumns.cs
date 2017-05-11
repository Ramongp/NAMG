using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RowsNColumns : MonoBehaviour {

	public GameObject[] Filas,Columnas;
	public GameObject CasillaSize;
	public int Fcont,Ccont;
	Vector3 start;
	GameObject F,C;
	int Fnum,Cnum;
	bool moving;
	float Lado;
	// Use this for initialization
	void Start () {
		Lado = CasillaSize.transform.localScale.x-0.1f;
		Refresh ();
	}
	// Update is called once per frame
	void Update () {

	}



	public void Moving(Vector3 NewEnd)
	{
		if (moving) {
			if ((start.y + Lado / 2 >= NewEnd.y) && (start.y - Lado / 2 <= NewEnd.y)) {
			} else {
				//	Debug.Log( start.y.ToString()+" "+NewEnd.y.ToString()+" " + (start.y + Lado / 2 ).ToString());
				if (start.y + Lado / 2 <= NewEnd.y) { //Sube
					if (Fnum > 0) {
						if (Filas [Fnum - 1].activeSelf.Equals (true)) { //volviendo
							F.GetComponent<Text>().color=Color.gray;
							F.SetActive (false);
							F = Filas [Fnum - 1];
							F.GetComponent<Text>().color=Color.green;
							Fnum--;
							Fcont--;

						} else { //creciendo
							Fcont++;
							F.GetComponent<Text>().color=Color.gray;
							F = Filas [Fnum - 1];
							F.GetComponent<Text>().color=Color.green;
							Fnum--;
							F.SetActive (true);
							F.GetComponent<Text> ().text = Fcont.ToString ();


						}
						start = new Vector3 (start.x, start.y + Lado, start.z);
					}
				} else {//Baja
					if (Fnum < 9) {
						if (Filas [Fnum + 1].activeSelf.Equals (true)) { //volviendo
							F.GetComponent<Text>().color=Color.gray;
							F.SetActive (false);
							F = Filas [Fnum + 1];
							F.GetComponent<Text>().color=Color.green;
							Fcont--;
							Fnum++;

						} else { //creciendo
							Fcont++;
							F.GetComponent<Text>().color=Color.gray;
							F = Filas [Fnum + 1];
							F.GetComponent<Text>().color=Color.green;
							Fnum++;
							F.SetActive (true);
							F.GetComponent<Text> ().text = Fcont.ToString ();

						}
						start = new Vector3 (start.x, start.y - Lado, start.z);
					}

				}
				
			}



			if ((start.x + Lado / 2 >= NewEnd.x) && (start.x - Lado / 2 <= NewEnd.x)) {
			} else {
				//	Debug.Log( start.y.ToString()+" "+NewEnd.y.ToString()+" " + (start.y + Lado / 2 ).ToString());
				if (start.x - Lado / 2 > NewEnd.x) { //Izquierda
					if (Cnum > 0) {
						if (Columnas [Cnum - 1].activeSelf.Equals (true)) { //volviendo
							C.GetComponent<Text>().color=Color.gray;
							C.SetActive (false);
							C = Columnas [Cnum - 1];
							C.GetComponent<Text>().color=Color.green;
							Cnum--;
							Ccont--;

						} else { //creciendo
							Ccont++;
							C.GetComponent<Text>().color=Color.gray;
							C = Columnas [Cnum - 1];
							C.GetComponent<Text>().color=Color.green;
							Cnum--;
							C.SetActive (true);
							C.GetComponent<Text> ().text = Ccont.ToString ();


						}
						start = new Vector3 (start.x-Lado, start.y, start.z);
					}
				} else {//Derecha
					if (Cnum < 9) {
						if (Columnas [Cnum + 1].activeSelf.Equals (true)) { //volviendo
							C.GetComponent<Text>().color=Color.gray;
							C.SetActive (false);
							C = Columnas [Cnum + 1];
							C.GetComponent<Text>().color=Color.green;
							Ccont--;
							Cnum++;

						} else { //creciendo
							Ccont++;
							C.GetComponent<Text>().color=Color.gray;
							C = Columnas [Cnum + 1];
							C.GetComponent<Text>().color=Color.green;
							Cnum++;
							C.SetActive (true);
							C.GetComponent<Text> ().text = Ccont.ToString ();

						}
						start = new Vector3 (start.x+Lado, start.y, start.z);
					}
				
				}

			}

		}
	}


		public void GetStart(Vector3 start, int F,int C)
		{
		moving = true;
		this.start = start;
		Fnum = F;
		Cnum = C;
		foreach (GameObject g in Filas) {
			if(g.name.Equals("F"+Fnum.ToString()))
				this.F=g;
		}
		this.F.SetActive (true);
		this.F.GetComponent<Text>().text=Fcont.ToString();
		this.F.GetComponent<Text>().color=Color.green;

		foreach (GameObject g in Columnas) {
			if(g.name.Equals("C"+Cnum.ToString()))
				this.C=g;
		}
		this.C.SetActive (true);
		this.C.GetComponent<Text>().text=Ccont.ToString();
		this.C.GetComponent<Text>().color=Color.green;

	}

	public void Refresh()
	{
		moving = false;
		foreach (GameObject g in Filas) {
			g.GetComponent<Text>().color=Color.gray;
			g.SetActive (false);

		}
		foreach (GameObject g in Columnas) {
			g.GetComponent<Text>().color=Color.gray;
			g.SetActive (false);

		}
		Fcont = 1;Ccont=1;
	}

}
