using UnityEngine;
using UnityEngine.Rendering;

public class testShakeing : MonoBehaviour {
    public ScreenShake shake;
    [SerializeField] float duration;
    [SerializeField] float magnitude;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V)){
            shake.TriggerShake(duration, magnitude);
        }
        
    }
}
