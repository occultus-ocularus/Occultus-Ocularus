///////////////////////////////////////////////////////////////////////////
//---------------------DOCUMENTATION-------------------------------------//
///////////////////////////////////////////////////////////////////////////

//INFO: This is the Audio script used within the Game Object audio_manager. This handles all our audio changes through scripts. It basically changes the parameters we see in the editor through code. The code in update is for allowing key input to change parameters such as volume, mute, global stop. 
//USAGE: use these sample methods in other classes:

/*public void stopAudio(){
      playingAudio = false;  //<- this just is for toggles
      GameObject am = GameObject.FindGameObjectWithTag("audio_manager");
      Audio_Script am_script = am.GetComponent<Audio_Script>();
      am_script.stopAudio();
   }
   
   public void playAudio(string file, [int time || double percent]){
      playingAudio = true; //<- this is just for toggles
      GameObject am = GameObject.FindGameObjectWithTag("audio_manager");
      Audio_Script am_script = am.GetComponent<Audio_Script>();
      am_script.playAudio(file, [time || percent]); You pass time xor percent; time is of type int; percent is of type double;  its an overloaded function. If you pass an integer it will start the audio that many seconds in. If you pass a double, it will play starting at that percent of the way through
   }
   
   playSFX(string file){
      used simlar to playAudio but intended for short sound effects as this method is made not to need a method to stop it
   }
*/

