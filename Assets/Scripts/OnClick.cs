using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class OnClick : MonoBehaviour {

	public int tileX;
	public int tileY;
	public CubeGrid map;

	void OnMouseUp() 
    {   
        //Check for  mouse click
        map.GeneratePathTo(tileX, tileY);
		
		/*
        if(EventSystem.current.IsPointerOverGameObject())
			return;

		map.GeneratePathTo(tileX, tileY);
        */
    }

}
