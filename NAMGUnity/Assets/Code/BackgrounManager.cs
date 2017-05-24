using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgrounManager : MonoBehaviour {

	public RectTransform point;
	public GameObject[] Birds;
	public GameObject cloud;
	float timer;
	public bool llegado,llegadoNube;
	int Speed;
	// Use this for initialization
	void Start () {
		cloud.SetActive (false);
		foreach (GameObject g in Birds) {
			g.SetActive (false);
		}
		llegado = true;
		StartCoroutine (MoveCloud ());
	}
	
	// Update is called once per frame
	void Update () {

		if (!llegado) {
			foreach (GameObject g in Birds) {
				if ((g.transform.position.x >= point.transform.position.x + point.rect.width / 2)&&(g.activeSelf)) {
					llegado = true;
					g.SetActive (false);
					//Debug.Log ("Ha cambiado el valor a false de llegado");
				} else {	
					llegado = false;
					//Debug.Log ("SE mueve");
					g.transform.Translate (Vector3.right * Time.deltaTime * Speed);
				}
			}
		} 
		else {
			StartCoroutine (CreateOrder());
		}
		if ((cloud.transform.position.x >= point.transform.position.x + point.rect.width / 2)&&(cloud.activeSelf)) {
			cloud.SetActive (false);
			StartCoroutine (MoveCloud ());
			//	Debug.Log ("Ha cambiado el valor a false de llegado");
		} else {	
			cloud.transform.Translate (Vector3.right * Time.deltaTime * Speed/4);
		}
	}

	void Move(GameObject G)
	{
		G.transform.position = new Vector3(point.transform.position.x-point.rect.width/2,point.transform.position.y+point.rect.height/6*Random.Range(1,3),G.transform.position.z);
		//Debug.Log ("Pasa por aqui");

	}

	IEnumerator CreateOrder()
	{
		llegado = false;
		Speed = Random.Range (2, 5);
		yield return  new WaitForSeconds(Random.Range (2, 5));
		foreach (GameObject g in Birds) {
			g.SetActive (true);
			Move (g);
			yield return new WaitForSeconds(0.5f);
		}
	}
	IEnumerator MoveCloud()
	{
		yield return  new WaitForSeconds(Random.Range (2, 5));
		cloud.SetActive (true);
		cloud.transform.position = new Vector3(point.transform.position.x-point.rect.width/2,point.transform.position.y+point.rect.height/6*Random.Range(2,4),cloud.transform.position.z);
	}
}

