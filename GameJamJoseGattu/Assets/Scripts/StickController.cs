using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickController : MonoBehaviour
{
    [SerializeField] RectTransform stickHandle, stickPivot;
    [SerializeField] GameObject rightButtonObj;
    [SerializeField] Sprite[] buttonRightIcons;
    [SerializeField] UnityEngine.UI.Button rightButton;
    Vector3 startPos, actualPos, endPos;
    bool isLeftTouchPressed;
    bool isTouchSupported;
    float screenMidX = Screen.height * .5f;
    float maxStickRange = 100;

    [HideInInspector] public Vector3 heading, direction;
    [HideInInspector] public float StickDistance { get => stickDistance * .01f; }
    float stickDistance;

    [HideInInspector] public bool isRightButton;
    public delegate void ActionOnRightTouch(System.Enum _event);
    ActionOnRightTouch activeRightAction;
    System.Enum activeEventType;

    private void Awake()
    {
        isTouchSupported = Input.touchSupported;
        rightButton.onClick.AddListener(PressRightButton);
        rightButtonObj.gameObject.SetActive(false);
    }

    public void DoAction()
    {
        if (isTouchSupported)
        {
            if (isLeftTouchPressed)
            {
                if (Input.touchCount > 0)
                {
                    foreach (Touch item in Input.touches)
                    {
                        if (item.position.x < screenMidX) OnLeftTouchPressed(item.position);
                        break;
                    }
                }
                else EndLeftTouch(actualPos);
            }
            else if (Input.touchCount > 0)
            {
                foreach (Touch item in Input.touches)
                {
                    if (item.position.x < screenMidX) StartLeftTouch(item.position);
                }
            }
            else EndLeftTouch(actualPos);
        }
        else
        {
            if (isLeftTouchPressed)
            {
                OnLeftTouchPressed(Input.mousePosition);
                if (Input.GetMouseButtonUp(0))
                {
                    EndLeftTouch(actualPos);
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                StartLeftTouch(Input.mousePosition);
            }
        }
    }

    void StartLeftTouch(Vector3 _position)
    {
        startPos = _position;
        stickPivot.gameObject.SetActive(true);
        stickPivot.position = startPos;
        isLeftTouchPressed = true;
    }

    void OnLeftTouchPressed(Vector3 _position)
    {
        actualPos = _position;
        heading = actualPos - startPos;
        stickDistance = heading.magnitude;
        direction = heading.normalized;
        stickHandle.position = actualPos;

        if (stickDistance >= maxStickRange)
        {
            stickHandle.position = startPos + (direction * maxStickRange);
            stickDistance = maxStickRange;
        }
    }

    void EndLeftTouch(Vector3 _position)
    {
        isLeftTouchPressed = false;
        endPos = _position;
        stickPivot.gameObject.SetActive(false);
        stickDistance = 0;
        direction = Vector3.zero;
    }

    public void ShowInteractionButton(ActionOnRightTouch _actionOnPress, System.Enum _eventType)
    {
        activeRightAction = _actionOnPress;
        activeEventType = _eventType;
        SetButtonIcon(_eventType);
        rightButtonObj.gameObject.SetActive(true);
        isRightButton = true;
    }

    void SetButtonIcon(System.Enum _eventType)
    {
        Sprite newSprite = null;
        //print(_eventType.ToString());

        rightButton.image.sprite = newSprite;
    }

    public void HideInteractionButton()
    {
        activeRightAction = null;
        rightButtonObj.gameObject.SetActive(false);
        isRightButton = false;
    }

    public void PressRightButton()
    {
        print("pressed " + activeEventType.ToString());
        activeRightAction.Invoke(activeEventType);
        isRightButton = false;
        rightButtonObj.gameObject.SetActive(false);
    }
}
