using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [SerializeField] private Transform _enemyCubesParent;

    // Karakterin saða sola kaydýrýlmasý için gerekli olan deðiþkenler.
    private Vector3 _mousePosStart;
    private Vector3 _mousePos;

    private float _posX;
    private float _sphereX;

    // Top ileri gidiyorsa kontrolü kapatmak için.
    private bool _isMoving;

    // Tag listesi.
    private string[] _tags = { "2", "4", "8", "16", "32", "64" };

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        CubeMovement();
    }

    #region Cube Movement
    void CubeMovement()
    {
        if (!GameManager.Instance.gameStopped && !_isMoving)
        {
            //SAÐA SOLA HAREKET
            if (Input.GetMouseButtonDown(0))
            {
                _mousePosStart = Input.mousePosition;
                _posX = gameObject.transform.GetChild(0).transform.localPosition.x;
            }

            if (Input.GetMouseButton(0))
            {
                _mousePos = Input.mousePosition;
                _sphereX = ((_mousePos.x - _mousePosStart.x) / (Screen.width / 30f)) + _posX;

                if (_sphereX > 1.5f) _sphereX = 1.5f;

                if (_sphereX < -1.5f) _sphereX = -1.5f;

                gameObject.transform.GetChild(0).transform.localPosition = new Vector3(
                    Mathf.Lerp(transform.GetChild(0).transform.localPosition.x, _sphereX, 0.1f),
                    transform.GetChild(0).localPosition.y,
                    transform.GetChild(0).transform.localPosition.z);
            }

            if (Input.GetMouseButtonUp(0))
            {
                // Child objeyi al.
                GameObject playerBall = transform.GetChild(0).gameObject;
                Vector3 targetPos = playerBall.transform.localPosition;
                targetPos.z = 7.8f;

                // Child objeyi force ile ileri taþý.
                playerBall.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                playerBall.GetComponent<Rigidbody>().AddForce(0f, 0f, 480f, ForceMode.Impulse);

                playerBall.transform.GetChild(GameManager.Square).gameObject.SetActive(false);
                playerBall.transform.GetChild(GameManager.Trail).gameObject.SetActive(true);

                // Parentini deðiþ.
                playerBall.transform.parent = _enemyCubesParent;

                _isMoving = true;
            }
        }
    }
    #endregion

    #region Create Player Ball
    public void CreatePlayerBall()
    {
        if (transform.childCount < 1)
        {
            // Yeni obje oluþtur.
            string tag = _tags[Random.Range(0, 5)];

            GameObject newBall = ObjectPooler.SharedInstance.GetPooledObject(tag);
            newBall.transform.parent = transform;
            newBall.transform.localPosition = new Vector3(0f, 0.3f, 0f);
            newBall.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            newBall.transform.GetChild(GameManager.Square).gameObject.SetActive(true);
            newBall.transform.GetChild(GameManager.Trail).gameObject.SetActive(false);
            newBall.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            // newBall.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            newBall.SetActive(true);

            _isMoving = false;
        }
    }
    #endregion
}