using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace CameraFreeMinimap
{
    public class Car : MonoBehaviour
    {

        float life = 13;
        float speed;
       
        public void SetCarColor (Color carColour)
        {
            GetComponent<MeshRenderer>().material.color = carColour;
            GetComponent<CFM_MinimapWorldElement>().MinimapIconColour = carColour;
            speed = Random.Range(8, 12f);
        }

        private void FixedUpdate()
        {

            transform.Translate(transform.forward * Time.deltaTime * speed, Space.World);
            life -= Time.fixedDeltaTime;

            if (life <= 0)
                Destroy(gameObject);
        }
    }
}
