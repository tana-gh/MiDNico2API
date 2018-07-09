namespace MiDNicoAPI
{
    public enum CommentCommnd
    {
        Top,
        Right,
        Bottom,
        Left,
        None,
    }

    internal static class CommentCommndEx
    {
        public static string Value (
            this CommentCommnd command
        )
        {
            switch (command)
            {
                case CommentCommnd.Top      : return "top";
                case CommentCommnd.Right    : return "right";
                case CommentCommnd.Bottom   : return "buttom";
                case CommentCommnd.Left     : return "left";
                default                     : return "";
            }
        }
    }
}
