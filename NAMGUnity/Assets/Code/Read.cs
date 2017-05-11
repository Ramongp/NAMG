using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Read : MonoBehaviour {
	public Text text;
	public TextAsset Dialogue;
	bool next;
	float delay = 0.1f;
	// Use this for initialization
	void Start () {
		Writetext(text,Dialogue);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void Writetext(Text t, TextAsset ta)
	{
		t.text = "";
		float actualDelay = delay;
		for(int i =0;i<Dialogue.text.Length;i++)
		{

			StartCoroutine(wait(t,ta.text [i] ,actualDelay));
			actualDelay += delay;
		}

	}
	IEnumerator wait(Text t, char c, float f)
	{
		yield return new WaitForSeconds(f);
		t.text += c;
	}
}
