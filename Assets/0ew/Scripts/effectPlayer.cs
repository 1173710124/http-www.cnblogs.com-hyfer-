using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace mod1
{
    public class effectPlayer : MonoBehaviour
    {
        public AudioClip bullet;
        public AudioClip hitPlayer;
        public AudioClip hitFlame;
        public AudioClip hitCircle;
        public static effectPlayer instance = null;
        // Use this for initialization
        void Start()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void playEffect(int cmd)
        {
            if (cmd == 1) gameObject.GetComponent<AudioSource>().clip = bullet;
            if (cmd == 2) gameObject.GetComponent<AudioSource>().clip = hitPlayer;
            if (cmd == 3) gameObject.GetComponent<AudioSource>().clip = hitFlame;
            if (cmd == 4) gameObject.GetComponent<AudioSource>().clip = hitCircle;
            gameObject.GetComponent<AudioSource>().Play();
        }
    }
}