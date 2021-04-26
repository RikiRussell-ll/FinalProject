using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffPortalScript : MonoBehaviour
{
    public GameObject PortalOn;
    public GameObject PortalOff;
    public AudioClip PortalIn;
    public AudioSource PortalSource;
    public GameObject PortalEffect;

	public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "RubyController") 
        {
            PortalSource.clip = PortalIn;
            PortalSource.Play();
            StartCoroutine (PortalRemove ());
        }


IEnumerator PortalRemove()
        {
        yield return new WaitForSeconds(0.5f);
        PortalOn.SetActive(true);
        PortalOff.SetActive(false);


		}
	}


}






