using ConsoleApp3.Serializacion;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConsoleApp3
{
    public class UnCGraficas : GameWindow
    {
        private Camara camara;
        public Escenario escenario = new Escenario(Color4.Black) ;
        private double anguloRotacion = 0.0; 
        public UnCGraficas()
            : base(1920, 1080, GraphicsMode.Default, "Mi Ventana", GameWindowFlags.Fullscreen)
        {
            VSync = VSyncMode.On;
            camara = new Camara(this);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(Color4.Black);

            camara.IniciarMatrices(Width, Height);

            Serializador ser = new Serializador();

            // Crear el objeto en forma de "T"
            UncObjeto objetoT = CrearObjetoEnFormaDeT();

            // Serializar el objeto
            ser.Serializar(objetoT, "ObjetoT");

            // Deserializar el objeto dos veces para obtener dos instancias independientes
            Figura3D objetoDeserializado = ser.Deserializar("ObjetoT");
            Figura3D objetoDeserializado2 = ser.Deserializar("ObjetoT");

            // Aplicar transformaciones a los objetos
            objetoDeserializado.Trasladar(2, 1, 0);      // Mover el primer objeto
            objetoDeserializado2.Trasladar(-2, 0, 0);    // Mover el segundo objeto

            // Agregar las figuras al escenario con identificadores únicos
            escenario.AgregarFigura("objetoT_CentroEscenario", objetoDeserializado);
            escenario.AgregarFigura("objetoT_CentroObjeto", objetoDeserializado2);
            // Aplicar transformaciones a partes y polígonos específicos sin casting
            Figura3D parteHorizontal = objetoDeserializado2.ObtenerElemento("rectanguloHorizontal");
            Figura3D poligonoFrontal = parteHorizontal.ObtenerElemento("caraFrontal");

            // Aplica la transformación directamente
            TransformarElemento(parteHorizontal, "Trasladar", 1.0, 0.0, 0.0, "CentroParte");
            TransformarElemento(poligonoFrontal, "Escalar", 1.5, 0.0, 0.0, "CentroPoligono");
        }

        // Método para crear un objeto en forma de "T"
        private UncObjeto CrearObjetoEnFormaDeT()
        {
            double anchoHorizontal = 2.0, altoHorizontal = 0.2, profundidad = 0.5;
            double anchoVertical = 0.5, altoVertical = 1.0;

            // Crear las partes horizontales y verticales
            UncParte rectanguloHorizontal = CrearRectangulo(anchoHorizontal, altoHorizontal, profundidad, Color4.Red);
            UncParte rectanguloVertical = CrearRectangulo(anchoVertical, altoVertical, profundidad, Color4.Blue);

            // Crear el objeto y añadir las partes
            UncObjeto objetoT = new UncObjeto(Color4.White);
            objetoT.AñadirParte("rectanguloHorizontal", rectanguloHorizontal);
            objetoT.AñadirParte("rectanguloVertical", rectanguloVertical);

            // Aplicar transformaciones a las partes
            objetoT.ObtenerParte("rectanguloHorizontal").Trasladar(0.0, 0.2, 0.0);
            objetoT.ObtenerParte("rectanguloVertical").Trasladar(0.0, 0.0, 0.0);
            

            // Retornar el objeto creado
            return objetoT;
        }

        // Método para crear un rectángulo como un prisma
        private UncParte CrearRectangulo(double ancho, double alto, double profundidad, Color4 color)
        {
            UncParte rectangulo = new UncParte(color);

            // Definir los vértices del prisma rectangular
            UncPunto v1 = new UncPunto(-ancho / 2, -alto / 2, -profundidad / 2);
            UncPunto v2 = new UncPunto(ancho / 2, -alto / 2, -profundidad / 2);
            UncPunto v3 = new UncPunto(ancho / 2, alto / 2, -profundidad / 2);
            UncPunto v4 = new UncPunto(-ancho / 2, alto / 2, -profundidad / 2);
            UncPunto v5 = new UncPunto(-ancho / 2, -alto / 2, profundidad / 2);
            UncPunto v6 = new UncPunto(ancho / 2, -alto / 2, profundidad / 2);
            UncPunto v7 = new UncPunto(ancho / 2, alto / 2, profundidad / 2);
            UncPunto v8 = new UncPunto(-ancho / 2, alto / 2, profundidad / 2);

            // Crear las caras del prisma y añadir los vértices
            UncPoligono caraFrontal = new UncPoligono(color);
            caraFrontal.AñadirVertice("v1", v1);
            caraFrontal.AñadirVertice("v2", v2);
            caraFrontal.AñadirVertice("v3", v3);
            caraFrontal.AñadirVertice("v4", v4);

            UncPoligono caraTrasera = new UncPoligono(color);
            caraTrasera.AñadirVertice("v5", v5);
            caraTrasera.AñadirVertice("v6", v6);
            caraTrasera.AñadirVertice("v7", v7);
            caraTrasera.AñadirVertice("v8", v8);

            UncPoligono caraSuperior = new UncPoligono(color);
            caraSuperior.AñadirVertice("v4", v4);
            caraSuperior.AñadirVertice("v3", v3);
            caraSuperior.AñadirVertice("v7", v7);
            caraSuperior.AñadirVertice("v8", v8);

            UncPoligono caraInferior = new UncPoligono(color);
            caraInferior.AñadirVertice("v1", v1);
            caraInferior.AñadirVertice("v2", v2);
            caraInferior.AñadirVertice("v6", v6);
            caraInferior.AñadirVertice("v5", v5);

            UncPoligono caraIzquierda = new UncPoligono(color);
            caraIzquierda.AñadirVertice("v1", v1);
            caraIzquierda.AñadirVertice("v5", v5);
            caraIzquierda.AñadirVertice("v8", v8);
            caraIzquierda.AñadirVertice("v4", v4);

            UncPoligono caraDerecha = new UncPoligono(color);
            caraDerecha.AñadirVertice("v2", v2);
            caraDerecha.AñadirVertice("v6", v6);
            caraDerecha.AñadirVertice("v7", v7);
            caraDerecha.AñadirVertice("v3", v3);

            // Añadir las caras al rectángulo
            rectangulo.AñadirPoligono("caraFrontal", caraFrontal);
            rectangulo.AñadirPoligono("caraTrasera", caraTrasera);
            rectangulo.AñadirPoligono("caraSuperior", caraSuperior);
            rectangulo.AñadirPoligono("caraInferior", caraInferior);
            rectangulo.AñadirPoligono("caraIzquierda", caraIzquierda);
            rectangulo.AñadirPoligono("caraDerecha", caraDerecha);

            return rectangulo;
        }

        // Método para renderizar el escenario
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            camara.ConfigurarMatrices(); // Configurar la cámara
            escenario.Dibujar();

            SwapBuffers();
        }




        public void TransformarElemento(Figura3D elemento, string tipoTransformacion, double anguloX, double anguloY, double anguloZ, string opcionPivote)
        {
            UncPunto centro = null;

            // Determinar el centro de transformación según la opción
            switch (opcionPivote)
            {
                case "CentroObjeto":
                    if (elemento is UncObjeto objeto)
                        centro = objeto.CalcularCentroDeMasa();
                    break;
                case "CentroParte":
                    if (elemento is UncParte parte)
                        centro = parte.CalcularCentroDeMasa();
                    break;
                case "CentroEscenario":
                    centro = escenario.CalcularCentroDeMasa();
                    break;
                case "CentroPoligono":
                    if (elemento is UncPoligono pol)
                        centro = pol.CalcularCentroDeMasa();
                    break;
                case "Origen":
                    centro = new UncPunto(0, 0, 0);
                    break;
                default:
                    centro = new UncPunto(0, 0, 0);
                    break;
            }

            // Aplicar la transformación
            switch (tipoTransformacion)
            {
                case "Rotar":
                    elemento.Rotar(anguloX, anguloY, anguloZ, centro);
                    break;
                case "Trasladar":
                    elemento.Trasladar(anguloX, anguloY, anguloZ);
                    break;
                case "Escalar":
                    elemento.Escalar(anguloX, centro); 
                    break;
                default:
                    Console.WriteLine("Tipo de transformación no reconocido.");
                    break;
            }
        }


        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            camara.MouseWheel(e);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            camara.MouseMove(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            camara.onMouseDown(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            KeyboardState input = Keyboard.GetState();

            if (input.IsKeyDown(Key.Escape))
            {
                Exit();
            }

            double VLiz = 40.0;

            anguloRotacion += VLiz * e.Time;

            if (anguloRotacion >= 360.0)
            {
                anguloRotacion -= 360.0;
            }

            // Rotar el primer objeto
            Figura3D objeto1 = escenario.ObtenerFigura("objetoT_CentroEscenario");
            //TransformarElemento(objeto1, "Rotar", 0.0, 0.0, VLiz * e.Time, "CentroEscenario");

            // Rotar el segundo objeto
            Figura3D objeto2 = escenario.ObtenerFigura("objetoT_CentroObjeto");
            UncPunto centroEsnario = escenario.CalcularCentroDeMasa();
            //escenario.RotarTodas(VLiz * e.Time,0.0,0.0,centroEsnario);
            // TransformarElemento(objeto2, "Rotar", VLiz * e.Time, 0.0, 0.0, "CentroObjeto");

            //// Rotar la parte "rectanguloHorizontal" del segundo objeto
            if (objeto2 is UncObjeto uncObjeto)
            {

                Figura3D parte = uncObjeto.ObtenerParte("rectanguloHorizontal");
                if (parte != null)
                {
                    TransformarElemento(parte, "Rotar", 0.0, VLiz * e.Time, 0.0, "CentroParte");
                    //TransformarElemento(parte,"Escalar",2f,VLiz*e.Time,0.0,"CentroParte");
                }
                //Figura3D parte2 = uncObjeto.ObtenerParte("rectanguloVertical");
                //if (parte2 != null)
                //{
                //    TransformarElemento(parte2, "Rotar", VLiz * e.Time, 0.0, 0.0, "CentroParte");
                //}
                //Figura3D caraFrontal = uncObjeto.ObtenerParte("rectanguloVertical").ObtenerPoligono("caraFrontal");
                //if (caraFrontal != null)
                //{
                //    TransformarElemento(caraFrontal, "Rotar", 0.0, VLiz * e.Time, 0.0, "CentroEscenario");
                //}

            }

        }


       

    }
}
