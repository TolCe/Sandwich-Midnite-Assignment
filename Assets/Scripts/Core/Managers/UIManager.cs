﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject levelCompletedPanel;
    public GameObject levelFailedPanel;
    public GameObject[] startElements;
    public GameObject[] levelElements;
    [Header("Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private EventTrigger playInputEventTrigger;
    public delegate void InputDelegate(bool state);
    public InputDelegate InputDel;
    public delegate void EmptyDelegate();
    public EmptyDelegate EmptyDel;

    private void Start()
    {
        UIEvents.Instance.OnAssignInputEvent += AssignGameInputs;
        UIEvents.Instance.OnAssignStartUIEvent += AssignStartUI;
        GameEvents.Instance.OnGameStartEvent += GameStart;
    }

    private void AssignGameInputs(InputDelegate inputDel)
    {
        InputDel = inputDel;
        AddPlayInputEvents();
    }
    private void AssignStartUI(EmptyDelegate emptyDel)
    {
        EmptyDel = emptyDel;
        SetListeners();
    }

    private void GameStart()
    {
        ChangeActivityOfElement(startElements, false);
        ChangeActivityOfElement(levelElements, true);
    }

    public void SetListeners()
    {
        startButton.onClick.AddListener(() => EmptyDel());
        //nextLevelButton.onClick.AddListener(() => LevelManager.Instance.NextLevel());
        //restartButton.onClick.AddListener(() => LevelManager.Instance.LoadLevel());
    }

    public void AddPlayInputEvents()
    {
        EventTrigger trigger = playInputEventTrigger;
        EventTrigger.Entry pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((data) => { InputDel(true); });
        trigger.triggers.Add(pointerDown);
        EventTrigger.Entry pointerUp = new EventTrigger.Entry();
        pointerUp.eventID = EventTriggerType.PointerUp;
        pointerUp.callback.AddListener((data) => { InputDel(false); });
        trigger.triggers.Add(pointerUp);
    }

    #region Text Methods
    public void ChangeText(Text aText, string aMessage)
    {
        aText.text = aMessage;
    }
    public void ChangeText(Text aText, string aMessage, Color aColor)
    {
        aText.text = aMessage;
        aText.color = aColor;
    }
    public void ChangeText(TMP_Text aText, string aMessage)
    {
        aText.text = aMessage;
    }
    public void ChangeText(TMP_Text aText, string aMessage, Color aColor)
    {
        aText.text = aMessage;
        aText.color = aColor;
    }
    public void ChangeTextWithDecreaseEffect(TMP_Text aText, string aMessage, float time)
    {
        StartCoroutine(DecreaseEffect(aText, aMessage, time));
    }
    private IEnumerator DecreaseEffect(TMP_Text aText, string aMessage, float time)
    {
        aText.text = aMessage;
        float _fontSizeCache = aText.fontSize;
        float _fontSize = _fontSizeCache;

        float _timer = 0;
        while (_timer < time)
        {
            _fontSize = Mathf.MoveTowards(_fontSize, 100, (_fontSizeCache - 100) / (time * (1 / Time.fixedDeltaTime)));
            aText.fontSize = (int)_fontSize;
            _timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        aText.text = "";
        aText.fontSize = (int)_fontSizeCache;
    }
    #endregion

    #region Image Methods
    public void ChangeImage(Image anImage, Sprite aSprite)
    {
        anImage.sprite = aSprite;
    }
    public void FillImage(Image anImage, float aRatio)
    {
        anImage.fillAmount = aRatio;
    }
    #endregion

    public void ChangePositionOfElement(Transform anElement, Vector3 aPos)
    {
        anElement.localPosition = aPos;
    }

    public void ChangeActivityOfElement(GameObject element, bool state)
    {
        element.SetActive(state);
    }
    public void ChangeActivityOfElement(GameObject[] elements, bool state)
    {
        for (int i = 0; i < elements.Length; i++)
        {
            elements[i].SetActive(state);
        }
    }

    public void ResetUI()
    {
        startButton.onClick.RemoveAllListeners();
        nextLevelButton.onClick.RemoveAllListeners();
        restartButton.onClick.RemoveAllListeners();

        EventTrigger.Entry pointerDown = new EventTrigger.Entry();
        EventTrigger.Entry pointerUp = new EventTrigger.Entry();
        playInputEventTrigger.triggers.Remove(pointerDown);
        playInputEventTrigger.triggers.Remove(pointerUp);

        ChangeActivityOfElement(levelElements, false);
        ChangeActivityOfElement(levelCompletedPanel, false);
        ChangeActivityOfElement(levelFailedPanel, false);
        ChangeActivityOfElement(startElements, true);
    }
}
