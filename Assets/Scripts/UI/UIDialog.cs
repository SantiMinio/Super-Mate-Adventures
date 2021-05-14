using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIDialog : MonoBehaviour
{
    [SerializeField, Range(1, 20)]
    float speed = 9;

    float timer = 0;
    const float time_to_go = 1;
    bool anim;
    bool go;
    [SerializeField] float appearTime = 2;
    [SerializeField] TextMeshProUGUI dialog = null;

    [SerializeField] bool automaticClose = true;

    Vector3 currentpos;
    Vector3 initHidepos;
    Vector3 finalHidepos;

    public enum AppearSide { Up, Down, Left, Right }
    [SerializeField] AppearSide appearSide;
    [SerializeField] AppearSide dissapearSide;

    private void Start()
    {
        currentpos = transform.localPosition;
        switch (appearSide)
        {
            case AppearSide.Up: initHidepos = new Vector3(transform.localPosition.x, transform.localPosition.y + Screen.height, transform.localPosition.z); break;
            case AppearSide.Down: initHidepos = new Vector3(transform.localPosition.x, transform.localPosition.y - Screen.height, transform.localPosition.z); break;
            case AppearSide.Left: initHidepos = new Vector3(transform.localPosition.x - Screen.width, transform.localPosition.y, transform.localPosition.z); break;
            case AppearSide.Right: initHidepos = new Vector3(transform.localPosition.x + Screen.width, transform.localPosition.y, transform.localPosition.z); break;
        }

        switch (dissapearSide)
        {
            case AppearSide.Up: finalHidepos = new Vector3(transform.localPosition.x, transform.localPosition.y + Screen.height, transform.localPosition.z); break;
            case AppearSide.Down: finalHidepos = new Vector3(transform.localPosition.x, transform.localPosition.y - Screen.height, transform.localPosition.z); break;
            case AppearSide.Left: finalHidepos = new Vector3(transform.localPosition.x - Screen.width, transform.localPosition.y, transform.localPosition.z); break;
            case AppearSide.Right: finalHidepos = new Vector3(transform.localPosition.x + Screen.width, transform.localPosition.y, transform.localPosition.z); break;
        }
        transform.localPosition = initHidepos;
    }

    void OnGo(float time_value)
    {
        transform.localPosition = Vector3.Lerp(initHidepos, currentpos, time_value);
    }
    void OnBack(float time_value)
    {
        transform.localPosition = Vector3.Lerp(currentpos, finalHidepos, time_value);
    }

    private void Update()
    {
        if (anim)
        {
            if (timer < time_to_go)
            {
                timer = timer + speed * Time.deltaTime;

                if (go)
                {
                    OnGo(timer);
                }
                else
                {
                    OnBack(timer);
                }
            }
            else
            {
                timer = 0;
                anim = false;
                if(go)open = true;
            }
        }

        if (!automaticClose) return;
        if (open)
        {
            openTimer += Time.deltaTime;

            if(openTimer >= appearTime)
            {
                openTimer = 0;
                open = false;
                Close();
            }
        }

    }
    bool open;
    float openTimer;
    public void Open(string text)
    {
        anim = true;
        go = true;
        open = false;
        openTimer = 0;
        timer = 0;
        dialog.text = text;
    }

    public void Close()
    {
        anim = true;
        go = false;
    }
}
