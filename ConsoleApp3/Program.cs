using ConsoleApp3;

class Program
{
    static void Main(string[] args)
    {
        using (UnCGraficas game = new UnCGraficas())
        {
            game.Run(60.0); // Ejecuta el juego a 60 FPS
        }
    }
}