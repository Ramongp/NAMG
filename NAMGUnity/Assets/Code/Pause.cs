using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour {
	public GameObject Salir,Continuar,dark;
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this.gameObject);
		Seguir ();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Pausar()
	{
		Time.timeScale = 0;
		Salir.SetActive (true);
		Continuar.SetActive (true);
		dark.SetActive (true);

	}

	public void Seguir()
	{
		Time.timeScale = 1;
		Salir.SetActive (false);
		Continuar.SetActive(false);
		dark.SetActive (false);


	}
	public void SalirDelJuego()
	{
		Application.Quit ();
	}

}
