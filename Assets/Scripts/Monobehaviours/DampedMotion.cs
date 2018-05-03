using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class DampedMotion : MonoBehaviour
{
    /*
    [Header("Input")]
    public float amplitude = 5f;
    public float omega = 3f;
    public float K = 16;
    public float mass = 1f;
    public float phi = 0f;
    */

    [Header("Tweaks")]
    public float yScaleFactor = 4f;
    public float xScaleFactor = 2f;

    [Header("Input Fields")]
    public InputField amplitudeInputField;
    public InputField omegaInputField;
    public InputField KInputField;
    public InputField massInputField;
    public InputField dampingInputField;

    [Header("Dropdowns")]
    public Dropdown graphDropdown;
    public Dropdown[] valuesDropdowns;
    public Dropdown baseDropdown;

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

    [Header("Graphable Values")]
    public GameObject graphableValuesGraph;
    public Transform[] tracers;

    public enum VisibleObjects
    {
        System,
        GraphableGraph
    };

    [Header("Visible Objects")]
    public VisibleObjects displayOption;

    private float amplitude = 5f;
    private float omega = 3f;
    private float K = 16;
    private float mass = 1f;
    private float damping = 5f;

    private float x = 0;
    private float lastX;
    private float v;
    private float a;

    private float currentAmplitude;
    private float dampedOmega;
    private float b;
    private float e;

    private float currentTime = 0f;

    private float ET;
    private float EK;
    private float EP;

    private Rigidbody rb;


    void Start()
    {
        e = Mathf.Exp(1);
        rb = cube.GetComponent<Rigidbody>();

        GetBaseInput();
        ManageDependentInput();

        CalculateAndOutputValues();

        SetCubePos();

    }

    // Update is called once per frame
    void Update()
    {
        UpdateTime();

        CalculateAndOutputValues();

        switch (displayOption)
        {
            case VisibleObjects.System:
                SetCubePos();
                break;

            case VisibleObjects.GraphableGraph:
                ManageGraphableValues();
                break;
        }

    }

    void SetCubePos()
    {
        cube.position = new Vector3(x, 0f, 0f);
    }

    public void GetBaseInput()
    {
        GetFloatFromInputField(ref amplitude, amplitudeInputField);
        GetFloatFromInputField(ref damping, dampingInputField);

        SetAdditionalValues();

        ResetDisplays();
    }

    void SetAdditionalValues()
    {
        x = amplitude;
        b = 0.5f * damping / mass;
        dampedOmega = Mathf.Sqrt(omega * omega - b * b);

        ResetTime();
    }

    public void ManageDependentInput()
    {
        if (omegaToggle.isOn)
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

        SetAdditionalValues();
        ResetDisplays();

    }

    void ResetTime()
    {
        currentTime = 0;
        cube.transform.position = new Vector3(amplitude, 0f, 0f);
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

    void SetTextFromFloat(Text text, float value, string description = "")
    {
        text.text = description + value.ToString("F3");
    }

    void CalculateAndOutputValues()
    {
        x = cube.transform.position.x;
        a = -K * x - damping * rb.velocity.x;
        rb.AddForce(new Vector3(a, 0f, 0f), ForceMode.Acceleration);

        v = (x - lastX) / Time.deltaTime;
        lastX = x;

        currentAmplitude = amplitude * Mathf.Exp(-b * currentTime);

        SetTextFromFloat(xText, x, "x = ");
        SetTextFromFloat(vText, v, "v = ");
        SetTextFromFloat(aText, a, "a = ");

        EK = K * x * x;
        EP = mass * v * v * 0.5f;
        ET = EP + EK;

        SetTextFromFloat(ETText, ET, "ET = ");
        SetTextFromFloat(EPText, EK, "EP = ");
        SetTextFromFloat(EKText, EP, "EK = ");

    }

    void ResetDisplays()
    {
        ResetTime();

        switch (displayOption)
        {
            case VisibleObjects.System:
                SetCubePos();
                break;

            case VisibleObjects.GraphableGraph:
                foreach (Transform tracer in tracers)
                    tracer.GetComponent<TrailRenderer>().Clear();
                break;
        }

    }

    public void DropdownSelectGraph()
    {
        switch (graphDropdown.value)
        {
            case 0:
                displayOption = VisibleObjects.System;

                system.SetActive(true);
                graphableValuesGraph.SetActive(false);

                break;

            case 1:
                displayOption = VisibleObjects.GraphableGraph;

                //system.SetActive(false);
                graphableValuesGraph.SetActive(true);

                break;
        }

        ResetDisplays();
    }

    void ManageGraphableValues()
    {

        float yParam = SetParamFromDropdown(baseDropdown);

        for (int i = 0; i < tracers.Length; i++)
        {
            if (tracers[i].gameObject.activeSelf)
                SetTracerPos(tracers[i], yParam, SetParamFromDropdown(valuesDropdowns[i]));
        }

    }

    void SetTracerPos(Transform tracer, float xParam, float yParam)
    {

        tracer.position = new Vector3(xScaleFactor * xParam, yScaleFactor * yParam, 0f);

    }

    float SetParamFromDropdown(Dropdown dropdown)
    {
        switch (dropdown.value)
        {
            case 1:
                return x;

            case 2:
                return v;

            case 3:
                return a;

            case 4:
                return currentTime;

            case 5:
                return EK;

            case 6:
                return EP;

            case 7:
                return ET;

            default:
                return 0;
        }

    }

    public void ResetTimeAndDisplay()
    {
        ResetTime();
        ResetDisplays();
    }

    public void ManageTracerActivity(int index)
    {
        if (valuesDropdowns[index].value == 0) tracers[index].gameObject.SetActive(false);
        else tracers[index].gameObject.SetActive(true);
    }
}
