using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using Random = UnityEngine.Random;

[System.Serializable]
public class Boundary
{
    public int xMin, xMax, zMin, zMax;
}

[System.Serializable]
public class Map
{
    public bool generated = false;
    public int[][] map;
    public Vector3 start, end;
    public List<Boundary> rooms = new List<Boundary>();
    public Dictionary<Boundary, Vector2> halls = new Dictionary<Boundary, Vector2>();
    public string[] monsters;
    public Boundary startRoom, endRoom;
}

public enum Dir
{
    none = 0,
    up = 1,
    right,
    down,
    left
}
public class LevelGeneratorScript : MonoBehaviour
{
    public GameObject fogOfWarCube, room, filler,plane, fogOfWarWall;
    List<GameObject> testedObjects = new List<GameObject>();
    Dictionary<int[], GameObject> cubeInstances = new Dictionary<int[], GameObject>();
    public List<Boundary> roomBoundaries = new List<Boundary>();
    public static LevelGeneratorScript
        LevelGenerator;
    Vector2 vector = new Vector2();
    Dir lastDirection = Dir.none;
    public int roomMin = 20, roomMax = 30, numRooms = 10, spaceMin = 5, spaceMax = 15, hallWidth = 7, xIterator, zIterator, lastSpot, distance;
    float spawnChance;
    public float SpawnChance
    {
        get
        {
            return spawnChance;
        }
        set
        {
            Mathf.Clamp(spawnChance, .1f, 1f);
        }
    }
    private int firstY;
    private int firstX;
    private int lastY;
    private int lastX;
    private int x;
    private int z;
    Dictionary<Dir, Dir> opposingDir = new Dictionary<Dir, Dir> { {Dir.none, Dir.none },{ Dir.up, Dir.down }, { Dir.down, Dir.up }, { Dir.left, Dir.right }, { Dir.right, Dir.left } };

    private void Awake()
    {
        if(LevelGenerator == null)
        {
            LevelGenerator = this;
        }
        if(LevelGenerator!= this)
        {
            Destroy(this);
        }
        spawnChance = .5f;
    }

    public Map InstantiateFogOfWar(Map m)
    {
        for (int i = 0; i < m.map.Length; i++)
        {
            for (int j = 0; j < m.map.Length; j++)
            {

                switch (m.map[i][j])
                {
                    case 1:
                        Instantiate(fogOfWarWall, new Vector3(i, 0, j), Quaternion.Euler(Vector3.zero), DungeonHolderScript.holder.gameObject.transform);
                        break;
                    case 2:
                        GameObject go = Instantiate(fogOfWarCube, new Vector3(i, 0, j), Quaternion.Euler(Vector3.zero), DungeonHolderScript.holder.gameObject.transform);
                        cubeInstances.Add(new int[] { i, j }, go);
                        break;
                    default:
                        Instantiate(filler, new Vector3(i, 0, j), Quaternion.Euler(Vector3.zero), DungeonHolderScript.holder.gameObject.transform);
                        break;
                }
            }
        }
        return GenerateFogOfWarRooms(m);
    }
    #region GenerateMap
    public Map GenerateMap()
    {
        Map m = new Map();
        m.map = new int[5 * (spaceMax + roomMax)][];
        for (int i = 0; i < m.map.Length; i++) {
            m.map[i] = new int [5 * (spaceMax + roomMax)];
        }
        x = UnityEngine.Random.Range(1 + roomMax, m.map.Length - (roomMax + 2));
        z = UnityEngine.Random.Range(1 + roomMax, m.map.Length - (roomMax + 2));
        for (int k = 0; k < numRooms; k++)
        {
            int height = UnityEngine.Random.Range(roomMin, roomMax);
            int width = UnityEngine.Random.Range(roomMin, roomMax);
            m = GetRoomPosition(m, k, height, width);

            m = CreateRoom(m, firstX, lastX, firstY, lastY);
            distance = UnityEngine.Random.Range(spaceMin, spaceMax);
            m = GetNextDirection(m);

            if (k < numRooms - 1)
            {
                m = CreateHall(m, xIterator, zIterator, distance, vector);
            }
            firstX = xIterator;
            firstY = zIterator;
        }
        return m;
    }

