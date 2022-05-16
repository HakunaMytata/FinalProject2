using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    LineDrawer lineDrawer;

    float xRotation = 0f;
    public Vector2 turn;
    public float sensitivity = 50;
    public Vector3 deltaMove;
    public float speed = 1;
    public Transform player;
    public GameObject playerCamera;
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;

        lineDrawer = new LineDrawer();
    }
    void Update()
    {

        


        RaycastHit hitter = new RaycastHit();
        Debug.DrawLine(playerCamera.transform.position , playerCamera.transform.TransformDirection(Vector3.forward) * 20f, Color.red);

        lineDrawer.DrawLineInGameView(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward) * 20f, Color.blue);

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward) * 100f, out hitter, 3.0f))
        {

            Debug.Log(hitter.collider.gameObject.name);

            
                if (Input.GetMouseButton(0) && hitter.collider.gameObject.tag == "Destroyable")
                {
                    Debug.Log("Destroyable");
                    Destroy(hitter.collider.gameObject);
                }
            }






        if (Time.timeSinceLevelLoad > 1)
        {
            turn.x += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            turn.y = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
            xRotation -= turn.y;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            var rotation = Quaternion.Euler(xRotation, turn.x, 0);
            var newLocalRotation = transform.localRotation;
            newLocalRotation.x = rotation.x;
            var newPlayerRotation = player.localRotation;
            newPlayerRotation.y = rotation.y;
            transform.localRotation = newLocalRotation;
            player.localRotation = newPlayerRotation;
        }
    }

    public struct LineDrawer
    {
        private LineRenderer lineRenderer;
        private float lineSize;

        public LineDrawer(float lineSize = 0.2f)
        {
            GameObject lineObj = new GameObject("LineObj");
            lineRenderer = lineObj.AddComponent<LineRenderer>();
            //Particles/Additive
            lineRenderer.material = new Material(Shader.Find("Hidden/Internal-Colored"));

            this.lineSize = lineSize;
        }

        private void init(float lineSize = 1f)
        {
            if (lineRenderer == null)
            {
                GameObject lineObj = new GameObject("LineObj");
                lineRenderer = lineObj.AddComponent<LineRenderer>();
                //Particles/Additive
                lineRenderer.material = new Material(Shader.Find("Hidden/Internal-Colored"));

                this.lineSize = lineSize;
            }
        }

        //Draws lines through the provided vertices
        public void DrawLineInGameView(Vector3 start, Vector3 end, Color color)
        {
            if (lineRenderer == null)
            {
                init(0.1f);
            }

            //Set color
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;

            //Set width
            lineRenderer.startWidth = lineSize;
            lineRenderer.endWidth = lineSize;

            //Set line count which is 2
            lineRenderer.positionCount = 2;

            //Set the postion of both two lines
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);
        }

        public void Destroy()
        {
            if (lineRenderer != null)
            {
                UnityEngine.Object.Destroy(lineRenderer.gameObject);
            }
        }
    }

}