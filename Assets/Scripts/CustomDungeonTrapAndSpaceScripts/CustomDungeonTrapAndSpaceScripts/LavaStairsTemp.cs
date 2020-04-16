using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaStairsTemp : MonoBehaviour {
    public GameObject
           FireObject,
           topStairObject,
           
           bottomStairObject,
           animatedObject;
    Material mat;

    public bool
        raised;
    public float
        nextLevel;

    private static Vector3
        topStairRaised = new Vector3(1.3f, 1.5f, 0),
        middleStairRaised = new Vector3(0, 1, 0),
        bottomStairRaised = new Vector3(-1.3f, .5f, 0),
        topStairLowered = new Vector3(1.3f, 0, 0),
        bottomStairLowered = new Vector3(-1.3f, 0, 0);

    private void Awake()
    {
        //if (!DungeonHolderScript.stairs.ContainsKey(gameObject))
            //DungeonHolderScript.stairs.Add(gameObject, false);
    }


    private void Start()
    {
        StartCoroutine(Initialize());



        raised = (Random.Range(0, 2) == 1);
        StartCoroutine(Initialize());
    }
    IEnumerator Initialize()
    {
        yield return new WaitWhile(() => FireObject == null);



        yield return new WaitWhile(() => topStairObject == null);
        



        yield return new WaitWhile(() => bottomStairObject == null);
        if (!raised)
        {
            topStairObject.transform.localPosition = topStairLowered;

            bottomStairObject.transform.localPosition = bottomStairLowered;

        }
        //DungeonHolderScript.stairs[gameObject] = true;

        yield return new WaitWhile(() => animatedObject == null);
        mat = animatedObject.transform.GetChild(1).gameObject.GetComponent<SkinnedMeshRenderer>().material;

        if (raised)
            animatedObject.GetComponent<Animator>().SetTrigger("Raise");
    }


    private void OnEnable()
    {
        CameraScript.UpdatingNavMesh += StartUpdateGlow;
        //if (!DungeonHolderScript.stairs.ContainsKey(gameObject))
            //DungeonHolderScript.stairs.Add(gameObject, false);
    }
    private void OnDisable()
    {
        CameraScript.UpdatingNavMesh -= StartUpdateGlow;
        //DungeonHolderScript.stairs.Remove(gameObject);
    }


    void StartUpdateGlow()
    {
        StartCoroutine(UpdateGlow());
    }




    IEnumerator UpdateGlow()
    {
        Debug.Log("UpdateGlow Started");
        //DungeonHolderScript.stairs[gameObject] = false;
        if (nextLevel == 1)
        {
            if (raised == false)
            {
                raised = true;

                FireObject.SetActive(true);

                animatedObject.GetComponent<Animator>().SetTrigger("Raise");
                
            }

            else
            {

                raised = false;
                animatedObject.GetComponent<Animator>().SetTrigger("Lower");

            }
        }

        //DungeonHolderScript.stairs[gameObject] = true;
        float StartTime = Time.time;
        if (nextLevel != 0)
        {

            if (nextLevel < 1)
            {
                nextLevel += .4f;


                Color baseColor = Color.white; //Replace this with whatever you want for your base color at emission level '1'

                Color finalColor = baseColor * Mathf.LinearToGammaSpace(nextLevel);

                mat.SetColor("_EmissionColor", finalColor);

                yield return null;

            }
            else
            {
                nextLevel = .2f;


                Color baseColor = Color.white; //Replace this with whatever you want for your base color at emission level '1'

                Color finalColor = baseColor * Mathf.LinearToGammaSpace(nextLevel);

                mat.SetColor("_EmissionColor", finalColor);

                yield return null;

            }
        }
        else
        {
            int randomInt = Random.Range(0, 3);
            nextLevel = (float)randomInt * .4f + .2f;
            


            Color baseColor = Color.white; //Replace this with whatever you want for your base color at emission level '1'

            Color finalColor = baseColor * Mathf.LinearToGammaSpace(nextLevel);

            mat.SetColor("_EmissionColor", finalColor);

            yield return null;
        }


    }
}

