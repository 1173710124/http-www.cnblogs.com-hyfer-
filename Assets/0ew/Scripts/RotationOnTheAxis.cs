using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace mod1
{
    public class RotationOnTheAxis : MonoBehaviour
    {
        public GameObject RotatePoint = null;
        public float rotationSpeedX = 90;
        public float rotationSpeedY = 0;
        public float rotationSpeedZ = 0;

        // Update is called once per frame
        void Update()
        {

            transform.RotateAround(RotatePoint.transform.position, new Vector3(0, 0, 1), GameManager.Instance.rotateSpeed * Time.deltaTime);
        }
    }
}