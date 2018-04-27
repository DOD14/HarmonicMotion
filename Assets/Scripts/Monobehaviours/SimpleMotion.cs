using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class SimpleMotion : MonoBehaviour
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
    public InputField phiInputField;

    [Header("Dropdowns")]
    public Dropdown graphDropdown;
    public Dropdown graphableDropdown1;
    public Dropdown graphableDropdown2;
    public Dropdown graphableDropdown3;
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

    [Header("Phasor Graph")]
    public GameObject phasorGraph;
    public Transform phasorParent;
    public Transform phasorTracer;

    [Header("Graphable Values")]
    public GameObject graphableValuesGraph;
    public Transform tracer1;
    public Transform tracer2;
    public Transform tracer3;

    public enum VisibleObjects
    {
        System,
        PhasorGraph,
        GraphableGraph
    };

    [Header("Visible Objects")]
    public VisibleObjects displayOption;

    private float amplitude = 5f;
    private float omega = 3f;
    private float K = 16;
    private float mass = 1f;
    private float phi = 0f;

    private float x = 0;
    private float v;
    private float a;

    private float currentTime = 0f;

    private float sinus;
    private float cosinus;


    void Start()
    {
        GetBaseInput();
        ManageDependentInput();

        SetCubePos();

        CalculateAndOutputValues();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTime();

        sinus = Mathf.Sin((currentTime * omega + phi) * Mathf.PI);
        cosinus = Mathf.Cos((currentTime * omega + phi) * Mathf.PI);

        CalculateAndOutputValues();

        switch (displayOption)
        {
            case VisibleObjects.System:
                SetCubePos();
                break;

            case VisibleObjects.PhasorGraph:
                ManagePhasorGraph();
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
        GetFloatFromInputField(ref phi, phiInputField);

        ResetDisplays();
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

        ResetDisplays();

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
        x = amplitude * sinus;
        v = omega * amplitude * cosinus;
        a = -omega * omega * x;

        SetTextFromFloat(xText, x, "x = ");
        SetTextFromFloat(vText, v, "v = ");
        SetTextFromFloat(aText, a, "a = ");

        SetTextFromFloat(ETText, K * amplitude * amplitude, "ET = ");
        SetTextFromFloat(EPText, K * x * x, "EP = ");
        SetTextFromFloat(EKText, mass * v * v * 0.5f, "EK = ");

    }

    void ManagePhasorGraph()
    {
        phasorTracer.localPosition = new Vector3(amplitude, 0f, 0f);
        phasorParent.Rotate(new Vector3(0f, 0f, omega * Mathf.PI * Mathf.Rad2Deg * Time.deltaTime));
    }

    void ResetDisplays()
    {
        ResetTime();

        switch (displayOption)
        {
            case VisibleObjects.System:
                SetCubePos();
                break;

            case VisibleObjects.PhasorGraph:
                phasorParent.rotation = Quaternion.identity;
                phasorParent.Rotate(new Vector3(0f, 0f, Mathf.PI * phi * Mathf.Rad2Deg));
                break;

            case VisibleObjects.GraphableGraph:
                tracer1.GetComponent<TrailRenderer>().Clear();
                tracer2.GetComponent<TrailRenderer>().Clear();
                tracer3.GetComponent<TrailRenderer>().Clear();
                break;
        }

    }

    public void DropdownSelectGraph()
    {
        switch(graphDropdown.value)
        {
            case 0:
                displayOption = VisibleObjects.System;

                system.SetActive(true);
                phasorGraph.SetActive(false);
                graphableValuesGraph.SetActive(false);

                break;

            case 1:
                displayOption = VisibleObjects.PhasorGraph;

                system.SetActive(false);
                phasorGraph.SetActive(true);
                graphableValuesGraph.SetActive(false);

                break;   

            case 2:
                displayOption = VisibleObjects.GraphableGraph;

                system.SetActive(false);
                phasorGraph.SetActive(false);
                graphableValuesGraph.SetActive(true);

                break;
        }

        ResetDisplays();
    }

    void ManageGraphableValues()
    {

        float yParam = SetParamFromDropdown(baseDropdown);
        Debug.Log(yParam);

        SetTracerPos(tracer1, yParam, SetParamFromDropdown(graphableDropdown1));
        SetTracerPos(tracer2, yParam, SetParamFromDropdown(graphableDropdown2));
        SetTracerPos(tracer3, yParam, SetParamFromDropdown(graphableDropdown3));

    }

    void SetTracerPos(Transform tracer, float xParam, float yParam)
    {
        
        tracer.position = new Vector3(xScaleFactor * xParam, yScaleFactor * yParam, 0f);

    }

    float SetParamFromDropdown(Dropdown dropdown)
    {
        switch(dropdown.value)
        {
            case 1:
                return sinus;

            case 2:
                return cosinus;

            case 3:
                return -sinus;

            case 4:
                return currentTime;

            case 5:
                return cosinus * cosinus;

            case 6:
                return sinus * sinus;

            case 7:
                return 1;

            default:
                return 0;
        }

    }

    public void ResetTimeAndDisplay()
    {
        ResetTime();
        ResetDisplays();
    }
}