    Map GetNextDirection(Map map)
    {
        Dir lastTry= Dir.none;
        Dir nextDirection; 
        int max = map.map.Length - 2;
        List<Dir> possibilities = new List<Dir>() { Dir.up, Dir.down, Dir.left, Dir.right};
        possibilities.Remove(opposingDir[lastDirection]);
        do
        {
            int random = UnityEngine.Random.Range(0, possibilities.Count);
            nextDirection = possibilities[random];
            possibilities.Remove(possibilities[random]);
            int tries = 0;
            bool tried = lastTry != Dir.none && nextDirection == lastTry;
            bool returning = lastDirection != Dir.none && nextDirection == opposingDir[lastDirection];
            while ((tried || returning) && possibilities.Count > 0)
            {
                random = UnityEngine.Random.Range(0, possibilities.Count);
                nextDirection = possibilities[random];
                possibilities.Remove(possibilities[random]);
                tries++;
                returning = lastDirection != Dir.none && nextDirection == opposingDir[lastDirection];
                tried = lastTry != Dir.none && nextDirection == lastTry;
            }
            lastTry = nextDirection;
            switch (nextDirection)
            {
                case Dir.up:
                    xIterator = UnityEngine.Random.Range(firstX, lastX - (hallWidth));
                    zIterator = firstY - 1;
                    vector = Vector2.down;
                    lastSpot = zIterator - distance - roomMax - 1;
                    break;
                case Dir.left:
                    xIterator = firstX - 1;
                    zIterator = UnityEngine.Random.Range(firstY, lastY - (hallWidth));
                    vector = Vector2.left;
                    lastSpot = xIterator - distance - roomMax - 1;
                    break;
                case Dir.down:
                    xIterator = UnityEngine.Random.Range(firstX, lastX - (hallWidth));
                    zIterator = lastY;
                    vector = Vector2.up;
                    lastSpot = zIterator + distance + roomMax + 1;
                    break;
                case Dir.right:
                    xIterator = lastX+1;
                    zIterator = UnityEngine.Random.Range(firstY, lastY - (hallWidth));
                    vector = Vector2.right;
                    lastSpot = xIterator + spaceMax + roomMax;
                    break;
                default:
                    xIterator = 0;
                    zIterator = 0;
                    lastSpot = 0;
                    vector = Vector2.zero;
                    distance = 0;
                    break;
            }

        }
        while (nextDirection == opposingDir[lastDirection] || lastSpot > max || lastSpot < 1);
        lastDirection = nextDirection;
        lastTry = Dir.none;
        return map;
    }

    Map CreateHall(Map map,int xIterator, int zIterator, int distance, Vector2 direction)
    {
        Boundary b = new Boundary();
        List<int> offAxisValues = new List<int>();
        for (int i = 0; i < distance; i++)
        {
            try
            {
                if (direction == Vector2.down || direction == Vector2.up)
                {
                    if (map.map[xIterator][zIterator] == 0)
                    {
                        map.map[xIterator][zIterator] = 1;
                    }
                    if (map.map[xIterator + hallWidth][zIterator] == 0)
                    {
                        map.map[xIterator + hallWidth][zIterator] = 1;
                    }
                    for (int l = 1; l < hallWidth; l++)
                    {
                        map.map[xIterator + l][zIterator] = 2;
                    }
                    offAxisValues.Add(zIterator);
                }
                else if (direction == Vector2.right || direction == Vector2.left)
                {
                    if (map.map[xIterator][zIterator] == 0)
                    {
                        map.map[xIterator][zIterator] = 1;
                    }
                    if (map.map[xIterator][zIterator + hallWidth] == 0)
                    {
                        map.map[xIterator][zIterator + hallWidth] = 1;
                    }
                    for (int l = 1; l < hallWidth; l++)
                    {
                        map.map[xIterator][zIterator + l] = 2;
                    }
                    offAxisValues.Add(xIterator);
                }
                if(i < distance - 1)
                {
                    xIterator += (int)direction.x;
                    zIterator += (int)direction.y;
                }
            }
            catch(Exception err)
            {
                Debug.Log(err.Message);
            }
        }
        if(direction == Vector2.down || direction == Vector2.up)
        {
            b.xMin = xIterator + 1;
            b.xMax = xIterator + hallWidth - 1;
            b.zMin = offAxisValues.Min();
            b.zMax = offAxisValues.Max();
        }
        else
        {
            b.zMin = zIterator + 1;
            b.zMax = zIterator + hallWidth - 1;
            b.xMin = offAxisValues.Min();
            b.xMax = offAxisValues.Max();
        }
        map.halls.Add(b, direction);
        z = zIterator;
        x = xIterator;
        return map;
    }

