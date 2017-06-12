using System.Collections;
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
	public Slider tiempo;
	bool startcrafting;
	GameObject reset;
	string action;
	Acciones acciones;
	public ParticleSystem[] Componentes;
	void Start () {
		tiempo.interactable = false;
		tiempo.value = 1;
		acciones = GameObject.Find ("Actions").GetComponent<Acciones> ();
		startcrafting = false;
		reset = GameObject.Find ("Reset");
		colors = new Color[6] {new Color (1,0,0, 0.5f),new Color (1,1,0, 0.5f),new Color (0,1,1, 0.5f), new Color (1,0.5f,0, 0.5f),new Color (0,1,0, 0.5f),new Color (1,0,1, 0.5f)};
		rect = GameObject.Find ("Main Camera").GetComponent<MouseToTouch> ().rect;
		Nums = new GameObject[6] { Troj, Tama, Tazu, Tnar, Tver, Tlila };
		//Recipes = new Recipe[]{new Recipe (30, 4, 0, 0, 4, 0, new Sprite (), 20)};
		//CurrentRecipe = Recipes [0];
		//ShowRecipe (CurrentRecipe);
		foreach (GameObject g in Nums) {
			g.SetActive (false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (startcrafting) {
			tiempo.value -= Time.deltaTime/CurrentRecipe.time;
		}
	}
	public void ShowRecipe(Recipe r,string action)
	{
		foreach (GameObject g in Nums) {
			g.SetActive (true);
		}
		this.action = action;
		startcrafting = true;
		CurrentRecipe = r;
		Troj.GetComponent<Text>().text = r.roj.ToString();
		Tama.GetComponent<Text>().text= r.ama.ToString();
		Tazu.GetComponent<Text>().text= r.azu.ToString();
		Tnar.GetComponent<Text>().text= r.nar.ToString();
		Tver.GetComponent<Text>().text= r.ver.ToString();
		Tlila.GetComponent<Text>().text = r.lila.ToString();

		MouseToTouch.pintando = false;

		colcont = 0;
		if ((r.roj.Equals (0)) && (r.nar.Equals (0)) && (r.lila.Equals (0))) {

			if ((r.ama.Equals (0)) && (r.ver.Equals (0))) {//Pasamos al azul
				colcont++;
			}

			colcont++;
			rect.GetComponent<SpriteRenderer> ().color = colors [colcont];
			reset.GetComponent<Image> ().color = colors [colcont];
		} 

		else {
			rect.GetComponent<SpriteRenderer> ().color = colors [colcont];
			reset.GetComponent<Image> ().color = colors [colcont];
		}
	}

	public void Refresh()
	{
		
		foreach (GameObject g in Nums) {
			g.SetActive (false);
			g.GetComponent<Text> ().color = new Color(0.19f,0.19f,0.19f);
			GameObject.Find ("Main Camera").GetComponent<MouseToTouch> ().RestartTable ();
		}
	}



	public void CorrectRect(int num){
	

		switch (colcont)
		{

		case 0:
			if (num.Equals (CurrentRecipe.roj+CurrentRecipe.nar+CurrentRecipe.lila)) {

				//poner result a verde
				MouseToTouch.correcto=true;
				if (CurrentRecipe.roj != 0) {
					Nums [colcont].GetComponent<Text> ().color = Color.green;
				}
				if ((CurrentRecipe.ama.Equals (0)) && (CurrentRecipe.nar.Equals (0)) && (CurrentRecipe.ver.Equals (0)) && (CurrentRecipe.lila.Equals (0)) && (CurrentRecipe.azu.Equals (0))) {
					//Objeto Acabado
					startcrafting = false;
					Debug.Log ("Objeto Acabado");
					MostrarCasillas ();
					reset.GetComponent<Image> ().color = Color.white;
					acciones.ObjetoHecho (tiempo.value > 0, action, CurrentRecipe.bonus,CurrentRecipe.image);
					tiempo.value = 1;
				} 
				else {
					if ((CurrentRecipe.ama.Equals (0)) && (CurrentRecipe.nar.Equals (0)) && (CurrentRecipe.ver.Equals (0))) {
						colcont += 2;
						MouseToTouch.pintando = false;
						rect.GetComponent<SpriteRenderer> ().color = colors [colcont];
						reset.GetComponent<Image> ().color = colors [colcont];
					}
					else {
						colcont ++;
						MouseToTouch.pintando = false;
						rect.GetComponent<SpriteRenderer> ().color = colors [colcont];
						reset.GetComponent<Image> ().color = colors [colcont];
					}
				}
			}
			break;

		case 1:
			int i = GameObject.Find ("Main Camera").GetComponent<MouseToTouch> ().Corroborar(colors [3]);
			if ((i.Equals(CurrentRecipe.nar))&&((num-i).Equals(CurrentRecipe.ama+CurrentRecipe.ver))) {
				//poner result a verde
				MouseToTouch.correcto=true;

				if (CurrentRecipe.ama != 0) {
					Nums [colcont].GetComponent<Text> ().color = Color.green;
				}

				if (CurrentRecipe.nar != 0) {
					Tnar.GetComponent<Text>().color = Color.green;
				}

				MouseToTouch.pintando = false;

				if((CurrentRecipe.azu.Equals(0))&&(CurrentRecipe.ver.Equals(0))&&(CurrentRecipe.lila.Equals(0)))
					{
					//Objeto acabado
					startcrafting = false;
					Debug.Log ("Objeto Acabado");
					MostrarCasillas ();
					reset.GetComponent<Image> ().color = Color.white;
					acciones.ObjetoHecho (tiempo.value > 0, action, CurrentRecipe.bonus,CurrentRecipe.image);
					tiempo.value = 1;
				}
			
					else{
					colcont ++;
					MouseToTouch.pintando = false;
					rect.GetComponent<SpriteRenderer> ().color = colors [colcont];
					reset.GetComponent<Image> ().color = colors [colcont];
				}

			}
			break;

		case 2:
			int verde = GameObject.Find ("Main Camera").GetComponent<MouseToTouch> ().Corroborar (colors [4]);
			int lila = GameObject.Find ("Main Camera").GetComponent<MouseToTouch> ().Corroborar (colors [5]);
			//Debug.Log ("Caso azul: " + verde.ToString () + " " + lila.ToString () + " " + num.ToString () + " DEberia dar " + CurrentRecipe.azu.ToString ());
			if ((verde.Equals (CurrentRecipe.ver)) && (lila.Equals (CurrentRecipe.lila)) && ((num- (verde + lila)).Equals (CurrentRecipe.azu))) {

				//poner result a verde
				MouseToTouch.correcto=true;

				if (CurrentRecipe.azu != 0) {
					Tazu.GetComponent<Text> ().color = Color.green;
				}

				if (CurrentRecipe.ver != 0) {
					Tver.GetComponent<Text> ().color = Color.green;
				}
				if (CurrentRecipe.lila != 0) {
					Tlila.GetComponent<Text> ().color = Color.green;
				}

				startcrafting = false;
				Debug.Log("Objeto Acabado");
				MostrarCasillas ();
				reset.GetComponent<Image> ().color = Color.white;
				acciones.ObjetoHecho (tiempo.value > 0, action, CurrentRecipe.bonus,CurrentRecipe.image);
				tiempo.value = 1;
			}


			break;
					}

			}


	void MostrarCasillas()
	{
		Componentes [0].Emit (CurrentRecipe.roj);
		Componentes [1].Emit (CurrentRecipe.ama);
		Componentes [2].Emit (CurrentRecipe.azu);
		Componentes [3].Emit (CurrentRecipe.nar);
		Componentes [4].Emit (CurrentRecipe.ver);
		Componentes [5].Emit (CurrentRecipe.lila);

	}
}

