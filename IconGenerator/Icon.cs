namespace IconGenerator
{
    public class Icon
    {
        public string InputPath { get; }

        public string OutputPath { get; }

        public int Width { get; }

        public int Height { get; }

        public int Margin { get; }

        public Icon(string input, string outputPath, int width, int height, int margin)
        {
            InputPath = input;
            OutputPath = outputPath;
            Width = width;
            Height = height;
            Margin = margin;
        }

        public void Generate()
        {
            ImageUtilities.ResizeImage(InputPath, OutputPath, Width, Height, Margin);
        }

        public override string ToString()
        {
            return $"{InputPath} -> {OutputPath} ({Width}x{Height})";
        }
    }
}