    Map GetRoomPosition(Map map, int k, int height, int width)
    {
        if (k == 0)
        {
            firstY = z - (int)Mathf.Floor(height / 2);
            firstX = x - (int)Mathf.Floor(width / 2);
            lastY = z + (int)Mathf.Ceil(height / 2f);
            lastX = x + (int)Mathf.Ceil(width / 2f);
            map.start = new Vector3(x, 0, z);
        }
        else
        {
            switch (lastDirection)
            {
                case Dir.none:
                    firstY = z;
                    firstX = x;
                    lastY = z;
                    lastX = x;
                    break;
                case Dir.up:
                    lastX = UnityEngine.Random.Range(x + hallWidth -1, x + 1 + width);
                    lastY = z;
                    firstX = lastX - width;
                    firstY = z - height;
                    break;
                case Dir.right:
                    lastX = x + width;
                    lastY = UnityEngine.Random.Range(z + hallWidth - 1, z+ height + 1);
                    firstY = lastY - height;
                    firstX = x;
                    break;
                case Dir.down:
                    lastX = UnityEngine.Random.Range( x + hallWidth -1, x + 1 + width);
                    lastY = z + height + 1;
                    firstX = lastX - width; 
                    firstY = z + 1;
                    break;
                case Dir.left:
                    lastX = x;
                    lastY = UnityEngine.Random.Range(z + hallWidth - 1, z + height + 1);
                    firstX = x- width;
                    firstY = lastY - height;
                    break;
                default:
                    firstY = z;
                    firstX = x;
                    lastY = z;
                    lastX = x;
                    break;
            }
            if(k == numRooms - 1)
            {
                map.end = new Vector3((firstX + lastX) / 2, 0, (firstY + lastY) / 2);
            }
        }

        int max = map.map.Length - 1;

        int offset;
        if (lastX > max)
        {
            offset = max - lastX - 2;
            lastX += offset;
            firstX += offset;
        }
        if (firstX < 1)
        {
            offset = Math.Abs(firstX) + 1;
            lastX += offset;
            firstX += offset;
        }
        if (lastY > max)
        {
            offset = max - lastY - 2;
            lastY += offset;
            firstY += offset;
        }
        if (firstY < 1)
        {
            offset = Math.Abs(firstY) + 1;
            lastY += offset;
            firstY += offset;
        }
        Boundary b = new Boundary();
        b.xMin = firstX;
        b.xMax = lastX;
        b.zMin = firstY;
        b.zMax = lastY;
        map.rooms.Add(b);
        if(k == 0)
        {
            map.startRoom = b;
        }
        else
        {
            map.endRoom = b;
        }
        return map;
    }

    Map CreateRoom(Map m, int xStart, int xEnd, int yStart, int yEnd)
    {
        try {
            for (int i = xStart - 1; i <= xEnd + 1; i++)
            {
                for (int j = yStart - 1; j <= yEnd + 1; j++)
                {
                    if (j < yStart || j == yEnd + 1 || i < xStart || i == xEnd + 1)
                    {
                        if (m.map[i][j] == 0)
                            m.map[i][j] = 1;
                    }
                    else
                    {
                        m.map[i][j] = 2;
                    }
                } }
        }
        catch
        {
            Debug.Log("MapFailed");
            
        }
        return m;
    }
    #endregion

