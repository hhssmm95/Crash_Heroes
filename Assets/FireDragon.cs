using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FireDragon : MonoBehaviourPunCallbacks, IPunObservable
{
    public float originZ;
    Animator dragonAnimator;
    float speed = 10;
    //bool turn;
    public CharacterMove Dragoon;
    public GameObject flame;
    //Quaternion newRotation;
    Vector3 dir;
    Vector3 masterPos;
    public bool isCheck;
    bool destroying;
    void Awake()
    {

        dragonAnimator = GetComponent<Animator>();
        //flame = GameObject.FindGameObjectWithTag("DragonBreath");
        Dragoon = GameObject.FindGameObjectWithTag("Dragoon").GetComponent<CharacterMove>();
        dir = Dragoon.transform.forward;
        masterPos = Dragoon.transform.position;
    }
    void Start()
    {
        //originY = transform.position.y;
        originZ = transform.localPosition.z;
        Vector3 dir = Dragoon.transform.forward;
        dragonAnimator.SetBool("Gliding", true);

    }

    void Update()
    {
        Move();
        //photonView.RPC("Move", RpcTarget.All);
        //if(turn)
        //{

        //    newRotation = Quaternion.LookRotation(new Vector3(0, 0, -1.0f));
        //    transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, speed/3 * Time.deltaTime);
        //}
    }

    //[PunRPC]
    void Move()
    {
        if (dragonAnimator.GetBool("Gliding"))
        {
            //if (transform.localPosition.z <= originZ + 22.0f && transform.localPosition.z >= originZ + 18.0f)
            if (isCheck)
            {
                dragonAnimator.SetBool("Gliding", false);

                dragonAnimator.SetBool("HoverBasic", true);
                dragonAnimator.SetBool("Hover", true);
                StartCoroutine("DragonBreath");
                return;
            }
            else
            {
                transform.position += transform.forward * speed * Time.deltaTime;
            }
        }

        if (dragonAnimator.GetBool("Flying"))
        {

            transform.position += transform.forward * speed * Time.deltaTime;
            if(!destroying)
            {
                destroying = true;
                Destroy(gameObject, 2.5f);
            }
        }
        
    }

        IEnumerator DragonBreath()
        {
            //flame.Play();
            flame.SetActive(true);
            yield return new WaitForSeconds(2.5f);
            flame.SetActive(false);
            //turn = true;
            //yield return new WaitForSeconds(1.0f);
            dragonAnimator.SetBool("HoverBasic", false);
            dragonAnimator.SetBool("Hover", false);
            dragonAnimator.SetBool("Flying", true);
        }
        // Update is called once per frame


        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            //throw new System.NotImplementedException();
        }
    
}
