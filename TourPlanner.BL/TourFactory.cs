namespace TourPlanner.BL;

public static class TourFactory
{
    private static ITourFactory? _instance;
    
    public static ITourFactory GetInstance()
    {
        return _instance ??= new TourFactoryImpl();
    }
}
