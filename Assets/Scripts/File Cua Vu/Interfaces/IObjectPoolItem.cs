using System;
using Saus.ObjectPoolSystem;
using UnityEngine;

namespace Saus.Interfaces
{
    public interface IObjectPoolItem
    {
        void SetObjectPool<T>(ObjectPool pool, T comp) where T : Component;

        void Release();
    }
}