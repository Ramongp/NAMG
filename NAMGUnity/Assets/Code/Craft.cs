﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Craft : MonoBehaviour {

	// Use this for initialization
	Recipe CurrentRecipe;
	Recipe[] Recipes;
	public GameObject Troj,Tama,Tazu,Tnar,Tver,Tlila;
	GameObject[] Nums;
	GameObject rect;
	Color[] colors;
	int[] playable;
	int colcont;
	void Start () {
		colors = new Color[6] {new Color (1,0,0, 0.5f),new Color (1,1,0, 0.5f),new Color (0,0,1, 0.5f), new Color (0,1,1, 0.5f),new Color (0,1,0, 0.5f),new Color (1,0,1, 0.5f)};
		rect = GameObject.Find ("Main Camera").GetComponent<MouseToTouch> ().rect;
		Nums = new GameObject[6] { Troj, Tama, Tazu, Tnar, Tver, Tlila };
		Recipes = new Recipe[]{new Recipe (30, 4, 0, 0, 4, 0, new Sprite (), 20)};
		playable = new int[6];
		CurrentRecipe = Recipes [0];
		Refresh ();
		ShowRecipe (CurrentRecipe);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void ShowRecipe(Recipe r)
	{
		
		Troj.GetComponent<Text>().text = r.roj.ToString();
		Tama.GetComponent<Text>().text= r.ama.ToString();
		Tazu.GetComponent<Text>().text= r.azu.ToString();
		Tnar.GetComponent<Text>().text= r.nar.ToString();
		Tver.GetComponent<Text>().text= r.ver.ToString();
		Tlila.GetComponent<Text>().text = r.lila.ToString();

		colcont = 0;
		foreach (GameObject g in Nums) {
			playable [colcont] = 0;
			if (g.GetComponent<Text> ().text != 0.ToString ()) {
				g.SetActive (true);
				playable [colcont] = 1;
			}
			colcont++;
		}
		colcont = 0;

		while ((playable [colcont].Equals (0))&&( colcont<playable.Length)) {
			colcont++;
		}
		rect.GetComponent<SpriteRenderer> ().color = colors [colcont];

	}

	public void Refresh()
	{
		foreach (GameObject g in Nums) {
			g.SetActive (false);
		}
	}



	public void CorrectRect(int num){
		Debug.Log (num.ToString());
		if (num.ToString ().Equals (Nums [colcont].GetComponent<Text> ().text)) {
			MouseToTouch.pintando = false;

			Nums [colcont].GetComponent<Text> ().color = Color.green;
			colcont++;
		
			while ((playable [colcont].Equals (0)) && (colcont < playable.Length)) {
				colcont++;
			}
			if (colcont.Equals (playable.Length)) {
				Debug.Log ("Objecto acabado");
			} else {
				rect.GetComponent<SpriteRenderer> ().color = colors [colcont];
			}
		}
	}
}
