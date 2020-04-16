using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairTrapScript : MonoBehaviour {

    public GameObject 
        HeightTarget1,
        HeightTarget2,
        HeightTargetMid;

	// Use this for initialization
	void Start () {
        IList<Collider> OverlappingGround = Physics.OverlapBox(gameObject.transform.position, Vector3.one);
        if(OverlappingGround.Count > 0)
        {
            foreach(Collider collider in OverlappingGround)
            {
                if (collider.gameObject.CompareTag("Ground")&& gameObject.transform.parent!= collider.transform.parent)
                {
                    Destroy(collider.gameObject);
                }
            }
        }
        gameObject.transform.parent.gameObject.GetComponent<StairHolderScript>().animatedObject = gameObject;

        Animator animator = gameObject.GetComponent<Animator>();
        int rotated = (int)Mathf.Round(gameObject.transform.rotation.eulerAngles.y);
        if (rotated == 0)
        {
            animator.SetTrigger("-90");
            StartCoroutine(Height2Taller());
        }


        else if(rotated == -90 || rotated == 270)
        {
            animator.SetTrigger("180");
            StartCoroutine(Height1Taller());
        }

        else if(rotated == 90)
        {
            animator.SetTrigger("0");
            StartCoroutine(Height2Taller());
        }
        

        else if(rotated == 180)
        {
            animator.SetTrigger("90");
            StartCoroutine(Height1Taller());
        }

        
        gameObject.transform.eulerAngles = Vector3.zero;

    }

    IEnumerator Height1Taller()
    {
        yield return new WaitWhile(() => HeightTarget1 == null);
        gameObject.transform.parent.gameObject.GetComponent<StairHolderScript>().topStairObject.GetComponent<TopStairScript>().HeightTarget = HeightTarget1;

        yield return new WaitWhile(() => HeightTarget2 == null);
        gameObject.transform.parent.gameObject.GetComponent<StairHolderScript>().bottomStairObject.GetComponent<BottomStairScript>().HeightTarget = HeightTarget2;

        yield return new WaitWhile(() => HeightTargetMid == null);
        gameObject.transform.parent.gameObject.GetComponent<StairHolderScript>().middleStairObject.GetComponent<MiddleStairScript>().HeightTarget = HeightTargetMid;
    }
    IEnumerator Height2Taller()
    {
        yield return new WaitWhile(() => HeightTarget2 == null);
        gameObject.transform.parent.gameObject.GetComponent<StairHolderScript>().topStairObject.GetComponent<TopStairScript>().HeightTarget = HeightTarget2;

        yield return new WaitWhile(() => HeightTarget1 == null);
        gameObject.transform.parent.gameObject.GetComponent<StairHolderScript>().bottomStairObject.GetComponent<BottomStairScript>().HeightTarget = HeightTarget1;

        yield return new WaitWhile(() => HeightTargetMid == null);
        gameObject.transform.parent.gameObject.GetComponent<StairHolderScript>().middleStairObject.GetComponent<MiddleStairScript>().HeightTarget = HeightTargetMid;
    }
	
	
}
