using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{

    
    public LineRenderer LineRenderer;
    public List<LineRenderer> lineRendererlist = new List<LineRenderer>();
    public int boardsize = 8;
    private int currLine = 0;
    public void DrawLine(Vector3 start, Vector3 end){
        LineRenderer.SetPosition(0, start);
        LineRenderer.SetPosition(1, end);
        Instantiate(LineRenderer,start,Quaternion.identity);
    }
}
