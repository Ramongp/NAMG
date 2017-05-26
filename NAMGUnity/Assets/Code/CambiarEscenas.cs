using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CambiarEscenas : MonoBehaviour {

	// Use this for initialization
	bool Level1;
	void Start () {
		Level1 = true;
		
	}
	
	// Update is called once per frame
	void Update () {
		if ((TutorialManager.Level.Equals (1)) && (Level1)) {
			Level1 = false;
			Fungus.Flowchart.BroadcastFungusMessage ("Level1");
		}
		
	}
	public void fromLv1ToLv2()
	{
		BatallaManager.Level = 1;
		Application.LoadLevel("Prueba");

	}
}
