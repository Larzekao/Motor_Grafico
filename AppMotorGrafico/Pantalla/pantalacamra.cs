using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Windows.Forms;

namespace AppMotorGrafico.Pantalla
{
    public class Camara3D
    {
        private double oldX, oldY;

        public double AngX { get; set; }
        public double AngY { get; set; }
        public double TlsX { get; set; }
        public double TlsY { get; set; }
        public double Scale { get; set; }

        public Camara3D()
        {
            AngX = AngY = 0.0;
            TlsX = TlsY = 0.0;
            oldX = oldY = 0.0;
            Scale = 1.0;
        }

        public void IniciarMatrices(int width, int height)
        {
            Matrix4 projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45.0f),
                (float)width / height,
                0.1f, 100.0f);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projectionMatrix);
        }

        public void ConfigurarMatrices()
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Translate(TlsX, TlsY, -10.0f);
            GL.Rotate((float)AngX, 1.0f, 0.0f, 0.0f);
            GL.Rotate((float)AngY, 0.0f, 1.0f, 0.0f);
            GL.Scale(Scale, Scale, Scale);
        }

        public void OnMouseDown(MouseEventArgs e)
        {
            oldX = e.X;
            oldY = e.Y;
        }

        public void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                RotarCamara(e);
            }
            else if (e.Button == MouseButtons.Right)
            {
                TrasladarCamara(e);
            }
        }

        private void RotarCamara(MouseEventArgs e)
        {
            double movedX = e.X - oldX;
            double movedY = e.Y - oldY;

            AngX += movedY * 0.1;  // Ajusta la sensibilidad según sea necesario
            AngY += movedX * 0.1;

            oldX = e.X;
            oldY = e.Y;
        }

        private void TrasladarCamara(MouseEventArgs e)
        {
            double movedX = e.X - oldX;
            double movedY = e.Y - oldY;

            TlsX += movedX * 0.01;
            TlsY -= movedY * 0.01;

            oldX = e.X;
            oldY = e.Y;
        }

        public void OnMouseWheel(MouseEventArgs e)
        {
            double zoomSpeed = 0.001;
            Scale += e.Delta * zoomSpeed;
            if (Scale < 0.1) Scale = 0.1;  // Evitar zoom negativo
        }
    }
}
