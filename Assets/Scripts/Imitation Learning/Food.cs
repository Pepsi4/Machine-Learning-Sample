using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    private void OnEnable()
    {
       
    }

    public void SetRandomPos()
    {
        this.transform.localPosition = new Vector3(Random.Range(-2.5f, 2f), transform.localPosition.y, Random.Range(3.5f, 2.5f));
    }
}
