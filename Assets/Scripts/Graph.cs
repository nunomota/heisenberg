using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Graph {

	public Vector2 position;
	public float width;
	public float height;
	public Texture label;
	public double variable1;
	public double variable2;
	public double variable3;
	public double xMax;
	public double xMin;
	public double yMax;
	public double yMin;
	public List<Vector2> points = new List<Vector2>();
	public bool moving;
	public bool resizing;
	public float midValue;
	public int graphType;
	
	public  Graph(Vector2 graphPos, float graphWidth, float graphHeight, Texture graphName, double var1, double var2, double var3, double XMax, double XMin, double YMax, double YMin, List<Vector2> graphPoints, int Type) {
		position = graphPos;
		width = graphWidth;
		height = graphHeight;
		label = graphName;
		variable1 = var1;
		variable2 = var2;
		variable3 = var3;
		xMax = XMax;
		xMin = XMin;
		yMax = YMax;
		yMin = YMin;
		cloneList(graphPoints);
		moving = false;
		resizing = false;
		graphType = Type;
	}

	void cloneList(List<Vector2> graphPoints) {
		for (int i = 0; i < graphPoints.Count; i++) {
			points.Add(new Vector2(graphPoints[i].x, graphPoints[i].y));
		}
	}
}
