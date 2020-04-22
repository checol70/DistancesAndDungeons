using UnityEngine;
using System.Collections;

public class StartManagerScript : MonoBehaviour {
    /*
     This script is to control the lighting so that if we return to a level it doesn't automatically reveal everything, 
     and to control if we return to a level that we don't lose a turn to being out of our movement range.  
      
      
      
     
    */
    public int 
        FramesTillReady;
    private GameObject[]
        Torches;
    public static GameObject
        startManager;
    public static bool
        StartGame;
	// Use this for initialization
    void Awake()
    {
        
        if (startManager == null)
        {
            startManager = gameObject;
            DontDestroyOnLoad(gameObject);
            Setup();
        }
        if(startManager != gameObject)
        {
            Destroy(gameObject);
        }
    }
	void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {
        if (FramesTillReady < 10)
            FramesTillReady++;
        if (FramesTillReady >= 10 && !StartGame)
        {
            StartGame = true;
            
        }
    }
    // to be called when we first start and also when we Load a new level and are ready to start the players
    void Setup()
    {
        
        FramesTillReady = 0;
        StartGame = false;
    }
    void NewLevel()
    {
        Setup();
    }
}
