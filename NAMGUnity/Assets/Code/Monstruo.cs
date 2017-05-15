using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monstruo{

	public Animator anim;
	public int salud, atk, carga, tiempTurno;
	public Text text;
	public Vector3 position;
	public Slider SSsalud,Sturno;
	bool defendiendo,defensagrupal;
	Guerrero[] guerreros;
	public int aumentoatk=12,ralentizar;

	public Monstruo(int salud,int atk,int tiempoTurno, Animator anim,Slider SSalud,Slider Sturno, Vector3 position,Text text,Guerrero G1,Guerrero G2, Guerrero G3)
	{
		guerreros = new Guerrero[] { G1, G2, G3 };
		this.text = text;
		this.position = position;
		HideText ();
		this.anim = anim;
		this.salud = salud;
		this.atk = atk;
		this.tiempTurno = tiempoTurno;
		this.SSsalud=SSalud;
		this.Sturno=Sturno;
		this.SSsalud.maxValue = salud;
		this.SSsalud.value = salud;
		carga = 0;

	}

	public void Jugada()
	{
		foreach(Guerrero gu in guerreros) //Enemigos que mata con el ataque
		{
			if ((gu.salud - atk) <= 0) {
				Atacar (gu);
				return;
			}
		}

		int i = Random.Range (1, 2);

		if (i.Equals (1)) {
			Guerrero objetivo = guerreros [0];
			foreach(Guerrero gu in guerreros) //Enemigos que mata con el ataque
			{
				if (gu.salud < objetivo.salud) {
					objetivo=gu;
				}
			}
			Atacar (objetivo);
		}
		else {
			Cargar ();
		}
	}

	public void Atacar( Guerrero G)
	{  //Animacion de atacar
		G.RecibirDano (atk);
	}
	public void ShowText()
	{
		text.transform.position = new Vector3( position.x, position.y,-5);
		text.color= new Color(text.color.r,text.color.g,text.color.b,1);;
	}

	public void HideText()
	{
		text.color = new Color (0,0,0,0);
	}

	public void RecibirDano(int dano)
	{ //Pasamos un valor de daño positivo

		Debug.Log ("le ha herido con "+ dano.ToString());
			salud -= dano;
		text.color = Color.red;
			text.text = "-" + dano.ToString ();
			SSsalud.value = salud;

			ShowText ();
			//animacion de herido
			if (salud <= 0) {
				Morir ();
			}
	}

	void Morir()
	{
		//Cambiar sprite a muerte
	}

	void Cargar(){
		if (carga < 3) {
			carga++;
			text.color = new Color (1, 0.5f, 0, 0.5f); // Color Naranja
			text.text = carga.ToString ();
			//animacion de carga
			ShowText ();
		}
		else {
			carga = 0;
			//animacion ataque especial
			int[] condena = new int[] {atk,atk+aumentoatk,atk-aumentoatk/2};
			foreach (Guerrero gu in guerreros) {
				gu.RecibirDano (condena [Random.Range (0, 2)]);
			}
		}
	}

	public void Ralentizar(int n)
	{
		ralentizar = n;
		tiempTurno += n;
		text.color= new Color (1, 0.5f, 0, 0.5f);
		text.text = "Ralentizado";
		ShowText ();
	}


}
