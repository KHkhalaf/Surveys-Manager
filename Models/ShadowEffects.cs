using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Surveys_Manager.Models
{
    public class ShadowEffects : RoutingEffect
    {
        public float Radius { get; set; }
        public Color color { get; set; }
        public float DictanceX { get; set; }
        public float DistanceY { get; set; }

        public ShadowEffects() : base("MyCompany.LabelShadowEffects")
        {
        }
    }
}
