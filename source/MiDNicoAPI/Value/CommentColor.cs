namespace MiDNicoAPI
{
    public enum CommentColor
    {
        White,
        White2,
        Red,
        Red2,
        Pink,
        Pink2,
        Orange,
        Orange2,
        Yellow,
        Yellow2,
        Green,
        Green2,
        Cyan,
        Cyan2,
        Blue,
        Blue2,
        Purple,
        Purple2,
        Black,
        Black2,
    }

    internal static class CommentColorEx
    {
        public static string Value (
            this CommentColor color
        )
        {
            switch (color)
            {
                case CommentColor.White     : return "white";
                case CommentColor.White2    : return "white2";
                case CommentColor.Red       : return "red";
                case CommentColor.Red2      : return "red2";
                case CommentColor.Pink      : return "pink";
                case CommentColor.Pink2     : return "pink2";
                case CommentColor.Orange    : return "orange";
                case CommentColor.Orange2   : return "orange2";
                case CommentColor.Yellow    : return "yellow";
                case CommentColor.Yellow2   : return "yellow2";
                case CommentColor.Green     : return "green";
                case CommentColor.Green2    : return "green2";
                case CommentColor.Cyan      : return "cyan";
                case CommentColor.Cyan2     : return "cyan2";
                case CommentColor.Blue      : return "blue";
                case CommentColor.Blue2     : return "blue2";
                case CommentColor.Purple    : return "purple";
                case CommentColor.Purple2   : return "purple2";
                case CommentColor.Black     : return "black";
                case CommentColor.Black2    : return "black2";
                default                     : return "white";
            }
        }
    }
}
