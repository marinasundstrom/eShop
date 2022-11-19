using YourBrand.Analytics.Domain.Enums;

namespace YourBrand.Analytics.Presentation.Controllers;

public record EventData(EventType EventType, Dictionary<string, object> Data);
