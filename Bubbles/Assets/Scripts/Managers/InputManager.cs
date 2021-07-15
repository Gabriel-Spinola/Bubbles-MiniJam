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
    public bool keyJumpHold;
    public bool btnThrowShuriken;

    private void Awake() => I = this;

    void Update()
    {
        if (PauseMenu.gameIsPaused)
            return;

        xAxis = Input.GetAxisRaw("Horizontal");

        keyD = Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
        keyA = Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow);
        keyS = Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow);

        keyJump = Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.UpArrow);
        keyJumpHold = Input.GetButton("Jump") || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.UpArrow);

        btnThrowShuriken = Input.GetButtonDown("Fire1");

        if (Input.GetKeyDown(KeyCode.RightAlt)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
