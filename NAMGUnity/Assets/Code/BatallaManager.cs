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
		CARGA,
		WIN
	}
	// Use this for initialization

	public static EstadosDeBatalla CurrenState;
	public static int Level;
	Guerrero[] guerreros;
	public Slider[] salud,turnos;
	Monstruo M;
	int bonus;
	public Animator[] anim;
	public Transform[] posicion;
	Acciones action;
	public Text[] textos;
	public Image[] artifacts;
	public ParticleSystem[] particulas;
	public Animator[] heridas;
	public Animator Monstruo;
	public SpriteRenderer[] Portraits;
	public AudioClip[] Sonidos;
	private AudioSource A;
	bool firework;

	void Start () {
		A = this.gameObject.GetComponent<AudioSource> ();
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
			new Guerrero (130, 10, 30, 5, 7, anim [0], salud [0], turnos [0], posicion [0].position, 2, "Caballero", textos [0], artifacts [0],particulas[0],particulas[1],particulas[2],particulas[3],heridas[0],Portraits[0]),
			//Provisional: Arquero Original 10 seg, por ahora 2 tiempoturno Mago 15
			new Guerrero (70, 15, 20, 0, 3, anim [1], salud [1], turnos [1], posicion [1].position, 2, "Arquero", textos [1], artifacts [1],particulas[4],particulas[5],particulas[6],particulas[7],heridas[1],Portraits[1]),
			new Guerrero (90, 10, 20, 3, 5, anim [2], salud [2], turnos [2], posicion [2].position, 2, "Mago", textos [2], artifacts [2],particulas[8],particulas[9],particulas[10],particulas[11],heridas[2],Portraits[2])
		};
		//Le pasa el nivel segun el nivel, el monstruo Original 100 provisional 3
		M= new Monstruo (200,20,8,new Animator(),salud[3],turnos[3],posicion[3].position,heridas[3],particulas[12],textos[3],guerreros[0],guerreros[1],guerreros[2],Portraits[3]);
		CurrenState = EstadosDeBatalla.ESPERANDO;


		M.HideText ();
		foreach (Guerrero gu in guerreros) {
			gu.HideText ();
		}

		switch (Level) {
		case 1:
			Monstruo.SetBool ("Level1", true);
			break;
		case 2:
			Monstruo.SetBool ("Level1", false);
			Monstruo.SetBool ("Level2", true);
			break;
		case 3:
			Monstruo.SetBool ("Level2", false);
			Monstruo.SetBool ("Level3", true);
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (M.charging) {
			M.charging = false;
			if (M.carga < 4) {
				A.PlayOneShot (Sonidos [0]);
				if (M.carga.Equals (3))
					particulas [13].gameObject.SetActive (true);
			}
			if (M.carga.Equals (4)) {
				particulas [13].gameObject.SetActive (false);
				particulas [14].Play();
				StartCoroutine (StopMonsterAttack ());
				M.carga = 0;
			}
		}
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
				if ((gu.Sturno.value.Equals (0))&&(gu.alive)) {
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
			//Debug.Log ("El monstruo ataca");
			StartCoroutine (StopTextoMonstruo ());
			foreach (Guerrero gu in guerreros) {
				StartCoroutine (StopTexto (gu));
			}
			M.Jugada ();
			M.Sturno.value = 1;
			Lose ();
			CurrenState = EstadosDeBatalla.ESPERANDO;
			break;

		case (EstadosDeBatalla.ESCOGIENDO):
			break;

		case (EstadosDeBatalla.EFECTO):
			Efecto (action.bonus, action.CurrenAction,action.CurrentGuerrero);
			CurrenState = EstadosDeBatalla.ESCOGIENDO;
			break;

		case (EstadosDeBatalla.WIN):
			if (!firework) {
				firework = true;
				StartCoroutine (FireworkSound ());
			}
			break;

		case (EstadosDeBatalla.LOSE):
			break;

		case(EstadosDeBatalla.CARGA):
			A.PlayOneShot (Sonidos [0]);
			action.CurrentGuerrero.artifact.GetComponent<Animator> ().SetBool ("Escogiendo", false);
			action.CurrentGuerrero.Sturno.value = 1;
			StartCoroutine(StopTexto(action.CurrentGuerrero));
			if (action.CurrentGuerrero.descansando)
				action.CurrentGuerrero.descansando = false;
			CurrenState = EstadosDeBatalla.ESPERANDO;
			break;
		}

	}

	void Escogiendo(Guerrero gu)
	{
		gu.artifact.GetComponent<Animator> ().SetBool ("Escogiendo", true);
		gu.artifact.overrideSprite = null;
		action.CurrentGuerrero = gu;
		CurrenState = EstadosDeBatalla.ESCOGIENDO;
		action.Show ();
		if (gu.SSsalud.value.Equals (gu.saludMax)) {
			action.buttons [2].interactable = false; //No se puede curar con vida maxima
			action.buttons [2].GetComponentInChildren<Text> ().text = "Salud Máxima";
		} 
		else {
			action.buttons [2].interactable = true;
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
	}
		
	public void Efecto(int bonus,string action,Guerrero g)
	{		
		g.artifact.GetComponent<Animator> ().SetBool ("Escogiendo", false);
		g.Sturno.value = 1;
		//Debug.Log (" La accion es "+action);
		switch (action) {

		case "atk":

			//Mostrar Objeto
			StartCoroutine (AttackArtifact (g));
			//Mostrar Efecto
			StartCoroutine (StopTextoMonstruo ());
			//Debug.Log ("Ha intentado atacar");
			M.RecibirDano (g.atk + bonus);
			if (M.salud < 1) {
				StartCoroutine(	Win ());
			}
			if (g.descansando)
				g.descansando = false;
			//Animacion guerrero
			break;

		case "def":
			//Mostrar Objeto
			A.PlayOneShot (Sonidos [4]);
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
				StartCoroutine(ShowArtifact(g));
			A.PlayOneShot (Sonidos [1]);
			StartCoroutine (StopTexto (g));
			g.Curar (bonus);
			if (g.descansando)
				g.descansando = false;
			break;


		case "SpecialAtkDaño":
			//Mostrar Objeto
					StartCoroutine(ShowArtifact(g));
			A.PlayOneShot (Sonidos [2]);

			//Animacion special
			M.RecibirDano (g.atk + bonus);
			if (M.salud < 1) {
				StartCoroutine(	Win ());
			}
			break;
		case "SpecialAtkAumentoDaño":
			//Mostrar Objeto
			StartCoroutine(ShowArtifact(g));
			A.PlayOneShot (Sonidos [0]);
			//animacion special
			foreach( Guerrero gu in guerreros)
			{
				if (gu.alive) {
					gu.AumentoAtaque (bonus);
					StartCoroutine (StopAtaque (gu));
					StartCoroutine (StopTexto (gu));
				}
			}
			break;
		case "CuracionGrupal":
			//Mostrar Objeto
			StartCoroutine(ShowArtifact(g));
			A.PlayOneShot (Sonidos [1]);	
			//animacion special
			foreach( Guerrero gu in guerreros)
			{
				if (!gu.salud.Equals (gu.saludMax)) {
					gu.Curar (g.curacion, bonus);
					StartCoroutine (StopTexto (gu));
				}
			}
			break;
		case "DormirMonstruo":
			//Mostrar Objeto
			StartCoroutine(ShowArtifact(g));
			A.PlayOneShot (Sonidos [3]);
			//animacion special
			M.Ralentizar (bonus);
			StartCoroutine (StopRalentizar ());
			StartCoroutine (StopTextoMonstruo ());
			break;
		case "ProteccionGrupal":
			//Mostrar Objeto
			StartCoroutine(ShowArtifact(g));
			//animacion special
			foreach( Guerrero gu in guerreros)
			{ 
				if (gu.alive) {
					A.PlayOneShot (Sonidos [4]);
					gu.defender.gameObject.SetActive (true);
					gu.Defender (bonus);
					gu.defensagrupal = true;
					StartCoroutine (StopTexto (gu));
					StartCoroutine (StopDefensaGrupal (gu));
				}
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
		//Debug.Log ("Mostarar Objeto");
		g.artifact.overrideSprite = Acciones.CurrentObject.sprite;
		g.artifact.GetComponent<Animator> ().SetTrigger("Show");
		CurrenState = EstadosDeBatalla.ESPERANDO;
	}

	IEnumerator AttackArtifact(Guerrero g)
	{
		yield return new WaitForSeconds (2);
		//Mostrar Objeto
		A.PlayOneShot (Sonidos [2]);
		g.artifact.overrideSprite = Acciones.CurrentObject.sprite;
		g.artifact.GetComponent<Animator> ().SetTrigger("Attack");
		M.heride.SetTrigger ("Heride");
		CurrenState = EstadosDeBatalla.ESPERANDO;
	}

	IEnumerator StopDefensa(Guerrero g)
				{
		yield return new WaitForSeconds (15);
					if (!g.defensagrupal) {
			g.defender.gameObject.SetActive (false);
						g.defendiendo = false;
			g.defensa -= g.aumentoDefBonus+g.aumentoDef;
						g.aumentoDefBonus = 0;
			g.defendiendo = false;
						//Detener la animacion
					}
				}
		
	IEnumerator StopAtaque(Guerrero g)
		{
		yield return new WaitForSeconds (15);
		g.augmAtaque.gameObject.SetActive (false);
		g.atk -= g.aumentoAtk;
		}


	IEnumerator StopDefensaGrupal(Guerrero g)
				{
		yield return new WaitForSeconds (20);
		g.defender.gameObject.SetActive (false);
					g.defensagrupal = false;
					g.defendiendo = false;
					g.defensa -= g.aumentoDefBonus;
					g.aumentoDefBonus = 0;
				}
	IEnumerator  StopReduccionTiempoTurno(Guerrero g)
				{
		yield return new WaitForSeconds (15);
					g.tiempTurno += g.ReduccTiempTurno;
				}


	IEnumerator StopRalentizar()
	{
		yield return new WaitForSeconds (10);
		M.tiempTurno -= M.ralentizar;
	}

	IEnumerator StopTexto(Guerrero g)
	{ 
		yield return new WaitForSeconds (1);
		g.HideText ();
		g.cargar.gameObject.SetActive(false);
		yield return new WaitForSeconds (1);
		g.curar.gameObject.SetActive(false);
	}

	IEnumerator StopTextoMonstruo()
	{ 
		yield return new WaitForSeconds (1);
		M.HideText ();
		M.cargar.gameObject.SetActive (false);
	}
	void Lose()
	{
		bool lost = true;
		foreach (Guerrero gu in guerreros) {
			if (gu.salud > 0)
				lost = false;
		}
		if (lost) {
			CurrenState = EstadosDeBatalla.ESCOGIENDO;
			Fungus.Flowchart.BroadcastFungusMessage ("Lose");
		}
	}

	IEnumerator RecargarEscena()
	{
		for (int i = 1; i < 12; i++) {
			GameObject.Find ("Franja " + i.ToString ()).GetComponent<Animator> ().SetTrigger ("Franja");
		}
		yield return new WaitForSeconds (1.5f);
		Application.LoadLevel("Prueba");
	}

	IEnumerator Win()
	{
		
		yield return new WaitForSeconds (2f);
		particulas [15].gameObject.SetActive (true);
		particulas [16].gameObject.SetActive (true);
		CurrenState = EstadosDeBatalla.WIN;
		yield return new WaitForSeconds (2f);
		CurrenState = EstadosDeBatalla.ESCOGIENDO;
		FromIntroToTutorial.level = Level + 1;
		for (int i = 1; i < 12; i++) {
			GameObject.Find ("Franja " + i.ToString ()).GetComponent<Animator> ().SetTrigger ("Franja");
		}
		yield return new WaitForSeconds (1.5f);
		Application.LoadLevel("Dialogue2");
	}

	void OnGUI(){
		if (CurrenState.Equals (EstadosDeBatalla.WIN)) {
			GUIStyle custom = new GUIStyle ();
			custom.fontSize = 100;
			custom.alignment = TextAnchor.MiddleCenter;
			GUI.Button (new Rect (0, 0, Screen.width, Screen.height), "Has Ganado",custom);
		}

	}
	IEnumerator StopMonsterAttack()
	{
		yield return new WaitForSeconds (0.5f);	
		particulas [14].Stop();
	}
	IEnumerator FireworkSound()
	{
		yield return new WaitForSeconds (Random.Range(0.2f,0.4f));	
		A.PlayOneShot (Sonidos [5]);
		firework = false;
	}
}
