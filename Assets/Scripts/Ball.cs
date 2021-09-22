using System;
using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private string _upgradeObjectTag;

    private Animator _animator;

    private bool _hasCollided;
    private bool _isMoved;

    [SerializeField] private ParticleSystem _collEffect;


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
        transform.GetChild(2).gameObject.SetActive(false); // DÝSABLE OLURKEN PARTÝCLEYÝ KAPATIYORUZ
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
        _rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        // Küp'ün player'den ayrýlma kýsmý bu blokta yapýlýyor.
        if (!GameManager.Instance.gameStopped && !_isMoved && other.gameObject.layer == 6 || other.gameObject.layer == 7)
        {
            transform.GetChild(GameManager.Instance.Trail).gameObject.SetActive(false);
            _isMoved = true;
            _rb.constraints = RigidbodyConstraints.None;
            Player.Instance.CreatePlayerBall();
        }

        // Küpleri birleþtirme bloðu.
        if (other.gameObject.layer == 6 && transform.CompareTag(other.transform.tag) && !other.transform.CompareTag("2048"))
        {
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
        GameManager.Instance.diamondText.text = GameManager.Instance._diamondAmount.ToString();
        PlayerPrefs.SetInt("Diamond", GameManager.Instance._diamondAmount);
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
        GameManager.Instance._goldAmount++;
        GameManager.Instance.goldText.text = GameManager.Instance._goldAmount.ToString();
        PlayerPrefs.SetInt("Gold", GameManager.Instance._goldAmount);

        switch (_upgradeObjectTag)
        {
            case "128":
                GameManager.Instance._diamondAmount++;
                DiamondSave();
                break;
            case "256":
                GameManager.Instance._diamondAmount += 2;
                DiamondSave();
                break;
            case "512":
                GameManager.Instance._diamondAmount += 3;
                DiamondSave();
                break;
            case "1024":
                GameManager.Instance._diamondAmount += 4;
                DiamondSave();
                break;
            case "2048":
                GameManager.Instance._diamondAmount += 5;
                DiamondSave();
                break;
            default:
                break;
        }

        GameObject GO = ObjectPooler.SharedInstance.GetPooledObject(_upgradeObjectTag);
        GO.transform.position = transform.position;
        GO.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        GO.transform.GetChild(GameManager.Instance.Square).gameObject.SetActive(false);
        GO.transform.GetChild(GameManager.Instance.Trail).gameObject.SetActive(false);
        GO.transform.GetChild(GameManager.Instance.Particle).gameObject.SetActive(true);



        GO.SetActive(true);

        Rigidbody rb = GO.GetComponent<Rigidbody>();
        rb.AddForce(0f, 100f, 0f, ForceMode.Impulse);
    }
    #endregion
}