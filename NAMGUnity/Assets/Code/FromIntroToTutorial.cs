using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FromIntroToTutorial : MonoBehaviour {
	static bool iniciado, animacion,level1;
	public static int level;
	private AudioSource A;
	// Use this for initialization
	void Start () {
		A = this.gameObject.GetComponent<AudioSource> ();
		level1 = true;
		if (!iniciado) {
			Screen.SetResolution (1024, 600, true);
			iniciado = true;
			level = 0;
		}
		if (animacion) {
			animacion = false;
			for (int i = 1; i < 12; i++) {
				GameObject.Find ("Franja " + i.ToString ()).GetComponent<Animator> ().SetTrigger ("Franja2");
			}
		}
		switch (level) {
		case 0:
			break;
		case 1:
			Fungus.Flowchart.BroadcastFungusMessage ("Level1");
			if(!BatallaManager.Level.Equals(1))
			GameObject.Find ("Portal").SetActive (false);
			break;
		case 2:
			Fungus.Flowchart.BroadcastFungusMessage ("Level2");
			break;
		case 3:
			Fungus.Flowchart.BroadcastFungusMessage ("Level3");
			break;
		case 4:
			Fungus.Flowchart.BroadcastFungusMessage ("Level4");
			GameObject.Find ("Portal").SetActive (true);
			break;

		}
		
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void ToIntroduction()
	{
		AudioClip correct = Resources.Load ("Sounds/Correct") as AudioClip;
		A.PlayOneShot (correct);
		StartCoroutine (CambiarNivel ("Dialogue1"));

	}
	void ToTutorial()
	{
		StartCoroutine (CambiarNivel ("Tutorial"));
	}
	public void ToD2()
	{
		level = 1;
		StartCoroutine (CambiarNivel ("Dialogue2"));
	}

	public void ToLD3()
	{
		StartCoroutine (CambiarNivel ("Dialogue2"));
		level = 2;
	}

	public void ToLv1()
	{
		Acciones.tutorial = false;
		BatallaManager.Level = 1;
		StartCoroutine (CambiarNivel ("Prueba"));
	}

	public void ToLv2()
	{
		BatallaManager.Level = 2;
		StartCoroutine (CambiarNivel ("Prueba"));
	}
	public void ToLv3()
	{
		BatallaManager.Level = 3;
		StartCoroutine (CambiarNivel ("Prueba"));
	}

	public void ToStart()
	{
		StartCoroutine (CambiarNivel ("Menu"));

	}



	IEnumerator CambiarNivel(string Nivel)
	{
		animacion = true;
		for (int i = 1; i < 12; i++) {
			GameObject.Find ("Franja " + i.ToString ()).GetComponent<Animator> ().SetTrigger ("Franja");
		}
		yield return new WaitForSeconds (1.5f);
		Application.LoadLevel(Nivel);
	}
		
}
