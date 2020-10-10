using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class Coordinate : MonoBehaviour
{

    [SerializeField] private Toggle uitoggle_XAxis, uitoggle_YAxis, uitoggle_ZAxis;

    private Toggle[] uitoggle_Axises;

    private Vector3 _activeAxis = Vector3.zero;
    private int count = 0;
    private Transform bind;

    // State
    public enum Mode
    {
        MAX_2_ACTIVE, MAX_1_ACTIVE, MAX_3_ACTIVE
    }
    private Mode _maxActive;
    // Start is called before the first frame update
    void Start()
    {
        uitoggle_Axises = new Toggle[3] { uitoggle_XAxis, uitoggle_YAxis, uitoggle_ZAxis };
    }

    // Update is called once per frame
    void Update()
    {
        if (bind)
        {
            transform.position = bind.position;
        }
    }

    public void bindTo(Transform transform)
    {
        bind = transform;
    }

    public void ToggleActiveAxis(int id)
    {
        ToggleActiveAxis(id, _activeAxis[id] == 0);
    }

    public void ToggleActiveAxis(int id, bool active)
    {
        if (active == (_activeAxis[id] != 0)) return;
        if (active)
        {
            if (_maxActive == Mode.MAX_3_ACTIVE || (_maxActive == Mode.MAX_2_ACTIVE && SumActive() < 2) || (_maxActive == Mode.MAX_1_ACTIVE && count == 0))
            {
                _activeAxis[id] = 1;
            }
            else return;
        }
        else _activeAxis[id] = 0;
        uitoggle_Axises[id].isOn = _activeAxis[id] != 0;
        count += (_activeAxis[id] != 0 ? 1 : -1) * (id + 1);
    }

    public int isActiveAxis(int id)
    {
        return (int)(_activeAxis[id]);
    }

    public int SumActive()
    {
        return (int)Vector3.Dot(_activeAxis, Vector3.one);
    }

    public bool HasActiveAxis()
    {
        return SumActive() > 0;
    }

    public Vector3 ActiveToVector()
    {
        return _activeAxis;
    }

    public int GetOnlyAcitve()
    {
        return count > 0 && SumActive() == 1 ? count - 1 : -1;
    }

    public int GetOnlyNonactive()
    {
        return count > 0 && SumActive() == 2 ? 5 - count : -1;
    }

    public Vector3 GetProjecttion(Vector3 objPos, Ray ray, int directionAxis, ref Vector3 projection)
    {
        float dbA = objPos[directionAxis] - ray.origin[directionAxis];
        float k = dbA / ray.direction[directionAxis];
        projection[directionAxis] = objPos[directionAxis];
        int cA = (directionAxis + 1) % 3;
        projection[cA] = k * ray.direction[cA] + ray.origin[cA];
        cA = (cA + 1) % 3;
        projection[cA] = k * ray.direction[cA] + ray.origin[cA];
        return projection;
    }

    public Vector3 GetProjecttion(Vector3 objPos, Ray ray, int directionAxis)
    {
        Vector3 projection = Vector3.zero;
        return GetProjecttion(objPos, ray, directionAxis, ref projection);
    }

    public int MaxAbsComponent(Vector3 vec)
    {
        int id = Math.Abs(vec[0]) < Math.Abs(vec[1]) ? 1 : 0;
        return Math.Abs(vec[id]) < Math.Abs(vec[2]) ? 2 : id;
    }

    public int MaxAbsComponentExcept(Vector3 vector, int exceptedDirection)
    {
        int a1 = (exceptedDirection + 1) % 3;
        int a2 = (a1 + 1) % 3;
        return (Math.Abs(vector[a1]) < Math.Abs(vector[a2]) ? a2 : a1);
    }

    // Get Set
    public Mode MaxActive
    {
        get
        {
            return _maxActive;
        }
        set
        {
            _activeAxis = Vector3.zero;
            count = 0;
            _maxActive = value;
        }
    }
}
