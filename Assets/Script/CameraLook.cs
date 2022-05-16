using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    LineDrawer lineDrawer;

    public float sphereRadius = 3f;

    public float zOffset = 10f;

    GameObject heldObj;
    Vector3 objOriginalPos;

    public float rotationSpeed = .5f;

    public float mouseSenstivity = 100.0f;
    public float clampAngle = 80.0f;
    float rotationX = 0;
    float rotationY = 0;
    public int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 rotation = transform.localRotation.eulerAngles;
        rotationX = rotation.x;
        rotationY = rotation.y;

        lineDrawer = new LineDrawer();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 eyePosition = transform.position;
        Vector3 mousePos = Input.mousePosition;

        mousePos.z = Camera.main.nearClipPlane;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);

        Vector3 dir = mouseWorldPos - eyePosition;
        dir.Normalize();

        RaycastHit hitter = new RaycastHit();

        Debug.DrawLine(eyePosition, dir * 1000f, Color.red);

        lineDrawer.DrawLineInGameView(eyePosition, dir * 1000f, Color.blue);

        if (Physics.SphereCast(eyePosition, sphereRadius, dir, out hitter))
        {
            
            Debug.Log(hitter.collider.gameObject.name);
            if (heldObj != null)
            {
                if (heldObj.name == hitter.collider.gameObject.name)
                {
                    float xRotate = Input.GetAxis("Mouse X") * rotationSpeed;
                    float yRotate = Input.GetAxis("Mouse Y") * rotationSpeed;
                    heldObj.transform.Rotate(Vector3.up, xRotate);
                    heldObj.transform.Rotate(Vector3.right, yRotate);
                }
            }
            else
            {
                if (Input.GetMouseButton(0) && hitter.collider.gameObject.tag == "Destroyable" )
                {
                    Debug.Log("Destroyable");
                    DestroyObject(hitter.collider.gameObject);
                }
            }
        }



        MoveCamera();



    }

    void DestroyObject(GameObject obj)
    {
        Destroy(obj);
        score += 1;
    }



    void MoveCamera()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotationY += mouseX * mouseSenstivity * Time.deltaTime;
        rotationX += mouseY * mouseSenstivity * Time.deltaTime;

        rotationX = Mathf.Clamp(rotationX, -clampAngle, clampAngle);

        Quaternion localRotation = Quaternion.Euler(-rotationX, rotationY, 0.0f);
        transform.rotation = localRotation;
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

        private void init(float lineSize = 0.2f)
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