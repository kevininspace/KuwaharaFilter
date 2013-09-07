using System;
using System.Collections.Generic;

namespace KuwaharaFilter
{
    public class EffectPluginConfigToken : PaintDotNet.Effects.EffectConfigToken
    {
        // Declare variables here
        // Using getters/setters, you can manipulate the "value" before assigning it to your private variable.

        // Example using getters/setters:
        //----------------------------------------
        // private double variable;
        // public double Variable
        // {
        //     get { return variable; }
        //     set { this.variable = value; }
        // }

        // Example not using getters/setters:
        //----------------------------------------
        // public double variable;

        public int ApertureSize { get; set; }

        public EffectPluginConfigToken()
            : base()
        {
            // Set default variables here
            ApertureSize = 25;
        }

        protected EffectPluginConfigToken(EffectPluginConfigToken copyMe)
            : base(copyMe)
        {
            ApertureSize = copyMe.ApertureSize;
        }

        public override object Clone()
        {
            return new EffectPluginConfigToken(this);
        }


    }
}