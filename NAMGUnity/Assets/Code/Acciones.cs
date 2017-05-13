using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Acciones : MonoBehaviour {

	Craft craft;
	Recipe[] atk,def,curation,spatkCa,spatkAr,spatkMa,spatakMo;
	public Button[] buttons;
	// Use this for initialization
	void Start () {
		craft=GameObject.Find ("CreadorRecetas").GetComponent<Craft> ();
		atk = new Recipe[2] {new Recipe(2,0,2,0,0,6,new Sprite(),20),new Recipe(12,0,0,0,0,0,new Sprite(),20)};
		def = new Recipe[2] {new Recipe(2,0,2,0,0,6,new Sprite(),20),new Recipe(0,6,0,0,0,0,new Sprite(),20)};
		curation = new Recipe[2] {new Recipe(0,0,0,0,8,0,new Sprite(),20),new Recipe(0,0,0,0,15,0,new Sprite(),20)};
		spatkCa = new Recipe[2] {new Recipe(2,0,2,0,0,6,new Sprite(),20),new Recipe(12,0,0,0,0,0,new Sprite(),20)};
		spatkAr = new Recipe[2] {new Recipe(2,0,2,0,0,6,new Sprite(),20),new Recipe(12,0,0,4,0,0,new Sprite(),20)};
		spatkMa = new Recipe[2] {new Recipe(2,0,2,0,0,6,new Sprite(),20),new Recipe(12,0,0,0,0,0,new Sprite(),20)};
		spatakMo = new Recipe[2] {new Recipe(2,0,2,0,0,6,new Sprite(),20),new Recipe(12,0,0,0,0,0,new Sprite(),20)};
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
		//envir el gerrero del turno
		//craft.ShowRecipe (specialattak [Random.Range (0, specialattak.spatkAr)]);
		//MouseToTouch.crafteando = true;
		//Hide ();
	}

	void Hide ()
	{
		foreach (Button b in buttons) {
			b.gameObject.SetActive (false);
		}

	}

	void Show ()
	{
		foreach (Button b in buttons) {
			b.gameObject.SetActive (true);
		}

	}

	public void ObjetoHecho(bool bonus,string action)
	{
		craft.Refresh ();
		MouseToTouch.crafteando = false;
		Show ();
	}
}
