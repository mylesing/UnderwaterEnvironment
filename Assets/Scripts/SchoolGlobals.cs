using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchoolGlobals : MonoBehaviour {
    public GameObject fish;
    static int size = 30;
    public static float radius = 120f;
    public static GameObject[] school = new GameObject[size];
    public static Vector3 target = new Vector3(0, 30, 0);

    // Start is called before the first frame update
    void Start() {
        Vector3 camera = Camera.main.transform.position;
        for (int i = 0; i < size; ++i) {
            Vector3 pos = new Vector3(Random.Range(- radius, radius),
                                      Random.Range(3, 60),
                                      Random.Range(-radius, radius));
            school[i] = (GameObject) Instantiate(fish, pos, Quaternion.identity);
        }

        //target = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0, 100f) < 0.5f) {
            target = new Vector3(Random.Range(- radius, radius),
                                      Random.Range(3, 60),
                                      Random.Range(-radius, radius));
        }
    }
}
