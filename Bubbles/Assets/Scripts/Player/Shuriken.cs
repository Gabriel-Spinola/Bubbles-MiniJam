using UnityEngine;
using Resources;

public class Shuriken : MonoBehaviour
{
    [SerializeField] private float speed = 10f;

    void Update()
    {
        Vector3 dir = StaticRes.LookDir(transform.position, true);

        transform.Translate(dir.normalized * speed * Time.deltaTime);
    }
}
