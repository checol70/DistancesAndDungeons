using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveFile1 : MonoBehaviour {

	// Use this for initialization
	void Start () {

        if(File.Exists(Application.persistentDataPath + "/SaveFile1name.tic"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/SaveFile1name.tic", FileMode.Open);

            gameObject.GetComponent<Text>().text = (string)bf.Deserialize(file);
            file.Close();
        }

    }
	
}
