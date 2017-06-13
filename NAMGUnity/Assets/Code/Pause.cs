using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour {
	public GameObject Salir,Continuar,dark;
	private AudioSource A;
	// Use this for initialization
	void Start () {
		A = this.gameObject.GetComponent<AudioSource> ();
		DontDestroyOnLoad(this.gameObject);
		Time.timeScale = 1;
		Salir.SetActive (false);
		Continuar.SetActive(false);
		dark.SetActive (false);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Pausar()
	{
		AudioClip correct = Resources.Load ("Sounds/Correct") as AudioClip;
		A.PlayOneShot (correct);
		Time.timeScale = 0;
		Salir.SetActive (true);
		Continuar.SetActive (true);
		dark.SetActive (true);

	}

	public void Seguir()
	{
		AudioClip correct = Resources.Load ("Sounds/Correct") as AudioClip;
		A.PlayOneShot (correct);
		Time.timeScale = 1;
		Salir.SetActive (false);
		Continuar.SetActive(false);
		dark.SetActive (false);


	}
	public void SalirDelJuego()
	{
		AudioClip correct = Resources.Load ("Sounds/Correct") as AudioClip;
		A.PlayOneShot (correct);
		Application.Quit ();
	}

}
