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
	// Use this for initialization
	void Start () {
		craft=GameObject.Find ("CreadorRecetas").GetComponent<Craft> ();
		atk = new Recipe[2] {new Recipe(2,0,2,0,0,6,new Sprite(),20,5),new Recipe(12,0,0,0,0,0,new Sprite(),20,5)};
		def = new Recipe[2] {new Recipe(2,0,2,0,0,6,new Sprite(),20,5),new Recipe(0,6,0,0,0,0,new Sprite(),20,5)};
		curation = new Recipe[2] {new Recipe(0,0,0,0,8,0,new Sprite(),20,5),new Recipe(0,0,0,0,15,0,new Sprite(),20,5)};
		spatkCa = new Recipe[2] {new Recipe(2,0,2,0,0,6,new Sprite(),20,5),new Recipe(12,0,0,0,0,0,new Sprite(),20,5)};
		spatkAr = new Recipe[2] {new Recipe(2,0,2,0,0,6,new Sprite(),20,5),new Recipe(12,0,0,4,0,0,new Sprite(),20,5)};
		spatkMa = new Recipe[2] {new Recipe(2,0,2,0,0,6,new Sprite(),20,5),new Recipe(12,0,0,0,0,0,new Sprite(),20,5)};
	}
	
	// Update is called once per frame
	void Update () {
		
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

	public void Show ()
	{
		foreach (Button b in buttons) {
			b.gameObject.SetActive (true);
		}

	}

	public void ObjetoHecho(bool bonus,string action, int b)
	{
		//Aquí realizo la acción si es un ataque

		if (bonus)
			this.bonus = b;
		else
			this.bonus = 0;
		CurrenAction = action;

		craft.Refresh ();
		MouseToTouch.crafteando = false;
		BatallaManager.CurrenState = BatallaManager.EstadosDeBatalla.EFECTO;
		//Provisional llamar a efecto()
		//BatallaManager.CurrenState = BatallaManager.EstadosDeBatalla.ESPERANDO;

	
	}
}
