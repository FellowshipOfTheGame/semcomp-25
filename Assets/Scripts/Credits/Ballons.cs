using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Ballons : MonoBehaviour
{
    [SerializeField] private float _fadeInTime;
    [SerializeField] private float _activeTime;
    [SerializeField] private float _fadeOutTime;

    [SerializeField] private Image _ballon_1;
    [SerializeField] private Image _ballon_2;
    [SerializeField] private Image _ballon_3;
    [SerializeField] private Image _ballon_4;

    [SerializeField] private TMP_Text _b1_title;
    [SerializeField] private TMP_Text _b1_names;
    [SerializeField] private TMP_Text _b2_title;
    [SerializeField] private TMP_Text _b2_names;
    [SerializeField] private TMP_Text _b3_title;
    [SerializeField] private TMP_Text _b3_names;
    [SerializeField] private TMP_Text _b4_title;
    [SerializeField] private TMP_Text _b4_names;
    
    private List<Image> _ballons = new List<Image>();
    private List<TMP_Text> _titles = new List<TMP_Text>();
    private List<TMP_Text> _names = new List<TMP_Text>();


    void Awake()
    {
        _ballons.Add(_ballon_1); _ballons.Add(_ballon_2);
        _ballons.Add(_ballon_3); _ballons.Add(_ballon_4);
        _titles.Add(_b1_title); _titles.Add(_b2_title);
        _titles.Add(_b3_title); _titles.Add(_b4_title);
        _names.Add(_b1_names); _names.Add(_b2_names);
        _names.Add(_b3_names); _names.Add(_b4_names);
    }

    void Start()
    {
        SetOpacityToZeroForAll();
        StartCoroutine(BallonFadeInAndOut(0));
    }

    IEnumerator SetTMPTextColorToZero(TMP_Text text, float seconds, bool isFading)
    {
        if (isFading)
            for (float i = seconds; i >= 0; i -= Time.deltaTime)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, i / seconds);
                yield return null;
            }
        else
            for (float i = 0.0f; i <= seconds; i += Time.deltaTime)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, i / seconds);
                yield return null;
            }
    }

    IEnumerator SetOpacityToZero(Image ballon, float seconds, bool isFading)
    {
        if (isFading)
            for (float i = seconds; i >= 0; i -= Time.deltaTime)
            {
                ballon.color = new Color(ballon.color.r, ballon.color.g, ballon.color.b, i / seconds);
                yield return null;
            }
        else
            for (float i = 0.0f; i <= seconds; i += Time.deltaTime)
            {
                ballon.color = new Color(ballon.color.r, ballon.color.g, ballon.color.b, i / seconds);
                yield return null;
            }
    }

    IEnumerator BallonFadeInAndOut(int index)
    {
        Debug.Log(index);
        if (index >= _ballons.Count)
            yield break;

        StartCoroutine(SetOpacityToZero(_ballons[index], _fadeInTime, false));
        StartCoroutine(SetTMPTextColorToZero(_titles[index], _fadeInTime, false));
        StartCoroutine(SetTMPTextColorToZero(_names[index], _fadeInTime, false));

        yield return new WaitForSeconds(_fadeInTime);
        yield return new WaitForSeconds(_activeTime);

        StartCoroutine(SetOpacityToZero(_ballons[index], _fadeOutTime, true));
        StartCoroutine(SetTMPTextColorToZero(_titles[index], _fadeOutTime, true));
        StartCoroutine(SetTMPTextColorToZero(_names[index], _fadeOutTime, true));

        StartCoroutine(BallonFadeInAndOut(index + 1));
    }

    private void SetOpacityToZeroForAll()
    {
        for (int i = 0; i < _ballons.Count; i++)
        {
            StartCoroutine(SetOpacityToZero(_ballons[i], 0, true));
            StartCoroutine(SetTMPTextColorToZero(_titles[i], 0, true));
            StartCoroutine(SetTMPTextColorToZero(_names[i], 0, true));
        }
    }


}
