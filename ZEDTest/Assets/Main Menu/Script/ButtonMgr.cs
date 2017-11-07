using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonMgr : MonoBehaviour {




	public void button_scene(string scene_name){
		SceneManager.LoadScene (scene_name);
	}

}
