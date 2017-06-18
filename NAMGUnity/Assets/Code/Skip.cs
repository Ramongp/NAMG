using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skip : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Saltar()
	{
		switch (FromIntroToTutorial.level)
		{
		case 0:
			if (Acciones.tutorial.Equals (true)) {
				FromIntroToTutorial.level = 1;
				StartCoroutine (CambiarNivel ("Dialogue2"));
			} else {
				StartCoroutine (CambiarNivel ("Tutorial"));
			}
			break;
		case 1:
			Acciones.tutorial = false;
			BatallaManager.Level = 1;
			StartCoroutine (CambiarNivel ("Prueba"));
			break;
		case 2:
			BatallaManager.Level = 2;
			StartCoroutine (CambiarNivel ("Prueba"));
			break;
		case 3:
			BatallaManager.Level = 3;
			StartCoroutine (CambiarNivel ("Prueba"));
			break;
		case 4:
			StartCoroutine (CambiarNivel ("Menu"));
			break;
		}
	}
	IEnumerator CambiarNivel(string Nivel)
	{
		FromIntroToTutorial.animacion = true;
		for (int i = 1; i < 12; i++) {
			GameObject.Find ("Franja " + i.ToString ()).GetComponent<Animator> ().SetTrigger ("Franja");
		}
		yield return new WaitForSeconds (1.5f);
		Application.LoadLevel(Nivel);
	}
}
