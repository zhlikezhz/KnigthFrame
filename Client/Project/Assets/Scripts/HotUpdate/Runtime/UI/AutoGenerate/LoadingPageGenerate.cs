//------------------------------------------------------------------------------
//--  <autogenerated>
//--      This code was generated by a tool.
//--      Changes to this file may cause incorrect behavior and will be lost if
//--      the code is regenerated.
//--  </autogenerated>
//------------------------------------------------------------------------------

using Huge.MVVM;
using UnityEngine;
using UnityEngine.UI;

[ViewSettingAttribute("Assets/Art/UI/Prefab/Common/LoadingPage.prefab")]
public class LoadingPageGenerate : Page
{
	public Text _TxtNum;
	public Text _TxtSpeed;
	public Slider _SldBar;
	public Transform _BGObj;

	protected override void OnInit()
	{
		_SldBar = transform.Find("_SldBar").GetComponent<Slider>();
		_TxtNum = transform.Find("_SldBar/_TxtNum").GetComponent<Text>();
		_TxtSpeed = transform.Find("_TxtSpeed").GetComponent<Text>();
		_BGObj = transform.Find("_BGObj");
	}
}
