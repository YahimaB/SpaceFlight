using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skin : MonoBehaviour {

    bool isAvailable;
    int skinNumber;
    string skinName;

    SaveLoadData saveLoadData = new SaveLoadData();
    LinkManager linkManager;

	// Use this for initialization


	void Awake () {
        linkManager = GameObject.Find("LinkManager").GetComponent<LinkManager>();

        skinName = this.name;
        skinNumber = System.Int32.Parse(skinName.Substring(skinName.IndexOf("#")+1));
        isAvailable = saveLoadData.LoadSkinData(skinName, skinNumber);
        if(isAvailable){
            this.transform.Find("Skin Price").
                GetComponent<UnityEngine.UI.Text>().text = "Use";
            
        }
	}
	
    public void OnDoubleTap(){
        if(isAvailable){
            linkManager.ShowSuccessMessage();
            Select();
        } else {
            Buy();
        }
    }

    void Buy(){
        print("Buying process");

        int price = System.Int32.Parse(this.transform.Find("Skin Price").
                                       GetComponent<UnityEngine.UI.Text>().text);
        int playerGems = saveLoadData.LoadCount(true);

        if(playerGems>=price){
            saveLoadData.SaveSkinData(skinName, skinNumber, true);
            isAvailable = saveLoadData.LoadSkinData(skinName, skinNumber);
            saveLoadData.SaveScore(-1, -price);
            this.transform.Find("Skin Price").
                GetComponent<UnityEngine.UI.Text>().text = "Use";
        } else {
            print("not enough money");
        }


    }

    //Cмена скина
    void Select(){
        ApplySkin();
        saveLoadData.SaveUsedSkin(skinName, skinNumber);
    }
    public void ApplySkin(){
        print("Selection process");
        //Получить имя текстуры и составить имя материала
        //Image planetTexture = this.transform.Find("Mask").Find("Skin Image").
        //gameObject.GetComponent<Image>().;
        string imageName = this.transform.Find("Mask").Find("Skin Image").
                               gameObject.GetComponent<Image>().sprite.name;


        string materialName = imageName.Replace("Image", "Material");
        Material newMaterial;
        //Выбрать нужный объект
        GameObject gameObj;
        if (materialName.Contains("Planet"))
        {
            //поиск материала
            newMaterial = Resources.Load("planets/" + materialName, typeof(Material)) as Material;
            //нахождение объекта и сохранение номера скина в массиве скинов в PlayerData
            gameObj = GameObject.Find("MainPlanet").gameObject;

            //Задать новый цвет освещения
            Texture2D newTexture = (Texture2D)newMaterial.mainTexture;
            Color32 newLightColor = AverageColor(newTexture);
            GameObject.Find("Main Light").GetComponent<Light>().color = newLightColor;
        }
        else if(materialName.Contains("Shield")){
            newMaterial = Resources.Load("shields/" + materialName, typeof(Material)) as Material;
            gameObj = Resources.Load("Pick Ups") as GameObject;
            gameObj = gameObj.transform.Find("Shield").gameObject;
        }else{
            newMaterial = null;
            gameObj = null;
        }
        //Установить материал
        gameObj.GetComponent<MeshRenderer>().material = newMaterial;
    }


    //Общий цвет текстуры
    Color32 AverageColor(Texture2D tex)
    {
        Color32[] texColors = tex.GetPixels32();
        int total = texColors.Length;
        float r = 0;
        float g = 0;
        float b = 0;
        for (int i = 0; i < total; i++)
        {
            r += texColors[i].r;
            g += texColors[i].g;
            b += texColors[i].b;
        }

        return new Color32((byte)(r / total), (byte)(g / total), (byte)(b / total), 0);

    }
}

