using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//=----------------------------------------------------------------------------------------------------------------------
public class myButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler 
{
	public bool 	f_up	= false;
	//=----------------------------------------------------------------------------------------------------------------------
	public void OnPointerDown(PointerEventData eventData)
	{
		
	}
	//=----------------------------------------------------------------------------------------------------------------------
	public void OnPointerUp(PointerEventData eventData)
	{
		f_up	= true;
	}
}
//=----------------------------------------------------------------------------------------------------------------------
