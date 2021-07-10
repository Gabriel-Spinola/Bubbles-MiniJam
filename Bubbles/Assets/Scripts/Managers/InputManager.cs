using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public static InputManager I { get; private set; }

    public float xAxis;

    public bool keyD;
    public bool keyA;
    public bool keyW;
    public bool keyS;

    public bool keyJump;
    public bool btnThrowShuriken;

    private void Awake() => I = this;

    void Update()
    {
        xAxis = Input.GetAxisRaw("Horizontal");

        keyD = Input.GetKeyDown(KeyCode.D);
        keyA = Input.GetKeyDown(KeyCode.A);
        keyW = Input.GetKeyDown(KeyCode.W);
        keyS = Input.GetKeyDown(KeyCode.S);

        keyJump = Input.GetButtonDown("Jump");

        btnThrowShuriken = Input.GetButtonDown("Fire1");

        if (Input.GetKeyDown(KeyCode.RightAlt)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
