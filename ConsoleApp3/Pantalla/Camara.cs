using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using System;

namespace ConsoleApp3
{
    public class Camara
    {
        private double rotaX, rotaY;
        private double transX, transY;
        private double oldX, oldY;
        private readonly UnCGraficas game;

        private readonly double speed = 0.5;

        public double AngX { get; set; }
        public double AngY { get; set; }
        public double TlsX { get; set; }
        public double TlsY { get; set; }
        public double Scale { get; set; }

        public Camara(UnCGraficas g)
        {
            game = g;
            rotaX = rotaY = 0.0;
            transX = transY = 0.0;
            AngX = AngY = 0.0;
            oldX = oldY = 0.0;
            Scale = 1;
        }

        public void IniciarMatrices(int width, int height)
        {
            Matrix4d projectionMatrix = Matrix4d.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0), (double)width / height, 0.1, 100.0);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projectionMatrix);
        }

        public void ConfigurarMatrices()
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Translate(TlsX, TlsY, -10.0);
            GL.Rotate(AngX, 1.0, 0.0, 0.0);
            GL.Rotate(AngY, 0.0, 1.0, 0.0);
            GL.Scale(Scale, Scale, Scale);
        }

        public void onMouseDown(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
            {
                MouseState m = Mouse.GetCursorState();
                oldX = m.X;
                oldY = m.Y;
            }
        }

        public void MouseMove(MouseMoveEventArgs e)
        {
            if (e.Mouse[MouseButton.Left])
            {
                RotarCamara(e);
            }
            else if (e.Mouse[MouseButton.Right])
            {
                TrasladarCamara(e);
            }
        }

        private void RotarCamara(MouseMoveEventArgs e)
        {
            double MovedX = e.X - oldX;
            double MovedY = e.Y - oldY;

            AngX += MovedY * 0.1;  // Controlar la rotación en el eje X
            AngY += MovedX * 0.1;  // Controlar la rotación en el eje Y

            oldX = e.X;
            oldY = e.Y;
        }

        private void TrasladarCamara(MouseMoveEventArgs e)
        {
            double MovedX = e.X - oldX;
            double MovedY = e.Y - oldY;

            TlsX += MovedX * 0.01;
            TlsY -= MovedY * 0.01;

            oldX = e.X;
            oldY = e.Y;
        }

        public void MouseWheel(MouseWheelEventArgs e)
        {
            double zoomSpeed = 0.1;
            Scale += e.DeltaPrecise * zoomSpeed;
            if (Scale < 0.1) Scale = 0.1;  // Evitar hacer zoom negativo
        }
    }
}
