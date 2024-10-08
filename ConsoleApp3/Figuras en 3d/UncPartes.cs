using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics;


namespace ConsoleApp3
{
    public class UncParte : Figura3D
    {
        public Dictionary<string, UncPoligono> Poligonos { get;  set; }
        public Color4 Color { get; set; }
        public UncParte()
        {
            // No inicializar Poligonos aquí
        }


        public UncParte(Color4 color)
        {
            Poligonos = new Dictionary<string, UncPoligono>();
            Color = color;
        }

        public void AñadirPoligono(string id, UncPoligono poligono)
        {
            Poligonos[id] = poligono;
        }

       
        public bool EliminarPoligono(string id)
        {
            return Poligonos.Remove(id);
        }

        // Nuevo método para obtener un polígono por su ID
        public UncPoligono ObtenerPoligono(string id)
        {
            if (Poligonos.TryGetValue(id, out UncPoligono poligono))
            {
                return poligono;
            }
            else
            {
                Console.WriteLine($"El polígono con ID '{id}' no existe en esta parte.");
                return null;
            }
        }
        public UncPunto CalcularCentroDeMasa()
        {
            if (Poligonos == null || Poligonos.Count == 0)
                return new UncPunto();

            var centros = Poligonos.Values.Select(p => p.CalcularCentroDeMasa()).ToList();

            double xProm = centros.Average(p => p.X);
            double yProm = centros.Average(p => p.Y);
            double zProm = centros.Average(p => p.Z);

            return new UncPunto(xProm, yProm, zProm);
        }


        public void Trasladar(double tx, double ty, double tz)
        {
            foreach (var poligono in Poligonos.Values)
            {
                poligono.Trasladar(tx, ty, tz);
            }
        }

        public void Escalar(double factor)
        {
            UncPunto centro = CalcularCentroDeMasa();
            Escalar(factor, centro);
        }

        public void Escalar(double factor, UncPunto centro)
        {
            foreach (var poligono in Poligonos.Values)
            {
                poligono.Escalar(factor, centro);
            }
        }

        public void Rotar(double anguloX, double anguloY, double anguloZ)
        {
            UncPunto centro = CalcularCentroDeMasa();
            Rotar(anguloX, anguloY, anguloZ, centro);
        }

        public void Rotar(double anguloX, double anguloY, double anguloZ, UncPunto centro)
        {
            foreach (var poligono in Poligonos.Values)
            {
                poligono.Rotar(anguloX, anguloY, anguloZ, centro);
            }
        }

        public Figura3D ObtenerElemento(string id)
        {
            if (Poligonos.ContainsKey(id))
                return Poligonos[id];
            else
                return null;
        }
        public void Dibujar()
        {
            Console.WriteLine($"Dibujando la parte...");
           
            
                foreach (var poligono in Poligonos.Values)
                {
                    poligono.Dibujar();
                }
            
            
        }

    }
}
