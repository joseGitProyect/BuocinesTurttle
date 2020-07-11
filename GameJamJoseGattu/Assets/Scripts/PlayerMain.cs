using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMain : MonoBehaviour
{
    [SerializeField] Movement movement;
    [SerializeField] ParticleSystem particles;
    [SerializeField] TMP_InputField IFrotateSpeedH;
    [SerializeField] TMP_InputField IFrotateSpeedV;
    [SerializeField] TMP_InputField IFwaterSpeed;
    [SerializeField] TMP_InputField IFwaterTime;
    [SerializeField] TMP_InputField IFspeed;
    public float rotateSpeedH = 5;
    public float rotateSpeedV = 5;
    public float waterSpeed = 10;
    public float waterTime = 3;
    public float speed;
    public Vector3 cameraOffSet;
    Vector3 offset;
    GameObject player;
    Transform cameraTransform;
    bool isOnWaterShoot;
    float startWaterShoot;
    float actualSpeed;
   
    void Start()
    {
        cameraTransform = Camera.main.transform;
        player = movement.gameObject;
        offset = cameraOffSet;
        Cursor.lockState = CursorLockMode.Locked;
        actualSpeed = speed;
        SetInputText();
    }

    private void Update()
    {
        if (!isOnWaterShoot && Input.GetMouseButtonDown(0))
        {
            movement.Dash(cameraTransform.forward, waterSpeed);
            startWaterShoot = Time.time;
            particles.Play();
            isOnWaterShoot = true;
        }
        else if(isOnWaterShoot)
        {
            if(Timer(startWaterShoot,waterTime,ref isOnWaterShoot))
            {
                actualSpeed = speed;
                particles.Stop();
            }
            else
            {
                actualSpeed = waterSpeed;
            }

        }
        
        movement.ControlMyVelocity(actualSpeed);

        if(Input.GetKeyDown(KeyCode.Alpha8))
        {
            if(Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

        }
    }

    void LateUpdate()
    {
        float horizontal = Input.GetAxis("Mouse X") * rotateSpeedH;
        float Vertical = Input.GetAxis("Mouse Y") * rotateSpeedV;
        player.transform.Rotate(Vertical, horizontal, 0);

        float desiredAngleH = player.transform.eulerAngles.y;
        float desiredAngleV = player.transform.eulerAngles.x;
        Quaternion rotation = Quaternion.Euler(desiredAngleV, desiredAngleH, 0);
        cameraTransform.position = player.transform.position - (rotation * offset);

        cameraTransform.LookAt(player.transform);
    }

    public bool Timer(float _startTime, float duration, ref bool isTimerActive)
    {
        float time = Time.time - _startTime;
        if(time >= duration)
        {
            isTimerActive = false;
            return true;
        }
        return false;
    }

    public void ChangeVal(string _string)
    {
        print(_string);
        switch (_string)
        {
            case "RHorizontal":
                rotateSpeedH = float.Parse(IFrotateSpeedH.text);
                break;
            case "RVertical":
                rotateSpeedV = float.Parse(IFrotateSpeedV.text);
                break;
            case "Speed":
                speed = float.Parse(IFspeed.text);
                break;
            case "WaterSpeed":
                waterSpeed = float.Parse(IFwaterSpeed.text);
                break;
            case "WaterTime":
                waterTime = float.Parse(IFwaterTime.text);
                break;
        }
    }

    public void SetInputText()
    {
        IFrotateSpeedH.text = rotateSpeedH.ToString();
        IFrotateSpeedV.text = rotateSpeedV.ToString();
        IFspeed.text = speed.ToString();
        IFwaterSpeed.text = waterSpeed.ToString();
        IFwaterTime.text = waterTime.ToString();
    }

    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.name);
    }
}
