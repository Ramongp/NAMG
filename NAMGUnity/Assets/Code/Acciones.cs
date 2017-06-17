using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Acciones : MonoBehaviour {
	public static bool tutorial;
	Craft craft;
	CraftTutorial craftTutorial;
	Recipe[] atk,def,curation,spatkCa,spatkAr,spatkMa;
	public Button[] buttons;
	public string CurrenAction;
	public Guerrero CurrentGuerrero;
	public int bonus;
	public Sprite[] PixelObjects;
	public static Image CurrentObject;
	bool desaparecer;
	float YImage;
	float XAction;
	Text textbonus;
	private AudioClip sonido;
	private AudioSource A;
	// Use this for initialization
	void Start () {
		//Sonido
		sonido = Resources.Load<AudioClip> ("Sounds/Correct");
		A=this.gameObject.GetComponent<AudioSource> ();
		textbonus = GameObject.Find ("CurrentObject").GetComponentInChildren<Text> ();
		CurrentObject = GameObject.Find ("CurrentObject").GetComponent<Image>();
		YImage = CurrentObject.transform.position.y;
		textbonus.color = new Color (textbonus.color.r, textbonus.color.g, textbonus.color.b, 0);
		CurrentObject.color = new Color (0, 0, 0, 0);
		XAction = buttons [0].transform.position.x;
		if (tutorial) {
			craftTutorial = GameObject.Find ("CreadorRecetas").GetComponent<CraftTutorial> ();
			curation = new Recipe[1] {new Recipe(30,4,0,0,4,0,PixelObjects[2],20,20)};
		}
		else{
			craft = GameObject.Find ("CreadorRecetas").GetComponent<Craft> ();
			atk = 	new Recipe[6] {new Recipe(2,0,2,0,0,6,PixelObjects[0],20,5),new Recipe(12,0,0,0,0,0,PixelObjects[1],20,5),
					new Recipe(0,32,0,0,0,0,PixelObjects[2],20,5),new Recipe(10,0,0,0,0,0,PixelObjects[3],20,5),
					new Recipe(10,8,0,0,0,0,PixelObjects[4],20,5),new Recipe(12,12,0,12,0,0,PixelObjects[5],20,5)};
			
			def =	new Recipe[6] {new Recipe(0,24,8,0,0,0,PixelObjects[6],20,5),new Recipe(30,0,15,0,0,0,PixelObjects[7],20,5),
					new Recipe(0,14,10,0,0,0,PixelObjects[8],20,5),new Recipe(0,0,27,0,0,0,PixelObjects[9],20,5),
					new Recipe(0,0,56,0,0,0,PixelObjects[10],20,5),new Recipe(8,42,0,0,0,0,PixelObjects[11],20,5)};
			
			curation = new Recipe[6] {new Recipe(30,4,0,0,4,0,PixelObjects[12],20,5),new Recipe(0,0,32,0,16,0,PixelObjects[13],20,5),
					new Recipe(4,0,0,0,8,0,PixelObjects[14],20,5),new Recipe(0,0,25,0,0,0,PixelObjects[15],20,5),
					new Recipe(0,0,0,0,0,42,PixelObjects[16],20,5),new Recipe(48,0,0,0,0,0,PixelObjects[17],20,5)};
			
			spatkCa = new Recipe[2] {new Recipe(20,8,0,12,0,0,PixelObjects[18],20,5),new Recipe(24,0,30,0,0,0,PixelObjects[19],20,5)};
			spatkAr = new Recipe[2] {new Recipe(0,6,0,0,24,0,PixelObjects[20],20,5),new Recipe(0,0,0,30,6,0,PixelObjects[21],20,5)};
			spatkMa = new Recipe[2] {new Recipe(0,0,18,0,0,36,PixelObjects[22],20,5),new Recipe(0,0,0,0,18,9,PixelObjects[23],20,5)};
		}

	}
	
	// Update is called once per frame
	void Update () {
		if ((CurrentObject.color.a != 0) || (desaparecer)) {
			CurrentObject.transform.Translate (Vector2.up * Time.deltaTime * 100);
			if (desaparecer) {
				float alpha = CurrentObject.color.a;
				CurrentObject.color = new Color (1, 1, 1, alpha - Time.deltaTime * 10);
				textbonus.color = new Color (textbonus.color.r, textbonus.color.g, textbonus.color.b, alpha - Time.deltaTime * 10);
			}
			if (CurrentObject.color.a < 0) {
				desaparecer = false;
				CurrentObject.color = new Color (0, 0, 0, 0);
			}
		
		}
		if (buttons[0].gameObject.activeSelf.Equals(true)) {
			foreach (Button b in buttons) {
				if (b.transform.position.x < XAction) {
					b.gameObject.transform.Translate (Vector3.right * Time.deltaTime * 1000);
				} else {
					b.gameObject.transform.position = new Vector3 (XAction, b.gameObject.transform.position.y, b.gameObject.transform.position.z);
				}
			}
		}
	}


	public void attack ()
	{
		A.PlayOneShot (sonido);
		craft.ShowRecipe (atk [Random.Range (0, atk.Length)],"atk");
		MouseToTouch.crafteando = true;
		Hide ();
	}

	public void defend ()
	{
		A.PlayOneShot (sonido);
		craft.ShowRecipe (def [Random.Range (0, def.Length)],"def");
		MouseToTouch.crafteando = true;
		Hide ();
	}

	public void heal()
	{
		A.PlayOneShot (sonido);
		if (tutorial) {
			craftTutorial.ShowRecipe (curation [0], "curation");
			Fungus.Flowchart.BroadcastFungusMessage ("Bloque5");
			Hide ();
			TutorialManager.CurrenState = TutorialManager.EstadosDeBatalla.BLOQUE5;
		} 
		else {
			craft.ShowRecipe (curation [Random.Range (0, curation.Length)], "curation");
			MouseToTouch.crafteando = true;
			Hide ();
		}
	}

	public void specialattak ()
	{
		A.PlayOneShot (sonido);
		if (CurrentGuerrero.carga < 3) {
			Hide ();
			CurrentGuerrero.Cargar ();
			BatallaManager.CurrenState = BatallaManager.EstadosDeBatalla.CARGA;
			TutorialManager.CurrenState = TutorialManager.EstadosDeBatalla.CARGA;
		}
		else{
			Hide ();
			MouseToTouch.crafteando = true;
			CurrentGuerrero.carga = 0;
			CurrentGuerrero.descansando = true;
		switch (CurrentGuerrero.clase) {

		case "Caballero":
			int i = Random.Range (0, 2);
			if(i.Equals(0))
				craft.ShowRecipe (spatkCa [0],"SpecialAtkDaño");
			else
				craft.ShowRecipe (spatkCa [1],"SpecialAtkAumentoDaño");
			break;


		case "Arquero":
			int e = Random.Range (0, 2);
			if(e.Equals(0))
				craft.ShowRecipe (spatkAr [0],"CuracionGrupal");
			else
				craft.ShowRecipe (spatkAr [1],"DormirMonstruo");
			break;

		case "Mago":
			int u = Random.Range (0, 2);
			if(u.Equals(0))
				craft.ShowRecipe (spatkMa [0],"ProteccionGrupal");
			else
				craft.ShowRecipe (spatkMa [1],"ReducciónTurnoGrupal");
			break;
		}
		//envir el gerrero del turno
		//craft.ShowRecipe (specialattak [Random.Range (0, specialattak.spatkAr)]);
		//MouseToTouch.crafteando = true;
		//Hide ();
		}
	}

	public void Hide ()
	{
		foreach (Button b in buttons) {
			b.gameObject.SetActive (false);
			b.interactable = true;
		}

	}


	public void ShowPixelArt()
	{
		StartCoroutine (ShowPixelArtAfterSecond());
	}

	IEnumerator ShowPixelArtAfterSecond()
	{
		yield return new WaitForSeconds (1);
		textbonus.color = new Color (textbonus.color.r, textbonus.color.g, textbonus.color.b, 1);
		desaparecer = false;
		CurrentObject.transform.position = new Vector3 (CurrentObject.transform.position.x, YImage - CurrentObject.transform.localScale.y, CurrentObject.transform.position.z);
		CurrentObject.color = new Color (1, 1, 1, 1);
	}
	IEnumerator HidePixelArt ()
	{
		yield return new WaitForSeconds (2);
		desaparecer = true;

	}

	public void Show ()
	{
		for (int i = 0; i < buttons.Length; i++) {
			buttons [i].gameObject.SetActive (true);
			buttons [i].transform.position = new Vector3 (buttons [i].transform.position.x - buttons [i].GetComponent<RectTransform>().rect.width-buttons [i].GetComponent<RectTransform>().rect.width*((float)(i+1)/4), buttons [i].transform.position.y, buttons [i].transform.position.z);
		}
		//Debug.Log (buttons [0].gameObject.activeSelf.ToString ());
	}

	public void ObjetoHecho(bool bonus,string action, int b,Sprite PixelArt)
	{
		//Sonido
		AudioClip correct = Resources.Load ("Sounds/ObjectMade") as AudioClip;
		AudioSource A = GameObject.Find ("SoundManager").GetComponent <AudioSource>();
		A.PlayOneShot(correct);

		//Aquí realizo la acción si es un ataque
		CurrentObject.sprite=PixelArt;
		ShowPixelArt ();
		if (bonus) {
			this.bonus = b;
			textbonus.text = "BONUS";
		} 
		else {
			this.bonus = 0;
			textbonus.text = " ";
		}
		CurrenAction = action;
		if (tutorial) {
			craftTutorial.Refresh ();
			MouseToTouchTutorial.crafteando = false;
			TutorialManager.CurrenState = TutorialManager.EstadosDeBatalla.EFECTO;
		} else {
			craft.Refresh ();
			MouseToTouch.crafteando = false;
			BatallaManager.CurrenState = BatallaManager.EstadosDeBatalla.EFECTO;
		}

		//Provisional llamar a efecto()
		//BatallaManager.CurrenState = BatallaManager.EstadosDeBatalla.ESPERANDO;
		StartCoroutine(HidePixelArt());

	
	}
}
