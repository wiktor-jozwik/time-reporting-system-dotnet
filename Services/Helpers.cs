using System;

public static class Helpers {
    public static string ToFriendlyString(this Boolean b)
    {
        return b ? "Yes" : "No";
    }
}
