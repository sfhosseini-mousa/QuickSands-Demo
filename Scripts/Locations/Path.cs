using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{

    [SerializeField] private List<Transform> checkpoints = new List<Transform>();
    [SerializeField] private List<Transform> checkpoints2 = new List<Transform>();
    private LineRenderer lineRenderer;

    public Color c1 = Color.yellow;
    public Color c2 = Color.red;
    // Start is called before the first frame update
    void Start()
    {
        this.DrawMainLine();
        this.DrawSideLine();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void DrawMainLine() {
        GameObject lineObject = new GameObject();
        this.lineRenderer = lineObject.AddComponent<LineRenderer>();
        this.lineRenderer.startWidth = 100f;
        this.lineRenderer.endWidth = 100f;
        this.lineRenderer.positionCount = checkpoints.Count;
       // this.lineRenderer.SetColors(c1, c2);

        Vector3[] checkPointArray = new Vector3[this.checkpoints.Count];

        for (int i = 0; i < this.checkpoints.Count; i++) {

            Vector3 checkPointPos = this.checkpoints[i].position;
            checkPointArray[i] = new Vector3(checkPointPos.x, checkPointPos.y, 0f);
        }

        this.lineRenderer.SetPositions(checkPointArray);
    }


    private void DrawSideLine()
    {
        GameObject lineObject = new GameObject();
        this.lineRenderer = lineObject.AddComponent<LineRenderer>();
        this.lineRenderer.startWidth = 10f;
        this.lineRenderer.endWidth = 10f;
        this.lineRenderer.positionCount = checkpoints2.Count;
        // this.lineRenderer.SetColors(c1, c2);

        Vector3[] checkPointArray2 = new Vector3[this.checkpoints2.Count];

        for (int i = 0; i < this.checkpoints2.Count; i++)
        {

            Vector3 checkPointPos = this.checkpoints2[i].position;
            checkPointArray2[i] = new Vector3(checkPointPos.x, checkPointPos.y, 0f);
        }

        this.lineRenderer.SetPositions(checkPointArray2);
    }

}
