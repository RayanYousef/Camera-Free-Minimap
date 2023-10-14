using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CameraFreeMinimap
{
    public class SpawnCar : MonoBehaviour
    {



        [SerializeField] Car carObject;

        [SerializeField] Color[] carColours;

        float timer;

        private void Awake()
        {
            timer = Random.Range(0, 6f);
        }
        private void FixedUpdate()
        {
            timer += Time.fixedDeltaTime;

            if(timer >10)
            {
                Car car = Instantiate(carObject, transform.position, transform.rotation,transform);

                car.SetCarColor(carColours[Random.Range(0, carColours.Length)]);

                timer = Random.Range(0, 8f);
            }    

        }
    }
}
