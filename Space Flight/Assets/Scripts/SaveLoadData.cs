using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Reflection;

public class SaveLoadData
{
    public void DeleteFiles(){
        File.Delete(Application.persistentDataPath + "/skinsInfo.data");
        File.Delete(Application.persistentDataPath + "/playerInfo.data");
    }

    public void SaveScore(int newScore, int gemCount)
    {
        int record = LoadCount(false);
        int oldGems = LoadCount(true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.data");
        PlayerData data = new PlayerData();

        if (newScore > record)
        {
            GameController gameController = new GameController();
            gameController.ScoreMore();
            record = newScore;
        }
        data.maxScore = record;
        data.gemCount = oldGems + gemCount;
        bf.Serialize(file, data);
        file.Close();

        //GameObject.Find("Gem Count").GetComponent<Gems>().UpdateGems();
        GameObject[] gemCounters = GameObject.FindGameObjectsWithTag("Gem Count");
        foreach (GameObject counter in gemCounters)
            counter.GetComponent<Gems>().UpdateGems();

    }


    public int LoadCount(bool isGemCount)
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.data"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.data",
                                        FileMode.Open);
            if (file.Length != 0)
            {
                PlayerData data = (PlayerData)bf.Deserialize(file);
                file.Close();
                int itemCount = isGemCount ? data.gemCount : data.maxScore;
                return itemCount;
            }
            file.Close();
            return 0;
        }
        return 0;
    }

    public void SaveSkinData(string skinName, int skinNumber, bool isAvailable)
    {
        ShopSettings data;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        if (File.Exists(Application.persistentDataPath + "/skinsInfo.data"))
        {
            file = File.Open(Application.persistentDataPath + "/skinsInfo.data",
                                        FileMode.Open);
            if (file.Length == 0)
            {
                file.Close();
                return;
            }
            data = (ShopSettings)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            data = new ShopSettings();
        }
        file = File.Create(Application.persistentDataPath + "/skinsInfo.data");

        if (skinName.Contains("Planet"))
        {
            data.PlanetSkins[skinNumber] = isAvailable;
        } else if (skinName.Contains("Shield")){
            data.ShieldSkins[skinNumber] = isAvailable;
        }

        bf.Serialize(file, data);
        file.Close();
    }

    public bool LoadSkinData(string skinName, int skinNumber)
    {
        if (File.Exists(Application.persistentDataPath + "/skinsInfo.data"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/skinsInfo.data",
                                        FileMode.Open);
            if (file.Length != 0)
            {
                ShopSettings data = (ShopSettings)bf.Deserialize(file);
                file.Close();
                bool isAvailable = false;
                if (skinName.Contains("Planet"))
                {
                    isAvailable = data.PlanetSkins[skinNumber];
                } else if (skinName.Contains("Shield")){
                    isAvailable = data.ShieldSkins[skinNumber];
                }
                return isAvailable;
            }
            file.Close();
            return false;
        }
        return false;
    }

    public void SaveUsedSkin(string skinName, int skinNumber)
    {
        ShopSettings data;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        if (File.Exists(Application.persistentDataPath + "/skinsInfo.data"))
        {
            file = File.Open(Application.persistentDataPath + "/skinsInfo.data",
                                        FileMode.Open);
            if (file.Length == 0)
            {
                file.Close();
                return;
            }
            data = (ShopSettings)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            data = new ShopSettings();
        }
        file = File.Create(Application.persistentDataPath + "/skinsInfo.data");

        if (skinName.Contains("Planet"))
        {
            data.PlanetSkinUsed = skinNumber;
        } else if (skinName.Contains("Shield"))
        {
            data.ShieldSkinUsed = skinNumber;
        }

        bf.Serialize(file, data);
        file.Close();
    }

    public int LoadUsedSkin(string skinName)
    {
        if (File.Exists(Application.persistentDataPath + "/skinsInfo.data"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/skinsInfo.data",
                                        FileMode.Open);
            if (file.Length != 0)
            {
                ShopSettings data = (ShopSettings)bf.Deserialize(file);
                file.Close();
                int usedSkin = 0;
                if (skinName.Contains("Planet"))
                {
                    usedSkin = data.PlanetSkinUsed;
                } else if (skinName.Contains("Shield")){
                    usedSkin = data.ShieldSkinUsed;
                }
                return usedSkin;
            }
            file.Close();
            return 0;
        }
        return 0;
    }

    //testing 
    public void SaveTestData(){
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/TestInfo.data");
        TestData data = new TestData();
        data.first = true;
        for (int i = 0; i < 5; i++){
            data.testArray[i] = true;
        }
        bf.Serialize(file, data);
        file.Close();
    }

    //зырь сюды )))
    public void LoadTestData(){
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/TestInfo.data",
                                    FileMode.Open); 
        //get the old class data from file
        TestData oldData = (TestData)bf.Deserialize(file);
        file.Close();
        //create the new class data
        TestData newData = new TestData();

                     //migrate data to changed class
        //get fields of the class
        foreach (FieldInfo field in oldData.GetType().GetFields()){
            //if the field is an array => start procedure
            if(field.GetValue(oldData).GetType().Name.Contains("[]")){
                //get the old array
                IList oldList = (IList)field.GetValue(oldData);
                //get the new array
                IList newList = (IList)field.GetValue(newData);
                //cope the old array to the new one
                oldList.CopyTo((System.Array)newList, 0);
                //set the new array into the class
                field.SetValue(newData, newList);
            } else { //if the field is a single value => just transfer the old value to the new one
                field.SetValue(newData, field.GetValue(oldData));
            }
        }
                    // end migration
        Debug.Log("new value in array = " + newData.testArray[1]);
        Debug.Log("not changed value in array = " + newData.testArray[6]);

        Debug.Log(oldData.first);
        Debug.Log(newData.first);
    }
}



[System.Serializable]
class ShopSettings
{
    public bool[] PlanetSkins = new bool[GameObject.Find("Planets Content").transform.childCount];
    public int PlanetSkinUsed = 0;

    public bool[] ShieldSkins = new bool[GameObject.Find("Shields Content").transform.childCount];
    public int ShieldSkinUsed = 0;
}

[System.Serializable]
class PlayerData
{
    public int maxScore;
    public int gemCount;
}

[System.Serializable]
class TestData
{
    public bool first;
    public bool second;
    public bool[] testArray = new bool[8];
}