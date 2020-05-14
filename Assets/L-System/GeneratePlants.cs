using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePlants : MonoBehaviour
{
    private int algae_pop = 50;
    private int coral_pop = 20;
    public LSystem algae; 
    public CoralLSystem coral; 
    private float range = 100f;

    private Vector3 center = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start() {
        // generate algae
        for (int i = 0; i < algae_pop; ++i) {
            // get random position on plane
            Vector2 circle_pos = Random.insideUnitCircle.normalized * Random.Range(range - 40f, range);
            Vector3 pos =  new Vector3(circle_pos.x, 0, circle_pos.y);
            // Vector3 pos = new Vector3(Random.Range(-range, range),
            //                           0,
            //                           Random.Range(-range, range));
            LSystem a = Instantiate(algae, pos, Quaternion.identity);
            if (Vector3.Distance(center, pos) < range - 20f) a.depth = Random.Range(2, 4);
            a.depth = Random.Range(4, 6);
            a.length = Random.Range(0.35f, 0.5f);
        }

        // generate corals
        for (int i = 0; i < coral_pop; ++i) {
            // get random position on plane
            Vector2 circle_pos = Random.insideUnitCircle.normalized * (float)Random.Range(75, 100);
            Vector3 pos =  new Vector3(circle_pos.x, 0, circle_pos.y);
            // Vector3 pos = new Vector3(Random.Range(-range, range),
            //                           0,
            //                           Random.Range(-range, range));
            CoralLSystem a = Instantiate(coral, pos, Quaternion.identity);
            a.depth = Random.Range(5, 10);
            a.length = Random.Range(7f, 10f);
        }
    }

}
