using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobeOD : MonoBehaviour
{
    public Globe globe;
    [Header("Dataset")]
    public Database dataset;
    public string latXLabel;
    public string lonXLabel;
    public string latYLabel;
    public string lonYLabel;

    [Header("Line settings")]
    public float lineWidth = 3f;
    public int lineResolution = 20;
    public Color startColor = Color.green;
    public Color endColor = Color.yellow;
    public float heightFactor = 0.5f;
    public Material lineMaterial;

    private List<float> latX = new List<float>();
    private List<float> latY = new List<float>();
    private List<float> lonX = new List<float>();
    private List<float> lonY = new List<float>();

    private List<LineRenderer> lines = new List<LineRenderer>();

    // Start is called before the first frame update
    void Start()
    {
        //initiate globe
        globe.Initiate();

        //read data
        latX = dataset.GetFloatRecordsByField(latXLabel, "");
        latY = dataset.GetFloatRecordsByField(latYLabel, "");
        lonX = dataset.GetFloatRecordsByField(lonXLabel, "");
        lonY = dataset.GetFloatRecordsByField(lonYLabel, "");

        lineMaterial = (lineMaterial == null) ? new Material(Shader.Find("Unlit/Color")) : lineMaterial;

        //create flows
        for (int i = 0; i < latX.Count; i++)
        {
            Vector2 point1 = new Vector2(latX[i], lonX[i]);
            Vector2 point2 = new Vector2(latY[i], lonY[i]);
            Vector3 midPoint = globe.GetMidPointBetween(point1, point2);

            float egoFlag = (globe.egocentric)? -1f : 1f;

            Vector3 p0 = globe.GeoToWorldPosition(point1);
            Vector3 p3 = globe.GeoToWorldPosition(point2);
            Vector3 p1 = globe.GeoToWorldPosition(point1) + (globe.GeoToWorldPosition(midPoint) - globe.transform.position).normalized * egoFlag * Vector3.Distance(p0, p3) * heightFactor; 
            Vector3 p2 = globe.GeoToWorldPosition(point2) + (globe.GeoToWorldPosition(midPoint) - globe.transform.position).normalized  * egoFlag * Vector3.Distance(p0, p3) * heightFactor;

            List<Vector3> points = BezierCurveFormula.GetCubic(p0, p1, p2, p3, 1, lineResolution);

            //create line
            GameObject g = new GameObject("Line");
            g.transform.SetParent(globe.transform);
            LineRenderer line = g.AddComponent<LineRenderer>();
            line.useWorldSpace = false;
            line.widthMultiplier = lineWidth;
            line.material = lineMaterial;
            line.positionCount = points.Count;
            line.startColor = startColor;
            line.endColor = endColor;
            //set points
            for(int j = 0; j < points.Count; j++)
            {
                line.SetPosition(j, line.transform.InverseTransformPoint(points[j]));
            }
            lines.Add(line);
        }
    }
}
