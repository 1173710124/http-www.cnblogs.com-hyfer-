using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace mod1
{
    public class Player : MonoBehaviour
    {
        public GameObject bullet = null;
        public GameObject arrow = null;
        private GameObject newArrow = null;
        private float bulletAngle = 0;
        private float deltaX;
        private float deltaY;

        private bool inKey = false;
        // Use this for initialization
        void tryMove()
        {
            transform.Translate(deltaX, deltaY, 0);

        }
        void Start()
        {
            // InvokeRepeating("GenerateBallet", 0f,GameManager.Instance.generateSpeed * Time.deltaTime);
            arrow = GameObject.Find("Arrow");
        }

        // Update is called once per frame
        void Update()
        {
            inKey = false;
            if (Input.GetKey(KeyCode.X))
            {
                GenerateBallet();
                GameManager.Instance.score -= 1;
            }
            if (Input.GetKey(KeyCode.Z))
            {
                GameManager.Instance.rotateSpeed -= 3;
                GameManager.Instance.score -= 1;

            }
            if (Input.GetKey(KeyCode.C))
            {
                GameManager.Instance.rotateSpeed += 3;
                GameManager.Instance.score -= 1;

            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                deltaX = -0.2f;
                deltaY = 0;
                inKey = true;
                //gameObject.GetComponent<Animator>().SetTrigger("goLeft");
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                deltaX = 0.2f;
                deltaY = 0;
                inKey = true;
                //gameObject.GetComponent<Animator>().SetTrigger("goRight");
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                deltaX = 0;
                deltaY = -0.2f;
                inKey = true;
                //gameObject.GetComponent<Animator>().SetTrigger("goDown");
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                deltaX = 0;
                deltaY = 0.2f;
                inKey = true;
                //gameObject.GetComponent<Animator>().SetTrigger("goUp");
            }

            if (inKey) tryMove();
            //Debug.Log(arrow.transform.rotation.eulerAngles.z);
            if (arrow.transform.rotation.eulerAngles.z >= 315.0f || arrow.transform.rotation.eulerAngles.z < 45.0f)
                gameObject.GetComponent<Animator>().SetTrigger("goDown");
            else if (arrow.transform.rotation.eulerAngles.z >= 45.0f && arrow.transform.rotation.eulerAngles.z < 135.0f)
                gameObject.GetComponent<Animator>().SetTrigger("goRight");
            else if (arrow.transform.rotation.eulerAngles.z >= 135.0f && arrow.transform.rotation.eulerAngles.z < 225.0f)
                gameObject.GetComponent<Animator>().SetTrigger("goUp");
            else gameObject.GetComponent<Animator>().SetTrigger("goLeft");

        }
        private void OnTriggerStay2D(Collider2D collision)
        {
           // if (collision.gameObject.tag.Equals("circle")) Debug.Log("fuck you");
            if (collision.gameObject.tag.Equals("enemybullet"))
            {
                GameManager.Instance.score -= 10;
                gameObject.GetComponent<SpriteRenderer>().color = new Color(0.3f, 0, 0, 1);
                GameObject.Find("scoreTip").GetComponent<Text>().color = Color.red;
                GameObject.Find("scoreTip").GetComponent<Text>().text = "您中弹了！减少分数10点";
                effectPlayer.instance.playEffect(2);
                Invoke("recoverFromBullet", 0.5f);
            }
            // Debug.Log("草拟吗");
            if (collision.gameObject.name.Equals("boardLeft")) gameObject.transform.position = new Vector3(collision.transform.position.x + 0.5f, gameObject.transform.position.y, gameObject.transform.position.z);
            if (collision.gameObject.name.Equals("boardRight")) gameObject.transform.position = new Vector3(collision.transform.position.x - 0.5f, gameObject.transform.position.y, gameObject.transform.position.z);
            if (collision.gameObject.name.Equals("boardUp")) gameObject.transform.position = new Vector3(gameObject.transform.position.x, collision.transform.position.y - 0.5f, gameObject.transform.position.z);
            if (collision.gameObject.name.Equals("boardBottom")) gameObject.transform.position = new Vector3(gameObject.transform.position.x, transform.transform.position.y + 0.5f, gameObject.transform.position.z);

        }
        private void GenerateBallet()
        {
            effectPlayer.instance.playEffect(1);
            newArrow = Instantiate(bullet, arrow.transform.position, Quaternion.identity);

            bulletAngle = arrow.transform.rotation.eulerAngles.z - 90.0f;
            newArrow.GetComponent<Rigidbody2D>().AddForce(new Vector2(GameManager.Instance.bulletSpeed * Mathf.Cos(bulletAngle / 180f * Mathf.PI), GameManager.Instance.bulletSpeed * Mathf.Sin(bulletAngle / 180f * Mathf.PI)));
            //Debug.Log(arrow.transform.rotation.eulerAngles);
        }
        private void recoverFromBullet()
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }

    }
}