using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairHolderScript : MonoBehaviour {

    public GameObject
        FireObject,
        topStairObject,
        middleStairObject,
        bottomStairObject,
        animatedObject;
    Material mat;
    private int
        LevelSpacer;
    public bool
        raised;
    public float
        nextLevel;


    private void Awake()
    {
        //if (!DungeonHolderScript.stairs.ContainsKey(gameObject))
        //    DungeonHolderScript.stairs.Add(gameObject, false);
    }


    private void Start()
    {
        StartCoroutine(Initialize());

        

        raised = (Random.Range(0,2) == 1);
        StartCoroutine(Initialize());
    }
        IEnumerator Initialize()
    {
        yield return new WaitWhile(() => FireObject == null);



        yield return new WaitWhile(() => topStairObject == null);

        

        yield return new WaitWhile(() => middleStairObject == null);



        yield return new WaitWhile(() => bottomStairObject == null);
        
        //DungeonHolderScript.stairs[gameObject] = true;

        yield return new WaitWhile(() => animatedObject == null);
        mat = animatedObject.transform.GetChild(1).gameObject.GetComponent<SkinnedMeshRenderer>().material;

        if (raised)
            animatedObject.GetComponent<Animator>().SetTrigger("Raise");
    }


    private void OnEnable()
    {
        //CameraScript.UpdatingNavMesh += StartUpdateGlow;
        //if(!DungeonHolderScript.stairs.ContainsKey(gameObject))
        //DungeonHolderScript.stairs.Add(gameObject,false);
    }
    private void OnDisable()
    {
        //CameraScript.UpdatingNavMesh -= StartUpdateGlow;
        //DungeonHolderScript.stairs.Remove(gameObject);
    }
   
    
    void StartUpdateGlow()
    {
        StartCoroutine(UpdateGlow());
    }




    IEnumerator UpdateGlow()
    {
        if (animatedObject != null)
        {
            Debug.Log("UpdateGlow Started");
            //DungeonHolderScript.stairs[gameObject] = false;
            if (nextLevel == 1 && LevelSpacer == 1)
            {
                //somehow raise and lower got swapped at birth, so now they do the opposite

                if (raised == false)
                {
                    raised = true;


                    animatedObject.GetComponent<Animator>().SetTrigger("Raise");

                }

                else
                {

                    raised = false;
                    animatedObject.GetComponent<Animator>().SetTrigger("Lower");


                    FireObject.SetActive(true);

                }
            }

            //DungeonHolderScript.stairs[gameObject] = true;
            float StartTime = Time.time;
            if (nextLevel != 0)
            {
                if (LevelSpacer >= 1)
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
                    LevelSpacer = 0;
                }
                else
                    LevelSpacer++;
            }
            else
            {
                int randomInt = Random.Range(0, 3);
                nextLevel = (float)randomInt * .4f + .2f;

                LevelSpacer = Random.Range(0, 2);


                Color baseColor = Color.white; //Replace this with whatever you want for your base color at emission level '1'

                Color finalColor = baseColor * Mathf.LinearToGammaSpace(nextLevel);

                mat.SetColor("_EmissionColor", finalColor);

                yield return null;
            }

        }
    }
}
