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

    // Küp ileri gitme hýzý
    [SerializeField] private float _cubeForwardMoveSpeed;

    // Küp ileri gidiyorsa kontrolü kapatmak için.
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
        if (!GameManager.Instance.gameStopped)
        {
            if (!_isMoving)
            {
                // Saða sola hareket kodlarý.
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
                    GameObject playerCube = transform.GetChild(0).gameObject;
                    Vector3 targetPos = playerCube.transform.localPosition;
                    targetPos.z = 7.8f;

                    // Child objeyi force ile ileri taþý.
                    playerCube.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                    playerCube.GetComponent<Rigidbody>().AddForce(Vector3.forward * _cubeForwardMoveSpeed * Time.deltaTime, ForceMode.Impulse);

                    playerCube.transform.GetChild(GameManager.Square).gameObject.SetActive(false);
                    playerCube.transform.GetChild(GameManager.Trail).gameObject.SetActive(true);

                    // Parentini deðiþ.
                    playerCube.transform.parent = _enemyCubesParent;

                    _isMoving = true;
                }
            }
        }
    }
    #endregion

    #region Create Player Cube
    public void CreatePlayerCube()
    {
        if (transform.childCount < 1)
        {
            // Yeni obje oluþtur.
<<<<<<< HEAD
            string tag = _tags[Random.Range(0, 5)];

            GameObject newBall = ObjectPooler.SharedInstance.GetPooledObject(tag);
            newBall.transform.parent = this.transform;
            newBall.transform.localPosition = new Vector3(0f, 0.3f, 0f);
            newBall.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            newBall.transform.GetChild(GameManager.Square).gameObject.SetActive(true);
            newBall.transform.GetChild(GameManager.Trail).gameObject.SetActive(false);
            newBall.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            // newBall.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            newBall.SetActive(true);


=======
            string tag = _tags[Random.Range(0, 6)];

            GameObject newCube = ObjectPooler.SharedInstance.GetPooledObject(tag);
            newCube.transform.parent = transform;
            newCube.transform.localPosition = new Vector3(0f, 0.6f, 0f);
            newCube.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            newCube.transform.GetChild(GameManager.Square).gameObject.SetActive(true);
            newCube.transform.GetChild(GameManager.Trail).gameObject.SetActive(false);
            newCube.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            // newCube.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            newCube.SetActive(true);
>>>>>>> Muhammed

            _isMoving = false;
        }
    }
    #endregion
}