using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Acciones : MonoBehaviour {

	Craft craft;
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
	// Use this for initialization
	void Start () {
		textbonus = GameObject.Find ("CurrentObject").GetComponentInChildren<Text> ();
		CurrentObject = GameObject.Find ("CurrentObject").GetComponent<Image>();
		YImage = CurrentObject.transform.position.y;
		textbonus.color = new Color (textbonus.color.r, textbonus.color.g, textbonus.color.b, 0);
		CurrentObject.color = new Color (0, 0, 0, 0);
		XAction = buttons [0].transform.position.x;
		craft=GameObject.Find ("CreadorRecetas").GetComponent<Craft> ();
		atk = new Recipe[2] {new Recipe(2,0,2,0,0,6,PixelObjects[0],20,5),new Recipe(12,0,0,0,0,0,PixelObjects[1],20,5)};
		def = new Recipe[2] {new Recipe(2,0,2,0,0,6,PixelObjects[0],20,5),new Recipe(0,6,0,0,0,0,PixelObjects[0],20,5)};
		curation = new Recipe[2] {new Recipe(0,0,0,0,8,0,PixelObjects[0],20,5),new Recipe(0,0,0,0,15,0,PixelObjects[0],20,5)};
		spatkCa = new Recipe[2] {new Recipe(2,0,2,0,0,6,PixelObjects[0],20,5),new Recipe(12,0,0,0,0,0,PixelObjects[1],20,5)};
		spatkAr = new Recipe[2] {new Recipe(2,0,2,0,0,6,PixelObjects[0],20,5),new Recipe(12,0,0,4,0,0,PixelObjects[0],20,5)};
		spatkMa = new Recipe[2] {new Recipe(2,0,2,0,0,6,PixelObjects[0],20,5),new Recipe(12,0,0,0,0,0,PixelObjects[0],20,5)};
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
		craft.ShowRecipe (atk [Random.Range (0, atk.Length)],"atk");
		MouseToTouch.crafteando = true;
		Hide ();
	}

	public void defend ()
	{
		craft.ShowRecipe (def [Random.Range (0, def.Length)],"def");
		MouseToTouch.crafteando = true;
		Hide ();
	}

	public void heal()
	{
		craft.ShowRecipe (curation [Random.Range (0, curation.Length)],"curation");
		MouseToTouch.crafteando = true;
		Hide ();
	}

	public void specialattak ()
	{
		if (CurrentGuerrero.carga < 3) {
			CurrentGuerrero.Cargar ();
			BatallaManager.CurrenState = BatallaManager.EstadosDeBatalla.CARGA;
			Hide ();
		}
		else{
			MouseToTouch.crafteando = true;
			CurrentGuerrero.carga = 0;
			CurrentGuerrero.descansando = true;
		switch (CurrentGuerrero.clase) {

		case "Caballero":
			int i = Random.Range (0, 1);
			if(i.Equals(0))
				craft.ShowRecipe (spatkCa [0],"SpecialAtkDaño");
			else
				craft.ShowRecipe (spatkCa [1],"SpecialAtkAumentoDaño");
			break;


		case "Arquero":
			int e = Random.Range (0, 1);
			if(e.Equals(0))
				craft.ShowRecipe (spatkAr [0],"CuracionGrupal");
			else
				craft.ShowRecipe (spatkAr [1],"DormirMonstruo");
			break;

		case "Mago":
			int u = Random.Range (0, 1);
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
		desaparecer = false;
		CurrentObject.transform.position = new Vector3 (CurrentObject.transform.position.x, YImage - CurrentObject.transform.localScale.y, CurrentObject.transform.position.z);
		CurrentObject.color = new Color (1, 1, 1, 1);
	}
	IEnumerator HidePixelArt ()
	{
		yield return new WaitForSeconds (1);
		desaparecer = true;

	}

	public void Show ()
	{
		for (int i = 0; i < buttons.Length; i++) {
			buttons [i].gameObject.SetActive (true);
			buttons [i].transform.position = new Vector3 (buttons [i].transform.position.x - buttons [i].GetComponent<RectTransform>().rect.width-buttons [i].GetComponent<RectTransform>().rect.width*((float)(i+1)/4), buttons [i].transform.position.y, buttons [i].transform.position.z);
		}
		Debug.Log (buttons [0].gameObject.activeSelf.ToString ());
	}

	public void ObjetoHecho(bool bonus,string action, int b,Sprite PixelArt)
	{
		//Aquí realizo la acción si es un ataque
		CurrentObject.sprite=PixelArt;
		ShowPixelArt ();
		if (bonus) {
			this.bonus = b;
			textbonus.text = "BONUS";
			textbonus.color = new Color (textbonus.color.r, textbonus.color.g, textbonus.color.b, 1);
		} 
		else {
			this.bonus = 0;
			textbonus.text = " ";
		}
		CurrenAction = action;

		craft.Refresh ();
		MouseToTouch.crafteando = false;
		BatallaManager.CurrenState = BatallaManager.EstadosDeBatalla.EFECTO;
		//Provisional llamar a efecto()
		//BatallaManager.CurrenState = BatallaManager.EstadosDeBatalla.ESPERANDO;
		StartCoroutine(HidePixelArt());

	
	}
}
