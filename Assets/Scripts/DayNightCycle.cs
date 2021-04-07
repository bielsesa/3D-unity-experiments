using System;
using UnityEngine;
using UnityEngine.UI;

public class DayNightCycle : MonoBehaviour
{
    public float time;
    public int speed;
    public TimeSpan currentTime;
    public Transform sunTransform;
    public Light sun;
    public Text timeText;
    public int days;
    public float intensity;
    public Color fogday;
    public Color fognight;

    private void Start()
    {
        speed = 5;
        days = 0;
        fogday = Color.grey;
        fognight = Color.black;
    }

    void Update()
    {
        ChangeTime();
    }

    private void ChangeTime()
    {
        // time update
        time += Time.deltaTime * speed;
        Debug.Log($"Time: {time}");

        // ++++ a day has passed
        if (time >= 86400)
        {
            days++;
            time = 0;
        }

        currentTime = TimeSpan.FromSeconds(time);

        // light update

        sunTransform.rotation = Quaternion.Euler(new Vector3((time - 21600) / 86400, 0, 0));
        Debug.Log($"X: {(time - 21600) / 86400}");

        if (time < 43200)
        {
            intensity = 1 - (43200 - time) / 43200;
        }
        else
        {
            intensity = 1 - ((43200 - time) / 43200) * -1;
        }

        RenderSettings.fogColor = Color.Lerp(fognight, fogday, intensity * intensity);

        // text update
        string[] tempTime = currentTime.ToString().Split(':');
        timeText.text = $"{tempTime[0]}:{tempTime[1]}\nDay: {days}";
    }
}
