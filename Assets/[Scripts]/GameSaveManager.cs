using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSaveManager : MonoBehaviour
{
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerBehaviour>().transform;
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
        // Player Prefs Example
        var positionString = JsonUtility.ToJson(player.position);
        var rotationString = JsonUtility.ToJson(player.localEulerAngles);

        PlayerPrefs.SetString("position", positionString);
        PlayerPrefs.SetString("rotation", rotationString);
        PlayerPrefs.Save();

        print("Player Data Saved!");
    }

    // Data Deserialization = Data Decoding
    private void LoadData()
    {
        // Player Prefs example
        var position = JsonUtility.FromJson<Vector3>(PlayerPrefs.GetString("position"));
        var rotation = JsonUtility.FromJson<Vector3>(PlayerPrefs.GetString("rotation"));

        player.gameObject.GetComponent<CharacterController>().enabled = false;
        player.position = position;
        player.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
        player.gameObject.GetComponent<CharacterController>().enabled = true;

        print("Player Data Loaded!");
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
