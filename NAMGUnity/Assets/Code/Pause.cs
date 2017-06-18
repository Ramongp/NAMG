using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour {
	public GameObject Salir, Continuar, dark, Info, guia, pausar;
	private AudioSource A;
	// Use this for initialization
	void Start () {
		A = this.gameObject.GetComponent<AudioSource> ();
		DontDestroyOnLoad(this.gameObject);
		Time.timeScale = 1;
		Salir.SetActive (false);
		Continuar.SetActive(false);
		dark.SetActive (false);
		guia.SetActive (false);
		Info.SetActive (false);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void Cerrar()
	{
		AudioClip correct = Resources.Load ("Sounds/Correct") as AudioClip;
		A.PlayOneShot (correct);
		Salir.SetActive (true);
		Continuar.SetActive(true);
		guia.SetActive (true);
		Info.SetActive (false);
		dark.GetComponent<Image>().color= new Color (0, 0, 0, 0.5f);
	}
	public void Guia()
	{
		AudioClip correct = Resources.Load ("Sounds/Correct") as AudioClip;
		A.PlayOneShot (correct);
		Salir.SetActive (false);
		Continuar.SetActive(false);
		guia.SetActive (false);
		Info.SetActive (true);
		dark.GetComponent<Image>().color = new Color (0, 0, 0, 0.8f);
	}
	public void Pausar()
	{
		AudioClip correct = Resources.Load ("Sounds/Correct") as AudioClip;
		A.PlayOneShot (correct);
		Time.timeScale = 0;
		Salir.SetActive (true);
		Continuar.SetActive (true);
		dark.SetActive (true);
		guia.SetActive (true);
		pausar.SetActive (false);

	}

	public void Seguir()
	{
		AudioClip correct = Resources.Load ("Sounds/Correct") as AudioClip;
		A.PlayOneShot (correct);
		Time.timeScale = 1;
		Salir.SetActive (false);
		Continuar.SetActive(false);
		dark.SetActive (false);
		guia.SetActive (false);
		pausar.SetActive (true);


	}
	public void SalirDelJuego()
	{
		AudioClip correct = Resources.Load ("Sounds/Correct") as AudioClip;
		A.PlayOneShot (correct);
		Application.Quit ();
	}

}
