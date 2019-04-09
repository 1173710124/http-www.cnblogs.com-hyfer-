using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace mod1
{
    public class Bullet : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            //Vector2 a = new Vector2(50, 50);
            // gameObject.GetComponent<Rigidbody2D>().AddForce(a);
        }

        // Update is called once per frame
        void Update()
        {
            // if (Mathf.Abs(transform.position.x) > GameManager.Instance.stageWeight || Mathf.Abs(transform.position.y) > GameManager.Instance.stageHeight) Destroy(gameObject);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag.Equals("board"))
            {
                Destroy(gameObject);
            }
        }
    }
}