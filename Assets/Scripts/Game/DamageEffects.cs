using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffects : MonoBehaviour
{

    private ObjectPool parentPool;
    public void SetParentPool(ObjectPool parent)
    {
        parentPool = parent;
    }
    void OnEnable()
    {
        StartCoroutine(Dispose());
    }

    private IEnumerator Dispose()
    {
        yield return new WaitForSeconds(1f);
        parentPool.DestroyObject(gameObject);
    }
}
