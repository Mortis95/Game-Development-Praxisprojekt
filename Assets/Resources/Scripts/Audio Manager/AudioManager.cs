using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    
    public Sound[] soundEffects;
    public Music[] soundTrack;
    public Music currentlyPlaying;

    private static AudioManager instance;

    public static AudioManager getInstance(){
        return instance;
    }
    

    void Awake(){
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }

        currentlyPlaying = null;

        foreach (Sound sound in soundEffects){
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.isLooping;
        }

        foreach (Music music in soundTrack){
            music.source = gameObject.AddComponent<AudioSource>();
            music.source.clip = music.clip;
            music.source.volume = music.volume;
            music.source.pitch = music.pitch;
            music.source.loop = true;       //All music loops by default
        }
    }


    public void PlaySound(string name){
        Sound sound = Array.Find(soundEffects, s => s.name == name);
        if (sound == null){
            Debug.LogWarning("Sound " + name + " konnte nicht gefunden werden im sound Array und wird deswegen nicht abgespielt. \nTypo? Vergessen den Sound hinzuzufügen? Sonderzeichen?");
            return;
        }
        sound.source.PlayOneShot(sound.clip, sound.volume);
    }

    public void PlaySoundNotOverlapping(string name){
        Sound sound = Array.Find(soundEffects, s => s.name == name);
        if (sound == null){
            Debug.LogWarning("Sound " + name + " konnte nicht gefunden werden im sound Array und wird deswegen nicht abgespielt. \nTypo? Vergessen den Sound hinzuzufügen? Sonderzeichen?");
            return;
        }
        if(sound.source.isPlaying){return;} //Don't allow overlapping or cancelling old activations
        sound.source.Play();
    }

    public void StopSound(string name){
        Sound sound = Array.Find(soundEffects, s => s.name == name);
        if (sound == null){
            Debug.LogWarning("Sound " + name + " konnte nicht gefunden werden im sound Array und wird deswegen nicht abgespielt. \nTypo? Vergessen den Sound hinzuzufügen? Sonderzeichen?");
            return;
        }
        if (sound.source.isPlaying){
            /* Debug.Log("Stopping Sound:" + sound.name); */
            sound.source.Stop();
        }
    }

    //Very similar to PlaySound, but only one music at a time can and should be played.
    public void PlayMusic(string name){
        if(currentlyPlaying != null){currentlyPlaying.source.Stop();}
        Music track = Array.Find(soundTrack, m => m.name == name);
        if(track == null){
            Debug.LogWarning("Musik " + name + " konnte nicht gefunden werden im SoundTrack Array! Tippfehler? Eventuell PlaySound() gemeint statt PlayMusic() ?");
            return;
        }
        if(!track.source.isPlaying){
            track.source.Play();
            currentlyPlaying = track;
        }
    }

    //Very similar to StopSound, but only one music at a time can and should be played.
    public void StopMusic(){
        if(currentlyPlaying != null){
            currentlyPlaying.source.Stop();
            currentlyPlaying = null;
        } else {
            Debug.LogWarning("No music is currently playing!");
        }
    }
}

