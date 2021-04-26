using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPortalScript : MonoBehaviour
{
	public GameObject Portal;
	public GameObject Player;
    public GameObject SPortal;
    public GameObject Self;


	// Use this for initialization
	void Start () {
		SPortal.SetActive(false); 
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "RubyController") 
		{

			StartCoroutine (Teleport ());
		}
	}


	IEnumerator Teleport()
	{
		yield return new WaitForSeconds (0.15f);
		Player.transform.position = new Vector2 (Portal.transform.position.x, Portal.transform.position.y);
        yield return new WaitForSeconds (0.5f);
        SPortal.SetActive(true);
        Self.SetActive(false);

        
    }
}