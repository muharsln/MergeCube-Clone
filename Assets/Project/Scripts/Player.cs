using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    //PUBLIC OBJECTS
    [SerializeField] private Transform _eCubesParent;

    // KARAKTERIN SAGA SOLA KAYDIRILMASI ���N GGEREKL�
    private Vector3 _mouse_pos_start;
    private Vector3 _mouse_pos;
    private float _position_x;
    private float _sphere_x;

    //TOP �LER� G�D�YORSA KONTROL� KAPATMAK ��N�
    [HideInInspector] public bool isMoving;

    private static int Square = 0, Trail = 1;

    //TAG LIST
    private string[] _tags = { "2", "4", "8", "16", "32", "64" };

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        CubeMovement();
    }

    void CubeMovement()
    {
        if (GameManager.Instance._gameStopped == false)
        {
            if (isMoving == false)
            {
                //SA�A SOLA HAREKET
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
                    /// CH�LD OBJEY� AL
                    GameObject playerBall = this.transform.GetChild(0).gameObject;
                    Vector3 targetPos = playerBall.transform.localPosition;
                    targetPos.z = 7.8f;


                    //CH�LD OBJEY� FORCE �LE �LER� TA�I
                    playerBall.GetComponent<Rigidbody>().AddForce(0f, 0f, 15f, ForceMode.Impulse);

                    
                    playerBall.transform.GetChild(Square).gameObject.SetActive(false);
                    playerBall.transform.GetChild(Trail).gameObject.SetActive(true);

                    //PARENT�N� DE���
                    playerBall.transform.parent = _eCubesParent;

                    isMoving = true;
                }
            }
        }
    }

    public void CreatePlayerBall()
    {
        if (this.transform.childCount < 1)
        {
            //YEN� OBJE OLU�TUR
            string tag = _tags[Random.Range(0, 5)];
            GameObject newBall = ObjectPooler.SharedInstance.GetPooledObject(tag);
            newBall.transform.parent = this.transform;
            newBall.transform.localPosition = new Vector3(0f, 0.3f, 0f);
            newBall.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            newBall.transform.GetChild(Square).gameObject.SetActive(true);
            newBall.transform.GetChild(Trail).gameObject.SetActive(false);
            newBall.SetActive(true);
            newBall.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
  
            isMoving = false;
        }
    }
}