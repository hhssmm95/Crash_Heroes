using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDragon : MonoBehaviour
{
    public float originZ;
    Animator dragonAnimator;
    float speed = 10;
    //bool turn;
    public FlamesToggle flame;
    Quaternion newRotation;
    void Start()
    {
        dragonAnimator = GetComponent<Animator>();
        //originY = transform.position.y;
        originZ = transform.localPosition.z;

        dragonAnimator.SetBool("Gliding", true);

    }

    void Update()
    {
        if(dragonAnimator.GetBool("Gliding"))
        {
            if(transform.localPosition.z <= originZ*2 -20.0f)
            {
                dragonAnimator.SetBool("Gliding", false);

                dragonAnimator.SetBool("HoverBasic", true);
                dragonAnimator.SetBool("Hover", true);
                StartCoroutine("DragonBreath");
                return;
            }
            transform.position += transform.forward * speed * Time.deltaTime;
        }

        if(dragonAnimator.GetBool("Flying"))
        {
            if (transform.localPosition.z <= originZ*2 -40.0f)
            {
                Destroy(gameObject);
                return;
            }
            transform.position += transform.forward * speed * Time.deltaTime;
        }

        //if(turn)
        //{

        //    newRotation = Quaternion.LookRotation(new Vector3(0, 0, -1.0f));
        //    transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, speed/3 * Time.deltaTime);
        //}
    }

    IEnumerator DragonBreath()
    {
        flame.activateFlame = true;
        flame.flameObject.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        flame.activateFlame = false;
        //turn = true;
        //yield return new WaitForSeconds(1.0f);
        dragonAnimator.SetBool("HoverBasic", false);
        dragonAnimator.SetBool("Hover", false);
        dragonAnimator.SetBool("Flying", true);
    }
    // Update is called once per frame
}
