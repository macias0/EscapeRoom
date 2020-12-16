using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
public class MeshDebug : MonoBehaviour
{
    public bool active = true;

    int selectedTriangleIndex = -1;
    // Start is called before the first frame update
    void OnDrawGizmos()
    {
        if (!active)
            return;
        var mesh = gameObject.GetComponentInChildren<MeshFilter>().mesh;
        var vertices = mesh.vertices;
        bool[] visited = new bool[mesh.vertexCount];
        
        int i = 0;

        Dictionary<Vector3, string> labels = new Dictionary<Vector3, string>();
        foreach(var t in mesh.triangles)
        {
            //int count = vertices.Where(x => x == vertices[t]).Count();
            // Debug.Log("COUNT: " + count);
            //Handles.color = Color.white;
            if (!visited[t] )
            {
                
                //Debug.Log("Triangle: " + i + ":" + t);
                Vector3 newPos = gameObject.transform.TransformPoint(vertices[t]);
                if(labels.ContainsKey(newPos))
                {
                    labels[newPos] += ", " + t.ToString();
                }
                else
                {
                    labels[newPos] = t.ToString();
                }
                //if (i == 3 * selectedTriangleIndex)
                
                    //Debug.Log("Rysuje selected na pozycji: " + newPos);
                    //Handles.color = Color.red;
                    //Handles.Label(newPos, t.ToString());
                //Debug.Log("Rysuje dla vertexu: " + t);

                
                visited[t] = true;
            }
            i++;
        }

        //foreach (KeyValuePair<Vector3, string> entry in labels)
        //{
        //    Handles.Label(entry.Key, entry.Value);
        //}
        Debug.Log("KONIEC");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit, 1000.0f))
            {
                selectedTriangleIndex = hit.triangleIndex;
                Debug.Log("Wcelowalem w obiekt: " + hit.collider.gameObject + "Triangle: " + hit.triangleIndex);
            }
        }
    }
}





