using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput : MonoBehaviour {

	public Vector2 start=new Vector2(0,0), end=new Vector2(0,0);
	Rect rect;
	Texture text;
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) 
		{
			start = Input.mousePosition;
		}
		if (Input.GetMouseButton (0))
		{
			end = Input.mousePosition;
			rect = new Rect (start.x, start.y, end.x - start.x,end.y - start.y);
		}
			
	}
	void OnDrawGizmos()
	{Gizmos.color = Color.red;
		print ("Pasa");
		Vector3 coord = new Vector3 (start.x, start.y, -10);
		Gizmos.DrawSphere(coord,end.x - start.x);}
	

}
