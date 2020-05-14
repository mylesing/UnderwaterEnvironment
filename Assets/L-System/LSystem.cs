using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSystem : MonoBehaviour {
    private string axiom = "F";
    private string curr_str;
    private Stack<Turtle> turtle_stack = new Stack<Turtle>();

    // dictionaries
    private Dictionary<char, string> algae_rules = new Dictionary<char, string>();
    private Dictionary<char, string> coral_rules = new Dictionary<char, string>();

    public float length = 0.5f;
    private float angle = 30.0f;
    public float depth = 5;
    public int type = 0;

    public GameObject algae;


    // Start is called before the first frame update
    void Start() {
        // F -> up
        // + -> turn right
        // - -> turn left
        // [] -> pop on / off stack
        algae_rules.Add('F', "F[+F]F[-F][F]");

        curr_str = axiom;

        for (int i = 0; i < depth; i++) {
            Generate();
        }

        algae.transform.localScale = new Vector3(length * 0.1f, length, length * 0.1f);
        
    }

    void Generate() {
        string new_str = "";

        foreach(char c in curr_str) {
            if (algae_rules.ContainsKey(c)) {
                if (type == 0) new_str += algae_rules[c];
            } else {
                new_str += c.ToString();
            }
        }

        curr_str = new_str;
        Debug.Log(curr_str);

        foreach (char c in curr_str) {
            // move up / 'forward'
            if (c == 'F') {
                float a = Random.Range(-60f, 60f);
                transform.Rotate(Vector3.up * a);
                Vector3 init = transform.position;
                transform.Translate(Vector3.up * length);
                Instantiate(algae, transform.position, transform.rotation);
                //Debug.DrawLine(init, transform.position, Color.white, 100000f, false);

            } else if (c == '+') {
                float a = Random.Range(10f, 45f);
                transform.Rotate(Vector3.forward * a);
            } else if (c == '-') {
                float a = Random.Range(10f, 45f);
                transform.Rotate(Vector3.forward * (-a));
            } else if (c == '[') {
                Turtle t = new Turtle();
                t.position = transform.position;
                t.rotation = transform.rotation;
                turtle_stack.Push(t);
            } else if (c == ']') {
                Turtle t = turtle_stack.Pop();
                transform.position = t.position;
                transform.rotation = t.rotation;
            }
        }
    }
}
