using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labe5.Object
{
    public class Dot : BaseObject
    {
        public float radius = 50;
        public Action<Dot> onDeath;

        public Dot(float x, float y, float angle, float radius, int timer) : base(x, y, angle)
        {

        }

        public override void Render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.GreenYellow), -radius / 2, -radius / 2, radius, radius);
            this.radius -= (float)0.5;
            if (radius <= 0)
            {
                radius = 50;
                onDeath(this);
            }
        }

        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-radius / 2, -radius / 2, radius, radius);
            return path;
        }
    }
}
