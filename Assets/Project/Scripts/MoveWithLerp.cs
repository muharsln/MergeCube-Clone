using System.Collections;
using UnityEngine;


public class MoveWithLerp : MonoBehaviour
{
    public static IEnumerator MoveToTarget(Transform follower, Vector3 TargetPos, float speed, float time)
    {
        //LERP DEÐERLERÝ
        float elapsedTime = 0;
        float waitTime = 0.3f;
        Vector3 currentPos = follower.transform.position;
        Debug.Log("test");
        ///LERP ÝLE OBJEDEN OBJEYE GÝDÝYOR
        while (elapsedTime < time)
        {
            follower.transform.position = Vector3.Lerp(currentPos, TargetPos, (elapsedTime / waitTime) * speed);
            elapsedTime += Time.deltaTime;

            Debug.Log("while calisiyor");
            yield return null;
        }

        Player.instance.CreateNewBall();
    }
}

