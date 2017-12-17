using UnityEditor;
using UnityEngine;

namespace HD
{
  [InitializeOnLoad]
  public class AlertOnError
  {
    double timeOfLastError = double.MinValue;

    AudioClip clip;

    /// <summary>
    /// Called before the game starts c/o InitializeOnLoad.
    /// </summary>
    static AlertOnError()
    {
      AlertOnError alerter = new AlertOnError();
    }

    AlertOnError()
    {
      Application.logMessageReceived += OnLogMessage;
      EditorApplication.playmodeStateChanged += OnPlaymodeChange;
      InitClip();
    }

    void OnPlaymodeChange()
    {
      timeOfLastError = double.MinValue;
    }

    void InitClip()
    {
      if (clip != null)
      {
        return;
      }

      clip = Resources.Load<AudioClip>("Unity.ErrorAlert.ErrorAlertSound");
      Debug.Assert(clip != null, 
        "A resource audio clip named Unity.ErrorAlert.ErrorAlertSound is required");
    }

    void OnLogMessage(
      string condition,
      string stackTrace,
      LogType type)
    {
      if (type == LogType.Log
        || type == LogType.Warning)
      { // Ignore 
        return;
      }

      InitClip();

      if((EditorApplication.timeSinceStartup - timeOfLastError) < clip.length)
      { // Too soon
        return;
      }

      AudioSource.PlayClipAtPoint(clip, Vector3.zero);
      timeOfLastError = EditorApplication.timeSinceStartup;
    }
  }
}
