using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawLines : MonoBehaviour {
	
	private Texture graphBackground;
	private Texture graphLine;
	private Texture correlationGraph;
	private bool graphPrompt = false;
	private bool heizenPrompt = false;
	private string heizenText;
	private string heizenText2;
	private float graphPromptWidth = Screen.width/5.0f;
	private float graphPromptHeight = Screen.height/3.0f;
	private float addButtonSide = Screen.width/40.0f;
	
	/*Default values for graph creation*/
	private Vector2 graphPos;
	private float graphWidth = Screen.width/3.5f;
	private float graphHeight = Screen.height/3.5f;
	//private string graphName = "   The name of the graphic...";
	private Vector3 graphOffset;
	/*---*/
	
	/*
	 * Greek letters: ? ? ? ? ? ? ? ? ? ? ? ? ? ?;
	 * http://en.wikipedia.org/wiki/Greek_alphabet
	*/
	
	private int graphLineThickness = 2;
	
	private Vector2 distance;
	private int movingGraphic = -1;
	private float comeBackSpeed = 500.0f;
	
	/*List of the toggle buttons and related variables*/
	private float toggleButtonSide;
	private bool toggle1 = true;
	private bool toggle2 = false;
	private bool toggle3 = false;
	private Texture toggleName1;
	private Texture toggleName2;
	private Texture toggleName3;
	private List<bool> toggleButton = new List<bool>();
	private List<Texture> toggleButtonName = new List<Texture>();
	
	/*Variables used in the math*/
	private Texture variableName1;
	private Texture variableName2;
	private float variableNameSide;
	private string variableString1 = "";
	private string variableString2 = "";
	private float variable1;
	private float variable2;
	private float variable3;

	/*list of all instantiated graphics*/
	private List<Graph> graphic = new List<Graph>();
	
	/*temporary list used to calculate the points of a graphic, that 
	 * after is copied into one of the graphic's attributes*/
	private List<Vector2> graphPoints = new List<Vector2>();
	
	/*variables used for graphic points calculation*/
	private double xMax;
	private double xMin;
	private double yMax;
	private double yMin;
	private double xValue;
	private double yValue;
	private double xStep;
	private List<double> tempPointsX = new List<double>();
	private List<double> tempPointsY = new List<double>();
	private Vector2 calcVector;
	private double calcVectorX;
	private double calcVectorY;
	private float midValue;
	private int numberOfMeasures = 1000;

	private List<Window> globalWindow = new List<Window>();
	
	void Start () {
		graphBackground = Resources.Load ("Graph_BG") as Texture;
		graphLine = Resources.Load ("Graph_Line") as Texture;
		correlationGraph = Resources.Load ("graph") as Texture;
		graphPos = new Vector2(addButtonSide, 0);
		graphOffset = new Vector3(graphWidth/10.0f, graphHeight/8.0f, 0);
		
		/*Add the toggle's values and names to the respective lists*/
		toggleButtonSide = graphPromptWidth/14.0f;
		toggleButton.Add(toggle1);
		toggleButton.Add(toggle2);
		toggleButton.Add(toggle3);
		
		for (int i = 0; i < toggleButton.Count; i++) {
			/*Add to a list every texture with the toggle names*/
			toggleButtonName.Add(Resources.Load("ToggleName"+(i+1).ToString()) as Texture);
		}
		
		/*Load Textures of the variables*/
		variableName1 = Resources.Load("variavel1") as Texture;
		variableName2 = Resources.Load("variavel2") as Texture;
		variableNameSide = graphPromptHeight/8.0f;

		heizenText = "O principio de incerteza foi enunciado pela primeira vez em 1927, por Werner Heisenberg, e diz-nos que:\n-'o conhecimento exato em simultaneo da posicao (x) de uma particula e da sua quantidade de movimento (p = ħ * k), nao tem qualquer significado na natureza'.\nExiste uma incerteza intrinseca na determinacao da posicao (σ(x) = Δx) e da quantidade de movimento (σ(p) = Δp = ħ * Δk), que obedecem a relacao: Δx * Δp = ħ/2.\n\nO grafico abaixo ilustra a relaçao entre Δp e Δx:";
		heizenText2 = "(Carregar no botao para gerar tres graficos da distribuicao de probabilidade - para Δk igual a 0.025, 0.05 e 0.1)\n\nAtraves dos graficos, podemos claramente observar que, quando Δk duplica, Δx e reduzido para metade;\nComo Δp nao e nada mais que ħ * Δk, se Δk duplica, entao Δp tambem duplica.\n\n(Nota: Nos graficos, σ(k) = Δk)";

		for (int j = 0; j < 3; j++) {
			Window newWindow = new Window();
			globalWindow.Add (newWindow);
		}
	}
	
	void Update () {
		checkMouseClick();
		checkGlobals();
	}
	
	void OnGUI() {

		/*Draw the horizontal scroller*/
		numberOfMeasures = (int)(GUI.HorizontalSlider(new Rect(18.0f*(Screen.width/20.0f), 0.25f*(Screen.height/10.0f), 2.0f*(Screen.width/20.0f), Screen.height/10.0f), (float)numberOfMeasures, 1.0f, 2000.0f));
		GUI.Label(new Rect(17.0f*(Screen.width/20.0f), 0, (Screen.width/20.0f), Screen.height/10.0f), "Measures: " + numberOfMeasures.ToString());


		if (graphic.Count > 0) {
			for (int i = 0; i < graphic.Count; i++) {
				
				/*Draw the background of the graphics*/
				GUI.DrawTexture(new Rect(graphic[i].position.x, graphic[i].position.y, graphic[i].width, graphic[i].height), graphBackground);
				
				/*Draw the graphic lines*/
				Vector2 topPoint = new Vector2(graphic[i].position.x + graphOffset.x, graphic[i].position.y + graphOffset.y);
				Vector2 midPoint = topPoint + new Vector2(0, graphHeight - 2.0f*graphOffset.y);
				GUI.DrawTexture(new Rect(topPoint.x, topPoint.y, graphLineThickness, graphic[i].height - 2.0f*graphOffset.y), graphLine, ScaleMode.StretchToFill);
				GUI.DrawTexture(new Rect(midPoint.x, midPoint.y, graphic[i].width - 2.0f*graphOffset.x, graphLineThickness), graphLine, ScaleMode.StretchToFill);

				for (int y = 0; y < 3; y++) {
					/*Draw y values*/
					GUI.DrawTexture(new Rect(topPoint.x - graphOffset.x/3.0f, topPoint.y + y*(graphic[i].height - 2.0f*graphOffset.y)/2.0f, graphOffset.x/3.0f, graphLineThickness), graphLine, ScaleMode.StretchToFill);
					GUI.Label(new Rect(topPoint.x - graphOffset.x + graphOffset.x/8.0f, topPoint.y + y*(graphic[i].height - 2.0f*graphOffset.y)/2.0f, 2.0f*(graphOffset.x/3.0f), (graphic[i].height-2.0f*graphOffset.y)/2.0f), ((float)(globalWindow[graphic[i].graphType].yMax - y*(globalWindow[graphic[i].graphType].yMax - globalWindow[graphic[i].graphType].yMin)/2.0f)).ToString());
				}

				for (int x = 0; x < 5; x++) {
					/*Draw x values*/
					GUI.DrawTexture(new Rect(midPoint.x + x*(graphic[i].width/5), midPoint.y, graphLineThickness, graphOffset.y/3.0f), graphLine, ScaleMode.StretchToFill);
					GUI.Label(new Rect(midPoint.x + x*(graphic[i].width/5), midPoint.y + graphOffset.y/3.0f, (graphic[i].width-2.0f*graphOffset.x)/5.0f, 2.0f*(graphOffset.y/3.0f)), (globalWindow[graphic[i].graphType].xMin + x*(globalWindow[graphic[i].graphType].xMax - globalWindow[graphic[i].graphType].xMin)/4.0f).ToString());
				}
				/*Draw the graphic points*/
				for (int h = 0; h < graphic[i].points.Count; h++) {
					float newX = (graphic[i].width-2.0f*graphOffset.x)*(graphic[i].points[h].x - globalWindow[graphic[i].graphType].xMin)/(globalWindow[graphic[i].graphType].xMax-globalWindow[graphic[i].graphType].xMin);
					float newY = (graphic[i].height-2.0f*graphOffset.y)*(graphic[i].points[h].y - globalWindow[graphic[i].graphType].yMin)/(globalWindow[graphic[i].graphType].yMax-globalWindow[graphic[i].graphType].yMin);
					GUI.DrawTexture(new Rect(graphic[i].position.x + graphOffset.x + newX, graphic[i].position.y + graphic[i].height - graphOffset.y - newY, graphLineThickness, graphLineThickness), graphLine, ScaleMode.StretchToFill);
				}
				
				/*Draw the top-left buttons of the graphics and name texture*/
				if (GUI.Button(new Rect(graphic[i].position.x, graphic[i].position.y, graphOffset.x, graphOffset.y), "o")) {
					/*To prevent any bugs*/
					graphic[movingGraphic].moving = false;
					movingGraphic = -1;
					/*---*/
					graphic.RemoveAt(i);
				} else {
					/*Draw the graphic name, and variables, next to the "o"*/
					GUI.DrawTexture(new Rect(graphic[i].position.x + graphOffset.x, graphic[i].position.y, 2.0f*graphOffset.x, graphOffset.y), graphic[i].label, ScaleMode.ScaleToFit);
					GUI.DrawTexture(new Rect(graphic[i].position.x + 4.0f*graphOffset.x, graphic[i].position.y, graphOffset.x, graphOffset.y), variableName1, ScaleMode.ScaleToFit);
					GUI.Label(new Rect(graphic[i].position.x + 5.0f*graphOffset.x, graphic[i].position.y, 2.0f*graphOffset.x, graphOffset.y), "= " + ((float)(graphic[i].variable1)).ToString());
					GUI.DrawTexture(new Rect(graphic[i].position.x + 7.0f*graphOffset.x, graphic[i].position.y, graphOffset.x, graphOffset.y), variableName2, ScaleMode.ScaleToFit);
					GUI.Label(new Rect(graphic[i].position.x + 8.0f*graphOffset.x, graphic[i].position.y, 2.0f*graphOffset.x, graphOffset.y), "= " + ((float)(graphic[i].variable2)).ToString());
					
					/*Check for a moveable graphic*/
					if (Input.GetMouseButton(0) && graphic[i].moving) {
						Vector2 mousePos = new Vector3(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
						graphic[i].position = mousePos + distance;
					} else {
						/*button is not being pressed... do nothing*/
					}
				}
			}
		}
		
		/*Button (+) opens a graphic prompt*/
		if (GUI.Button(new Rect(0, 0, addButtonSide, addButtonSide), "(+)")) {
			variableString1 = "";
			variableString2 = "";
			graphPrompt = !graphPrompt;
		}

		/*Button (H) opens the Heizenberg's uncertainty prompt*/
		if (GUI.Button(new Rect(0, addButtonSide, addButtonSide, addButtonSide), "(H)")) {
			heizenPrompt = !heizenPrompt;
		}

		/*Draws the prompt to create a graphic*/
		if (graphPrompt) {
			GUI.DrawTexture(new Rect(addButtonSide, 0, graphPromptWidth, graphPromptHeight), graphBackground, ScaleMode.StretchToFill);
			/*Draw the toggle buttons*/
			for (int j = 0; j < toggleButton.Count; j++) {
				GUI.DrawTexture(new Rect(addButtonSide + toggleButtonSide/2.0f + j*(graphPromptWidth/toggleButton.Count), 0, graphPromptWidth/toggleButton.Count - toggleButtonSide, graphPromptHeight/8.0f), toggleButtonName[j], ScaleMode.ScaleToFit);
				toggleButton[j] = GUI.Toggle(new Rect(addButtonSide + (j+1)*(graphPromptWidth/(toggleButton.Count + 1)) - toggleButtonSide/2.0f, (graphPromptHeight/8.0f), toggleButtonSide, toggleButtonSide), toggleButton[j], "");
			}
			/*Draw the variable fields*/
			GUI.DrawTexture(new Rect(addButtonSide + graphPromptWidth/8.0f, 2.5f*(graphPromptHeight/8.0f), variableNameSide, variableNameSide), variableName1, ScaleMode.ScaleToFit);
			variableString1 = GUI.TextField(new Rect(addButtonSide + graphPromptWidth/8.0f + variableNameSide, 2.5f*(graphPromptHeight/8.0f), graphPromptWidth/2.5f, variableNameSide), variableString1, 10);
			GUI.DrawTexture(new Rect(addButtonSide + graphPromptWidth/8.0f, 4.5f*(graphPromptHeight/8.0f), variableNameSide, variableNameSide), variableName2, ScaleMode.ScaleToFit);
			variableString2 = GUI.TextField(new Rect(addButtonSide + graphPromptWidth/8.0f + variableNameSide, 4.5f*(graphPromptHeight/8.0f), graphPromptWidth/2.5f, variableNameSide), variableString2, 10);
			
			/*Draw "create" button*/
			if (GUI.Button(new Rect(addButtonSide + graphPromptWidth/3.0f, 6.0f*(graphPromptHeight/8.0f), graphPromptWidth/3.0f, graphPromptHeight/8.0f), "Create")) {
				/*Checks if the input is valid*/
				if (float.TryParse(variableString1, out variable1) && float.TryParse(variableString2, out variable2) && variable2 > 0) {
					
					/*Button "Create" creates new instances of the graphs, with the choosen attributes*/
					for (int z = 0; z < toggleButton.Count; z++) {
						if (toggleButton[z]) {
							calcGraphPoints(variable1, variable2, z);
							Graph newGraph = new Graph(graphPos + new Vector2(z*graphOffset.x, z*graphOffset.y), graphWidth, graphHeight, toggleButtonName[z], variable1, variable2, variable3, xMax, xMin, yMax, yMin, graphPoints, z);
							newGraph.midValue = midValue;
							graphic.Add(newGraph);
						}
					}
					
					/*Closes the prompt*/
					graphPrompt = false;
				} else {
					variableString1 = "";
					variableString2 = "";
				}
			}
		}

		if (heizenPrompt) {
			GUI.DrawTexture(new Rect(2.0f*(Screen.width/3.0f), Screen.height/10.0f, Screen.width/3.0f, 9.0f*(Screen.height/10.0f)), graphBackground, ScaleMode.StretchToFill);
			GUI.Label(new Rect(2.0f*(Screen.width/3.0f) + graphOffset.x, Screen.height/10.0f + graphOffset.y, Screen.width/3.0f - 2.0f*graphOffset.x, (9.0f*(Screen.height/10.0f) - 2.0f*graphOffset.y)/3.0f), heizenText);
			GUI.DrawTexture(new Rect(2.0f*(Screen.width/3.0f) + graphOffset.x, Screen.height/10.0f + graphOffset.y + (9.0f*(Screen.height/10.0f) - 2.0f*graphOffset.y)/3.0f, Screen.width/3.0f - 2.0f*graphOffset.x, (9.0f*(Screen.height/10.0f) - 2.0f*graphOffset.y)/3.0f), correlationGraph, ScaleMode.ScaleToFit);
			GUI.Label(new Rect(2.0f*(Screen.width/3.0f) + graphOffset.x, (Screen.height/10.0f + graphOffset.y) + 2.0f*((9.0f*(Screen.height/10.0f) - 2.0f*graphOffset.y)/3.0f), Screen.width/3.0f - 2.0f*graphOffset.x, (9.0f*(Screen.height/10.0f) - 2.0f*graphOffset.y)/3.0f), heizenText2);
			if (GUI.Button(new Rect(2.0f*(Screen.width/3.0f) + graphOffset.x + (Screen.width/3.0f - 2.0f*graphOffset.x), (Screen.height/10.0f + graphOffset.y) + 2.0f*((9.0f*(Screen.height/10.0f) - 2.0f*graphOffset.y)/3.0f), addButtonSide, addButtonSide), "(+)")) {
				for (int k = 0; k < 3; k++) {
					variable1 = 1;
					variable2 = 0.025f*Mathf.Pow(2, k);
					calcGraphPoints(variable1, variable2, 2);
					Graph newGraph2 = new Graph(new Vector2(Screen.width/5.0f, k*(Screen.height/3.0f)), graphWidth, graphHeight, toggleButtonName[2], variable1, variable2, variable3, xMax, xMin, yMax, yMin, graphPoints, 2);
					newGraph2.midValue = midValue;
					graphic.Add(newGraph2);
				}
				variable1 = 0;
				variable2 = 0;
			}
		}
	}
	
	void LateUpdate() {
		checkBoundaries();
	}

	void checkGlobals() {
		if (graphic.Count == 0) {
			for (int j = 0; j < globalWindow.Count; j++) {
				globalWindow[j].xMax = 0;
				globalWindow[j].xMin = 0;
				globalWindow[j].yMax = 0;
				globalWindow[j].yMin = 0;
			}
		} else {
			for (int h = 0; h < graphic.Count; h++) {
				globalWindow[graphic[h].graphType].xMax = (float)graphic[h].xMax;
				globalWindow[graphic[h].graphType].xMin = (float)graphic[h].xMin;
				globalWindow[graphic[h].graphType].yMax = (float)graphic[h].yMax;
				globalWindow[graphic[h].graphType].yMin = (float)graphic[h].yMin;
			}

			for (int i = 0; i < graphic.Count-1; i++) {
				if (graphic[i].xMax > globalWindow[graphic[i].graphType].xMax) {
					globalWindow[graphic[i].graphType].xMax = (float)graphic[i].xMax; 
				}
				if (graphic[i].xMin < globalWindow[graphic[i].graphType].xMin) {
					globalWindow[graphic[i].graphType].xMin = (float)graphic[i].xMin;
				}
				if (graphic[i].yMax > globalWindow[graphic[i].graphType].yMax) {
					globalWindow[graphic[i].graphType].yMax = (float)graphic[i].yMax;
				}
				if (graphic[i].yMin < globalWindow[graphic[i].graphType].yMin) {
					globalWindow[graphic[i].graphType].yMin = (float)graphic[i].yMin;
				}
			}
		}
	}

	void calcGraphPoints(float var1, float var2, int z) {
		tempPointsX.Clear();
		tempPointsY.Clear();
		graphPoints.Clear();
		int i = 0;
		while (true) {
			if (z == 0) {
				//Calc A(k)
				xMax = var1 + 3.0f*var2;
				xMin = var1 - 3.0f*var2;
				xStep = (xMax-xMin)/numberOfMeasures;
				xValue = xMin + i*xStep;
				midValue = (float)variable1;
				variable3 = var2;
				if (xValue > xMax) {
					break;
				}
				yValue = System.Math.Exp(-0.5f*System.Math.Pow((xValue - var1)/var2, 2));
				yMax = 1;
				yMin = 0;
			} else if (z == 1) {
				//Calc Re(Y(x))
				xMax = 3.0f*(1/(2.0f*var2));
				xMin = -3.0f*(1/(2.0f*var2));
				xStep = (xMax-yMax)/numberOfMeasures;
				xValue = xMin + i*xStep;
				midValue = 0.0f;
				variable3 = 1/(2.0f*var2);
				if (xValue > xMax) {
					break;
				}
				yValue = 2.0f*var2*System.Math.Sqrt(System.Math.PI)*System.Math.Exp(-1.0f*System.Math.Pow(var2*xValue, 2))*System.Math.Cos(var1*xValue); 
				yMax = 2.0f*var2*System.Math.Sqrt(System.Math.PI);
				yMin = -yMax;
			} else {
				//Calc |Y(x)|**2
				xMax = 3.0f*(1/(2.0f*var2));
				xMin = -3.0f*(1/(2.0f*var2));
				xStep = (xMax-xMin)/numberOfMeasures;
				xValue = xMin + i*xStep;
				midValue = 0.0f;
				variable3 = 1/(2.0f*var2);
				if (xValue > xMax) {
					break;
				}
				yValue = 4.0f*System.Math.Pow(var2, 2)*System.Math.PI*System.Math.Exp(-2.0f*System.Math.Pow(var2*xValue, 2));
				yMax = 4.0f*System.Math.Pow(var2, 2)*System.Math.PI;
				yMin = 0;
			}
			tempPointsX.Add (xValue);
			tempPointsY.Add (yValue);
			i++;
		}

		for (int j = 0; j < tempPointsX.Count; j++) {
			calcVector = new Vector2((float)tempPointsX[j], (float)tempPointsY[j]);
			graphPoints.Add (calcVector);
		}
	}
	
	void checkBoundaries() {
		/*Check if every graph is inside the window*/
		for (int i = 0; i < graphic.Count; i++) {
			/*checks for the x coordinates*/
			if (graphic[i].position.x <= 0) {
				graphic[i].position.x += Time.deltaTime * comeBackSpeed;
			} else if (graphic[i].position.x + graphic[i].width >= Screen.width) {
				graphic[i].position.x -= Time.deltaTime * comeBackSpeed;
			}
			
			/*checks for the y coordinates*/
			if (graphic[i].position.y <= 0) {
				graphic[i].position.y += Time.deltaTime * comeBackSpeed;
			} else if (graphic[i].position.y + graphic[i].height >= Screen.height) {
				graphic[i].position.y -= Time.deltaTime * comeBackSpeed;
			}
		}
	}
	
	void checkMouseClick() {
		
		if (Input.GetMouseButtonDown(0)) {
			/*if user pressed mouse left button, check for graphics to move*/
			selectGraphicToMove();
		} else if (Input.GetMouseButtonUp(0) && movingGraphic >= 0) {
			/*if user releases the mouse button, it means he no longer is dragging a graphic*/
			graphic[movingGraphic].moving = false;
			movingGraphic = -1;
		}
	}
	
	void selectGraphicToMove() {
		Vector2 tempDistance;
		/*WARNING: below "graphic.Count-1-i" is used instead of just "i", so that the TOP graph is selected*/
		for (int i = 0; i < graphic.Count; i++) {
			/*calculate the distance from the mouse click to all the graphic's positions*/
			tempDistance = graphic[graphic.Count-1-i].position - new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
			if ((tempDistance.x <= 0 && tempDistance.x >= -1.0f*graphic[graphic.Count-1-i].width) && (tempDistance.y <= 0 && tempDistance.y >= -1.0f*graphic[graphic.Count-1-i].height) && movingGraphic == -1) {
				/*if one of them is suitable to be moved, enable it's moving attribute*/
				graphic[graphic.Count-1-i].moving = true;
				movingGraphic = graphic.Count-1-i;
				distance = tempDistance;
			}
		}
	}
}