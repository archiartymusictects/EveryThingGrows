using UnityEngine;
using UnityEngine.UI;

public class Chuck_VirusAudioSystem : MonoBehaviour {
    public ChuckSubInstance chuck;
    public Slider virusSlider;
    private string lastState = ""; // Track last state sent to prevent spam

    void Start() {
        chuck = GetComponent<ChuckSubInstance>();

        if (chuck == null) {
            Debug.LogError("ChuckSubInstance not found on this gameObject!");
            return;
        }

        Debug.Log("[Chuck] ChuckSubInstance found, running heartbeat...");

        chuck.RunCode(@"
            global float virusScore;
            <<< ""[Chuck] Heartbeat running"" >>>;

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
            //------NEW: Ambient voice function-----------------------------------
            fun void ambientLoop() {
                SinOsc lfo => blackhole;
                0.3 => lfo.freq;

                // Number of voices (adds thickness)
                3 => int numVoices;
                TriOsc voices[numVoices];
                Gain   gains[numVoices];
    
                // Shared filter + reverb
                Gain mix => LPF f => JCRev r => dac;
                0.12 => mix.gain;
                0.07 => r.mix;
                600 => f.freq;

                // Connect all voices through their gains into mix
                for (0 => int i; i < numVoices; i++) {
                    voices[i] => gains[i] => mix;
                    1.0 / numVoices => gains[i].gain; // equal gain per voice
                }

                // Base detune spread for chord (in Hz)
                [ -10.0, 0.0, 7.0 ] @=> float detunes[]; // minor 2nd/dyad interval feel

                while (true) {
                    // Calculate base pitch based on virusScore
                    150.0 + (350.0 * virusScore) => float baseFreq;
                    Math.random2f(-0.1, 0.1) * 20 => float drift;
                    baseFreq + drift => baseFreq;

                    // For each voice, apply detune, LFO vibrato
                    for (0 => int i; i < numVoices; i++) {
                        float lfoOffset => float phaseOffset; // optionally add different LFO offsets per voice
                        // Simulate LFO offset using last() and a time delta
                        spork ~ modulateVoice(voices[i], baseFreq + detunes[i], lfo, i * 0.05);
                    }

                    // Adjust filter based on virusScore
                    300 + (700 * virusScore) => f.freq;
        
                    // Wait before regenerating base note
                    6::second => now;
                }
            }

            // Sporked function to modulate a single voice
            fun void modulateVoice(TriOsc osc, float freq, SinOsc lfo, float phaseOffset) {
                freq => float base;

                // Modulate for several seconds with slow LFO-based vibrato
                for (0 => int t; t < 300; t++) {
                    // Create a small vibrato using LFO phase offset
                    base + (lfo.last() * 4.0) + Math.sin(2*pi*lfo.phase() + phaseOffset) * 2.0 =>
                        osc.freq;
                    20::ms => now;
                }
            }                      
            //------Virus Effects — glitchy noise pings-----------------------------------
            fun void virusEffectLoop() {
                Noise n => BPF b => ADSR env => dac;
                2000 => b.freq;
                5 => b.Q;
                env.set(0.001, 0.05, 0, 0.1);
                0.2 => n.gain;

                while (true) {
                    // Trigger more often as virusScore rises
                    if (Std.rand2f(0.0, 1.0) < virusScore * 0.4) {
                        env.keyOn();
                        50::ms => now;
                        env.keyOff();
                    }

                    // Short random interval between shots
                    (100 + Std.rand2(0, 400))::ms => now;
                }
            }

            //---Antibody Effects — gentle bell-like pulses--------------------------------------
            fun void antibodyEffectLoop()
            {
                ModalBar mb => JCRev r1 => dac;
                TubeBell tb => JCRev r2 => dac;

                0.12 => mb.gain;
                0.12 => tb.gain;

                0.25 => r1.mix;
                0.25 => r2.mix;

                while (true)
                {
                    // Immune sparkle event: more likely when virusScore is low (body healthier)
                    if (Std.rand2f(0.0, 1.0) < (1.0 - virusScore) * 0.4)
                    {
                        if (Std.rand2f(0, 1) < 0.5)
                        {
                            // --- ModalBar: bell/mallet strike ---
                            Std.rand2(0, 4) => mb.preset; // different materials
                            Std.rand2f(2.0, 4.5) => mb.stickHardness;
                            600.0 + Std.rand2f(0, 800.0) => mb.freq;
                            mb.noteOn(1.0);
                            300::ms => now;
                            mb.noteOff(0.5);
                        }
                        else
                        {
                            // --- TubeBell: resonant tubular bell tone ---
                            600.0 + Std.rand2f(0, 800.0) => tb.freq;
                            0.3 + Std.rand2f(0.0, 0.5) => float velocity;
                            tb.noteOn(velocity);
                            300::ms => now;
                            tb.noteOff(velocity * 0.5);
                        }
                    }

                    // Slightly randomized delay
                    (300 + Std.rand2(0, 400))::ms => now;
                }
            }            
            //-----------------------------------------------------------------------------------
            spork ~ heartLoop();
            spork ~ ambientLoop();
            spork ~ virusEffectLoop();
            spork ~ antibodyEffectLoop();

            // Keep VM alive
            while (true) {
                100::ms => now;
            }
        ");
    }

    void Update() {
        if (chuck != null && virusSlider != null) {
            chuck.SetFloat("virusScore", virusSlider.value);
        }
    }
}








