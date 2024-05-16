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
    private int WavePoints = 2, xSize = 10, ySize = 5, yRows = 3;

    [SerializeField]
    private float wavelength, speed, amplitude, yDisplacement, xClamp, yClamp, playerX;

    private float height;
    private float width;


    void GenerateMesh () {
        var mesh = new Mesh {
            name = "Procedural Mesh"
        };

        int index = 0;
        int check = 0;
        Vector3[] points = new Vector3[(WavePoints + 2) * (yRows)];
        int[] triangles = new int[6 * (WavePoints + 1) * (yRows - 1)];

        height = (float)ySize / ((float)yRows);
        width = (float)xSize / ((float)WavePoints + 2);

        for (int j = 0, i = 0; i < yRows; i++) {
            for (int g = 0; g < WavePoints + 2; g++) {
                points[j] = new Vector3(width * g, height * i, 0);
                j++;
            }
        }

        mesh.vertices = points;

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

        height = (float)ySize / ((float)yRows);
        width = (float)xSize / ((float)WavePoints + 2);
        
        float k = (2 * Mathf.PI * Mathf.Deg2Rad) / wavelength; 
        int sides = 0;
        int endVert = vertices.Length - 1;

        for (int i = endVert; i > endVert - (WavePoints + 2); i--) {
            if (sides == WavePoints + 1) {
                sides = 0;
                continue;
            } else if (sides == 0) {
                sides++; 
                i--;
            }
            
            float pX = vertices[endVert].x - sides * width;
            float f = k * (vertices[i].x - speed * (Time.fixedTime - playerX / 2));
            float x = (pX) + xClamp * (Mathf.Cos(pX + (speed * Time.fixedTime)) * Mathf.Cos(xClamp * pX + (speed * Time.fixedTime)));
            float y = amplitude * Mathf.Sin(f) * Mathf.Sin(yClamp * f) + yDisplacement; 
            
            vertices[i] = new Vector3(x, y, 0);
            
            sides++;
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
