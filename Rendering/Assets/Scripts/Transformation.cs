﻿using UnityEngine;

namespace Assets.Scripts
{
    public abstract class Transformation : MonoBehaviour
    {
        public abstract Vector3 Apply(Vector3 point);
    }
}