using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowPolyLavaStoneTrap : MonoBehaviour
{

    Renderer render;
    Material mat;
    public int
        LevelSpacer;
    public float
        nextLevel;
    private GameObject fire;
    public GameObject hider;
    public bool revealed;


    private void OnEnable()
    {
        CameraScript.UpdatingNavMesh += StartUpdateGlow;
    }
    private void OnDisable()
    {
        CameraScript.UpdatingNavMesh -= StartUpdateGlow;
    }
    private void Start()
    {
        render = gameObject.GetComponent<Renderer>();
        mat = render.material;
        fire = gameObject.transform.GetChild(0).gameObject;
        StartCoroutine(UpdateGlow());

    }

    // Update is called once per frame
    void StartUpdateGlow()
    {
        StartCoroutine(UpdateGlow());
    }




    IEnumerator UpdateGlow()
    {
        if (revealed)
        {
            fire.SetActive(true);
        }
        float StartTime = Time.time;
        if (nextLevel != 0)
        {
            if (true)
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
