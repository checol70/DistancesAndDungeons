using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarRoomScript : MonoBehaviour {

    List<FogOfWarCube> cubes = new List<FogOfWarCube>();
    List<MonsterScript> contains = new List<MonsterScript>();
    List<GameObject> players = new List<GameObject>();
    bool containsExit;
    public bool HasPlayer()
    {
        return players.Count > 0;
    }
	private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            foreach (FogOfWarCube c in cubes)
            {
                c.Disappear(other.gameObject.transform.position);
            }
            foreach(MonsterScript m in contains)
            {
                m.PlayerEntered(other.gameObject);
            }
            players.Add(other.gameObject);
        }
        else
        {
            MonsterScript m = other.gameObject.GetComponent<MonsterScript>();
            if(m != null)
            {
                contains.Add(m);
                m.EnteredRoom(this);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        MonsterScript m = other.gameObject.GetComponent<MonsterScript>();
        if (m != null)
        {
            contains.Remove(m);
            m.LeftRoom(this);
        }
    }
    public void SetBoxCollider(float x, float z)
    {
        gameObject.GetComponent<BoxCollider>().size = new Vector3(x, 5f, z);
    }
    public void AssignCubes(FogOfWarCube[] gos)
    {
        cubes.AddRange(gos);
    }
}
