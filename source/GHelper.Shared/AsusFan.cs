/// <summary>
/// The fan endpoints ASUS firmware exposes. Lives in the shared layer because
/// <see cref="AppConfig"/> keys its stored fan curves by this value.
/// </summary>
public enum AsusFan
{
    CPU = 0,
    GPU = 1,
    Mid = 2,
    XGM = 3
}
