using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [HideInInspector] public bool hasCollided = false;

    [HideInInspector] public bool _isMoved;

    [SerializeField] private string _tag;


    private static int Square = 0, Trail = 1;


    private void OnDisable()
    {
        _isMoved = false;
        hasCollided = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }


    private void OnCollisionEnter(Collision other)
    {
        //KÜPÜN PLAYER DEN AYRILMA KISMI BURADAN YAPILIYOR
        if (GameManager.Instance._gameStopped == false)
        {
            if (this._isMoved == false)
            {
                if (other.gameObject.layer == 6 || other.gameObject.layer == 7)
                {
                    
                    this.transform.GetChild(Trail).gameObject.SetActive(false);

                    //this.gameObject.layer = 6;
                    this._isMoved = true;
                    this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    Player.Instance.CreatePlayerBall();
                }
            }
        }

        //BÝRLEÞTÝRME
        if (other.gameObject.layer == 6 && this.transform.tag == other.transform.tag && other.transform.tag != "2048")
        {
            if (other.gameObject.GetComponent<Ball>().hasCollided == true)
            {
                return;
            }


            other.gameObject.GetComponent<Ball>().hasCollided = true;
            this.hasCollided = true;

            StartCoroutine(MoveToTarget(this.transform, other.transform, 2f, 0.1f, _tag));
            return;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // FAIL OLMA BÖLÜMÜ // BÝRAZ BUGLU DÜZENLENECEK
        if (other.gameObject.CompareTag("StartPointLine") && this._isMoved == true)
        {
            other.gameObject.GetComponent<BoxCollider>().isTrigger = false;
            GameManager.Instance.GameFail();
        }
    }


    public IEnumerator MoveToTarget(Transform follower, Transform Target, float speed, float time, string tag)
    {
        //LERP DEÐERLERÝ
        float elapsedTime = 0;
        float waitTime = 0.3f;
        Vector3 currentPos = follower.transform.position;

        ///LERP ÝLE OBJEDEN OBJEYE GÝDÝYOR
        while (elapsedTime < time)
        {
            follower.transform.position = Vector3.Lerp(currentPos, Target.position, (elapsedTime / waitTime) * speed);
            elapsedTime += Time.deltaTime;

            Debug.Log("while calisiyor");
            yield return null;
        }


        follower.gameObject.SetActive(false);
        Target.gameObject.SetActive(false);

        CreateBall(Target);
    }

    void CreateBall(Transform target)
    {
        GameObject GO = ObjectPooler.SharedInstance.GetPooledObject(_tag);
        GO.transform.GetChild(Square).gameObject.SetActive(false);
        GO.transform.GetChild(Trail).gameObject.SetActive(false);
        GO.transform.position = target.transform.position;
        GO.transform.rotation = target.transform.rotation;
        GO.SetActive(true);
    }
}