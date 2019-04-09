using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace mod1
{
    public class Flame : MonoBehaviour
    {
        public GameObject bullet;
        public GameObject nowBullet;
        private float bulletAngle;
        public float repeatTime;
        public float sleepTime;
        // Use this for initialization
        void Start()
        {
            InvokeRepeating("GenerateBallet", sleepTime, repeatTime);
            //Debug.Log(Quaternion.identity);
            //Debug.Log(this.transform.rotation);

        }

        // Update is called once per frame
        void Update()
        {

        }
        private void GenerateBallet()
        {

            for (int i = 0; i < 360; i += 30)
            {
                nowBullet = Instantiate(bullet, gameObject.transform.position, Quaternion.identity);
                nowBullet.transform.Rotate(new Vector3(0, 0, i));
                bulletAngle = nowBullet.transform.rotation.eulerAngles.z - 90.0f;
                nowBullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(50.0f * Mathf.Cos(bulletAngle / 180f * Mathf.PI), 50.0f * Mathf.Sin(bulletAngle / 180f * Mathf.PI)));
            }



            //Debug.Log(arrow.transform.rotation.eulerAngles);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {


            if (collision.gameObject.tag == "bullet")
            {
                GameObject.Find("scoreTip").GetComponent<Text>().color = Color.yellow;
                GameObject.Find("scoreTip").GetComponent<Text>().text = "您消灭了一个火炬！它再也不能威胁到您了";
                effectPlayer.instance.playEffect(3);
                Destroy(gameObject);
            }
        }
    }
}