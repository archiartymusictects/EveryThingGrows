using UnityEngine;

public class VirusController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<ChuckSubInstance>().RunCode(@"
		SinOsc osc => dac;
        0.4 => osc.gain;
		while( true )
		{
			Math.random2f( 300, 1000 ) => osc.freq;
			100::ms => now;
		}
	");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
