using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {
	
		public enum EstadosDeBatalla
		{
		BLOQUE1,
		BLOQUE2,
		BLOQUE3,
		BLOQUE4,
		BLOQUE5,
		BLOQUE6,
		BLOQUE7,
		BLOQUE8,
		BLOQUE9,
		ESPERANDO,
		ESCOGIENDO,
		EFECTO,
		CARGA,
		}
		// Use this for initialization

		public static EstadosDeBatalla CurrenState;
		public static int Level;
		Guerrero[] guerreros;
		public Slider[] salud,turnos;
		int bonus;
		public Animator[] anim;
		public Transform[] posicion;
		Acciones action;
		public Text[] textos;
		public Image[] artifacts;
		public ParticleSystem[] particulas;
		public Animator[] heridas;
		public AudioClip[] Sonidos;
		private AudioSource A;
		void Start () {
		A = this.gameObject.GetComponent<AudioSource> ();
		Acciones.tutorial = true;
			//La escen anterior le cambiará el valor a level provisional
			Level=1;
			foreach (Slider S in turnos) {
				S.value = 1;
				S.interactable = false;
			}
			foreach (Slider S in salud) {
				S.interactable = false;
			}

			action = GameObject.Find ("Actions").GetComponent<Acciones> ();
			action.Hide ();//Esconder los botones
			guerreros = new Guerrero[] {
			new Guerrero (100, 20, 20, 20, 10, anim [0], salud [0], turnos [0], posicion [0].position, 10, "Caballero", textos [0], artifacts [0],particulas[0],particulas[1],particulas[2],particulas[3],heridas[0],new SpriteRenderer()),
				//Provisional: Arquero Original 10 seg, por ahora 2 tiempoturno Mago 15
			new Guerrero (80, 20, 20, 20, 3, anim [1], salud [1], turnos [1], posicion [1].position, 10, "Arquero", textos [1], artifacts [1],particulas[4],particulas[5],particulas[6],particulas[7],heridas[1],new SpriteRenderer()),
			new Guerrero (90, 20, 20, 20, 5, anim [2], salud [2], turnos [2], posicion [2].position, 10, "Mago", textos [2], artifacts [2],particulas[8],particulas[9],particulas[10],particulas[11],heridas[2],new SpriteRenderer())
			};
			//Le pasa el nivel segun el nivel, el monstruo Original 100 provisional 3
		CurrenState = EstadosDeBatalla.BLOQUE1;


			foreach (Guerrero gu in guerreros) {
				gu.HideText ();
			}
			//Herir
		guerreros[2].salud=5;
		guerreros[2].SSsalud.value=guerreros[2].salud;
		}

		// Update is called once per frame
		void Update () {

			foreach (Guerrero gu in guerreros) {
				if (gu.text.color.a != 0) {
					gu.text.transform.Translate(Vector3.up * Time.deltaTime);
				}
			}

			switch (CurrenState) {

		case (EstadosDeBatalla.BLOQUE1):
			break;

		case (EstadosDeBatalla.BLOQUE2):
			break;

		case (EstadosDeBatalla.BLOQUE3):
			break;
		case (EstadosDeBatalla.BLOQUE7):
			break;

			case (EstadosDeBatalla.ESPERANDO):
				foreach (Guerrero gu in guerreros) {
					gu.Sturno.value -= Time.deltaTime / gu.tiempTurno;
					//Debug.Log(gu.clase + " turno: "+gu.Sturno.value.ToString());
					if (gu.Sturno.value.Equals (0)) {
					if (gu.clase.Equals ("Arquero")) {
						Fungus.Flowchart.BroadcastFungusMessage ("Bloque2");
						CurrenState = EstadosDeBatalla.BLOQUE2;
						Escogiendo (gu);
					}
					if (gu.clase.Equals ("Mago")) {
						Fungus.Flowchart.BroadcastFungusMessage ("Bloque4");
						CurrenState = EstadosDeBatalla.BLOQUE2;
						Escogiendo (gu);
					}
						return;
					}
				}
				break;



			case (EstadosDeBatalla.ESCOGIENDO):
				break;

			case (EstadosDeBatalla.EFECTO):
				Efecto (action.bonus, action.CurrenAction,action.CurrentGuerrero);
				CurrenState = EstadosDeBatalla.ESCOGIENDO;
				break;


			case(EstadosDeBatalla.CARGA):
			A.PlayOneShot (Sonidos [0]);
				action.CurrentGuerrero.artifact.GetComponent<Animator> ().SetBool ("Escogiendo", false);
				action.CurrentGuerrero.Sturno.value = 1;
				StartCoroutine(StopTexto(action.CurrentGuerrero));
				if (action.CurrentGuerrero.descansando)
					action.CurrentGuerrero.descansando = false;
			CurrenState = EstadosDeBatalla.BLOQUE3;
			Fungus.Flowchart.BroadcastFungusMessage ("Bloque3");
				break;
			}

		}


	void HideTutorial()
	{
		foreach (Button b in action.buttons) {
			b.interactable = false;
		}
	}

		void Escogiendo(Guerrero gu)
		{
			gu.artifact.GetComponent<Animator> ().SetBool ("Escogiendo", true);
			gu.artifact.overrideSprite = null;
			action.CurrentGuerrero = gu;
			CurrenState = EstadosDeBatalla.ESCOGIENDO;
			action.Show ();
			if (gu.salud.Equals (gu.saludMax)) {
				action.buttons [2].interactable = false; //No se puede curar con vida maxima
				action.buttons [2].GetComponentInChildren<Text> ().text = "Salud Máxima";
			} 
			else {
				//action.buttons [2].interactable = true;
				action.buttons [2].GetComponentInChildren<Text> ().text = "Curar";
			}

			if ((gu.defendiendo) || (gu.defensagrupal)) {
				action.buttons [1].interactable = false; //Ya se está protegiendo
				action.buttons [1].GetComponentInChildren<Text> ().text = "Defendiendo";
			} 
			else {
				action.buttons [1].GetComponentInChildren<Text> ().text = "Defenderse";
			}

			if (gu.descansando) {
				action.buttons [3].GetComponentInChildren<Text> ().text = "Recuperando Fuerzas"; //Tras ataque special un turno de recuperacion
				action.buttons [3].interactable = false;
			} 

			else {
				action.buttons [3].interactable = true;
				if (gu.carga < 3)
					action.buttons [3].GetComponentInChildren<Text> ().text = "Cargar Ataque Especial";
				else
					action.buttons [3].GetComponentInChildren<Text> ().text = "Ataque Especial";
			}
		HideTutorial ();
		}

		public void Efecto(int bonus,string action,Guerrero g)
		{		
			g.artifact.GetComponent<Animator> ().SetBool ("Escogiendo", false);
			g.Sturno.value = 1;
			Debug.Log (" La accion es "+action);
			switch (action) {

			case "def":
				//Mostrar Objeto
				StartCoroutine (ShowArtifact (g));
				g.defender.gameObject.SetActive (true);
				StartCoroutine (StopTexto (g));
				g.Defender (bonus);
				StartCoroutine (StopDefensa(g));
				if (g.descansando)
					g.descansando = false;
				break;

		case "curation":
				//Mostrar Objeto
			A.PlayOneShot (Sonidos [1]);
			StartCoroutine (ShowArtifact (g));

			StartCoroutine (StopTexto (g));
			g.SSsalud.value = g.saludMax;
				if (g.descansando)
					g.descansando = false;
				break;


			case "SpecialAtkAumentoDaño":
				//Mostrar Objeto
				StartCoroutine(ShowArtifact(g));

				//animacion special
				foreach( Guerrero gu in guerreros)
				{
					gu.AumentoAtaque (bonus);
					StartCoroutine (StopAtaque (gu));
					StartCoroutine (StopTexto (gu));
				}
				break;
			case "CuracionGrupal":
				//Mostrar Objeto
				StartCoroutine(ShowArtifact(g));

				//animacion special
				foreach( Guerrero gu in guerreros)
				{
					gu.Curar (g.curacion,bonus);
					StartCoroutine (StopTexto (gu));
				}
				break;

			case "ProteccionGrupal":
				//Mostrar Objeto
				StartCoroutine(ShowArtifact(g));
				//animacion special
				foreach( Guerrero gu in guerreros)
				{
					gu.Defender(bonus);
					gu.defensagrupal = true;
					StartCoroutine (StopTexto (gu));
					StartCoroutine (StopDefensaGrupal(gu));
				}
				break;
			case "ReducciónTurnoGrupal":
				//Mostrar Objeto
				StartCoroutine(ShowArtifact(g));

				//animacion special
				foreach( Guerrero gu in guerreros)
				{
					gu.ReduccionTiempoTurno (bonus);
					StartCoroutine (StopTexto (gu));
				}
				break;

			default:
				break;
			}
		}


		IEnumerator ShowArtifact(Guerrero g)
		{
			yield return new WaitForSeconds (1);
			//Mostrar Objeto
			Debug.Log ("Mostarar Objeto");
			g.artifact.overrideSprite = Acciones.CurrentObject.sprite;
			g.artifact.GetComponent<Animator> ().SetTrigger("Show");
			//CurrenState = EstadosDeBatalla.ESPERANDO;
		}


		IEnumerator StopDefensa(Guerrero g)
		{
			yield return new WaitForSeconds (200);
			if (!g.defensagrupal) {
				g.defender.gameObject.SetActive (false);
				g.defendiendo = false;
				g.defensa -= g.aumentoDefBonus;
				g.aumentoDefBonus = 0;
				g.defendiendo = false;
				//Detener la animacion
			}
		}

		IEnumerator StopAtaque(Guerrero g)
		{
			yield return new WaitForSeconds (100);
			g.augmAtaque.gameObject.SetActive (false);
			g.atk -= g.aumentoAtk;
		}


		IEnumerator StopDefensaGrupal(Guerrero g)
		{
			yield return new WaitForSeconds (250);
			g.defender.gameObject.SetActive (false);
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



		IEnumerator StopTexto(Guerrero g)
		{ 
			yield return new WaitForSeconds (1);
			g.HideText ();
			g.cargar.gameObject.SetActive(false);
			yield return new WaitForSeconds (1);
			g.curar.gameObject.SetActive(false);
		}



	public void FromB1ToB2()
	{
		CurrenState = EstadosDeBatalla.ESPERANDO;
	}

	public void FromB2ToB3()
	{
		action.buttons [3].interactable = true;
	}
	public void FromB3toB4()
	{
		CurrenState = EstadosDeBatalla.ESPERANDO;
	}
	public void FromB4toB5()
	{
		//CurrenState = EstadosDeBatalla.ESPERANDO;
		action.buttons [2].interactable = true;
	}
	public void FromB5toB6()
	{
		MouseToTouchTutorial.crafteando = true;
		CurrenState = EstadosDeBatalla.BLOQUE6;
	}
	public void FromB6toB7()
	{
		MouseToTouchTutorial.crafteando = true;
		CurrenState = EstadosDeBatalla.BLOQUE7;
	}
	public void FromB7toB8()
	{
		MouseToTouchTutorial.crafteando = true;
		CurrenState = EstadosDeBatalla.BLOQUE8;
	}
	public void FromB8toB9()
	{
		MouseToTouchTutorial.crafteando = true;
		CurrenState = EstadosDeBatalla.BLOQUE9;
	}
	}



