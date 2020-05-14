using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequriedComponent(typeof(MeshFilter))] 
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] indices;
    Vector2[] uvs;

    public int width = 30;
    public int length = 30;
    public int radius;
    public Material land_material;

    private float min_height, max_height;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        radius = width / 2 - width / 5;

        min_height = 10000000.0f;
        max_height = 0.0f;

        CreateMesh();
        UpdateMesh(); 
    }

    void CreateMesh() {
        // create vertices
        vertices = new Vector3[(width + 1) * (length + 1)];
        int i = 0;
        int dec = 1;
        for (int z = 0; z <= length; z++) {
            float offset = Mathf.RoundToInt(Random.Range(0.0f, 5.0f));

            for (int x = 0; x <= width; x++) {
                Vector2 center = new Vector2(width / 2.0f, length / 2.0f);
                // Vector2 curr = new Vector2(x, z);
                // float dist = Vector2.Distance(curr, center);
                // float y;
                // if (dist > radius) {
                //     y = Mathf.PerlinNoise(x * .05f, z * .05f);
                //     y *= Mathf.RoundToInt(dist) - radius;

                //     // mountain height curve
                //     y +=  5.0f * Mathf.Sin((dist - radius) / (width - radius));
                
                // } else {
                //     y = Mathf.PerlinNoise(x * .1f, z * .1f);
                // }
                vertices[i] = new Vector3(x, 0, z);

                // find min and max height
                // if (y < min_height) {
                //     min_height = y;
                // } else if (y > max_height) {
                //     max_height = y;
                // }
                i++;
            }
        }

        // each square mesh has 2 triangles w 3 indices each
        indices = new int[width * length * 6];
        int vs = 0, ts = 0;
        for (int z = 0; z < length; z++, vs++) {
            for (int x = 0; x < width; x++, vs++) {
                indices[ts] = vs;
                indices[ts + 1] = vs + width + 1;
                indices[ts + 2] = vs + 1;
                indices[ts + 3] = vs + 1;
                indices[ts + 4] = vs + width + 1;
                indices[ts + 5] = vs + width + 2;
                ts += 6;
            }
        }

        //i = 0;
        // colors = new Color[vertices.Length];
        // for (int z = 0; z <= length; z++) {
        //     for (int x = 0; x <= width; x++) {
        //         float height = Mathf.InverseLerp(min_height, max_height, vertices[i].y);
        //         colors[i] = gradient.Evaluate(height);
        //         i++;
        //     }
        // }

    }

    // Update is called once per frame
    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = indices;
        //mesh.colors = colors;
        mesh.RecalculateNormals();
    }

    // vertex renderer for testing
    private void OnDrawGizmos() {
        if (vertices == null) return;
        
        for (int i = 0; i < vertices.Length; ++i) {
            Gizmos.DrawSphere(vertices[i], .1f);
        }
    }
}
