using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Labe5.Object;

namespace Labe5
{
    public partial class Form1 : Form
    {
        List<BaseObject> objects = new();
        Player player;
        Marker? marker;
        Dot? dot1;
        Dot? dot2;
        ushort score = 0;
        Random random = new Random();

        public Form1()
        {
            InitializeComponent();

            player = new Player(pbMain.Width / 2, pbMain.Height / 2, 0);
            var random = new Random();

            player.OnOverlap += (player, obj) =>
            {
                txtLog.Text = $"[{DateTime.Now:HH:mm:ss:ff}] »грок пересекс€ с {obj}\n" + txtLog.Text;
            };

            player.OnMarkerOverlap += (m) =>
            {
                objects.Remove(m);
                marker = null;
            };

            player.OnDotOverlap += (dot) =>
            {
                ResetDot(dot);  
                score++;
                Score_Label.Text = $"—чЄт: {score}";
            };


            dot1 = new(random.Next(20, 539), random.Next(20, 384), 0, random.Next(30, 50), random.Next(60, 150));
            dot2 = new(random.Next(20, 539), random.Next(20, 384), 0, random.Next(30, 50), random.Next(60, 150));
            updatePoints();

            objects.Add(player);
            objects.Add(dot1);
            objects.Add(dot2);
        }

        public void ResetDot(Dot dot)
        {

            dot.X = random.Next(20, 539);
            dot.Y = random.Next(20, 384);
            dot.radius = random.Next(30, 50);
        }


        private void updatePlayer()
        {
            if (marker != null)
            {
                float dx = marker.X - player.X;
                float dy = marker.Y - player.Y;
                float lenght = MathF.Sqrt(dx * dx + dy * dy);
                dx /= lenght;
                dy /= lenght;
                player.vX += dx * 0.5f;
                player.vY += dy * 0.5f;
                player.Angle = 90 - MathF.Atan2(player.vX, player.vY) * 180 / MathF.PI;
            }
            player.vX += -player.vX * 0.1f;
            player.vY += -player.vY * 0.1f;

            player.X += player.vX;
            player.Y += player.vY;
        }
        public void updatePoints()
        {

            dot1.onDeath += onPointDeath;
            dot2.onDeath += onPointDeath;
        }
        private void onPointDeath(Dot m)
        {
            m.X = random.Next(15, 430);
            m.Y = random.Next(15, 240);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pbMain.Invalidate();
        }


        private void pbMain_Click(object sender, EventArgs e)
        {

        }

        private void pbMain_MouseClick(object sender, MouseEventArgs e)
        {
            if (marker == null)
            {
                marker = new Marker(0, 0, 0);
                objects.Add(marker);
            }

            marker.X = e.X;
            marker.Y = e.Y;
        }

        private void pbMain_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            g.Clear(Color.White);

            updatePlayer();

            foreach (var obj in objects.ToList())
            {
                if (obj != player && player.Overlaps(obj, g))
                {
                    player.Overlap(obj);
                    obj.Overlap(player);
                }
            }
            foreach (var obj in objects.ToList())
            {
                g.Transform = obj.GetTransform();
                obj.Render(g);
            }
        }
    }
}