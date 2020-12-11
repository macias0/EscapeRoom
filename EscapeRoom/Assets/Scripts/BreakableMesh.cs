using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(AudioSource))]
public class BreakableMesh : MonoBehaviour, IHitable
{
    private Mesh mesh;
    private new AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        audio = GetComponent<AudioSource>();
        //MergeMesh(); //to avoid multiple vertices at the same coorrds
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit(Item weapon, RaycastHit hit)
    {
        Debug.Log("TriangleIndex: " + hit.triangleIndex);
        DestroyCuboid(hit.triangleIndex);
    }

    
    int[] MergeMesh(int [] triangles, Vector3 [] vertices, List<int> favorizedIndexes)
    {




        int[] newTriangles = new int[triangles.Length];

        Dictionary<Vector3, int> map = new Dictionary<Vector3, int>();

        foreach (int fi in favorizedIndexes)
        {
            map[vertices[fi]] = fi;
        }

        for (int i=0; i<triangles.Length; ++i)
        {
            int index = triangles[i];
            int value;
            if(map.TryGetValue(vertices[index], out value))
            {
                //Debug.Log("Podmieniam triangle " + triangles[i] + " na " + value);
                newTriangles[i] = value;
                //mesh.triangles[i] = value;
            }
            else
            {
                map[vertices[index]] = index;
                newTriangles[i] = index;
            }
        }

        return newTriangles;

        
        
    }



    private List<int> indexesToDraw = null;


    private void DestroyCuboid(int triangleIndex)
    {
        int neighbourIndex = FindNeighbour(triangleIndex);

        //Debug.Log("Bazowy kwadrat: " + triangleIndex + "," + neighbourIndex);

        DestroyTriangles(FindCuboidIndexes(triangleIndex, neighbourIndex));

        audio.Play();


    }

    private List<int> FindCuboidIndexes(int index1, int index2) //index1, index2 - front wall triangles indices
    {
        List<int> result = new List<int> { index1, index2 };

        int[] triangles = mesh.triangles;
        Vector3[] verticies = mesh.vertices;

        //get all points to discard diagonal and find out which points are corners
        List<int> corners = GetVertexIndexes(result); 

        //foreach (var x in corners)
        //{
        //    Debug.Log("CORNERS: " + x);
        //}

        corners = corners.Distinct().ToList(); // indices of front wall

        //indexesToDraw = corners;

        //Debug.Log("Szukam sasiednich scian dla indexow: " + corners[0] + "," + corners[1] + "," + corners[2] + "," + corners[3]);

        int[] mergedTriangles = MergeMesh(mesh.triangles, mesh.vertices, corners);

        //1st normal wall
        AddCornerNeighbour(corners[0], corners[1], new List<int> { index1, index2 }, ref result, mergedTriangles);

        //2nd normal wall
        AddCornerNeighbour(corners[0], corners[2], new List<int> { index1, index2 }, ref result, mergedTriangles);

        //3rd normal wall
        AddCornerNeighbour(corners[0], corners[3], new List<int> { index1, index2 }, ref result, mergedTriangles);

        //4th normal wall
        AddCornerNeighbour(corners[1], corners[2], new List<int> { index1, index2 }, ref result, mergedTriangles);

        //5th normal wall
        AddCornerNeighbour(corners[2], corners[3], new List<int> { index1, index2 }, ref result, mergedTriangles);

        //6th normal wall
        AddCornerNeighbour(corners[1], corners[3], new List<int> { index1, index2 }, ref result, mergedTriangles);


        //Debug.Log("RESULT COUNT: " + result.Count);


        //FIND TOP WALL

        List<int> cuboidVertices = GetVertexIndexes(result.Where(e => e != index1 && e != index2).ToList(), mergedTriangles).Where(e => !corners.Contains(e)).Distinct().ToList();

        AddCornerNeighbour(cuboidVertices[0], cuboidVertices[1], result, ref result, mergedTriangles);


        return result;
    }

    private List<int> GetVertexIndexes(List<int> triangleIndexes, int [] triangles = null)
    {
        //Debug.Log("GETVERTEXINDEXES COUNT: " + triangleIndexes.Count);
        if (triangles == null)
            triangles = mesh.triangles;
        List<int> result = new List<int>();
        foreach(int i in triangleIndexes)
        {
            result.Add(triangles[3 * i]);
            result.Add(triangles[3 * i + 1]);
            result.Add(triangles[3 * i + 2]);
        }
        return result;
    }

