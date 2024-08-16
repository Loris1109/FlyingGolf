/////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////
/////////////////              Lorenz Steiniger-Brach          //////////////
/////////////////                  Flying Golf                 //////////////
/////////////////                  DragableObject              //////////////
/////////////////           Seminarfacharbeit ÖG 19.01.21      //////////////
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragableObject : MonoBehaviour
{
    protected bool stopedDraging = false;
    //Vector3, da Positionen immer 3D sind auch in 2D Spielen
    private Vector3 mOffset;
    protected GameObject underPointer;

    
    private void OnMouseDown()
    {
        // Offset = gameobject world pos - mous world pos
        mOffset = gameObject.transform.position - getMousWorldPos();
        stopedDraging = false;
    }

    private Vector3 getMousWorldPos()
    {
        //Pixel-Koordinaten
        Vector2 mousPos = Input.mousePosition;
        //Welt-Koordinaten
        return Camera.main.ScreenToWorldPoint(mousPos);
    }

    private void OnMouseDrag()
    {
        transform.position = getMousWorldPos() + mOffset;
        stopedDraging = false;
    }

    private void OnMouseExit()
    {
        stopedDraging = true;
    }

    
}
