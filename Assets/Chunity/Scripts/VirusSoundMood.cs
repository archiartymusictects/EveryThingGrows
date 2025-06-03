using UnityEngine;

public class VirusSoundMood : MonoBehaviour {
    public ChuckSubInstance chuck;
    [Range(0f, 1f)] public float virusScore = 0.5f;  // 0 = antibodies win, 1 = virus dominates

    void Start() {
        chuck.RunCode(@"
            // Global variable controlled by Unity
            global float virus_score;

            // === Virus Layer ===
            SinOsc virus => LPF virusLPF => Gain virusGain => JCRev r1 => dac;
            0.2 => virusGain.gain;
            0.2 => r1.mix;

            spork ~ playVirus();

            fun void playVirus()
            {
                while (true)
                {
                    virus_score => float v;
                    100 + v * 600 => virus.freq;
                    1000 + v * 5000 => virusLPF.freq;
                    v * 0.5 => virusGain.gain;
                    200::ms => now;
                }
            }

            // === Antibody Layer ===
            SawOsc antibody => BPF antibodyBPF => Gain antibodyGain => JCRev r2 => dac;
            0.2 => antibodyGain.gain;
            0.3 => r2.mix;

            spork ~ playAntibody();

            fun void playAntibody()
            {
                while (true)
                {
                    virus_score => float v;
                    (1 - v) * 500 + 100 => antibody.freq;
                    600 + (1 - v) * 3000 => antibodyBPF.freq;
                    (1 - v) * 0.5 => antibodyGain.gain;
                    200::ms => now;
                }
            }

            // === Ambient Pad ===
            TriOsc pad => LPF padLPF => Gain padGain => JCRev r3 => dac;
            0.15 => padGain.gain;
            0.6 => r3.mix;

            spork ~ playPad();

            fun void playPad()
            {
                [220, 330, 440] @=> int notes[];
                0 => int i;

                while (true)
                {
                    virus_score => float v;
                    notes[i % notes.cap()] => pad.freq;
                    1000 + (1 - v) * 5000 => padLPF.freq;
                    0.1 + (0.2 * (1 - v)) => padGain.gain;
                    i + 1 => i;
                    600::ms => now;
                }
            }
        ");
    }

    void Update() {
        chuck.SetFloat("virus_score", virusScore);
    }
}