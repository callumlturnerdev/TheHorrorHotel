using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour {

    private void OnCollisionEnter(Collision collision)
    {
     
        IPausable testobj = collision.gameObject.GetComponent<IPausable>();

        if (testobj != null)
        {
            testobj.Pause();
        }
        else
        {
          
        }
    }
}
