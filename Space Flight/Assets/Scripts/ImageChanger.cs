using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageChanger : MonoBehaviour {

    //public Sprite initialImage;
    public AudioController audioController;

	private void Awake()
	{
        ChangeImage();
	}

	public void ChangeImage()
	{
        if (gameObject.name.Contains("Music"))
        {
            if (audioController.GetMusicState())
            {
                gameObject.GetComponent<Image>().color = Color.black;
            }
            else
            {
                gameObject.GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f);
            }
        } 
        else
        {
            if (audioController.GetSoundState())
            {
                gameObject.GetComponent<Image>().color = Color.black;
            }
            else
            {
                gameObject.GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f);
            }
        }
	}
}
