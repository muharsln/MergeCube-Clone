using UnityEngine;

public class Ball : MonoBehaviour
{
    private bool _isMoved;

    [SerializeField] private GameObject _trace;

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
        }
    }
}