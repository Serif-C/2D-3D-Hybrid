using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReduceAlphaValue : MonoBehaviour
{ 
    [SerializeField] PlayerManager player;

    Color obstacleColor;
    GameObject obstacleHit;

    private void Update()
    {
        RevealPlayer();
    }

    public void RevealPlayer()
    {
        Ray ray = new Ray(player.transform.position, Camera.main.transform.position - player.transform.position);
        RaycastHit hit;

        Debug.DrawRay(player.transform.position, Camera.main.transform.position - player.transform.position, Color.blue);

        if(Physics.Raycast(ray, out hit, 20f))
        {

            if(hit.collider.gameObject.CompareTag("Tree"))
            {
                if (Vector3.Distance(player.transform.position, hit.collider.gameObject.transform.position) < 5f)
                {
                    // Grabs the color of the obstacle and change its alpha value
                    obstacleColor = hit.collider.gameObject.GetComponentInChildren<SpriteRenderer>().color;
                    obstacleColor.a = 155f / 255f;

                    // Assign the new alpha value to the obstacle's color
                    obstacleHit = hit.collider.gameObject;
                    obstacleHit.GetComponentInChildren<SpriteRenderer>().color = obstacleColor;

                    /*
                     * Should add another raycast here starting from the tree being hit to the next tree or the camera
                     * to check for more obstacles between the player and the camera
                     */
                }
            }
        }
        else
        {
            if(obstacleHit != null)
            {
                // Assign the new alpha value to the obstacle's color
                obstacleColor.a = 255f / 255f;
                obstacleHit.GetComponentInChildren<SpriteRenderer>().color = obstacleColor;
            }
        }
    }
}
