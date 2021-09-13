using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    //PUBLIC OBJECTS
    [SerializeField] private Transform _eCubesParent;

    // KARAKTERIN SAGA SOLA KAYDIRILMASI ÝÇÝN GGEREKLÝ
    private Vector3 _mouse_pos_start;
    private Vector3 _mouse_pos;
    private float _position_x;
    private float _sphere_x;

    //TOP ÝLERÝ GÝDÝYORSA KONTROLÜ KAPATMAK ÝÇNÝ
    public bool isMoving;

    //TAG LIST
    private string[] _tags = { "2", "4", "8", "16", "32", "64" };

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        CubeMovement();
    }

    void CubeMovement()
    {
        if (GameManager.instance._gameStopped == false)
        {
            if (isMoving == false)
            {
                //SAÐA SOLA HAREKET
                if (Input.GetMouseButtonDown(0))
                {
                    _mouse_pos_start = Input.mousePosition;
                    _position_x = this.gameObject.transform.GetChild(0).transform.localPosition.x;
                }

                if (Input.GetMouseButton(0))
                {
                    _mouse_pos = Input.mousePosition;
                    _sphere_x = ((_mouse_pos.x - _mouse_pos_start.x) / (Screen.width / 30f)) + _position_x;


                    if (_sphere_x > 1.5f)
                    {
                        _sphere_x = 1.5f;
                    }
                    if (_sphere_x < -1.5f)
                    {
                        _sphere_x = -1.5f;
                    }

                    this.gameObject.transform.GetChild(0).transform.localPosition = new Vector3(
                        Mathf.Lerp(this.transform.GetChild(0).transform.localPosition.x, _sphere_x, 0.1f),
                        this.transform.GetChild(0).localPosition.y,
                        this.transform.GetChild(0).transform.localPosition.z);
                }

                if (Input.GetMouseButtonUp(0))
                {
                    /// OBJEYI LERP ÝLE ÝLERÝYE TAÞI
                    GameObject playerBall = this.transform.GetChild(0).gameObject;
                    Vector3 targetPos = playerBall.transform.localPosition;
                    targetPos.z = 7.8f;

                    playerBall.GetComponent<Rigidbody>().AddForce(0f, 0f, 15f, ForceMode.Impulse);
                    // StartCoroutine(MoveWithLerp.MoveToTarget(playerBall.transform, targetPos, 0.5f, 0.5f));

                    //PARENTÝNÝ DEÐÝÞ
                    playerBall.transform.parent = _eCubesParent;

                    isMoving = true;
                }
            }
        }
    }

    public void CreateNewBall()
    {
        if (this.transform.childCount < 1)
        {
            //YENÝ OBJE OLUÞTUR
            string tag = _tags[Random.Range(0, 5)];
            GameObject newBall = ObjectPooler.SharedInstance.GetPooledObject(tag);
            newBall.transform.parent = this.transform;
            newBall.transform.localPosition = new Vector3(0f, 0.4f, 0f);
            newBall.SetActive(true);

            newBall.layer = 0;

            isMoving = false;
        }
    }
}