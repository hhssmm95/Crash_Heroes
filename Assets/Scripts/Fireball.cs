using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 10.0f;
    Rigidbody myRig;
    GameObject explosion;
    bool deadTimer;
    float timer;

    void Start()
    {
        myRig = GetComponent<Rigidbody>();
        explosion = GameObject.FindGameObjectWithTag("FireballExp");

        myRig.AddForce(Vector3.forward * speed, ForceMode.Impulse);

    }

    private void OnCollisionEnter(Collision collision)
    {
        explosion.SetActive(true);
        deadTimer = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (deadTimer)
            timer += Time.deltaTime;
        if (timer >= 0.5f)
            Destroy(gameObject);
    }
}
