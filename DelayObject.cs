// -------------------------------------------------- //
// Author:      Takayoshi Hagiwara (Toyohashi University of Technology)
// Created:     2019/8/27
// Summary:     Apply delays to the movements of avatars, etc. The master and slave should be the same object structure.
// Environment: Unity 2017 or Later
// -------------------------------------------------- //

using System.Collections.Generic;
using UnityEngine;

public class DelayObject : MonoBehaviour
{
    [SerializeField, Tooltip("Objects that are the source of movement")]
    private Transform _fromRoot = default;
    [SerializeField, Tooltip("Objects that reflect movement with delay")]
    private Transform _toRoot = default;

    [SerializeField, Tooltip("Delay [s]")]
    private float _delay = 0;
    private float _elapsedTime;

    // Temporarily save the Transform of a GameObject with the same name in a dictionary type.
    private Dictionary<string, List<Vector3>> _positionBuffer = new Dictionary<string, List<Vector3>>();
    private Dictionary<string, List<Quaternion>> _rotationBuffer = new Dictionary<string, List<Quaternion>>();

    private bool _isDelayStart = false;

    // Use this for initialization
    void Start()
    {
        InitializeDictionary(_fromRoot);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RecordParameter(_fromRoot);

        if (_isDelayStart)
            ApplyParameter(_toRoot);
        else
            CheckCurrentTime();
    }

    /// <summary>
    /// Temporarily save the master Transform.
    /// </summary>
    /// <param name="from">Base transform</param>
    public void RecordParameter(Transform from)
    {
        if (_positionBuffer.ContainsKey(from.name))
        {
            _positionBuffer[from.name].Add(from.position);
            _rotationBuffer[from.name].Add(from.rotation);
        }

        for (int iChild = 0; iChild < from.childCount; iChild++)
        {
            RecordParameter(from.GetChild(iChild));
        }
    }

    /// <summary>
    /// If the to object contains a GameObject with the same name as the from,
    /// the temporarily saved Transform is applied to the to object.
    /// </summary>
    /// <param name="to">The Transform to which the Transform is retargeted.</param>
    public void ApplyParameter(Transform to)
    {
        if (_rotationBuffer.ContainsKey(to.name))
        {
            to.position = _positionBuffer[to.name][0];
            to.rotation = _rotationBuffer[to.name][0];

            _positionBuffer[to.name].RemoveAt(0);
            _rotationBuffer[to.name].RemoveAt(0);
        }

        for (int iChild = 0; iChild < to.childCount; iChild++)
        {
            ApplyParameter(to.GetChild(iChild));
        }
    }

    /// <summary>
    /// Clear and initialize dictionary for temporarily save the Transform of a GameObject.
    /// Initialize with the structure and name of the from object.
    /// </summary>
    /// <param name="from">Base transform</param>
    private void InitializeDictionary(Transform from)
    {
        _positionBuffer.Clear();
        _rotationBuffer.Clear();

        InitializeDictionaryKeyValue(from);
    }

    /// <summary>
    /// Initialize dictionary key and value for temporarily save the Transform of a GameObject.
    /// Initialize with the structure and name of the from object.
    /// </summary>
    /// <param name="from">Base transform</param>
    private void InitializeDictionaryKeyValue(Transform from)
    {
        _positionBuffer.Add(from.name, new List<Vector3>());
        _rotationBuffer.Add(from.name, new List<Quaternion>());

        for (int iChild = 0; iChild < from.childCount; iChild++)
            InitializeDictionaryKeyValue(from.GetChild(iChild));
    }

    /// <summary>
    /// Check current time duration.
    /// If elapsedTime exceeds the set delay time, the delay start flag is set to True.
    /// </summary>
    private void CheckCurrentTime()
    {
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= _delay)
            _isDelayStart = true;
    }

    /// <summary>
    /// Reset all parameters.
    /// When you update the delay, please run this methods.
    /// </summary>
    /// <param name="from">Base transform</param>
    public void ResetAll(Transform from)
    {
        InitializeDictionary(from);
        _elapsedTime = 0;
        _isDelayStart = false;
    }
}
