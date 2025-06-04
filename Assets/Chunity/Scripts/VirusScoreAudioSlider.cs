using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VirusScoreAudioSlider : MonoBehaviour
{
    public ChuckSubInstance chuck;
    public Slider virusSlider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (chuck == null || virusSlider == null) {
            Debug.LogError("Chuck or Slider not assigned!");
            return;
        }

        // Run the heartbeat ChucK code
        chuck.RunCode(@"
        global float virusScore;

        class Heartbeat extends Chugraph {
            SinOsc s => ADSR env => dac;
            60 => s.freq;
            0.3 => s.gain;
            env.set(0.01, 0.1, 0, 0.1);

            fun void bang() {
                env.keyOn();
                0.2::second => now;
                env.keyOff();
            }
        }

        Heartbeat hb;

        fun void heartLoop() {
            while (true) {
                float interval;
                (0.6 - 0.4 * virusScore) => interval;
                if (interval < 0.1) 0.1 => interval;

                hb.bang();
                interval::second => now;
            }
        }

        spork ~ heartLoop();
    ");

    }

    // Update is called once per frame
    void Update()
    {
        float virusValue = virusSlider.value;
        chuck.SetFloat("virusScore", virusValue);


    }
}
