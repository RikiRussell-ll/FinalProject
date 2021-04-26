using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashCollectableScrip : MonoBehaviour
{
    public AudioClip collectedClip;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            if (controller.Dashes < controller.maxDashes)
            {
                controller.ChangeDashes(1);
                Destroy(gameObject);
                controller.PlaySound(collectedClip);
            }
        }

    }
}