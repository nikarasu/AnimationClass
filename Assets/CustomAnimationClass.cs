using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomAnimationClass : MonoBehaviour
{
    private int currentKeyFrame = 0;
    private int nextKeyFrame = 0;
    private int amountOfKFInArray = 0;

    private float totalTimeElapsed;
    private float elapsedTimeInCurrentFrame;
    
    //An array that stores key frames
    public KeyFrame[] keyFrames = new KeyFrame[0];

    //Function to add key frames
    public void AddKF(Vector3 aPosition, Vector3 aRotation, float aTimeStamp)
    {
        KeyFrame newKF;
        newKF.timeStamp = aTimeStamp;
        newKF.storedRotation = Quaternion.Euler(aRotation.x, aRotation.y, aRotation.z);
        newKF.storedPosition = new Vector3(aPosition.x, aPosition.y, aPosition.z); 
        
        SizeCheck(amountOfKFInArray);
        keyFrames[amountOfKFInArray] = newKF;
        amountOfKFInArray++;
        SortArray();
    }

    //Function to remove key frames
    public void RemoveKeyFrame(int aIndex)
    {
        int lastIndex = amountOfKFInArray - 1;

        if (aIndex < amountOfKFInArray && aIndex >= 0)
        {
            for (int index = aIndex; index < lastIndex; ++index)
            {
                keyFrames[index] = keyFrames[index + 1];
            }
            keyFrames[lastIndex] = new KeyFrame();
            amountOfKFInArray--;
        }
        else if(aIndex == lastIndex)
        {
            keyFrames[lastIndex] = new KeyFrame();
            amountOfKFInArray--;
        }
        else
        {
            Debug.Log("Remove Keyframe: Invalid index(" + aIndex + ")");
        }
    }

    public void Clear()
    {
        keyFrames = new KeyFrame[0];
        currentKeyFrame = 0;
        amountOfKFInArray = 0;
        totalTimeElapsed = 0f;
        elapsedTimeInCurrentFrame = 0f;
    }

    private void UpdateCurrentAndNextFrame()
    {
        if (currentKeyFrame >= amountOfKFInArray)
        {
            currentKeyFrame = 0;
        }

        nextKeyFrame = currentKeyFrame + 1;

        if (nextKeyFrame >= amountOfKFInArray)
        {
            nextKeyFrame = currentKeyFrame;
        }

    }

    private void ResetAnimation()
    {
        totalTimeElapsed = 0f;
        currentKeyFrame = 0;
        elapsedTimeInCurrentFrame = 0f;

        if (amountOfKFInArray > 0)
        {
            transform.position = keyFrames[0].storedPosition;
            transform.rotation = keyFrames[0].storedRotation;
        }
    }

    private void UpdateTime(float aDeltaTime)
    {
        totalTimeElapsed += aDeltaTime;
        elapsedTimeInCurrentFrame = totalTimeElapsed - keyFrames[currentKeyFrame].timeStamp;

        if (totalTimeElapsed > keyFrames[amountOfKFInArray - 1].timeStamp)
        {
            ResetAnimation();
        }
    }

    public void OnDrawGizmos()
    {
        for(int index = 0; index < amountOfKFInArray - 1; ++index)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(keyFrames[index].storedPosition, keyFrames[index + 1].storedPosition);
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(keyFrames[index].storedPosition, 1f);
        }
    }

    public void OnValidate()
    {
        if(amountOfKFInArray > 0)
        {
            if (totalTimeElapsed < 0f)
            {
                ResetAnimation();
            }
            else if (totalTimeElapsed > keyFrames[amountOfKFInArray - 1].timeStamp)
            {
                int lastIndex = amountOfKFInArray - 1;
                totalTimeElapsed = keyFrames[lastIndex].timeStamp;
                currentKeyFrame = lastIndex - 1;
                nextKeyFrame = lastIndex;

                float currKeyTimeStamp = keyFrames[lastIndex - 1].timeStamp;
                float nextKeyTimeStamp = keyFrames[lastIndex].timeStamp;
                float timeDiff = nextKeyTimeStamp - totalTimeElapsed;
                elapsedTimeInCurrentFrame = timeDiff;

                if (amountOfKFInArray > 0)
                {
                    transform.position = keyFrames[lastIndex].storedPosition;
                    transform.rotation = keyFrames[lastIndex].storedRotation;
                }
            }
            else
            {
                for (int index = 0; index < amountOfKFInArray; ++index)
                {
                    if (totalTimeElapsed <= keyFrames[index].timeStamp)
                    {
                        currentKeyFrame = index - 1;
                        nextKeyFrame = index;
                        elapsedTimeInCurrentFrame = totalTimeElapsed - keyFrames[currentKeyFrame].timeStamp;

                        Vector3 nextPos = keyFrames[nextKeyFrame].storedPosition;
                        Vector3 currKFPos = keyFrames[currentKeyFrame].storedPosition;
                        Vector3 currDistToNext = (nextPos - currKFPos);

                        float currKeyTimeStamp = keyFrames[currentKeyFrame].timeStamp;
                        float nextKeyTimeStamp = keyFrames[nextKeyFrame].timeStamp;

                        float timeDiff = Mathf.Abs(nextKeyTimeStamp - currKeyTimeStamp);

                        transform.position = keyFrames[currentKeyFrame].storedPosition + (currDistToNext * elapsedTimeInCurrentFrame / timeDiff);

                        index = int.MaxValue;
                    }
                }
            }
        }
    }

    public void Update()
    {
        if (keyFrames.Length > 1)
        {
            UpdateCurrentAndNextFrame();
            UpdateTime(Time.deltaTime);
            
            Vector3 nextPos = keyFrames[nextKeyFrame].storedPosition;
            Vector3 currKFPos = keyFrames[currentKeyFrame].storedPosition;
            Vector3 currDistToNext = (nextPos - currKFPos);

            float currKeyTimeStamp = keyFrames[currentKeyFrame].timeStamp;
            float nextKeyTimeStamp = keyFrames[nextKeyFrame].timeStamp;

            float timeDiff = nextKeyTimeStamp - currKeyTimeStamp;
            
            transform.position += (currDistToNext * Time.deltaTime / timeDiff);

            float dt = elapsedTimeInCurrentFrame / timeDiff;
            transform.rotation = Quaternion.Slerp(keyFrames[currentKeyFrame].storedRotation, keyFrames[nextKeyFrame].storedRotation, dt);

            if ((nextKeyTimeStamp - totalTimeElapsed) <= 0.01f)
            {
                currentKeyFrame++;
                elapsedTimeInCurrentFrame = 0f;
                transform.position = keyFrames[nextKeyFrame].storedPosition;
            }

        }
    }

    //Function to resize the array
    private void SizeCheck(int aSize)
    {
        if(aSize >= keyFrames.Length)
        {
            KeyFrame[] newArray;
            if (keyFrames.Length > 0)
            {
                newArray = new KeyFrame[2 * keyFrames.Length];
            }
            else
            {
                newArray = new KeyFrame[32];
            }

            for(int index = 0; index < keyFrames.Length; ++index)
            {
                newArray[index] = keyFrames[index];
            }
            keyFrames = newArray;
        }
    }

    //Function to sort the array
    public void SortArray()
    {
        KeyFrame temp;
        int smallestValIndex = 0;
        for(int sortedIndex = 0; sortedIndex < amountOfKFInArray; ++sortedIndex)
        {
            smallestValIndex = sortedIndex;

            for(int unsortedIndex = sortedIndex; unsortedIndex < amountOfKFInArray; ++unsortedIndex)
            {
                if(keyFrames[unsortedIndex].timeStamp < keyFrames[smallestValIndex].timeStamp)
                {
                    smallestValIndex = unsortedIndex;
                }
            }

            if(smallestValIndex != sortedIndex)
            {
                temp = keyFrames[sortedIndex];
                keyFrames[sortedIndex] = keyFrames[smallestValIndex];
                keyFrames[smallestValIndex] = temp;
            }

        }
    }
}
