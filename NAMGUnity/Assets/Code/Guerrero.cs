﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Guerrero{

	public Animator anim;
	public int salud, curacion, atk, defensa, carga, tiempTurno, aumentoDef, aumentoDefBonus, saludMax, aumentoAtk, ReduccTiempTurno;
	public Text text;
	public Vector3 position;
	public Slider SSsalud,Sturno;
	public bool defendiendo, defensagrupal, descansando, alive;
	public string clase;
	public Image artifact;
	public Animator heride;
	public SpriteRenderer portrait;
	public ParticleSystem curar, defender, augmAtaque, cargar;

	public Guerrero(int salud,int curacion,int atk, int defensa,int tiempoTurno, Animator anim,Slider SSalud,Slider Sturno, Vector3 position,int aumentoDef,string clase,Text text, Image artifact,ParticleSystem curar,ParticleSystem defender,ParticleSystem augmAtaque,ParticleSystem cargar, Animator heride,SpriteRenderer portrait)
	{
		this.portrait = portrait;
		alive = true;
		this.cargar = cargar;
		this.augmAtaque = augmAtaque;
		this.defender = defender;
		this.curar=curar;
		this.heride = heride;
		this.artifact = artifact;
		this.clase = clase;
		saludMax = salud;
		this.text = text;
		this.aumentoDef=aumentoDef;
		this.position = position;
		HideText ();
		this.anim = anim;
		this.salud = salud;
		this.curacion = curacion;
		this.atk = atk;
		this.defensa = defensa;
		this.tiempTurno = tiempoTurno;
		this.SSsalud=SSalud;
		this.Sturno=Sturno;
		this.SSsalud.maxValue = salud;
		this.SSsalud.value = salud;
		carga = 0;
		
	}

	public void RecibirDano(int dano)
	{
		 int danoReal = defensa - dano;
		if (danoReal < 0) {
			BatallaManager.hurt = true;
			heride.SetTrigger ("Heride");
			salud += danoReal;
			text.color = Color.red;
			text.text = "-" + danoReal.ToString ();
			SSsalud.value = salud;
			ShowText ();
			//animacion de herido
			if (salud <= 0) {
				Morir ();
			}
		}
		else {
			text.color = Color.blue;
			text.fontSize = 9;
			text.text = "IMPENETRABLE";
			ShowText ();
		}
	}

	public void ReduccionTiempoTurno(int n)
	{
		ReduccTiempTurno = n;
		tiempTurno -= n;
		//Indicador reductor del tiempo de turno
		text.color = Color.yellow;
		text.text = "- "+n+" SEGUNDO DE ESPERA";
	}

	public void Defender(int n)
	{
		//Debug.Log ("DEfendiendose");
		defendiendo = true;
		aumentoDefBonus = n;
		defensa += aumentoDef+aumentoDefBonus;
		text.color = Color.blue;
		text.text = "+"+aumentoDef.ToString ();
		ShowText ();
		//animacion defensa
	}

	public void Curar(int n)
	{
		//mostrar curacion
		curar.gameObject.SetActive(true);
		int currentCuracion = curacion + n;
		salud += currentCuracion;
		if (salud + currentCuracion > saludMax) 
		{
			currentCuracion = saludMax - salud;
			salud = saludMax;
		}
		SSsalud.value += currentCuracion;
		text.color = Color.green;
		text.text = "+" + currentCuracion.ToString ();
		ShowText ();
		//animacion de curacion

	}

	public void Curar(int b,int n)
	{
		curar.gameObject.SetActive(true);
		int currentCuracion = b + n;
		if (salud + currentCuracion > saludMax) 
		{
			currentCuracion = saludMax - salud;
		}
		SSsalud.value += currentCuracion;
		text.color = Color.green;
		text.text = "+" + currentCuracion.ToString ();
		ShowText ();
		//animacion de curacion
		
	}

	public void Cargar()
	{
		cargar.gameObject.SetActive (true);
		carga++;
		text.color = new Color (1, 0.5f, 0, 0.5f); // Color Naranja
		text.text = carga.ToString ();
		//animacion de carga
		ShowText();
	}

	public void Morir()
	{
		//sprite muerto
		//detener su turno
		alive=false;
		portrait.color = Color.gray;
		portrait.gameObject.GetComponent<Animator> ().Stop ();

	}

	public void ShowText()
	{
		text.transform.position = new Vector3( position.x, position.y,-5);
		text.color= new Color(text.color.r,text.color.g,text.color.b,1);
	}

	public void HideText()
	{
		text.color = new Color(0,0,0,0);
		text.fontSize = 20;
	}




	public void AumentoAtaque(int n)
	{
		augmAtaque.gameObject.SetActive (true);
		aumentoAtk =n;
		atk += aumentoAtk;
		text.color = new Color (0.58f, 0.18f, 0.35f); //Color Burdeos
		text.text="+" + aumentoAtk.ToString();
		ShowText();
	}



}
