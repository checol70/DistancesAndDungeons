using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LowPolyCobblestonePulse : MonoBehaviour {

    Renderer render;
    Material mat;
    public float nextLevel;
    public float emission;
    int LevelSpacer;


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
        StartCoroutine(UpdateGlow());
    }

    // Update is called once per frame
    void StartUpdateGlow()
    {
        StartCoroutine(UpdateGlow());
    }




    IEnumerator UpdateGlow()
    {
        float StartTime = Time.time;
        if (nextLevel != 0)
        {
            if (LevelSpacer < 1)
            {
                if (nextLevel < .2f)
                {
                    nextLevel += .1f;


                    Color baseColor = Color.white; //Replace this with whatever you want for your base color at emission level '1'

                    Color finalColor = baseColor * Mathf.LinearToGammaSpace(nextLevel);

                    mat.SetColor("_EmissionColor", finalColor);

                    yield return null;

                }
                else
                {
                    nextLevel = .1f;


                    Color baseColor = Color.white; //Replace this with whatever you want for your base color at emission level '1'

                    Color finalColor = baseColor * Mathf.LinearToGammaSpace(nextLevel);

                    mat.SetColor("_EmissionColor", finalColor);

                    yield return null;

                }
            }
            LevelSpacer++;
            if (LevelSpacer > 1)
                LevelSpacer = 0;
        }
        else
        {
            int randomInt = Random.Range(0, 4);
            nextLevel = (float)randomInt * .1f;
            LevelSpacer = Random.Range(0, 2);


            if (nextLevel > .2f)
            {
                nextLevel = .2f;
            }

            Color baseColor = Color.white; //Replace this with whatever you want for your base color at emission level '1'

            Color finalColor = baseColor * Mathf.LinearToGammaSpace(nextLevel);

            mat.SetColor("_EmissionColor", finalColor);

            yield return null;
        }
    }
}
