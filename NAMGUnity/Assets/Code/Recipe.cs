using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe{

	// Use this for initialization

	public int roj,ama,azu,nar,ver,lila,bonus;
	public Sprite image;
	public float time; //los objetos más dificlies tiene mas tiempo
	public Recipe (int roj,int ama,int azu,int nar,int ver,int lila,Sprite sprite,float time,int bonus) {
		this.roj = roj;
		this.ama = ama;
		this.azu = azu;
		this.nar = nar;
		this.ver = ver;
		this.lila = lila;
		image = sprite;
		this.time = time;
		this.bonus = bonus;
	}
}
