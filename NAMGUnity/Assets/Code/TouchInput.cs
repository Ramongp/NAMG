using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour {
	Vector2 start,End;
	Rect rect;
	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 0) 

		{

			Touch touch = Input.touches[0];



			switch (touch.phase) {

			case TouchPhase.Began:

				print ("clikado");

				break;

			}
		/*if (Input.GetMouseButtonDown(0)) 
		{
			print("Ha reaccionado");
			Rect rect2 = new Rect (Input.mousePosition.x,Input.mousePosition.y,20,20);
		}*/
			
		}
	}
}