using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorScript : MonoBehaviour
{
    GameObject[] cubesToAnimate;

    public GameObject cubePrefab;

    private int cubeAmount = 20;

    public void Start()
    {
        cubesToAnimate = new GameObject[cubeAmount];

        for (int index = 0; index < cubeAmount; index++)
        {
            Vector3 pos = new Vector3();
            cubesToAnimate[index] = Instantiate(cubePrefab, pos, Quaternion.identity);
            cubesToAnimate[index].AddComponent<CustomAnimationClass>();
            cubesToAnimate[index].GetComponent<CustomAnimationClass>().Clear();

            cubesToAnimate[index].GetComponent<CustomAnimationClass>().keyFrames = new KeyFrame[0];

            Vector3 zeroPos = new Vector3
                    (
                    Random.Range(0f, 25f),
                    Random.Range(0f, 25f),
                    Random.Range(0f, 25f)
                    );

            Vector3 zeroRot = new Vector3
                (
                Random.Range(0f, 360f),
                Random.Range(0f, 360f),
                Random.Range(0f, 360f)
                );

            cubesToAnimate[index].GetComponent<CustomAnimationClass>().AddKF(zeroPos, zeroRot, 0f);

            int temp_amountOfKFToAdd = 8;

            for (int kfindex = 0; kfindex < temp_amountOfKFToAdd; ++kfindex)
            {
                Vector3 kfpos = new Vector3
                    (
                    Random.Range(0f, 25f),
                    Random.Range(0f, 25f),
                    Random.Range(0f, 25f)
                    );

                Vector3 rot = new Vector3
                    (
                    Random.Range(0f, 360f),
                    Random.Range(0f, 360f),
                    Random.Range(0f, 360f)
                    );
                cubesToAnimate[index].GetComponent<CustomAnimationClass>().AddKF(kfpos, rot, Random.Range(0.3f, (float)temp_amountOfKFToAdd));
            }

            cubesToAnimate[index].transform.position = cubesToAnimate[index].GetComponent<CustomAnimationClass>().keyFrames[0].storedPosition;
        }

        
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int index = 0; index < cubeAmount; ++index)
        {
            cubesToAnimate[index].GetComponent<CustomAnimationClass>().Update();
        }
    }
}
