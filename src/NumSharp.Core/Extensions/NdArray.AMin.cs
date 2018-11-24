﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumSharp.Core.Extensions
{
    public static partial class NDArrayExtensions
    {
        public static NDArrayGeneric<double> AMin(this NDArrayGeneric<double> np, int? axis = null)
        {
            NDArrayGeneric<double> res = new NDArrayGeneric<double>();
            if (axis == null)
            {
                res.Data = new double[1] { np.Data.Min() };
                res.Shape = new Shape(new int[] { 1 });
            }
            else
            {
                if (axis < 0 || axis >= np.NDim)
                    throw new Exception("Invalid input: axis");
                int[] resShapes = new int[np.Shape.Shapes.Count - 1];
                int index = 0; //index for result shape set
                //axis departs the shape into three parts: prev, cur and post. They are all product of shapes
                int prev = 1; 
                int cur = 1;
                int post = 1;
                int size = 1; //total number of the elements for result
                //Calculate new Shape
                for (int i = 0; i < np.Shape.Shapes.Count; i++)
                {
                    if (i == axis)
                        cur = np.Shape.Shapes[i];
                    else
                    {
                        resShapes[index++] = np.Shape.Shapes[i];
                        size *= np.Shape.Shapes[i];
                        if (i < axis)
                            prev *= np.Shape.Shapes[i];
                        else
                            post *= np.Shape.Shapes[i];
                    }
                }
                res.Shape = new Shape(resShapes);
                //Fill in data
                index = 0; //index for result data set
                int sameSetOffset = np.Shape.DimOffset[axis.Value];
                int increments = cur * post;
                res.Data = new double[size];
                int start = 0;
                double min = 0;
                for (int i = 0; i < np.Size; i += increments)
                {
                    for (int j = i; j < i + post; j++)
                    {
                        start = j;
                        min = np.Data[start];
                        for (int k = 0; k < cur; k++)
                        {
                            min = Math.Min(min, np.Data[start]);
                            start += sameSetOffset;
                        }
                        res.Data[index++] = min;
                    }
                }
            }
            return res;
        }
    }
}