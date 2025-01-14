/* Original source Farseer Physics Engine:
 * Copyright (c) 2014 Ian Qvist, http://farseerphysics.codeplex.com
 * Microsoft Permissive License (Ms-PL) v1.1
 */

using System;
using Internal.tainicom.Aether.Physics2D.Common;

namespace Internal.tainicom.Aether.Physics2D.Fluids
{
    /// <summary>
    /// Fluid parameters, see pvfs.pdf for a detailed explanation
    /// </summary>
    public struct FluidDefinition
    {
        /// <summary>
        /// Distance of influence between the particles
        /// </summary>
        public float InfluenceRadius;

        /// <summary>
        /// Density of the fluid
        /// </summary>
        public float DensityRest;

        /// <summary>
        /// Stiffness of the fluid (when particles are far)
        /// </summary>
        public float Stiffness;

        /// <summary>
        /// Stiffness of the fluid (when particles are near)
        /// Set by Check()
        /// </summary>
        public float StiffnessNear;

        /// <summary>
        /// Toggles viscosity forces
        /// </summary>
        public bool UseViscosity;

        /// <summary>
        /// See pvfs.pdf for more information
        /// </summary>
        public float ViscositySigma;

        /// <summary>
        /// See pvfs.pdf for more information
        /// </summary>
        public float ViscosityBeta;

        /// <summary>
        /// Toggles plasticity computation (springs etc.)
        /// </summary>
        public bool UsePlasticity;

        /// <summary>
        /// Plasticity, amount of memory of the shape
        /// See pvfs.pdf for more information
        /// </summary>
        public float Plasticity;

        /// <summary>
        /// K of the springs used for plasticity
        /// </summary>
        public float KSpring;

        /// <summary>
        /// Amount of change of the rest length of the springs (when compressed)
        /// </summary>
        public float YieldRatioCompress;

        /// <summary>
        /// Amount of change of the rest length of the springs (when stretched)
        /// </summary>
        public float YieldRatioStretch;

        public static FluidDefinition Default
        {
            get
            {
                FluidDefinition def = new FluidDefinition
                                          {
                                              InfluenceRadius = 1.0f,
                                              DensityRest = 10.0f,
                                              Stiffness = 10.0f,
                                              StiffnessNear = 0.0f, // Set by Check()

                                              UseViscosity = false,
                                              ViscositySigma = 10.0f,
                                              ViscosityBeta = 0.0f,

                                              UsePlasticity = false,
                                              Plasticity = 0.3f,
                                              KSpring = 2.0f,
                                              YieldRatioCompress = 0.1f,
                                              YieldRatioStretch = 0.1f
                                          };

                def.Check();

                return def;
            }
        }

        public void Check()
        {
            InfluenceRadius = MathUtils.Clamp(InfluenceRadius, 0.1f, 10.0f);
            DensityRest = MathUtils.Clamp(DensityRest, 1.0f, 100.0f);
            Stiffness = MathUtils.Clamp(Stiffness, 0.1f, 10.0f);
            StiffnessNear = Stiffness * 100.0f; // See pvfs.pdf

            ViscositySigma = Math.Max(ViscositySigma, 0.0f);
            ViscosityBeta = Math.Max(ViscosityBeta, 0.0f);

            Plasticity = Math.Max(Plasticity, 0.0f);
            KSpring = Math.Max(KSpring, 0.0f);
            YieldRatioCompress = Math.Max(YieldRatioCompress, 0.0f);
            YieldRatioStretch = Math.Max(YieldRatioStretch, 0.0f);
        }
    }
}