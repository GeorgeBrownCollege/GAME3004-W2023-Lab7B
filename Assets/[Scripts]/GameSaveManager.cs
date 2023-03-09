using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string position;
    public string rotation;

    // constructor
    public PlayerData()
    {
        position = "";
        rotation = "";
    }
}


public class GameSaveManager : MonoBehaviour
{
    public Transform player;

    private string dataPath;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerBehaviour>().transform;

        dataPath = Application.persistentDataPath + "/playerData.dat";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadData();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            SaveData();
        }
    }

    // Data Serialization = Data Encoding
    private void SaveData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(dataPath);
        PlayerData data = new PlayerData(); // creates an empty PlayerData object

        data.position = JsonUtility.ToJson(player.position);
        data.rotation = JsonUtility.ToJson(player.localEulerAngles);

        bf.Serialize(file, data);
        file.Close();

        // Player Prefs Example
        //var positionString = JsonUtility.ToJson(player.position);
        //var rotationString = JsonUtility.ToJson(player.localEulerAngles);

        //PlayerPrefs.SetString("position", positionString);
        //PlayerPrefs.SetString("rotation", rotationString);
        //PlayerPrefs.Save();

        print("Player Data Saved!");
    }

    // Data Deserialization = Data Decoding
    private void LoadData()
    {
        if (File.Exists(dataPath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(dataPath, FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            var position = JsonUtility.FromJson<Vector3>(data.position);
            var rotation = JsonUtility.FromJson<Vector3>(data.rotation);

            player.gameObject.GetComponent<CharacterController>().enabled = false;
            player.position = position;
            player.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
            player.gameObject.GetComponent<CharacterController>().enabled = true;

            print("Player Data Loaded!");
        }

        // Player Prefs example
        //var position = JsonUtility.FromJson<Vector3>(PlayerPrefs.GetString("position"));
        //var rotation = JsonUtility.FromJson<Vector3>(PlayerPrefs.GetString("rotation"));

        //player.gameObject.GetComponent<CharacterController>().enabled = false;
        //player.position = position;
        //player.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
        //player.gameObject.GetComponent<CharacterController>().enabled = true;

        
    }

    private void ResetData()
    {
        PlayerPrefs.DeleteAll();
        print("Player Data Removed!");
    }

    public void OnSaveButton_Pressed()
    {
        SaveData();
    }

    public void OnLoadButton_Pressed()
    {
        LoadData();
    }
}