    private void AddCornerNeighbour(int c1, int c2, List<int> notIndex, ref List<int> list, int [] triangles)
    {
        //Debug.Log("AddCornerNeighbour dla " + c1 + "," + c2 + "t1, t2: " + t1 + "," + t2);
        int n1 = FindTriangleIndex(c1, c2, notIndex, triangles);
        //Debug.Log("N!: " + n1);
        
        if(n1 != -1)
        {
            int n2 = FindNeighbour(n1);
            //Debug.Log("AddCornerNeighbour: " + n1 + "," + n2);
            list.Add(n1);
            list.Add(n2);
        }
    }



    private int FindNeighbour(int triangleIndex)
    {
        int[] triangles = mesh.triangles;
        Vector3[] verticies = mesh.vertices;

        Vector3 p1 = verticies[triangles[triangleIndex * 3 + 0]];
        Vector3 p2 = verticies[triangles[triangleIndex * 3 + 1]];
        Vector3 p3 = verticies[triangles[triangleIndex * 3 + 2]];

        //Debug.Log("Find Neighbour indexy vertexow: " + triangles[triangleIndex * 3] + "," + triangles[triangleIndex * 3 + 1] + "," + triangles[triangleIndex * 3 + 2]);

        float d1 = Vector3.Distance(p1, p2);
        float d2 = Vector3.Distance(p1, p3);
        float d3 = Vector3.Distance(p2, p3);

        int v1, v2;
        if(d1 > d2 && d1 > d3)
        {
            v1 = triangles[triangleIndex * 3 + 0];
            v2 = triangles[triangleIndex * 3 + 1];
        }
        else if(d2 > d1 && d2 > d3)
        {
            v1 = triangles[triangleIndex * 3 + 0];
            v2 = triangles[triangleIndex * 3 + 2];
        }
        else
        {
            v1 = triangles[triangleIndex * 3 + 1];
            v2 = triangles[triangleIndex * 3 + 2];
        }
        //Debug.Log("Find neighbour shared: " + v1 + "," + v2);
        return FindTriangleIndex(v1, v2, new List<int> { triangleIndex });
    }


    private int FindTriangleIndex(int v1, int v2, List<int> notIndex, int [] triangles = null)
    {
        if (triangles == null)
            triangles = mesh.triangles;
        int i = 0;
        List<int> three = new List<int>();
        while(i < triangles.Length)
        {
            //Debug.Log("i=" + i);
            //Debug.Log("Trojka: " + triangles[i] + "," + triangles[i + 1] + "," + triangles[i + 2]);
            //if (i != notIndex * 3 && i != notIndex2 * 3)
            if( !notIndex.Contains(i/3))
            {
                three.Add(triangles[i]);
                three.Add(triangles[i+1]);
                three.Add(triangles[i+2]);

                //Debug.Log("FindTriangleIndex v1, v2: " + v1 + "," + v2);
                

                if (three.Contains(v1) && three.Contains(v2))
                {
                  //  Debug.Log("Znalazlem neighbora id: " + (i / 3) + " z indeksami vertexow: " + triangles[i] + "," + triangles[i+1] + "," + triangles[i+2]);
                    return i / 3;
                }

                three.Clear();
            }
            else
            {
                //Debug.Log("POMIJAM TROJKE: " + triangles[i] + "," + triangles[i + 1] + "," + triangles[i + 2]);
                //Debug.Log("Dla v1,v2: " + v1 + "," + v2);
                //Debug.Log("NotIndex: " + notIndex + "," + notIndex2);
            }
            i += 3;

        }
        return -1;
    }



    private void DestroyTriangles(List<int> indexes)
    {
        Destroy(GetComponent<MeshCollider>());
        //Vector3[] verticies = mesh.vertices;
        int[] triangles = mesh.triangles;

        int[] newTriangles = new int[triangles.Length - 3 * indexes.Count];


        for(int i=0, j=0; i<triangles.Length;)
        {
            if ( indexes.Contains(i/3))
            {
                //Debug.Log("Wywalam triangle i: " + i);
                //Debug.Log("Indeksy vertexow: " + triangles[i] + "," + triangles[i + 1] + "," + triangles[i + 2]);
                i += 3;
            }
            else
            {
                newTriangles[j++] = triangles[i++];
            }
        }

        mesh.triangles = newTriangles;
        gameObject.AddComponent<MeshCollider>();
    }

    public static List<List<T>> GetAllCombos<T>(List<T> list)
    {
        int comboCount = (int)System.Math.Pow(2, list.Count) - 1;
        List<List<T>> result = new List<List<T>>();
        for (int i = 1; i < comboCount + 1; i++)
        {
            // make each combo here
            result.Add(new List<T>());
            for (int j = 0; j < list.Count; j++)
            {
                if ((i >> j) % 2 != 0)
                    result.Last().Add(list[j]);
            }
        }
        return result;
    }
}
