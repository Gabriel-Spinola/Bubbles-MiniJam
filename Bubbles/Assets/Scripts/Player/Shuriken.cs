using UnityEngine;
using Resources;
using System.Collections;

public class Shuriken : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float reflectForce = 20f;

    void Update()
    {
        Vector3 dir = StaticRes.LookDir(transform.position, true);

        transform.Translate(dir.normalized * speed * Time.deltaTime);
    }

    public IEnumerator DieOnTimer(float time)
    {
        yield return new WaitForSeconds(time);

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Ue");

        var dir = collision.gameObject.transform.position - transform.position;

        transform.Translate(dir * reflectForce * Time.deltaTime);
    }
}
