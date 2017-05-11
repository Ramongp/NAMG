using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ChangeRectangle : MonoBehaviour {

	int numpuntos = 4;
	string cuadrante;
	public GameObject rect;
	bool pintando;
	Vector3[] puntos;
	Vector3 originalPos;
	void Start () {
		puntos = new Vector3[4];
		pintando = false;
		for (int i = 0; i < numpuntos; i++) {
			puntos [i] = GameObject.Find(i.ToString()).GetComponent<Transform>().position;
		}
	}

	void Update () {

		// Handle native touch events
		foreach (Touch touch in Input.touches) {
			HandleTouch (touch.fingerId, Camera.main.ScreenToWorldPoint (touch.position), touch.phase);
		}

		// Simulate touch events from mouse events
		if (Input.touchCount == 0) {
			if (Input.GetMouseButtonDown (0)) {
				HandleTouch (10, Camera.main.ScreenToWorldPoint (Input.mousePosition), TouchPhase.Began);
			}
			if (Input.GetMouseButton (0)) {
				HandleTouch (10, Camera.main.ScreenToWorldPoint (Input.mousePosition), TouchPhase.Moved);
			}
			if (Input.GetMouseButtonUp (0)) {
				HandleTouch (10, Camera.main.ScreenToWorldPoint (Input.mousePosition), TouchPhase.Ended);
			}
		}
	}

	private void HandleTouch(int touchFingerId, Vector3 touchPosition, TouchPhase touchPhase) {
		switch (touchPhase) {
		case TouchPhase.Began:

			RaycastHit hit;
			if (Physics.Raycast (touchPosition,Vector3.forward*20, out hit)) {
				if (hit.collider.tag.Equals ("Quad")) {
				cuadrante = hit.collider.name;
				originalPos = touchPosition;
				originalPos.z = 10;
				pintando = true;
				Posicion (originalPos, cuadrante);
					rect.transform.position = originalPos;
					Debug.Log (originalPos);
				//rect.transform.position = originalPos;
				}
			}
			break;

		case TouchPhase.Moved:
			if (pintando) {
				rect.transform.localScale = new Vector3 ((touchPosition.x - originalPos.x),( touchPosition.y - originalPos.y), 0);
				rect.transform.position = new Vector3 (originalPos.x+rect.transform.localScale.x/2, originalPos.y+rect.transform.localScale.y/2, rect.transform.position.z);
			}
			break;

		case TouchPhase.Ended:
			pintando = false;
			break;
		}

	}
	void Posicion (Vector3 t,string cuadrante){
		int princ, fin;
		float dist;
		Vector3 optimo;
		switch (cuadrante)
		{
		case "C1":
			princ = 0;
			fin = 4;
			break;
		default:
			princ = 0;
			fin = 4;
			break;
		}
		dist = Vector3.Distance (t, puntos [princ]);
		optimo = puntos [princ];
		princ++;
		for(int i =princ;i<fin;i++)
		{
			if (dist > Vector3.Distance (t, puntos [i])) 
			{
				dist = Vector3.Distance (t, puntos [i]);
				optimo = puntos [i];
			}
		}			
		originalPos = optimo;
				}
}