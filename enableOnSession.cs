using UnityEngine;
using System.Collections;

public class enableOnSession : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Hand_Destroy();
	}
	public GameObject[] gos;
	
	void Hand_Create(Vector3 pos)
	{
		foreach (GameObject go in gos)
		{
			go.SetActive(true);
		}
	}

	void Hand_Destroy()
	{
		foreach (GameObject go in gos)
		{
			go.SetActive(false);
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
