using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Transform EnemyCubes;

    [HideInInspector] public bool _gameStopped;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void GameFail()
    {
        Debug.Log("Game Fail");
        _gameStopped = true;
    }


    public IEnumerator MoveToTarget(Transform follower, Transform Target, float speed, float time, string tag)
    {
        //LERP DEÐERLERÝ
        float elapsedTime = 0;
        float waitTime = 0.3f;
        Vector3 currentPos = follower.transform.position;

        ///LERP ÝLE OBJEDEN OBJEYE GÝDÝYOR
        while (elapsedTime < time)
        {
            follower.transform.position = Vector3.Lerp(currentPos, Target.position, (elapsedTime / waitTime) * speed);
            elapsedTime += Time.deltaTime;

            Debug.Log("while calisiyor");
            yield return null;
        }


        follower.gameObject.SetActive(false);
        Target.gameObject.SetActive(false);

        // 2 OBJENÝN BÝRLEÞTÝÐÝ KISIM
        GameObject GO = ObjectPooler.SharedInstance.GetPooledObject(tag);
        ////GO.GetComponent<Ball>().hasCollided = false;
        GO.transform.parent = EnemyCubes;
        GO.transform.position = follower.transform.position;
        GO.transform.rotation = follower.transform.rotation;
        GO.SetActive(true);
    }
}