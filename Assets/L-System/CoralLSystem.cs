using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoralLSystem : MonoBehaviour {
    private string axiom = "F";
    private string curr_str;
    private Stack<Turtle> turtle_stack = new Stack<Turtle>();

    // dictionaries
    private Dictionary<char, string> coral_rules = new Dictionary<char, string>();

    public float length = 10f;
    private float angle = 30.0f;
    public float depth = 10;
    private int type = 1;

    public GameObject coral;


    // Start is called before the first frame update
    void Start() {
        // F -> up
        // + -> turn right (along forward)
        // - -> turn left (along forward)
        // R -> rotate about up axis
        // S -> decrease scale
        // [] -> pop on / off stack

        coral_rules.Add('X', "X");
        coral_rules.Add('F', "X[R-F]R+F");

        // set up string
        curr_str = "F";
        for (int i = 0; i < depth; i++) {
            string new_str = "";

            foreach(char c in curr_str) {
                if (coral_rules.ContainsKey(c)) {
                    new_str += coral_rules[c];
                } else {
                    new_str += c.ToString();
                }
            }

            curr_str = new_str;
            Debug.Log(curr_str);
        }

        // generate coral
        Generate();
        
    }

    void Generate() {
        int iter = 0;

        foreach (char c in curr_str) {
            
            if (c == 'F' || c == 'X') {
                // move up / 'forward'
                Vector3 init = transform.position;
                float r = Random.Range(0.5f, 0.9f);
                transform.Translate(Vector3.up * r * length);

                // draw branch
                Vector3 draw_pos = (init + transform.position + Vector3.up * 0.2f) / 2f;
                GameObject a = Instantiate(coral, draw_pos, transform.rotation);
                if (iter == 0) a.transform.localScale = new Vector3(length * 0.2f, length * 0.38f, length * 0.2f);
                else a.transform.localScale = new Vector3(length * 0.15f, length * 0.4f, length * 0.15f);
               
               //Debug.DrawLine(init, transform.position, Color.white, 100000f, false);
            } else if (c == 'R') {
                // rotate on own up axis
                float a = Random.Range(-60f, 60f);
                transform.Rotate(Vector3.up * a);
            } else if (c == '+') {
                // rotate right
                float a = Random.Range(30f, 45f);
                transform.Rotate(Vector3.forward * a);
            } else if (c == '-') {
                // rotate left
                float a = Random.Range(30f, 45f);
                transform.Rotate(Vector3.forward * -a);
            } else if (c == '[') {
                // store current settings
                Turtle t = new Turtle();
                t.position = transform.position;
                t.rotation = transform.rotation;
                t.length = length;
                turtle_stack.Push(t);

                // shorten length
                length /= 1.2f;
            } else if (c == ']') {
                // get previous settings
                Turtle t = turtle_stack.Pop();
                transform.position = t.position;
                transform.rotation = t.rotation;
                length = t.length;

                // fix length
                length /= 1.2f;
            }

            iter++;
        }

    }
}
