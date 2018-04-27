using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class SimpleMotion : MonoBehaviour
{
    [Header("Input")]
    public float amplitude = 5f;
    public float omega = 3f;
    public float K = 16;
    public float mass = 1f;
    public float phi = 0f;

    [Header("Input Fields")]
    public InputField amplitudeInputField;
    public InputField omegaInputField;
    public InputField KInputField;
    public InputField massInputField;
    public InputField phiInputField;

    [Header("Toggles")]
    public Toggle omegaToggle;
    public Toggle KToggle;
    public Toggle massToggle;

    [Header("Button Text")]
    public Text timescaleButtonText;

    [Header("Output Text")]
    public Text xText;
    public Text vText;
    public Text aText;
    public Text ETText;
    public Text EKText;
    public Text EPText;

    [Header("System")]
    public GameObject system;
    public Transform cube;

    [Header("Phasor Graph")]
    public GameObject phasorGraph;
    public Transform phasorParent;
    public Transform phasorTracer;

    public enum VisibleObjects
    {
        System,
        PhasorGraph
    };

    [Header("Visible Objects")]
    public VisibleObjects displayOption;


    private float x = 0;
    private float v;
    private float a;

    private float currentTime = 0f;


    void Start()
    {
        GetBaseInput();
        ManageDependentInput();

        GetCubePos();

        CalculateAndOutputValues();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTime();

        GetCubePos();

        switch (displayOption)
        {
            case VisibleObjects.System:
                SetCubePos();
                break;

            case VisibleObjects.PhasorGraph:
                phasorTracer.localPosition = new Vector3(amplitude, 0f, 0f);
                phasorParent.Rotate(new Vector3(0f, 0f, omega * Mathf.PI * Mathf.Rad2Deg * Time.deltaTime));
                break;
        }

        CalculateAndOutputValues();
    }

    void GetCubePos()
    {
        x = amplitude * Mathf.Sin((currentTime * omega + phi) * Mathf.PI);
    }

    void SetCubePos()
    {
        cube.position = new Vector3(x, 0f, 0f);
    }

    public void GetBaseInput()
    {
        GetFloatFromInputField(ref amplitude, amplitudeInputField);
        GetFloatFromInputField(ref phi, phiInputField);
    }
    
    public void ManageDependentInput()
    {
        if(omegaToggle.isOn)
        {
            GetFloatFromInputField(ref K, KInputField);
            GetFloatFromInputField(ref mass, massInputField);

            omega = Mathf.Sqrt(K / mass);

            SetInputFieldTextFromFloat(omegaInputField, omega);
        }

        else if (KToggle.isOn)
        {
            GetFloatFromInputField(ref omega, omegaInputField);
            GetFloatFromInputField(ref mass, massInputField);

            K = mass * omega * omega;

            SetInputFieldTextFromFloat(KInputField, K);

        }

        else if (massToggle.isOn)
        {
            GetFloatFromInputField(ref K, KInputField);
            GetFloatFromInputField(ref omega, omegaInputField);

            mass = K / (omega * omega);

            SetInputFieldTextFromFloat(massInputField, mass);
        }

    }

    void ResetTime()
    {
        currentTime = 0;
    }

    void UpdateTime()
    {
        currentTime += Time.deltaTime;
    }

    public void ToggleTimeScale()
    {
        if (Time.timeScale > 0)
        { Time.timeScale = 0f; timescaleButtonText.text = "Play"; }
        else { Time.timeScale = 1f; timescaleButtonText.text = "Pause"; }

    }

    void GetFloatFromInputField(ref float value, InputField text)
    {
        value = float.Parse(text.text);
    }

    void SetInputFieldTextFromFloat(InputField text, float value)
    {
        text.text = value.ToString("F1");
    }

    void SetTextFromFloat (Text text, float value, string description = "")
    {
        text.text = description + value.ToString("F3");
    }

    void CalculateAndOutputValues()
    {
        v = omega * amplitude * Mathf.Cos((currentTime * omega + phi) * Mathf.PI);
        a = -omega * omega * x;

        SetTextFromFloat(xText, x, "x = ");
        SetTextFromFloat(vText, v, "v = ");
        SetTextFromFloat(aText, a, "a = ");

        SetTextFromFloat(ETText, K * amplitude * amplitude, "ET = ");
        SetTextFromFloat(EPText, K * x * x, "EP = ");
        SetTextFromFloat(EKText, mass * v * v * 0.5f, "EK = ");

    }

}
