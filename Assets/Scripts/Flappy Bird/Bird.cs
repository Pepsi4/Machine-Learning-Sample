using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlappyBird
{
    public class Bird : MonoBehaviour
    {
        [SerializeField] Level Level;
        [SerializeField] float jumpForce = 10f;
        private Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }

        public void Jump()
        {
            Debug.Log("Jump");
            rb.velocity = Vector2.up * jumpForce;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent<PointTrigger>(out PointTrigger trigger)) return;


            OnDie();
        }

        public void Reset()
        {
            this.transform.position = Vector3.zero;
            rb.velocity = Vector3.zero;
        }

        //private void OnCollisionEnter2D(Collision2D collision)
        //{
        //    //if (collision.gameObject.TryGetComponent<Pipe>(out Pipe pipe) == false) return;


        //    Debug.Log(collision.gameObject.name);
        //    OnDie();
        //}
        public Vector3  GetVelocity()
        {
            return rb.velocity;
        }
        private void OnDie()
        {
            Level.Restart();
        }
    }
}