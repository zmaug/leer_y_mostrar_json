using UnityEngine;
using UnityEngine.UI;
using System.IO;
using SimpleJSON;
//------------------------------------------------------------------------------
public class mainCode : MonoBehaviour
{
	private GameObject				obj_canvas;				// canvas
	private GameObject				obj_but_refresh;		// boton para refrescar
	private GameObject				obj_but_salir;			// boton salir
	//------------------------------------------------------------------------------
	private void Start()
    {
		// referencia
		obj_canvas					= GameObject.Find("CANVAS");
		obj_but_refresh				= GameObject.Find("BUT_refresh");
		obj_but_salir				= GameObject.Find("BUT_salir");
		// read from file and show on screen
		readDataFromJSON();
	}
	//------------------------------------------------------------------------------
	private void Update()
    {
		// Se presiono boton refrescar ?
		if(obj_but_refresh.GetComponent<myButton>().f_up == true)
		{
			// reset flag
			obj_but_refresh.GetComponent<myButton>().f_up	= false;
			// read data and draw UI texts
			readDataFromJSON();
		}
		// Se presiono boton salir ?
		else if((obj_but_salir.GetComponent<myButton>().f_up == true)||(Input.GetKey(KeyCode.Escape)))
		{
			#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying 		= false;
			#else
			Application.Quit();
			#endif
		}
	}
	//------------------------------------------------------------------------------
	// lee los datos de un .json usando simpleJSON
	// luego construye los UI Text con los datos dentro del canvas
	//------------------------------------------------------------------------------
	private void readDataFromJSON()
	{
		string filedata			= "";
		JSONNode data			= null;

		// destroy canvas UI text gameobjects
		destroyFromGameObject(obj_canvas,"MyText");

		// read file and parse json
		filedata				= File.ReadAllText(Application.dataPath + "/StreamingAssets/JsonChallenge.json");
		if(filedata == "")		return;
		data					= JSON.Parse( filedata );
		if(data == null)		return;

		// posicion fija de los datos dentro del canvas
		Vector2 pos0			= new Vector2(-300f,150f);
		// posicion variable dependiendo del elemento
		Vector2 posF			= new Vector2(0f,0f);
		// incrementos en la posicion dentro del canvas de los textos UI
		float   incX			= 125f;
		float   incY			= -25f;

		// loop a
		for(int a=0;a<data.Count;a++)
		{
			posF.x				= 0f;
			// label
			if(data[a].Value != "")					createTextLabel( pos0+posF, new Vector2(150f,30f), data[a], 18, true);
			// loop b
			for(int b=0;b<data[a].Count;b++)
			{
				if(data[a][b].Count > 0)	posF.x		= 0f;
				// label
				if(data[a][b].Value != "")			createTextLabel( pos0+posF, new Vector2(135f,30f), data[a][b], 16, true);
				// loop c
				for(int c=0;c<data[a][b].Count;c++)
				{
					// label
					if(data[a][b][c].Value != "")	createTextLabel( pos0+posF, new Vector2(125f,25f), data[a][b][c], 14, false);
					// incremento pos X en canvas
					posF.x 	+= incX;
				}
				// incremento pos X e Y en canvas
				if(data[a][b].Count > 0)	posF.y 		+= incY;
				else						posF.x 		+= incX;
			}
			// incremento pos Y en canvas
			posF.y += incY;
		}
	}
	//------------------------------------------------------------------------------
	// crea un UI Text en el canvas y lo configura
	//------------------------------------------------------------------------------
	private void createTextLabel(Vector2 pos,Vector2 size,string texto,int fnt_size,bool f_bold)
	{
		GameObject	obj								= new GameObject("MyText");
		obj.transform.SetParent( obj_canvas.transform );
		obj.transform.localPosition					= pos;
		Text txt 									= obj.AddComponent<Text>();
		txt.GetComponent<Text>().font				= Resources.GetBuiltinResource<Font>("Arial.ttf");
		txt.GetComponent<RectTransform>().sizeDelta = size;
		txt.alignment 								= TextAnchor.MiddleCenter;
		txt.fontSize								= fnt_size;
		txt.resizeTextForBestFit					= false;
		txt.text 									= texto;
		// set bold
		if(f_bold == true)
		  	txt.fontStyle	= FontStyle.Bold;
	}
	//------------------------------------------------------------------------------
	// destruye los gameobjects partiendo de otro gameobject
	// se usa para destruir los elementos UI "Text" cuando se lee de nuevo un json
	//------------------------------------------------------------------------------
	private void destroyFromGameObject(GameObject fromGameObject, string withName)
	{
		if(fromGameObject == null)
			return;
		// obtiene todos los childrens
		Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>(true);
		// los destruye si es que le nombre es "withName"
		foreach (Transform t in ts) 
		{
			if (t.gameObject.name == withName) 
				Destroy(t.gameObject);
		}
	}
}
//------------------------------------------------------------------------------