using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FireDragon : MonoBehaviourPunCallbacks, IPunObservable
{
    private void Start()
    {
        Destroy(this.gameObject, 3.0f);
    }

    void Update()
    {
        transform.position += transform.forward * 13.0f * Time.deltaTime;
    }
    
    //    public float originZ;
    //    Animator dragonAnimator;
    //    float speed = 10;
    //    //bool turn;
    //    public CharacterMove Dragoon;
    //    public GameObject flame;
    //    public GameObject breathPos;
    //    //Quaternion newRotation;
    //    Vector3 dir;
    //    Vector3 masterPos;
    //    public bool isCheck;
    //   // public bool breathEnd;
    //    //float breathEndTimer;
    //    bool destroying;

    //    float soundTimer;
    //    bool soundBool;
    //    void Awake()
    //    {

    //        dragonAnimator = GetComponent<Animator>();
    //        //flame = GameObject.FindGameObjectWithTag("DragonBreath");
    //        Dragoon = GameObject.FindGameObjectWithTag("Dragoon").GetComponent<CharacterMove>();
    //        dir = Dragoon.transform.forward;
    //        masterPos = Dragoon.transform.position;
    //    }
    //    void Start()
    //    {
    //        //originY = transform.position.y;
    //        originZ = transform.localPosition.z;
    //        Vector3 dir = Dragoon.transform.forward;
    //        dragonAnimator.SetBool("Gliding", true);

    //    }

    //    void Update()
    //    {
    //        Move();
    //        //photonView.RPC("Move", RpcTarget.All);
    //        //if(turn)
    //        //{

    //        //    newRotation = Quaternion.LookRotation(new Vector3(0, 0, -1.0f));
    //        //    transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, speed/3 * Time.deltaTime);
    //        //}


    //        if (soundBool)
    //        {
    //            soundTimer += Time.deltaTime;
    //        }

    //        if (soundBool && soundTimer >= 1.0f)
    //        {
    //            soundTimer = 0;
    //            SoundManager.Instance.DragonSoundPlay(0);
    //        }
    //        //if (breathEnd)
    //        //{
    //        //    breathEndTimer += Time.deltaTime;
    //        //    if (breathEndTimer >= 2.5f)
    //        //    {
    //        //        breathEnd = false;
    //        //        dragonAnimator.SetBool("HoverBasic", false);
    //        //        dragonAnimator.SetBool("Hover", false);
    //        //        dragonAnimator.SetBool("Flying", true);
    //        //    }

    //        //}
    //    }

    //    //[PunRPC]
    //    void Move()
    //    {
    //        if (dragonAnimator.GetBool("Gliding"))
    //        {
    //            //if (transform.localPosition.z <= originZ + 22.0f && transform.localPosition.z >= originZ + 18.0f)
    //            if (isCheck)
    //            {
    //                soundBool = false;
    //                soundTimer = 0;
    //                dragonAnimator.SetBool("Gliding", false);

    //                dragonAnimator.SetBool("HoverBasic", true);
    //                dragonAnimator.SetBool("Hover", true);
    //                StartCoroutine("DragonBreath");
    //                return;
    //            }
    //            else
    //            {
    //                transform.position += transform.forward * speed * Time.deltaTime;
    //                if (!soundBool)
    //                    soundBool = true;
    //            }
    //        }

    //        if (dragonAnimator.GetBool("Flying"))
    //        {
    //            if (!soundBool)
    //                soundBool = true;
    //            transform.position += transform.forward * speed * Time.deltaTime;
    //            if (!destroying)
    //            {
    //                destroying = true;
    //                Destroy(gameObject, 2.5f);
    //            }
    //        }

    //    }

    //    [PunRPC]
    //    void Breath()
    //    {
    //        if (photonView.IsMine)
    //        {
    //            PhotonNetwork.Instantiate("Prefebs/Flames", breathPos.transform.position, breathPos.transform.rotation);

    //        }
    //    }

    //    IEnumerator DragonBreath()
    //    {

    //        //breathEnd = true;
    //        //flame.Play();
    //        //flame.SetActive(true);
    //        if (photonView.IsMine)
    //            photonView.RPC("Breath", RpcTarget.All);
    //        SoundManager.Instance.DragonSoundPlay(1);
    //        yield return new WaitForSeconds(3.0f);
    //        //flame.SetActive(false);
    //        //turn = true;
    //        //yield return new WaitForSeconds(1.0f);

    //        dragonAnimator.SetBool("HoverBasic", false);
    //        dragonAnimator.SetBool("Hover", false);
    //        dragonAnimator.SetBool("Flying", true);

    //        yield return null;
    //    }
    //    // Update is called once per frame


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
   {
       //throw new System.NotImplementedException();
   }

}
