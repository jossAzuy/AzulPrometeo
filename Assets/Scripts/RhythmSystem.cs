using UnityEngine;

public class RhythmSystem
{
    public float perfectPrecision = 0.05f;
    public float goodPrecision = 0.1f;

    public float bpm;
    public float interval;
    public float dspStartTime;
    public float songPosition;
    public float songPositionInBeats;
    public float beatPosition;
    public int lastBeat = -1;

    public RhythmSystem(float bpm)
    {
        this.bpm = bpm;
        interval = 60f / bpm;
        dspStartTime = (float)AudioSettings.dspTime;
    }

    public void UpdateRhythm()
    {
        songPosition = (float)(AudioSettings.dspTime - dspStartTime);
        songPositionInBeats = songPosition / interval;
        beatPosition = songPositionInBeats - Mathf.Floor(songPositionInBeats);
    }

    public int GetCurrentBeat()
    {
        return Mathf.FloorToInt(songPositionInBeats);
    }

    public bool IsPerfectBeat()
    {
        float comparedBeat = Mathf.Round(songPositionInBeats);
        float difference = songPositionInBeats - comparedBeat;
        return Mathf.Abs(difference) <= perfectPrecision;
    }

    public bool IsGoodBeat()
    {
        float comparedBeat = Mathf.Round(songPositionInBeats);
        float difference = songPositionInBeats - comparedBeat;
        return Mathf.Abs(difference) <= goodPrecision;
    }
}
