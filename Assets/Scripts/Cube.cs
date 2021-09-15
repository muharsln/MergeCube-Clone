using System;
using System.Collections;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private string _upgradeObjectTag;

    private bool _hasCollided;
    private bool _isMoved;

    Rigidbody _rb;

    private void OnDisable()
    {
        //OBJECT POOL DAN ALIP GER� G�NDERD���M�Z ���N BUNLARI SIFIRLAMAMIZ GEREK�YOR
        _isMoved = false;
        _hasCollided = false;
        //GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

<<<<<<< HEAD:Assets/Scripts/Ball.cs
    private void Start()
    {
        this._rb = this.GetComponent<Rigidbody>();
    }

=======
>>>>>>> Muhammed:Assets/Scripts/Cube.cs
    private void OnCollisionEnter(Collision other)
    {
        // K�p'�n player'den ayr�lma k�sm� bu blokta yap�l�yor.
        if (!GameManager.Instance.gameStopped)
        {
            if (!_isMoved)
            {
                if (other.gameObject.layer == 6 || other.gameObject.layer == 7)
                {
<<<<<<< HEAD:Assets/Scripts/Ball.cs

                    this.transform.GetChild(GameManager.Trail).gameObject.SetActive(false);

                    //this.gameObject.layer = 6;
                    this._isMoved = true;
                    this._rb.constraints = RigidbodyConstraints.None;
                    Player.Instance.CreatePlayerBall();
=======
                    transform.GetChild(GameManager.Trail).gameObject.SetActive(false);
                    //gameObject.layer = 6;
                    _isMoved = true;
                    gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    Player.Instance.CreatePlayerCube();
>>>>>>> Muhammed:Assets/Scripts/Cube.cs
                }
            }
        }

        // K�pleri birle�tirme blo�u.
        if (other.gameObject.layer == 6 && transform.tag == other.transform.tag && other.transform.tag != "2048")
        {
            if (other.gameObject.GetComponent<Cube>()._hasCollided)
            {
                return;
            }

            other.gameObject.GetComponent<Cube>()._hasCollided = true;
            _hasCollided = true;

            StartCoroutine(MoveToTarget(transform, other.transform, 2f, 0.1f, _upgradeObjectTag));
            return;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Fail olma b�l�m�(Buglar d�zeltilecek).
        if (other.gameObject.CompareTag("StartPointLine") && _isMoved)
        {
            other.gameObject.GetComponent<BoxCollider>().isTrigger = false;
            GameManager.Instance.GameFail();
        }
    }

    #region Move To Target
    public IEnumerator MoveToTarget(Transform follower, Transform Target, float speed, float time, string tag)
    {
        // Lerp de�erleri.
        float elapsedTime = 0;
        float waitTime = 0.3f;

        Vector3 currentPos = follower.transform.position;

        // Lerp ile objeden di�er objeye do�ru ilerliyor.
        while (elapsedTime < time)
        {
            follower.transform.position = Vector3.Lerp(currentPos, Target.position, (elapsedTime / waitTime) * speed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        follower.gameObject.SetActive(false);
        Target.gameObject.SetActive(false);

        CreateBall(Target);
    }
    #endregion

    #region Create Ball
    void CreateBall(Transform target)
    {
        GameObject GO = ObjectPooler.SharedInstance.GetPooledObject(_upgradeObjectTag);
        GO.transform.position = target.transform.position;
        GO.transform.rotation = target.transform.rotation;

        GO.transform.GetChild(GameManager.Square).gameObject.SetActive(false);
        GO.transform.GetChild(GameManager.Trail).gameObject.SetActive(false);

        GO.SetActive(true);

        Rigidbody rb = GO.GetComponent<Rigidbody>();
        rb.AddForce(0f, 4f, 0f, ForceMode.Impulse);
    }
    #endregion
}