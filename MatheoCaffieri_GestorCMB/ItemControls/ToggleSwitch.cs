using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.LinkLabel;
using System.Windows.Forms;

namespace MatheoCaffieri_GestorCMB.ItemControls
{
    public class ToggleSwitch : Control
    {
        // Fields
        private bool isOn;
        private int radius = 15;
        private int switchPadding = 3;
        private int switchX; // Posición del botón
        private Timer animationTimer; // Timer para la animación

        // Constructor
        public ToggleSwitch()
        {
            Width = 50;
            Height = 25;
            isOn = false;
            DoubleBuffered = true;
            Click += ToggleSwitch_Click;

            switchX = switchPadding; // Posición inicial del switch

            // Configurar el Timer
            animationTimer = new Timer();
            animationTimer.Interval = 10; // Tiempo de actualización en ms (velocidad de animación)
            animationTimer.Tick += AnimateSwitch;
        }

        // Properties
        public Color OnColor { get; set; } = Color.FromArgb(76, 175, 80);
        public Color OffColor { get; set; } = Color.FromArgb(180, 180, 180);
        public Color SwitchColor { get; set; } = Color.White;
        public Color BorderColor { get; set; } = Color.FromArgb(150, 150, 150);

        public bool IsOn
        {
            get { return isOn; }
            set
            {
                isOn = value;
                animationTimer.Start(); // Iniciar la animación
            }
        }

        // Métodos
        private void ToggleSwitch_Click(object sender, EventArgs e)
        {
            IsOn = !IsOn;
            OnToggleChanged(EventArgs.Empty);
        }

        protected virtual void OnToggleChanged(EventArgs e) => ToggleChanged?.Invoke(this, e);
        public event EventHandler ToggleChanged;

        private GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int corner)
        {
            var path = new GraphicsPath();
            var diameter = corner * 2;
            var arcRect = new Rectangle(rect.Location, new Size(diameter, diameter));

            path.AddArc(arcRect, 180, 90);
            arcRect.X = rect.Right - diameter;
            path.AddArc(arcRect, 270, 90);
            arcRect.Y = rect.Bottom - diameter;
            path.AddArc(arcRect, 0, 90);
            arcRect.X = rect.Left;
            path.AddArc(arcRect, 90, 90);
            path.CloseFigure();

            return path;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.Clear(Parent.BackColor);

            // Dibujar el fondo del switch
            var toggleColor = IsOn ? OnColor : OffColor;
            var switchRect = new Rectangle(0, 0, Width - 1, Height - 1);

            using (var path = CreateRoundedRectanglePath(switchRect, radius))
            {
                e.Graphics.FillPath(new SolidBrush(toggleColor), path);
                using (var pen = new Pen(BorderColor, 1))
                    e.Graphics.DrawPath(pen, path);
            }

            // Dibujar el botón deslizante con animación
            int switchSize = Height - switchPadding * 2;
            var buttonRect = new Rectangle(switchX, switchPadding, switchSize, switchSize);

            using (var buttonPath = CreateRoundedRectanglePath(buttonRect, radius - 3))
            {
                e.Graphics.FillPath(new SolidBrush(SwitchColor), buttonPath);
                using (var pen = new Pen(BorderColor, 1))
                    e.Graphics.DrawPath(pen, buttonPath);
            }
        }

        private void AnimateSwitch(object sender, EventArgs e)
        {
            int targetX = IsOn ? Width - (Height - switchPadding * 2) - switchPadding : switchPadding;

            if (Math.Abs(switchX - targetX) <= 3) // Si está cerca del destino, lo ajustamos exacto
            {
                switchX = targetX;
                animationTimer.Stop();
            }
            else if (switchX < targetX)
                switchX += 3; // Mueve el botón a la derecha
            else if (switchX > targetX)
                switchX -= 3; // Mueve el botón a la izquierda

            Invalidate(); // Vuelve a dibujar el control con la nueva posición del botón
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);
        }
    }
}
