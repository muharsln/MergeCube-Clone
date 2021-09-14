using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private string _upgradeObjectTag;

    private bool _hasCollided;
    private bool _isMoved;

    private void OnDisable()
    {
        _isMoved = false;
        _hasCollided = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }


    private void OnCollisionEnter(Collision other)
    {
        // Küp'ün player'den ayrýlma kýsmý bu blokta yapýlýyor.
        if (GameManager.Instance.gameStopped == false)
        {
            if (this._isMoved == false)
            {
                if (other.gameObject.layer == 6 || other.gameObject.layer == 7)
                {

                    this.transform.GetChild(GameManager.Trail).gameObject.SetActive(false);

                    //this.gameObject.layer = 6;
                    this._isMoved = true;
                    this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    Player.Instance.CreatePlayerBall();
                }
            }
        }

        // Küpleri birleþtirme bloðu.
        if (other.gameObject.layer == 6 && this.transform.tag == other.transform.tag && other.transform.tag != "2048")
        {
            if (other.gameObject.GetComponent<Ball>()._hasCollided == true)
            {
                return;
            }

            other.gameObject.GetComponent<Ball>()._hasCollided = true;
            this._hasCollided = true;

            StartCoroutine(MoveToTarget(this.transform, other.transform, 2f, 0.1f, _upgradeObjectTag));
            return;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Fail olma bölümü(Buglar düzeltilecek).
        if (other.gameObject.CompareTag("StartPointLine") && this._isMoved == true)
        {
            other.gameObject.GetComponent<BoxCollider>().isTrigger = false;
            GameManager.Instance.GameFail();
        }
    }

    #region Move To Target
    public IEnumerator MoveToTarget(Transform follower, Transform Target, float speed, float time, string tag)
    {
        // Lerp deðerleri.
        float elapsedTime = 0;
        float waitTime = 0.3f;

        Vector3 currentPos = follower.transform.position;

        // Lerp ile objeden diðer objeye doðru ilerliyor.
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
        GO.transform.GetChild(GameManager.Square).gameObject.SetActive(false);
        GO.transform.GetChild(GameManager.Trail).gameObject.SetActive(false);
        GO.transform.position = target.transform.position;
        GO.transform.rotation = target.transform.rotation;
        GO.SetActive(true);
    }
    #endregion
}