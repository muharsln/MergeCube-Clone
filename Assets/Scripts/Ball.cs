using System;
using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private string _upgradeObjectTag;

    private bool _hasCollided;
    private bool _isMoved;

    private int _goldAmount, _diamondAmount;

    Rigidbody _rb;

    private void OnDisable()
    {
        //OBJECT POOL DAN ALIP GERÝ GÖNDERDÝÐÝMÝZ ÝÇÝN BUNLARI SIFIRLAMAMIZ GEREKÝYOR
        _isMoved = false;
        _hasCollided = false;
        //GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

    private void Start()
    {
        this._rb = this.GetComponent<Rigidbody>();

        _goldAmount = PlayerPrefs.GetInt("Gold");
        _diamondAmount = PlayerPrefs.GetInt("Diamond");
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
                    this._rb.constraints = RigidbodyConstraints.None;
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

    private void DiamondSave()
    {
        GameManager.Instance.diamondText.text = _diamondAmount.ToString();
        PlayerPrefs.SetInt("Diamond", _diamondAmount);
    }

    #region Create Ball
    void CreateBall(Transform target)
    {
        _goldAmount++;
        GameManager.Instance.goldText.text = _goldAmount.ToString();
        PlayerPrefs.SetInt("Gold", _goldAmount);

        switch (_upgradeObjectTag)
        {
            case "128":
                _diamondAmount++;
                DiamondSave();
                break;
            case "256":
                _diamondAmount += 2;
                DiamondSave();
                break;
            case "512":
                _diamondAmount += 3;
                DiamondSave();
                break;
            case "1024":
                _diamondAmount += 4;
                DiamondSave();
                break;
            case "2048":
                _diamondAmount += 5;
                DiamondSave();
                break;
            default:
                break;
        }

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