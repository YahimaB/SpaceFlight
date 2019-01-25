using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour {
	
    private bool allowRotation;
    private bool rotateForw;

	void Update(){
        if (allowRotation){
            if(rotateForw){
                if (gameObject.transform.rotation.x < 0)
                {
                    gameObject.transform.Rotate(Vector3.right * (Time.deltaTime * 60));
                } else {
                    allowRotation = false;
                }
            } else {
                if (gameObject.transform.rotation.x > -0.5372996)
                {
                    gameObject.transform.Rotate(-Vector3.right * (Time.deltaTime * 60));
                }
                else
                {
                    allowRotation = false;
                }
            }
        }
    }

    public void MoveCamera()
    {
        print(gameObject.transform.rotation.x);
        allowRotation = true;
        rotateForw = !rotateForw;
    }
}
