using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AnimationGUI : EditorWindow
{
    private static GameObject mySelectedObject = null;
    
    private static Rect mySliderRect = new Rect(10, 140, 100, 50);
    private static float mySliderVal;
    
    [MenuItem("Animation GUI/Enable")]
    public static void Enable()
    {
        SceneView.duringSceneGui += OnScene;
        Debug.Log("Animation GUI : Enabled");
    }
    
    [MenuItem("Animation GUI/Disable")]
    public static void Disable()
    {
        SceneView.duringSceneGui -= OnScene;
        Debug.Log("Animation GUI : Disabled");
    }
    
    private static void OnScene(SceneView sceneview)
    {
        Handles.BeginGUI();
       
        if (mySelectedObject != null)
        {
            CustomAnimationClass animClass = mySelectedObject.GetComponent<CustomAnimationClass>();
            if (animClass != null)
            {
                //if(animClass.keyFrames.Length > 0)
                //{
                //    int yPos = 80;
                //    for(int index = 0; index < animClass.amountOfKFInArray; ++index)
                //    {
                //        Vector3 pos = animClass.keyFrames[index].storedPosition;
                //        int x = (int)pos.x;
                //        int y = (int)pos.y;
                //        int z = (int)pos.z;
                //
                //        string str = "Frame: " + index + " - X: " + x + " Y: " + y + " Z: " + z;
                //        GUI.TextArea(new Rect(10, yPos, 200, 20), str);
                //
                //        yPos += 30;
                //    }
                //}


                //if (GUI.Button(new Rect(10, 10, 100, 50), "Empty Array"))
                //{
                //    animClass.Clear();
                //}
                //
                //if (GUI.Button(new Rect(120, 10, 50, 50), "Save KeyFrame"))
                //{
                //    Debug.Log("Saving KeyFrame.");
                //    animClass.AddKF(0f);
                //}

                //animClass.currentFrame = (int)GUI.HorizontalSlider(mySliderRect, animClass.currentFrame, 0, animClass.currentFrame);

               // GUI.BeginScrollView(new Rect(200, 200, 200, 200), new Vector2(0, 0), new Rect(0, 0, 200, 200));
               // int pos = 0;
               // if(animClass != null)
               // {
               //     foreach (var item in animClass.keyFrames)
               //     {
               //         GUI.TextField(new Rect(0,pos, 200,20), "test");
               //         pos += 20;
               //     }
               // }
               // GUI.EndScrollView();
            }
            else
            {
                //Debug.Log("Does not have animation class");
            }
        }

        Handles.EndGUI();
     
        mySelectedObject = Selection.activeObject as GameObject;
    }
}
