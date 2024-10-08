namespace ConsoleApp3
{
    public interface Figura3D
    {
        void Trasladar(double tx, double ty, double tz);
        void Escalar(double factor);
        void Escalar(double factor, UncPunto centro);
        //void Rotar(double anguloX, double anguloY, double anguloZ);

        void Rotar(double anguloX, double anguloY, double anguloZ, UncPunto centro);
        // Otros métodos...

        UncPunto CalcularCentroDeMasa();
        Figura3D ObtenerElemento(string id);
        void Dibujar();
    }
    

}
