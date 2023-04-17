using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;	//For TextBox
using UnityEngine.SceneManagement;	//For SceneManager / LoadScene

public class SceneLoader : MonoBehaviour
{
	private bool isRunningWebGL = false;
	public Text textBox;
    
    void Start()
    {
        #if !UNITY_EDITOR && UNITY_WEBGL
			isRunningWebGL = true;
		#endif

		if (isRunningWebGL){
			//textBox.text = Application.absoluteURL;

			string absoluteURL = Application.absoluteURL;
			try{
				string query = absoluteURL.Split("?"[0])[1];
				string[] queryValues = query.Split("&");

				string[] queryValue0 = queryValues[0].Split("=");
				string[] queryValue1 = queryValues[1].Split("=");

				textBox.text = "absoluteURL: " + absoluteURL;
				textBox.text += "\r\n";
				textBox.text += "query: " + query;
				textBox.text += "\r\n";
				textBox.text += "DioRamaID: " + queryValue0[1];
				textBox.text += "\r\n";
				textBox.text += "UniqueKey: " + queryValue1[1];


				//-----------------------------------------------------------------
				//TODO: Checken of UniqueKey een hogere waarde heeft dan de huidige UniqueKey die nu 'live' kan zijn (oftewel er logt nu iemand opnieuw in)
				//-----------------------------------------------------------------


				//-----------------------------------------------------------------
				//TODO: Hier juiste scene inladen a.d.h.v. de DioRamaID
				
				//int dioRamaID = queryValue0[1];
				//string dioRamaSceneName = "";
				/*switch (dioRamaID){
					case 0:
						dioRamaSceneName = "DioRamaScene_hiernaam";
						break;

					default:
						//
						break;
				}*/
				//SceneManager.LoadScene(dioRamaSceneName);
				//-----------------------------------------------------------------
			}
			catch (System.Exception e){
				textBox.text = "Error with loadingscene. /r/n absoluteURL: " + absoluteURL;
			}
		}
		else{
			textBox.text = "Not running in WebGL!";
		}
    }

    void Update()
    {
        
    }
}
