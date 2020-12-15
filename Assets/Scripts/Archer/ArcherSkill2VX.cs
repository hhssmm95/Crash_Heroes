using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ArcherSkill2VX : MonoBehaviourPunCallbacks, IPunObservable
{
    CharacterMove Archer;
    float atk;
    void Start()
    {
        Archer = GameObject.FindGameObjectWithTag("Archer").GetComponent<CharacterMove>();
        atk = Archer.GetComponent<CharacterMove>().atk;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Archer") && other.gameObject.layer == 9)
        {
            CharacterMove enemy = other.GetComponent<CharacterMove>();

            enemy.GetComponent<PhotonView>().RPC("OnHeavyDamage", RpcTarget.All, atk * 2.6f, transform.forward, Archer.gameObject.tag);
            Debug.Log(tag + "스킬이 " + enemy.gameObject.name + "에게 " + atk * 2.6f + "감소 전 피해를 입힘.");
            var effect = PhotonNetwork.Instantiate("Prefebs/Effect_18_HitEffect", new Vector3(enemy.transform.position.x, enemy.transform.position.y + 0.5f, enemy.transform.position.z), Quaternion.LookRotation(-transform.forward) * enemy.transform.rotation);
            StartCoroutine(destroyEffect(effect));
            //SoundManager.Instance.HitSoundPlay(0);
            //hit = true;
        }
    }
    IEnumerator destroyEffect(GameObject effect)
    {
        yield return new WaitForSeconds(1.5f);
        if (effect != null)
            PhotonNetwork.Destroy(effect);
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }

}
