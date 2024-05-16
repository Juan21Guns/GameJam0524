using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class WaveMeshGenerator : MonoBehaviour
{
    #if UNITY_EDITOR
    [ContextMenu("Construct sQuad")]
        void ConstrucutMesh () {
            Debug.Log("constructing waves");
            GenerateMesh();
        }
    #endif

    [SerializeField]
    private int WavePoints = 2, xSize = 10, ySize = 5, yRows = 3, iff = 1;

    [SerializeField]
    private float wavelength, speed, amplitude, yDisplacement;

    void GenerateMesh () {
        var mesh = new Mesh {
            name = "Procedural Mesh"
        };
        
        float width = (float)xSize / ((float)WavePoints + 2);
        float height = (float)ySize / ((float)yRows);

        Vector3[] points = new Vector3[(WavePoints + 2) * (yRows)];
        for (int j = 0, i = 0; i < yRows; i++) {
            for (int g = 0; g < WavePoints + 2; g++) {
                points[j] = new Vector3(width * g, height * i, 0);
                j++;
            }
        }

        mesh.vertices = points;

        int[] triangles = new int[6 * (WavePoints + 1) * (yRows - 1)];
        // triangles[0] = 0;
        // triangles[1] = (WavePoints + 2);
        // triangles[2] = 1;
        // triangles[3] = (WavePoints + 2);
        // triangles[4] = (WavePoints + 3);
        // triangles[5] = 1;

        int index = 0;
        int check = 0;
        for (int i = 0; i < (WavePoints + 1) * (yRows - 1) + (yRows - 2); i++) {
            if (check == WavePoints + 1) {
                check = 0;
                continue;
            }

            triangles[index] = i;
            triangles[index + 1] = i + (WavePoints) + 2; 
            triangles[index + 2] = i + 1;
            triangles[index + 3] = i + WavePoints + 2;
            triangles[index + 4] = i + WavePoints + 3;
            triangles[index + 5] = i + 1;

            index += 6;
            check++;
        }

        mesh.triangles = triangles;

        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void UpdateMesh() {
        var meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = meshFilter.mesh;
        Vector3[] vertices = mesh.vertices;
        
        float k = (2 * Mathf.PI * Mathf.Deg2Rad) / wavelength; 

        for (int i = vertices.Length - 1; i > vertices.Length - (WavePoints + 3); i--) {
            float f = k * (vertices[i].x - speed * Time.fixedTime);
            float y = amplitude * Mathf.Sin(f) + yDisplacement; 
            vertices[i] = new Vector3(vertices[i].x, y, 0);
        }

        mesh.vertices = vertices;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMesh();       
    }
}
