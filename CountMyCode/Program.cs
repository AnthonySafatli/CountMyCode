namespace CountMyCode
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string initialPath = Environment.CurrentDirectory;
            if (args.Length > 0)
            {
                initialPath = args[0];
            }

            App app = new App(initialPath);
            app.Run();
        }
    }
}
