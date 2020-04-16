using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveFile3Script : MonoBehaviour {

    private void Start()
    {
        if (File.Exists(Application.persistentDataPath + "/SaveFile3name.tic"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/SaveFile3name.tic", FileMode.Open);

            gameObject.GetComponent<Text>().text = (string)bf.Deserialize(file);
            file.Close();
        }
    }
}
