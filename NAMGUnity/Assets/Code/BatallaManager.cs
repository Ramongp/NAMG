using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BatallaManager : MonoBehaviour {


	public enum EstadosDeBatalla
	{
		ESPERANDO,
		TURNOMONSTRUO,
		ESCOGIENDO,
		EFECTO,
		LOSE,
		WIN
	}
	// Use this for initialization

	public static EstadosDeBatalla CurrenState;
	int Level;
	Guerrero[] guerreros;
	public Slider[] salud,turnos;
	Monstruo M;
	int bonus;
	public Animator[] anim;
	public Transform[] posicion;
	Acciones action;
	public Text[] textos;

	void Start () {

		foreach (Slider S in turnos) {
			S.value = 1;
		}
		action = GameObject.Find ("Actions").GetComponent<Acciones> ();
		action.Hide ();//Esconder los botones
		guerreros = new Guerrero[] {
			new Guerrero (100, 20, 20, 20, 20, anim [0], salud [0], turnos [0], posicion [0].position, 10,"Caballero",textos[0]),
			//Provisional: Arquero Original 10 seg, por ahora 2 tiempoturno Mago 15
			new Guerrero (80, 20, 20, 20, 2, anim [1], salud [1], turnos [1], posicion [1].position, 10,"Arquero",textos[1]),
			new Guerrero (90, 20, 20, 20, 5, anim [2], salud [2], turnos [2], posicion [2].position, 10,"Mago",textos[2])
		};
		//Le pasa el nivel segun el nivel, el monstruo
		M= new Monstruo (600,10,40,new Animator(),salud[3],turnos[3],posicion[3].position,textos[3],guerreros[0],guerreros[1],guerreros[2]);
		CurrenState = EstadosDeBatalla.ESPERANDO;


		M.HideText ();
		foreach (Guerrero gu in guerreros) {
			gu.HideText ();
		}
	}
	
	// Update is called once per frame
	void Update () {

		foreach (Guerrero gu in guerreros) {
			if (gu.text.color.a != 0) {
				gu.text.transform.Translate(Vector3.up * Time.deltaTime);
			}
		}

		if (M.text.color.a != 0) {
			if (M.text.color.a != 0) {
				M.text.transform.Translate(Vector3.up * Time.deltaTime);
			}
		}

		switch (CurrenState) {

		case (EstadosDeBatalla.ESPERANDO):
			foreach (Guerrero gu in guerreros) {
				gu.Sturno.value -= Time.deltaTime / gu.tiempTurno;
				//Debug.Log(gu.clase + " turno: "+gu.Sturno.value.ToString());
				if (gu.Sturno.value.Equals (0)) {
					Escogiendo (gu);
					return;
				}
			}
			M.Sturno.value -= Time.deltaTime / M.tiempTurno;
			//Debug.Log("Monstruo: "+M.Sturno.value.ToString());
			if (M.Sturno.value.Equals (0)) {
				CurrenState = EstadosDeBatalla.TURNOMONSTRUO;
			}
			break;

		case (EstadosDeBatalla.TURNOMONSTRUO):
			Debug.Log ("El monstruo artaca");
			M.Sturno.value = 1;
			CurrenState = EstadosDeBatalla.ESPERANDO;
			break;

		case (EstadosDeBatalla.ESCOGIENDO):
			break;

		case (EstadosDeBatalla.EFECTO):
			Efecto (action.bonus, action.CurrenAction,action.CurrentGuerrero);
			CurrenState = EstadosDeBatalla.ESPERANDO;
			break;

		case (EstadosDeBatalla.WIN):
			break;

		case (EstadosDeBatalla.LOSE):
			break;
		}

	}

	void Escogiendo(Guerrero gu)
	{
		action.CurrentGuerrero = gu;
		CurrenState = EstadosDeBatalla.ESCOGIENDO;
		action.Show ();
		if (gu.salud.Equals (gu.saludMax)) {
			action.buttons [2].interactable = false; //No se puede curar con vida maxima
			action.buttons [2].GetComponentInChildren<Text> ().text = "Salud Máxima";
		} 
		else {
			action.buttons [2].GetComponentInChildren<Text> ().text = "Curar";
		}

		if ((gu.defendiendo) || (gu.defensagrupal)) {
			action.buttons [1].interactable = false; //Ya se está protegiendo
			action.buttons [1].GetComponentInChildren<Text> ().text = "Defendiendo";
		} 
		else {
			action.buttons [1].GetComponentInChildren<Text> ().text = "Defenderse";
		}

		if (gu.carga < 3)
			action.buttons [3].GetComponentInChildren<Text> ().text = "Cargar Ataque Especial";
		else
			action.buttons [3].GetComponentInChildren<Text> ().text = "Ataque Especial";
	}

	public void Efecto(int bonus,string action,Guerrero g)
	{
		StartCoroutine (StopTexto (g));
		g.Sturno.value = 1;
		Debug.Log (" La accion es "+action);
		switch (action) {

		case "atk":
			Debug.Log ("Ha intentado atacar");
			M.RecibirDano (g.atk + bonus);
			//Animacion guerrero
			break;

		case "def":
			g.Defender (bonus);
			StartCoroutine (StopDefensa(g));
			break;

		case "curation":
			g.Curar (bonus);
			break;

		default:
			break;
		}
	}



	IEnumerator StopDefensa(Guerrero g)
				{
		yield return new WaitForSeconds (200);
					if (!g.defensagrupal) {
						g.defendiendo = false;
			g.defensa -= g.aumentoDefBonus;
						g.aumentoDefBonus = 0;
			g.defendiendo = false;
						//Detener la animacion
					}
				}

	IEnumerator StopDefensaGrupal(Guerrero g)
				{
		yield return new WaitForSeconds (250);
					g.defensagrupal = false;
					g.defendiendo = false;
					g.defensa -= g.aumentoDefBonus;
					g.aumentoDefBonus = 0;
				}
	IEnumerator  StopReduccionTiempoTurno(Guerrero g)
				{
		yield return new WaitForSeconds (120);
					g.tiempTurno += g.ReduccTiempTurno;
				}


	IEnumerator StopRalentizar()
	{
		yield return new WaitForSeconds (60);
		M.tiempTurno -= M.ralentizar;
	}

	IEnumerator StopTexto(Guerrero g)
	{ 
		yield return new WaitForSeconds (1);
		g.HideText ();
	}
}
