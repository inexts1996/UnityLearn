﻿using UnityEngine;

namespace Assets.Scripts
{
    public class PositionTransformation : Transformation
    {
        public Vector3 position;

        public override Vector3 Apply(Vector3 point)
        {
            return point + position;
        }
    }
}