    public Map CreateFullMap(int tempSeed, string[] monsters)
    {
        int seed;
        if (tempSeed == 0)
        {
            float f = Random.value;
            seed = (int)((float)int.MaxValue * f);
        }
        else
        {
            seed = tempSeed;
        }
        Map map = new Map();
        Random.InitState(seed);
        map = GenerateMap();
        if (monsters == null)
        {
            map.monsters = new string[] { "TargetDummy" };
        }
        else
        {
            map.monsters = monsters;
        }
        return InstantiateFogOfWar(map);
    }


    Map GenerateFogOfWarRooms(Map map)
    {
        if (!map.generated)
        {
            Dictionary<string, GameObject> monsterPrefabs = new Dictionary<string, GameObject>();
            foreach(string s in map.monsters)
            {
                monsterPrefabs.Add(s, Resources.Load("EnemyPrefabs/" + s) as GameObject);
            }
            map.rooms.ForEach((Boundary b) =>
            {
                float f = Random.value;
                if(f < spawnChance)
                {
                    float sc = spawnChance;
                    while (f < sc)
                    {//get monster variation
                        string monster = map.monsters[Random.Range(0, map.monsters.Length)];
                        //creating the monster randomly within the area.
                        Instantiate(monsterPrefabs[monster], new Vector3(Random.Range(b.xMin + 1, b.xMax - 1), 0, Random.Range(b.zMin + 1, b.zMax - 1)), Quaternion.Euler(Vector3.zero));
                        sc = sc / 2;
                    }
                }
            });
            foreach (Boundary boundary in map.halls.Keys)
            {
                while (map.halls[boundary].x != 0 && map.map[boundary.xMax + 1][boundary.zMax] == 2 && map.map[boundary.xMax + 1][boundary.zMin] == 2)
                {
                    boundary.xMax++;
                }
                while (map.halls[boundary].x != 0 && map.map[boundary.xMin - 1][boundary.zMax] == 2 && map.map[boundary.xMin - 1][boundary.zMin] == 2)
                {
                    boundary.xMin--;
                }
                while (map.halls[boundary].y != 0 && map.map[boundary.xMax][boundary.zMax + 1] == 2 && map.map[boundary.xMin][boundary.zMax + 1] == 2)
                {
                    boundary.zMax++;
                }
                while (map.halls[boundary].y != 0 && map.map[boundary.xMax][boundary.zMin - 1] == 2 && map.map[boundary.xMin][boundary.zMin - 1] == 2)
                {
                    boundary.zMin--;
                }
                map.rooms.Add(boundary);
            }
        }
        map.rooms.ForEach((Boundary b) =>
        {
            float x = ((float)b.xMin + (float)b.xMax) / 2f;
            float z = ((float)b.zMin + (float)b.zMax) / 2f;
            GameObject go = Instantiate(room, new Vector3(x, 0, z), Quaternion.Euler(Vector3.zero));
            FogOfWarRoomScript fowrs = go.GetComponent<FogOfWarRoomScript>();

            fowrs.SetBoxCollider(Mathf.Abs(b.xMax - b.xMin + 1), Mathf.Abs(b.zMax - b.zMin + 1));

            List<FogOfWarCube> cubes = new List<FogOfWarCube>();
            for (int i = b.xMin; i <= b.xMax; i++)
            {
                for (int j = b.zMin; j <= b.zMax; j++)
                {
                    KeyValuePair<int[], GameObject> keyValuePair = cubeInstances.FirstOrDefault(e =>
                         {
                             return e.Key[0] == i && e.Key[1] == j;
                         });
                    if(keyValuePair.Value != null)
                    {
                        FogOfWarCube cube = keyValuePair.Value.GetComponent<FogOfWarCube>();
                        cubes.Add(cube);
                        testedObjects.Add(cube.gameObject);
                    }
                }
            }
            fowrs.AssignCubes(cubes.ToArray());
        });
        return map;
    }

    bool ContainsBoundary(Boundary bound)
    {
        Boundary key = roomBoundaries.FirstOrDefault((c) => c.xMax == bound.xMax
                    && c.xMin == bound.xMin
                    && c.zMax == bound.zMax
                    && c.zMin == bound.zMin);
        if (key == null)
        {
            return true;
        }
        return false;
    }
}
