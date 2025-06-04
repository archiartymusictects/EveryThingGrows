using UnityEngine;

public class Chuck_AmbientSound : MonoBehaviour {
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        GetComponent<ChuckSubInstance>().RunCode(@"
		            // Woosh sound generator
            class WooshVoice extends Chugraph {
                Noise n => LPF f => Gain g => JCRev r => ADSR env => outlet;

                0.2 => g.gain;
                0.5 => r.mix;
                env.set(0.01, 1.0, 0.0, 0.5); // slow envelope for whoosh

                fun void woosh(float startFreq, float endFreq, float dur) {
                dur::second => dur totalDur;
                totalDur / 100 => dur step;

                for (0 => int i; i < 100; i++) {
                    startFreq + (endFreq - startFreq) * (i/100.0) => f.freq;
                    step => now;
                }
            }

                fun void bang(float startFreq, float endFreq, float duration) {
                    env.keyOn();
                    spork ~ woosh(startFreq, endFreq, duration);
                    duration::second => now;
                    env.keyOff();
                }
            }
            // Heartbeat
            class Heartbeat extends Chugraph {
                SinOsc kick => ADSR env => JCRev r => outlet;
                60 => kick.freq;
                1.0 => kick.gain;
                0.2 => r.mix;
                env.set(0.01, 0.1, 0.0, 0.1);
                fun void bang() {
                    env.keyOn();
                    0.2::second => now;
                    env.keyOff();
                }
            }

            // Virus — Glitchy rising chaos
            class VirusVoice extends Chugraph {
                Noise n => BPF b => Gain g => JCRev r => ADSR env => outlet;
                0.25 => g.gain;
                0.3 => r.mix;
                env.set(0.005, 0.05, 0.2, 0.05);

                fun void bang(float freq) {
                    freq => b.freq;
                    10 + (freq / 100) => b.Q;
                    env.keyOn();
                    0.06::second => now;
                    env.keyOff();
                }
            }

            // Antibody — Sharp pulsing
            class AntibodyVoice extends Chugraph {
                SawOsc s => LPF f => Gain g => JCRev r => ADSR env => outlet;
                0.25 => g.gain;
                0.4 => r.mix;
                env.set(0.01, 0.1, 0.1, 0.05);
                fun void bang(float freq) {
                    freq => s.freq;
                    freq * 1.5 => f.freq;
                    env.keyOn();
                    0.1::second => now;
                    env.keyOff();
                }
            }

            // Create voices
            Heartbeat hb;
            VirusVoice virus;
            AntibodyVoice antibody;
            WooshVoice woosh;


            // Apply limiter
            Dyno dyno => dac;
            0::ms => dyno.attackTime;
            0.8 => dyno.thresh;
            SawOsc saw => dyno;
            0.002=>saw.gain;
            200=> saw.freq;
            // Connect all to limiter
            hb => dyno;
            virus => dyno;
            antibody => dyno;
            woosh => dyno;

            // Heartbeat loop
            fun void heartLoop() {
                while (true) {
                    hb.bang();
                    0.8::second => now;
                }
            }

            // Virus pattern: ascending freq bursts with chaos
            fun void virusLoop() {
                float freq;
                while (true) {
                    Math.random2(12,24) => int rand;
                    for (0 => int i; i < rand; i++) {
                        Math.random2(200,350) + i * Math.random2f(120,150) => freq;
                        virus.bang(freq);
                        Std.rand2f(0.05, 0.15)::second => now;
                    }
                    1::second => now; // pause between surges
                }
            }
            // Intermittent whooshes with rising and falling sweeps
            fun void wooshLoop() {
                while (true) {
                    // Randomize direction and length
                    if (Std.rand2(0, 1) == 0) {
                        woosh.bang(200, 2000, Std.rand2f(1.5, 3.0)); // rising whoosh
                    } else {
                        woosh.bang(2000, 300, Std.rand2f(1.5, 3.0)); // falling whoosh
                    }

                    // Wait a random amount before the next
                    Std.rand2f(2.0, 6.0)::second => now;
                }
            }

            // Antibody loop: slower, patterned pulses
            fun void antibodyLoop() {
                int max;
                while (true) {
                    Math.random2(3,9) => int rand;
                    if (rand > 5)700 =>  max;
                    else 2400 => max;
                    for (0 => int i; i < rand; i++) {
                        antibody.bang(Std.rand2f(300, max));
                        0.3::second => now;
                    }
                    1.5::second => now;
                }
            }

            // Launch all
            spork ~ heartLoop();
            spork ~ virusLoop();
            spork ~ antibodyLoop();
            spork ~ wooshLoop();
            while (true){
                100::ms =>now;
            }
	"); 
    }

    
}

/*
 using UnityEngine;

public class Chuck_AmbientSound : MonoBehaviour {
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
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

    
}
 
 * */
