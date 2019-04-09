using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace mod1
{
    public class GameManager : MonoBehaviour
    {
        public float rotateSpeed;//炮口旋转速度
        public float bulletSpeed;//子弹速度
        public bool circleButton;
        public bool flameButton;
        public GameObject[] flames;
        public GameObject[] circles;
        public Text scoreDash = null;
        static int flameCount = 3;
        static int circleCount = 5;
        public static float stageWeight;
        public static float stageHeight;
        public static GameManager Instance = null; //实例化GameManager类，便于其他对象访问
        private GameObject bug1;
        public Plane[] pan;
        private int roller;
        public StageClass stage;
        public TextAsset stageNow;
        private int timeCount = 0;
        private int hitPoint = 0;
        private int hitCount = 0;
        private float gameRate = 1f;
        private int myRoller;
        private float nowX, nowY;
        public int score = 0;
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
            stageHeight = Camera.main.orthographicSize;
            stageWeight = stageHeight * Camera.main.aspect;
            stage = new StageClass(stageNow.text);
            
            hitCount = stage.HitObjects.LisHitObjects.Count;
            hitPoint = timeCount = 0;
        }
        private void Start()
        {

            Camera cam = Camera.main;
            pan = GeometryUtility.CalculateFrustumPlanes(cam);
            if (circleButton) InvokeRepeating("generateCircle", 0, 0.5f);
            if (flameButton) InvokeRepeating("generateFlame", 0, 1.0f);
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        }
        private void generateCircle()
        {
            Instantiate(circles[(int)Random.Range(0, circleCount)], new Vector3(Random.Range(-stageWeight, stageWeight), Random.Range(-stageHeight, stageHeight), 0), Quaternion.identity);
        }
        private void generateFlame()
        {
            bug1 = Instantiate(flames[Random.Range(0, flameCount)]);
            bug1.transform.position = new Vector3(Random.Range(-stageWeight, stageWeight), Random.Range(-stageHeight, stageHeight), 0);

        }
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(KeyCode.Q))
            {
                SceneManager.LoadScene(0);
            }

            scoreDash.GetComponent<Text>().text = "您当前的分数为：" + score.ToString();
            timeCount += (int)((Time.deltaTime * 1000) * gameRate);
            if (hitPoint >= hitCount) return;
            Debug.Log(hitCount+" "+timeCount+" "+ stage.HitObjects.LisHitObjects[hitPoint].IntTime);
            /*  if (stage.HitObjects.LisHitObjects[hitPoint].IntEndtime - timeCount >= 3000) gameRate = 5;
              else gameRate = 1;*/

           

            if (stage.HitObjects.LisHitObjects[hitPoint].IntTime <= timeCount)
            {
                //Debug.Log(stage.HitObjects.LisHitObjects[hitPoint].IntEndtime);
                nowX = -stageWeight + (stageWeight * (stage.HitObjects.LisHitObjects[hitPoint].Vec2Position.x / 512f)) * 2;
                nowY = -stageWeight + (stageWeight * (stage.HitObjects.LisHitObjects[hitPoint].Vec2Position.y / 512f)) * 2;
               // Debug.Log(nowX + " " + nowY);
                myRoller = Random.Range(1, 10);
                if (myRoller <= 6)
                {
                    Instantiate(circles[(int)Random.Range(0, circleCount)], new Vector3(nowX, nowY, 0), Quaternion.identity);
                }
                else
                {
                    bug1 = Instantiate(flames[Random.Range(0, flameCount)]);
                    bug1.transform.position = new Vector3(nowX, nowY, 0);
                }
                hitPoint++;
            }
            //stage.HitObjects.LisHitObjects.
        }
    }
}