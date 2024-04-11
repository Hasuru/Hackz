using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnswerData : MonoBehaviour
{
  [Header("UI Elements")]
  [SerializeField] TextMeshProUGUI infoTextObject;
  [SerializeField] TextMeshProUGUI infoLetter;
  [SerializeField] Image selector;

  [Header("Textures")]
  [SerializeField] Color selectedColor;
  [SerializeField] Color idleColor;

  [Header("References")]
  [SerializeField] Q_GameEvents events;
  [HideInInspector] Q_GameManager manager;

  private RectTransform _rect;
  public RectTransform Rect
  {
    get
    {
      if (_rect == null)
      {
        // ?? -> returns the first if not null, otherwise reutrns the second
        _rect = GetComponent<RectTransform>() ?? gameObject.AddComponent<RectTransform>();
      }
      return _rect;
    }
  }

  private int _answerIndex = -1;
  public int AnswerIndex { get { return _answerIndex; } }

  private bool Checked = false;

  void Awake()
  {
    manager = GameObject.Find("Managers").GetComponent<Q_GameManager>();
  }

  public void UpdateData(string info, int index)
  {
    infoTextObject.text = info;
    infoLetter.text = Char.ConvertFromUtf32('A' + index) + '.';
    _answerIndex = index;
  }

  public void Reset()
  {
    Checked = false;
    UpdateUI();
  }

  public void SwitchState()
  {
    Checked = !Checked;
    events.UpdateQuestionAnswer?.Invoke(this);
    if (manager != null)
    {
      manager.SetPickedAnswer(this);
      manager.AcceptAnswer();
    }
    Debug.Log("manager null");
  }

  void UpdateUI()
  {
    selector.color = (Checked) ? selectedColor : idleColor;
  }
}