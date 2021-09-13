using UnityEngine;

public class Ball : MonoBehaviour
{
    [HideInInspector] public bool hasCollided = false;

    [HideInInspector] public bool _isMoved;

    [SerializeField] private string _tag;

    [SerializeField] private GameObject _trace;

    [SerializeField] private TrailRenderer _trailRenderer;


    private void OnDisable()
    {
        _isMoved = false;
        hasCollided = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }


    private void OnCollisionEnter(Collision other)
    {
        if (GameManager.instance._gameStopped == false)
        {
            if (this._isMoved == false)
            {
                if (other.gameObject.layer == 6 || other.gameObject.layer == 7)
                {
                    this.gameObject.layer = 6;
                    this._isMoved = true;
                    this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    Player.instance.CreateNewBall();
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

            StartCoroutine(GameManager.instance.MoveToTarget(this.transform, other.transform, 2f, 0.15f, _tag));
            return;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("StartPointLine") && this._isMoved == true)
        {
            other.gameObject.GetComponent<BoxCollider>().isTrigger = false;
            GameManager.instance.GameFail();
        }
    }

    private void Update()
    {
        // Top hareket ediyorsa önünde bulunan iz objesinin görünürlüðünü pasif et.
        if (Player.instance.isMoving)
        {
            _trace.SetActive(false);
            _trailRenderer.emitting = true;
        }
    }
}