//---------------------------------------------------------------------------


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioScript : MonoBehaviour {
   public AudioSource src0;  //background audio source (read: track)
   public AudioClip file0;  //background audio file
   public AudioSource src1;  //Announcer audio source (read: track);
   public AudioClip file1;   //Announcer audio file
   public AudioSource src2;  //Other track
   public AudioClip file2;
   
   //these two are used for actually changing audio in the methods. Most method calls in this class contains an integer that specifies the track it should affect. They are kind of like pointers and point to src# and file# where # is the track integer given as input. This allows you to change between which track to change without having to make a new set of methods for each audio_track.  passing 0 as a track uses the audio_manager's internal amsrc and amfile.   
   public AudioSource src;
    public AudioClip head;
    public AudioClip body;
    public bool wait = false;
    public bool dualAudio = false;
    public AudioClip file;
   public static AudioSource staticSrc;
   public static AudioClip staticFile;
   public float stopTime;  //used to hold where you are at when you stopped
   public bool playingAudio = false;

    public static AudioScript instance = null;

   public void Awake(){/*
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(this);

        } else {
            Destroy(gameObject);
        }*/
    }
   
	// Use this for initialization
	void Start () {
        staticSrc = GetComponents<AudioSource>()[0];
        staticFile = file0;
        src = GetComponents<AudioSource>()[0];
        src0 = GetComponents<AudioSource>()[0];
        src1 = GetComponents<AudioSource>()[1];
        src2 = GetComponents<AudioSource>()[2];
        src.clip = file0;
        src0.clip = file0;
        src1.clip = file1;
        src2.clip = file2;
    }

    public void playDelayedSFX(string name, int time) {
        StartCoroutine(playSFXDelayed(name, time));
    }

    IEnumerator playSFXDelayed(string name, int time) {
        yield return new WaitForSeconds(time);
        AudioClip sfx = Resources.Load<AudioClip>("Audio/" + name);
        src1.PlayOneShot(sfx);
        if (sfx == null) print("sfx problem");
    }

    public void playSFX(string name){// This would be for playing short sounds like sound effects. Think collision, level complete, footsteps
      AudioClip sfx = Resources.Load<AudioClip> ("Audio/"+name);
      src2.PlayOneShot(sfx);
        if (sfx == null) print("sfx problem");
    
   }

    public static void playStaticSFX(string name) {// This would be for playing short sounds like sound effects. Think collision, level complete, footsteps
        AudioClip sfx = Resources.Load<AudioClip>("Audio/" + name);
        staticSrc.PlayOneShot(sfx);
        if (sfx == null) print("sfx problem");
    }


    public void chooseTrack(int track){ //this method is called in most other functions in here and serves to take in the track integer input that is passed to the methods in this class that is then passed here. It takes this integer and sets src and file to point to the the class regarding the track integer variable. They are kind of like pointers and point to src# and file# where # is the track integer given as input. This makes it so that we only affect the track sent to us.
        if (track == 1){ //these are to set the pointer src and file to the audio_track you want to change
         src = src1;
         file = file1;
      }else if(track == 2){
         src = src2;
         file = file2;
      }else{
         src = src0;
         file = file0;
      }
   }
   
   
    IEnumerator playDelayed(string nameOrPath,int track, int delay) {
        yield return new WaitForSeconds(delay);
        chooseTrack(track);
        playAudio(nameOrPath, track);
    }

   
    public void playAudioDelayed(string nameOrPath, int track, int delay) {
        StartCoroutine(playDelayed(nameOrPath, track, delay));
    } 

   //NEW SONG FUNCTIONS: These change the song you are working with
   public void playAudio(string name, double percent, int track){//percent //overloaded method that serves to start to start the audio at X Percent of the way through. //plays audio through a string passed to it. This string is the file path to the audio file in Assets/Resources. Example: Assets/Resources/raiju.mp3  would be: raiju.mp3
      
      chooseTrack(track);
      src.Stop();
      src.time = 0;
      playingAudio = true;
      //AudioClip clip = Resources.Load <AudioClip> (name); //This is you don't
      file = Resources.Load<AudioClip>(name);   //This is if you want to override the AudioClip defined in the editor.
      float percentage = (float)percent/100; //For typecasting the double into a float
      src.time = percentage * file.length; //||percentage * clip.length  //changes the start time to X percent in
      Debug.Log("perc");
      src.Play();
   }

   public void playAudio(string name,int stamp, int track){//time //overloaded method that serves to start the audio at X Seconds in. //plays audio through a string passed to it. This string is the file path to the audio file in Assets/Resources. Example: Assets/Resources/raiju.mp3  would be: raiju.mp3
      
      chooseTrack(track);
      src.Stop();
      src.time = 0;
      playingAudio = true;
      float timestamp = (float)stamp;  //typecast int to a float
      src.time = timestamp; //sets the time to X seconds in
      //Audio clip = Resources.Load <AudioClip> (name); //This is you don't
      file = Resources.Load<AudioClip>(name);   //This is if you want to override the AudioClip defined in the editor.
      Debug.Log("time");
      src.Play();
   }
   
   public void playAudio(string filePath, int track){ //just play new song //overloaded method that just plays the audio from where it last stopped. Call this if you dont want to change the place of the current song
      
      string name = filePath;
      chooseTrack(track);
      src.Stop();
      src.time = 0;
      playingAudio = true;
      src.clip = Resources.Load <AudioClip> ("Audio/"+name); //This is you don't
      if (src.clip == null) print("Play Audio: null Audio Source");
      src.Play();
      return;
   }
   
    public void playDualAudio(string head, string body) {
        chooseTrack(0);
        src.Stop();
        src.time = 0;
        wait = true;
        dualAudio = true;
        this.head = Resources.Load<AudioClip>("Audio/" + head);
        this.body = Resources.Load<AudioClip>("Audio/" + body);
        if (this.head == null) print("Dual Audio: null Audio Source - head");
        if (this.body == null) print("Dual Audio: null Audio Source - body");
        print("Playing dual audio");
        src.clip = this.head;
        src.Play();
        
    }


   
   //SAME SONG FUNCTIONS: These work with the song currently being played
   
   public void jumpTo(float jumpTime, int track){ //jumps to x seconds in the current song
      chooseTrack(track);
      playingAudio = true;
      src.Stop();
      src.time = (float)jumpTime;
      src.Play();
   }

   public void jumpTo(int jumpPercent, int track){ //jumps to x seconds in the current song
      chooseTrack(track);
      playingAudio = true;
      src.Stop();
      src.time = (float)jumpPercent/100F * file.length;
      src.Play();
   }

   public void playAudio(int track){ //play the same song from where you paused
      chooseTrack(track);
      src.Stop();
      src.time = 0;
      playingAudio = true;
      Debug.Log(src.clip);
      src.time = stopTime;
      src.Play();
   }
   
   public void stopAudio(int track){  //stops all audio on this track 
      chooseTrack(track);
      playingAudio = false;
      stopTime = src.time;
      src.Stop();
   }
   
   public void resetAudio(int track){  //stops all audio on this track and starts this from beginning
      chooseTrack(track);
      playingAudio = false;
      src.time = 0;
      src.Stop();
   }


    public void IncreaseVolume(int track) {
        chooseTrack(track);
        src.volume += 0.1f;
    }
   
    public void DecreaseVolume(int track) {
        chooseTrack(track);
        src.volume -= 0.1f;
    }

   
   public void stopAllAudio(){
        resetAllAudio();
   }
   
   public void resetAllAudio(){
      src.time = 0;
      src.Stop();
      src0.time = 0;
      src0.Stop();
      src1.time = 0;
      src1.Stop();
      src2.time = 0;
      src2.Stop();
   }


    // Update is called once per frame
    public void Update() {
       // if (instance == null) Destroy(gameObject); //QUESTIONABLE CODE
       
        if(src == null || src0 == null || src1 == null || src2 == null) {
        staticSrc = GetComponents<AudioSource>()[0];
        staticFile = file0;
        src = GetComponents<AudioSource>()[0];
        src0 = GetComponents<AudioSource>()[0];
        src1 = GetComponents<AudioSource>()[1];
        src2 = GetComponents<AudioSource>()[2];
        src.clip = file0;
        src0.clip = file0;
        src1.clip = file1;
        src2.clip = file2;
        }
        if(dualAudio && Mathf.Abs(src.clip.length-src.time) < .5) {
            src.clip = body;
            src.Play();
        }

        //Debug.Log("src1: "+ src.time);
        //Debug.Log("src2: "+src2.time);
        //these do things based upon keys pressed
        //https://docs.unity3d.com/ScriptReference/KeyCode.html
        if (Input.GetKeyDown(KeyCode.Backspace)){  
        resetAllAudio();
      }else if(  (Input.GetKeyDown(KeyCode.Equals)) || (Input.GetKeyDown(KeyCode.Plus)) ){
        src.volume+=0.1F;
      }else if(Input.GetKeyDown(KeyCode.Minus)){
        src.volume-=0.1F;
      }else if(Input.GetKeyDown(KeyCode.Alpha0)){
        if(src.mute){src.mute = false;}else{src.mute = true;} 
      }
      
      
   }//end Update
   
}//end Class Audio_Script