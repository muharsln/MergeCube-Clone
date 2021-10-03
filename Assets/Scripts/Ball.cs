using System;
using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private string _upgradeObjectTag;

    private Animator _animator;

    private bool _hasCollided;
    private bool _isMoved;

    private int _goldAmount;
    private int _diamondAmount;

    Rigidbody _rb;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnDisable()
    {
        //OBJECT POOL DAN ALIP GERÝ GÖNDERDÝÐÝMÝZ ÝÇÝN BUNLARI SIFIRLAMAMIZ GEREKÝYOR
        _isMoved = false;
        _hasCollided = false;
    }

    private void OnEnable()
    {
        if (transform.parent != null)
        {
            if (!gameObject.CompareTag("2") && transform.parent.CompareTag("Player")) _animator.enabled = false;
            else if (!gameObject.CompareTag("2")) _animator.enabled = true;
        }
    }

    private void Start()
    {
        //  this._rotationAnimate = this.GetComponent<Animation>();
        _rb = GetComponent<Rigidbody>();
        _goldAmount = PlayerPrefs.GetInt("Gold");
        _diamondAmount = PlayerPrefs.GetInt("Diamond");
    }

    private void OnCollisionEnter(Collision other)
    {
        // Küp'ün player'den ayrýlma kýsmý bu blokta yapýlýyor.
        if (!GameManager.Instance.gameStopped && !_isMoved && other.gameObject.layer == 6 || other.gameObject.layer == 7)
        {
            transform.GetChild(GameManager.Trail).gameObject.SetActive(false);
            //this.gameObject.layer = 6;
            _isMoved = true;
            _rb.constraints = RigidbodyConstraints.None;
            Player.Instance.CreatePlayerBall();
        }

        // Küpleri birleþtirme bloðu.
        if (other.gameObject.layer == 6 && transform.CompareTag(other.transform.tag) && !other.transform.CompareTag("2048"))
        {
            _goldAmount++;
            Debug.Log(_goldAmount);
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

            if (other.gameObject.GetComponent<Ball>()._hasCollided) return;

            other.gameObject.GetComponent<Ball>()._hasCollided = true;
            _hasCollided = true;

            StartCoroutine(MoveToTarget(transform, other.transform, 5f, 0.05f, _upgradeObjectTag));
            return;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Fail olma bölümü(Buglar düzeltilecek).
        if (other.gameObject.CompareTag("StartPointLine") && _isMoved)
        {
            other.gameObject.GetComponent<BoxCollider>().isTrigger = false;
            GameManager.Instance.GameFail();
        }
    }
    private void DiamondSave()
    {
        GameManager.Instance.diamondText.text = _diamondAmount.ToString();
        PlayerPrefs.SetInt("Diamond", _diamondAmount);
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
        yield return null;
    }
    #endregion

    #region Create Ball
    void CreateBall(Transform target)
    {
        GameObject GO = ObjectPooler.SharedInstance.GetPooledObject(_upgradeObjectTag);
        GO.transform.position = transform.position;
        GO.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        GO.transform.GetChild(GameManager.Square).gameObject.SetActive(false);
        GO.transform.GetChild(GameManager.Trail).gameObject.SetActive(false);

        GO.SetActive(true);

        Rigidbody rb = GO.GetComponent<Rigidbody>();
        rb.AddForce(0f, 100f, 0f, ForceMode.Impulse);
    }
    #endregion
}