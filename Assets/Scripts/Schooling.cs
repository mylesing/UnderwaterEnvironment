using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Schooling : MonoBehaviour {
    public float speed;
    float rot_speed = 4.0f;
    Vector3 avg_pos;
    Vector3 avd_dir;
    float school_dist = 40.0f;
    bool reverse = false;

    // Start is called before the first frame update
    void Start() {
        speed = Random.Range(1f, 5f);
    }

    // Update is called once per frame
    void Update() {

        reverse = (Vector3.Distance(transform.position, Vector3.zero) >= SchoolGlobals.radius);

        if (reverse) {
            Vector3 dir = -1f *  transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, 
                                                  Quaternion.LookRotation(dir),
                                                  rot_speed * Time.deltaTime);
        }
        //only check for schooling 1/5th of the time
        if (Random.Range(0, 10) < 0.5) {
            SchoolingBehavior();
        }

        // move fish accordingly
        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    void SchoolingBehavior() {
        GameObject[] school = SchoolGlobals.school;

        Vector3 center = new Vector3(0, 0, 0);
        Vector3 avoid = new Vector3(0, 0, 0);
        float school_speed = 0.1f;

        Vector3 target = SchoolGlobals.target;
        float dist = 0;
        int size = 0;

        // for each fish:
        // check distance between this fish and other fish
        // if within bounds of what the school radius should be, 
        // we include that fish in the school
        foreach (GameObject g in school) {
            //if (size > 8) break;
            if (g != this.gameObject) {
                dist = Vector3.Distance(g.transform.position, transform.position);
                if (dist <= school_dist && size < 8) {
                    center += g.transform.position;
                    size++;
                    if (dist < 30.0f) {
                        avoid += (transform.position - g.transform.position);
                    }

                    Schooling other = g.GetComponent<Schooling>();
                    school_speed += other.speed;
                }
            }
        }

        // once we've found all fish within appropriate radius, we lead them towards the radius
        if (size > 0) {
            center /= size;
            center += (target - transform.position);
            speed = school_speed / size;
            if (speed >= 20f) {
                speed = 19f;
            }

            Vector3 dir = (center + avoid) - transform.position;
            if (dir != new Vector3(0, 0, 0)) {
                transform.rotation = Quaternion.Slerp(transform.rotation, 
                                                      Quaternion.LookRotation(dir),
                                                      rot_speed * Time.deltaTime);
            }


        }

    }
}
