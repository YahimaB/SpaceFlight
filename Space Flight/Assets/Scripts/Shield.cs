using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {
    
    public float dieTime;

	private void Awake()
	{
        Destroy(gameObject, dieTime);
	}

	void OnTriggerEnter(Collider other)
	{
        if (other.tag == "Asteroid")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
	}

}
