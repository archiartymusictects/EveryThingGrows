using UnityEngine;
using UnityEngine.UI;

public class Chuck_VirusAudioSystem : MonoBehaviour {
    public ChuckSubInstance chuck;
    public Slider virusSlider;

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
            fun void antibodyEffectLoop() {
                SndBuf buf => Pan2 p => dac;

                // Load a short chime sample or simulate one
                // ?? If you don't have an audio file, use simple FM beep instead:
                SinOsc c => ADSR env => dac;
                0.3 => c.gain;
                env.set(0.01, 0.2, 0.0, 0.1);
                0.5 => env.gain;

                while (true) {
                    // Emit more often when virusScore is low (immune is strong)
                    if (Std.rand2f(0.0, 1.0) < (1.0 - virusScore) * 0.3) {
                        c.freq() + Std.rand2f(-20, 20) => c.freq;
                        env.keyOn();
                        200::ms => now;
                        env.keyOff();
                    }
                    (200 + Std.rand2(0, 600))::ms => now;
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








//----------------------------------------
/*

global float virusScore;

// Smoothed virus score
class VirusState {
0.0 => float current;

fun void updateLoop() {
    while (true) {
        virusScore => float target;
        (target - current) * 0.05 => float delta;
        current + delta => current;
        0.1::second => now;
    }
}

}

VirusState vs;
spork ~ vs.updateLoop();

// ************************************
// Voice Definitions
// ************************************

// Heartbeat — Strength & pattern varies with virusScore
class Heartbeat extends Chugraph {
Impulse beat => LPF f => ADSR env => Gain g => JCRev r => outlet;

80 => f.freq;
0.2 => g.gain;
0.3 => r.mix;
env.set(0.01, 0.15, 0, 0.1);
}

Heartbeat hb;
hb => dac;

// *******************************************
// Virus Voice — From subtle to swarm chaos
class VirusVoice extends Chugraph {
Noise n => BPF b => Gain g => JCRev r => ADSR env => Pan2 p => outlet;
0.15 => g.gain;
0.3 => r.mix;
env.set(0.01, 0.06, 0.0, 0.05);
}

// Factory for virus swarm
class VirusAgent {
VirusVoice v;
fun void go() {
    while (true) {
        vs.current => float danger;

        // density increases, frequency shifts
        danger * 0.3 + 0.05 => float spawnRate;
        0 => int burstCount;

        if (danger > 0.2) Math.random2(3, 8) => burstCount;
        else Math.random2(0, 2) => burstCount;

        for (0 => int i; i < burstCount; i++) {
            400 + Std.rand2f(-200, 600 * danger) => v.b.freq;
            v.env.keyOn();
            Std.rand2f(-1, 1) => v.p.pan;
            0.05::second => now;
            v.env.keyOff();
        }

        (1.0 - danger * 0.9) * 0.4::second => now;
    }
}
}

for (0 => int i; i < 8; i++) {
VirusAgent a;
spork ~ a.go();
}

// ************************************************
// Antibody — Warm, resonant defensive patrols
class AntibodyVoice extends Chugraph {
SawOsc s => LPF f => Gain g => JCRev r => ADSR env => Pan2 p => outlet;
0.2 => g.gain;
0.25 => r.mix;
env.set(0.01, 0.18, 0.1, 0.05);
}

fun void antibodyPatrol() {
while (true) {
    vs.current => float danger;
    (1 - danger) * 0.5 + 0.3 => float calm;

    AntibodyVoice ab;
    300 + Std.rand2f(0, 500 * calm) => ab.s.freq;
    ab.s.freq() * 1.5 => ab.f.freq;
    Std.rand2f(-0.9, 0.9) => ab.p.pan;
    ab.env.keyOn();
    0.2::second => now;
    ab.env.keyOff();
    (1.0 - danger) * 0.6::second => now;
}
}

spork ~ antibodyPatrol();

// ****************************************
// Ambient — Circulatory & body hum
class AmbientVoice extends Chugraph {
SinOsc h => Gain g => JCRev j => ADSR env => outlet;
0.004 => g.gain;
0.3 => j.mix;
h.freq(50);
env.set(1, 2, 0.5, 1.5);
}

AmbientVoice ambient;
ambient => dac;

fun void ambientModLoop() {
while (true) {
    vs.current => float danger;

    ambient.h.freq(50 + 30 * danger);
    1.0 + Math.sin(now / second * 0.5 + danger * 5.0) * 0.4 => ambient.j.mix;
    danger * 0.8 + 0.1 => ambient.g.gain;
    0.5::second => now;
}
}

spork ~ ambientModLoop();

// ****************************************
// Heartbeat Reactivity
fun void heartbeatLoop() {
while (true) {
    vs.current => float intensity;

    // BPM modulated by virusScore
    (0.6 - 0.4 * intensity)::second => dur beatDur;
    hb.env.keyOn();
    beatDur => now;
    hb.env.keyOff();

    // Glitch skips (erratic rhythm)
    if (intensity > 0.7 && Std.rand2f(0,1) < 0.3) {
        (Std.rand2f(0.05, 0.2))::second => now;
    } else {
        beatDur => now;
    }
}
}

spork ~ heartbeatLoop();

// ****************************************
// Organ Voices (Layered sine/chaos)
class OrganVoice extends Chugraph {
SinOsc tone => BPF f => Gain g => JCRev r => outlet;
0.01 => g.gain;
0.2 => r.mix;
}

fun void organLoop() {
OrganVoice organs[4];
for (0 => int i; i < organs.size(); i++) {
    organs[i] => dac;
    organs[i].tone.freq(100 + i * 60);
    organs[i].f.freq(organs[i].tone.freq() * 3);
}

while (true) {
    vs.current => float d;
    for (0 => int i; i < 4; i++) {
        // Update tone frequency and gain
        80 + i * 60 + Std.rand2f(-10, 10) * d => organs[i].tone.freq;
        organs[i].f.freq(organs[i].tone.freq() * 3);
        0.01 + 0.05 * d => organs[i].g.gain;
    }

    0.5::second => now;
}
}

spork ~ organLoop();

// ****************************************
// Cellular Voice — Short ticks
class CellularVoice extends Chugraph {
BlitSquare b => Gain g => ADSR env => Pan2 p => outlet;
0.05 => g.gain;
env.set(0.01, 0.04, 0.0, 0.02);
}

fun void cellularActivity() {
CellularVoice cell;
cell => dac;

while (true) {
    vs.current => float d;
    (1.0 - d) * 0.3 + 0.05 => float baseRate;
    400 + Std.rand2f(-100, 350 * d) => cell.b.freq;
    Std.rand2f(-1, 1) => cell.p.pan;
    cell.env.keyOn();
    0.05::second => now;
    cell.env.keyOff();
    baseRate::second => now;
}
}

spork ~ cellularActivity();

// ****************************************
// End fallback loop
while (true) {
20::second => now;
}    */


