using UnityEngine;

[System.Serializable]
public class ObjectPoolItem
{
    public int amountToPool;

    public GameObject objectToPool;

    public Transform parent;

    public bool shouldExpand;
}