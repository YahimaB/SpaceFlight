using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole : MonoBehaviour {
    bool grown;
    public float deformSpeed;
    public float rotationDegreePerSec;

	// Use this for initialization
	private void Start()
	{
        this.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
	}

	// Update is called once per frame
	void FixedUpdate () {
        if (!this.grown && this.transform.localScale.magnitude < 1.3)
        {
            this.transform.localScale += new Vector3(deformSpeed, deformSpeed, deformSpeed);   
        } 
        else 
        {
            grown = true;
            if (this.transform.localScale.x > 0)
            {
                this.transform.localScale -= new Vector3(deformSpeed, deformSpeed, deformSpeed);
            } else {
                Destroy(gameObject);
            }
        }
        this.transform.Rotate(Vector3.up * (Time.deltaTime*rotationDegreePerSec));
	}
}
