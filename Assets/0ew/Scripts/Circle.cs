using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace mod1
{
    public class Circle : MonoBehaviour
    {
        public GameObject score;
        private Text scoreNow = null;
        public int scoreLeft = 400;
        private bool isDie = false;
        // Use this for initialization  
        private void Awake()
        {
            score = Instantiate(score, new Vector3(0, 0, 0), Quaternion.identity);
            score.transform.SetParent(GameObject.Find("Canvas").transform);
            score.GetComponent<RectTransform>().anchoredPosition = new Vector2(gameObject.GetComponent<Transform>().position.x * 100, gameObject.GetComponent<Transform>().position.y * 100 + 75);
            scoreNow = score.GetComponent<Text>();
           // Debug.Log(score.transform.eulerAngles);
        }


        // Update is called once per frame
        void Update()
        {
            if (isDie) return;
            scoreLeft -= 1;
            scoreNow.text = scoreLeft.ToString();
            if (scoreLeft <= 200) gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, scoreLeft / 200.0f);
            if (scoreLeft <= 0) suicide();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "player")
            {
                score.GetComponent<Text>().color = Color.green;
                score.GetComponent<Text>().text = "+" + scoreLeft.ToString();
                GameManager.Instance.score += scoreLeft;
                GameObject.Find("scoreTip").GetComponent<Text>().color = Color.green;
                GameObject.Find("scoreTip").GetComponent<Text>().text = "取得水晶！获得" + scoreLeft.ToString() + "分数！";
                effectPlayer.instance.playEffect(4);
                isDie = true;
                Invoke("suicide", 0.5f);
            }
        }
        private void suicide()
        {
            Destroy(score);
            Destroy(gameObject);
        }

    }
}