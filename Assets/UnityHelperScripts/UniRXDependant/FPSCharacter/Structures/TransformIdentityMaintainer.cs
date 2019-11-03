using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformIdentityMaintainer : IDisposable
{
    TransformData m_sourceData;
    Transform m_source;

    public IDisposable MaintainIdentityOn(Transform source)
    {
        m_source = source;
        m_sourceData = m_source.ExtractData();
        return this;
    }

    public void Dispose()
    {
        if(m_source != null)
            m_sourceData.ApplyToTransform(m_source);
    }
}
