using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class ParallelMotion : MonoBehaviour
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
    public InputField[] amplitudeInputFields;
    public InputField[] omegaInputFields;
    public InputField[] phiInputFields;

    [Header("Dropdowns")]
    public Dropdown graphDropdown;
    public Dropdown[] valuesDropdowns;
    public Dropdown baseDropdown;

    [Header("Button Text")]
    public Text timescaleButtonText;

    [Header("Output Text")]
    public Text[] xTexts;
    public Text[] vTexts;
    public Text[] aTexts;

    [Header("System")]
    public GameObject system;
    public Transform cube;
    public Transform[] markers;

    [Header("Phasor Graph")]
    public GameObject phasorGraph;
    public Transform[] phasorParents;
    public Transform[] phasorTracers;

    [Header("Graphable Values")]
    public GameObject graphableValuesGraph;
    public Transform[] tracers;

    public enum VisibleObjects
    {
        System,
        PhasorGraph,
        GraphableGraph
    };

    [Header("Visible Objects")]
    public VisibleObjects displayOption;

    private float[] amplitudes = new float[3];
    private float[] omegas = new float[2];
    private float[] phis = new float[2];

    private float[] x = new float[3];
    private float[] v = new float[3];
    private float[] a = new float[3];

    private float currentTime = 0f;

    private float[] sinus = new float[2];
    private float[] cosinus = new float[2];


    void Start()
    {
        GetInput();

        CalculateAndOutputValues();

        SetCubePos();

    }

    // Update is called once per frame
    void Update()
    {
        UpdateTime();

        for (int i = 0; i < 2; i++)
        {
            sinus[i] = Mathf.Sin((currentTime * omegas[i] + phis[i]) * Mathf.PI);
            cosinus[i] = Mathf.Cos((currentTime * omegas[i] + phis[i]) * Mathf.PI);
        }
    

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

    public void GetInput()
    {
        for (int i = 0; i < 2; i++)
        {
            GetFloatFromInputField(ref amplitudes[i], amplitudeInputFields[i]);
            GetFloatFromInputField(ref phis[i], phiInputFields[i]);
            GetFloatFromInputField(ref omegas[i], omegaInputFields[i]);
        }

        AddIntoThirdFloat(amplitudes);
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
        for (int i = 0; i < amplitudes.Length-1; i++)
        {
            x[i] = amplitudes[i] * sinus[i];
            v[i] = omegas[i] * amplitudes[i] * cosinus[i];
            a[i] = -omegas[i] * omegas[i] * x[i];
        }

        AddIntoThirdFloat(x);
        AddIntoThirdFloat(v);
        AddIntoThirdFloat(a);

        for (int i = 0; i < amplitudes.Length; i++)
        {
            SetTextFromFloat(xTexts[i], x[i], "x = ");
            SetTextFromFloat(vTexts[i], v[i], "v = ");
            SetTextFromFloat(aTexts[i], a[i], "a = ");
        }

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
                foreach(Transform tracer in tracers)
                    tracer.GetComponent<TrailRenderer>().Clear();
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

        for (int i = 0; i < tracers.Length; i++)
        {
            if (tracers[i].gameObject.activeSelf)
                SetTracerPos(tracers[i],yParam, SetParamFromDropdown(valuesDropdowns[i]));
        }

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

    public void ManageTracerActivity(int index)
    {
        if (valuesDropdowns[index].value == 0) tracers[index].gameObject.SetActive(false);
        else tracers[index].gameObject.SetActive(true);
    }

    void AddIntoThirdFloat(float[] value)
    {
        value[2] = value[0] + value[1];
    }
}
