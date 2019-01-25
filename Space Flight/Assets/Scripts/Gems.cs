using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gems : MonoBehaviour {

    SaveLoadData saveLoadData = new SaveLoadData();

	private void Awake()
	{
        UpdateGems();
	}

	private void FixedUpdate()
	{
        UpdateGems();
	}

	public void UpdateGems () {
        GetComponent<Text>().text = saveLoadData.LoadCount(true).ToString();
	}
}
