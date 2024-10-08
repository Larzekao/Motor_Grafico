using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Windows.Forms;
using System.Drawing;
using ConsoleApp3.Serializacion;
using ConsoleApp3;
using AppMotorGrafico.Pantalla;

namespace AppMotorGrafico
{
    public partial class Form1 : Form
    {
        private GLControl glControl1;
        private System.Windows.Forms.Timer timer;
        private Escenario escenario;
        private Camara3D camara;  // Cambiado a Camara3D

        // Velocidades angulares en grados por frame
        private double deltaAnguloT1 = 1;  // Velocidad de rotación del objeto T1
        private double deltaAnguloT2 = 1;  // Velocidad de rotación del objeto T2

        public Form1()
        {
            InitializeComponent();

            // Establecer el tamaño del formulario
            // Ajusta el tamaño según tus necesidades
            this.WindowState = FormWindowState.Maximized;
            // Crear el GLControl
            glControl1 = new GLControl();
            glControl1.Location = new Point(0, 0);
            glControl1.Size = new Size(800, 600);
            glControl1.Load += glControl1_Load;
            glControl1.Paint += glControl1_Paint;
            glControl1.Resize += glControl1_Resize;
            glControl1.MouseDown += GlControl1_MouseDown;
            glControl1.MouseMove += GlControl1_MouseMove;
            glControl1.MouseWheel += GlControl1_MouseWheel;

            // Crear un Panel para el GLControl
            Panel panelGL = new Panel();
            panelGL.Location = new Point(150, 10);
            panelGL.Size = new Size(800, 600);
            panelGL.BorderStyle = BorderStyle.FixedSingle;

            // Agregar el GLControl al panel
            panelGL.Controls.Add(glControl1);

            // Agregar el panel al formulario
            this.Controls.Add(panelGL);

          

            // Inicializar la cámara
            camara = new Camara3D();

            // Inicializar el timer
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 16; // Aproximadamente 60 FPS
            timer.Tick += Timer_Tick;
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            // Configuración inicial de OpenGL
            GL.ClearColor(Color4.Black);
            GL.Enable(EnableCap.DepthTest);

            // Crear la instancia de la cámara
            camara = new Camara3D();  // Cambiado a Camara3D

            // Configurar la matriz de proyección
            camara.IniciarMatrices(glControl1.Width, glControl1.Height);

            // Inicializar la escena y los objetos
            InicializarEscena();

            // Iniciar el timer
            timer.Start();
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            // Limpiar el buffer de color y profundidad
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Configurar las matrices de la cámara
            camara.ConfigurarMatrices();

            // Dibujar el escenario completo
            escenario.Dibujar();

            // Intercambiar los buffers
            glControl1.SwapBuffers();
        }

        private void glControl1_Resize(object sender, EventArgs e)
        {
            if (glControl1.ClientSize.Height == 0)
                glControl1.ClientSize = new System.Drawing.Size(glControl1.ClientSize.Width, 1);

            GL.Viewport(0, 0, glControl1.Width, glControl1.Height);

            // Verificar si la cámara está inicializada antes de usarla
            if (camara != null)
            {
                camara.IniciarMatrices(glControl1.Width, glControl1.Height);
            }
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            // Aplicar las transformaciones a los objetos usando el delta de rotación
            AplicarTransformaciones();

            // Forzar el repintado del GLControl
            glControl1.Invalidate();
        }

        private void InicializarEscena()
        {
            escenario = new Escenario(Color4.Black);

            // Crear una instancia del serializador
            Serializador ser = new Serializador();

            // Deserializar el objeto 'T' dos veces para obtener dos instancias independientes
            Figura3D objetoT1 = ser.Deserializar("ObjetoT");
            Figura3D objetoT2 = ser.Deserializar("ObjetoT");

            if (objetoT1 != null && objetoT2 != null)
            {
                // Aplicar transformaciones iniciales a los objetos deserializados
                objetoT1.Trasladar(2, 1, 0);      // Mover el primer objeto
                objetoT2.Trasladar(-2, 0, 0);     // Mover el segundo objeto

                // Agregar las figuras al escenario
                escenario.AgregarFigura("objetoT1", objetoT1);
                escenario.AgregarFigura("objetoT2", objetoT2);
            }
            else
            {
                MessageBox.Show("Error al deserializar los objetos 'T'.");
            }
        }

        private void AplicarTransformaciones()
        {
            // Obtener los objetos del escenario
            Figura3D objetoT1 = escenario.ObtenerFigura("objetoT1");
            Figura3D objetoT2 = escenario.ObtenerFigura("objetoT2");

            // Rotar el primer objeto alrededor de su propio centro de masa
            if (objetoT1 != null)
            {
                UncPunto centroT1 = objetoT1.CalcularCentroDeMasa();
                objetoT1.Rotar(0.0, deltaAnguloT1, 0.0, centroT1);
            }

            // Rotar el segundo objeto alrededor del centro del escenario
            if (objetoT2 != null)
            {
                UncPunto centroEscenario = escenario.CalcularCentroDeMasa();
                objetoT2.Rotar(0.0, deltaAnguloT2, 0.0, centroEscenario);
            }
        }

        private void GlControl1_MouseDown(object sender, MouseEventArgs e)
        {
            if (camara != null)
            {
                camara.OnMouseDown(e);
            }
        }

        private void GlControl1_MouseMove(object sender, MouseEventArgs e)
        {
            camara.OnMouseMove(e);
            glControl1.Invalidate(); // Forzar repintado
        }

        private void GlControl1_MouseWheel(object sender, MouseEventArgs e)
        {
            camara.OnMouseWheel(e);
            glControl1.Invalidate(); // Forzar repintado
        }
    }
}
