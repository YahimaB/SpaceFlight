using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyGems : MonoBehaviour {

    SaveLoadData saveLoadData = new SaveLoadData();


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddGems(){
        saveLoadData.SaveScore(-1, 230);
        print("got gems");
    }
}
