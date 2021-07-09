using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager I { get; private set; }

    public float xAxis;
    public bool keyJump;

    private void Awake() => I = this;

    void Update()
    {
        xAxis = Input.GetAxis("Horizontal");

        keyJump = Input.GetButtonDown("Jump");
    }
}
