using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public  class singleton : MonoBehaviour {

    public static singleton instance;
    public List<revisedBoid> boids = new List<revisedBoid>();
    public GameObject birdy;
    public Text cohesionRO;
    public Text alignRO;
    public Text seperationRO;
    public Text birdCountRO;
    public Text momMagRO;
    public Text setMagRO;
    public float cohesionDistance = 0;
    public float alignDistance = 0;
    public float seperationDistance = 0;
    public float momMag = 0;
    public float setMag = 0;
    public List<Slider> UI_sliders;
    public List<float> defaultSliderValues;
    int targetBirdCount = 0;
    int minFieldSize = 10;
    int maxFieldSize = 30;
    public int fieldSize = 0;

    void changeFieldSize() {
        fieldSize = targetBirdCount / 4;
        if (fieldSize < minFieldSize)
        {
            fieldSize = minFieldSize;
        }
        else if (fieldSize > maxFieldSize) {
            fieldSize = maxFieldSize;
        }
        gameObject.transform.position = new Vector3(0, 0, -(fieldSize + 5));
        foreach (revisedBoid b in boids) {
            b.fieldSize = fieldSize;
        }
    }



    // Use this for initialization
    void Awake () {
        if (singleton.instance != null)
        {
            Destroy(gameObject);
        }
        else {
            instance = this;
        }
	}
    public void changeBirdCount(float value) {
        int _value = Mathf.FloorToInt(value);

        if (_value > boids.Count)
        {
            for (int i = 0; i < _value - boids.Count; i++)
            {
                Instantiate<GameObject>(birdy);
            }
        }
        else if (boids.Count > _value) {
            for (int i = 0; i < boids.Count - _value; i++) {
                Destroy(boids[i].gameObject);
                boids.RemoveAt(i);
            }
        }
        targetBirdCount = _value;
        birdCountRO.text = _value.ToString();
        changeFieldSize();
    }

    public void changeMomMag(float value)
    {
        foreach (revisedBoid b in boids)
        {
            b.momMagnitude = value;
        }
        momMagRO.text = value.ToString();
        momMag = value;
    }

    public void changeSetMag(float value)
    {
        foreach (revisedBoid b in boids)
        {
            b.settingMagnitudes = value;
        }
        setMagRO.text = value.ToString();
        setMag = value;
    }


    public void changeCohesionDistance(float value) {
        foreach (revisedBoid b in boids) {
            b.cohesionDistance = value;
        }
        cohesionRO.text = value.ToString();
        cohesionDistance = value;
    }
    public void changeSeperationDistance(float value)
    {
        foreach (revisedBoid b in boids)
        {
            b.avoidanceDistance = value;
        }
        seperationRO.text = value.ToString();
        seperationDistance = value;
    }
    public void changeAlignDistance(float value)
    {
        foreach (revisedBoid b in boids)
        {
            b.alignDistance = value;
        }
        alignRO.text = value.ToString();
        alignDistance = value;
    }


    private void Start()
    {
        for (int i = 0; i < UI_sliders.Count; i++) {
            UI_sliders[i].value = defaultSliderValues[i];
        }
    }

    // Update is called once per frame
    void Update () {
        if (targetBirdCount != boids.Count) {
            changeBirdCount(targetBirdCount);
        }
	}
}
