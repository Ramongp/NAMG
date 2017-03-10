using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Casilla {
	
		public GameObject g;
		public Text text;
		public int f;
		public int c;
		public Casilla (GameObject g,int f,int c, Text text)
		{
			this.g = g;
			this.f = f;
			this.c = c;
			this.text = text;
		}


}

