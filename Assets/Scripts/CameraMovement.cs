using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public Transform cameraTarget;

    public float cameraSpeed = 2.5f;

	public float minX = 0;
	public float maxX = 100;
	public float minY = 0;
	public float maxY = 100;

    void FixedUpdate(){

        if(cameraTarget != null){

            Vector2 newPos = Vector2.Lerp(transform.position, cameraTarget.position, Time.deltaTime * cameraSpeed);

            float clampX = Mathf.Clamp(newPos.x, minX, maxX);
            float clampY = Mathf.Clamp(newPos.y, minY, maxY);

            transform.position = new Vector3(clampX, clampY, -10);

        }
    }
}
