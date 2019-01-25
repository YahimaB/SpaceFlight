using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float rotationDegreePerSec;
    public GameController gameController;

    SaveLoadData saveLoadData = new SaveLoadData();
    AudioController audioController;

	private void Awake()
	{
        int skinNum = saveLoadData.LoadUsedSkin("Planet");
        string skinName = "Planet#" + skinNum;
        print(skinName);
        GameObject skin = GameObject.Find("Canvas").transform.Find("Shop Panel").
                                    Find("Planets Scroller").Find("Viewport").
                                    Find("Planets Content").Find(skinName).gameObject;
        print(skin.name);
        skin.GetComponent<Skin>().ApplySkin();

        audioController = GameObject.Find("Audio Controller").GetComponent<AudioController>();
	}

	private void FixedUpdate()
	{
        this.transform.Rotate(Vector3.up * (Time.deltaTime * rotationDegreePerSec));
	}

	void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Asteroid")
        {
            gameController.GameOver();
            gameObject.SetActive(false);
            Destroy(other.gameObject);

            return;
        }

        if(other.tag == "Gem")
        {
            audioController.PlayGemSound();
            gameController.AddScore(true, (int)(Random.value * 8));
            Destroy(other.gameObject);

            return;

        }

        if (other.tag == "Shield")
        {
            gameController.CreateShield();
            Destroy(other.gameObject);

            return;

        }
    }

}
