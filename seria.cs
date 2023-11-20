using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class Seria : MonoBehaviour
{
   public static Seria Instance;
    SerialPort Serial;
    public bool ManuelPort = false;
    public string PortName = "COM3";
    public float timeRead = 0.4f;
    bool connected = false;
    bool tryConnection = false;
    public List<string> data = new List<string>();
    
    void Start () {
        StartCoroutine(Connect());
        InvokeRepeating("CheckValue", 0.0f, timeRead);
	}

    void Update() {
        if (Serial!= null && !Serial.IsOpen)
        {
            if(!tryConnection) StartCoroutine(Connect());
        }
        if(!connected) return;
        
    }

    void CheckValue(){
        try
        {
            List<string> listTemp = new List<string>();
            string[] val = Serial.ReadLine().Split('/');
            for (int i = 0; i < val.Length; i++)
            {
                if(i == val.Length -1) break;
                Debug.Log("i " + i + "  val " + val[i]);
                listTemp.Add(val[i]);
            }
            data = listTemp;    
        }
        catch (System.Exception)
        {
            Debug.LogWarning("error read port");
        }        
    }

    IEnumerator Connect()
    {
        tryConnection = true;
        yield return new WaitForSeconds(1);
        if (ManuelPort)
        {
            Serial = new SerialPort(PortName, 9600);
            Serial.ReadTimeout = 1;
            Serial.Open();
            if (Serial.IsOpen)
            {
                Debug.Log("Serial port open");
                connected = true;
            }
            else
            {
                Debug.LogWarning("Serial port not open");
            }
            tryConnection = false;
            yield break;
        }
        string[] ports = SerialPort.GetPortNames();
        foreach (string port in ports)
        {
            Debug.Log(port);
        }
        // open the port
        if (ports.Length == 0)
        {
            Debug.LogWarning("No ports");
            yield break;
        }
        Serial = new SerialPort(ports[0], 9600);
        Serial.ReadTimeout = 1;
        Serial.Open();
        if (Serial.IsOpen)
        {
            Debug.Log("Serial port open");
            connected = true;
        }
        else
        {
            Debug.LogWarning("Serial port not open");
        }
        tryConnection = false;
        
    }
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}

