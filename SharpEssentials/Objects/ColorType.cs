namespace SharpEssentials {
    public enum ColorType {
        RED,
        GREEN,
        BLUE,
    }

    public static class BasicColorExtensions {
        public static string GetName(this ColorType color) {
            switch(color) {
                case ColorType.RED:
                    return "Red";
                case ColorType.GREEN:
                    return "Green";
                case ColorType.BLUE:
                    return "Blue";
                default:
                    return "Unknown";
            }
        }
    }

}
