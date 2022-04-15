// -------------------------------------------------- //
// Author:      Takayoshi Hagiwara (Toyohashi University of Technology)
// Created:     2019/8/27
// Summary:     Apply delays to the movements of avatars, etc. The master and slave should be the same object structure.
// Environment: Unity 2017 or Later
// -------------------------------------------------- //

using System.Collections.Generic;
using UnityEngine;

public class DelayObject : MonoBehaviour {

    [SerializeField, Tooltip("Objects that are the source of movement")]
    private Transform masterRoot    = default;
    [SerializeField, Tooltip("Objects that reflect movement with delay")]
    private Transform slaveRoot     = default;

    [SerializeField, Tooltip("Delay [s]")]
    private float delay = 0;
    private float elapsedTime;

    // Temporarily save the Transform of a GameObject with the same name in a dictionary type.
    private Dictionary<string, List<Vector3>> positionBuffer    = new Dictionary<string, List<Vector3>>();
    private Dictionary<string, List<Quaternion>> rotationBuffer = new Dictionary<string, List<Quaternion>>();

    bool isDelayStart = false;

    // Use this for initialization
    void Start()
    {
        InitializeDictionary(masterRoot);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RecordParameter(masterRoot);

        if (isDelayStart)
            ApplyParameter(slaveRoot);
        else
            CheckCurrentTime();
    }

    /// <summary>
    /// Temporarily save the master Transform.
    /// </summary>
    /// <param name="mst">master object</param>
    public void RecordParameter(Transform mst)
    {
        if (positionBuffer.ContainsKey(mst.name))
        {
            positionBuffer[mst.name].Add(mst.position);
            rotationBuffer[mst.name].Add(mst.rotation);
        }

        for(int iChild = 0; iChild < mst.childCount; iChild++)
        {
            RecordParameter(mst.GetChild(iChild));
        }
    }

    /// <summary>
    /// If the slave object contains a GameObject with the same name as the master,
    /// the temporarily saved Transform is applied to the slave object.
    /// </summary>
    /// <param name="slv">slave object</param>
    public void ApplyParameter(Transform slv)
    {
        if (rotationBuffer.ContainsKey(slv.name))
        {
            slv.position = positionBuffer[slv.name][0];
            slv.rotation = rotationBuffer[slv.name][0];

            positionBuffer[slv.name].RemoveAt(0);
            rotationBuffer[slv.name].RemoveAt(0);
        }

        for(int iChild = 0; iChild < slv.childCount; iChild++)
        {
            ApplyParameter(slv.GetChild(iChild));
        }
    }

    /// <summary>
    /// Initialize dictionary for temporarily save the Transform of a GameObject.
    /// Initialize with the structure and name of the master object.
    /// </summary>
    /// <param name="mst">master object</param>
    private void InitializeDictionary(Transform mst)
    {
        positionBuffer.Clear();
        rotationBuffer.Clear();

        positionBuffer.Add(mst.name, new List<Vector3>());
        rotationBuffer.Add(mst.name, new List<Quaternion>());

        for(int iChild = 0; iChild < mst.childCount; iChild++)
        {
            InitializeDictionary(mst.GetChild(iChild));
        }
    }

    /// <summary>
    /// Check current time duration.
    /// If elapsedTime exceeds the set delay time, the delay start flag is set to True.
    /// </summary>
    private void CheckCurrentTime()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= delay)
            isDelayStart = true;
    }

    /// <summary>
    /// Reset all parameters.
    /// When you update the delay, please run this methods.
    /// </summary>
    /// <param name="mst">master object</param>
    public void ResetAll(Transform mst)
    {
        InitializeDictionary(mst);
        elapsedTime     = 0;
        isDelayStart    = false;
    }
}
