using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToLeft : MonoBehaviour
{

    [SerializeField] float speed = 3f;
    private void Update()
    {
        this.transform.Translate(Vector2.left * speed * Time.deltaTime);
    }
}
